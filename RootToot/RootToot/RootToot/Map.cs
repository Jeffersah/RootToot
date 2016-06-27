using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;


namespace RootToot
{
    class Map
    {
        const double ELIFE_SPAWN_CHANCE = .001;
        const int MAX_BONUS = 480;

        int currentBonus;

        static Image image_horz;
        static Image image_vert;
        
        int currentTime;
        public LevelData myData;

        bool timePause;

        public Player player;
        public List<Note> allNotes;
        public List<NoteProjectile> NoteProj;
        public List<Enemy> AllEnemies;
        public List<PopupPoints> points;

        public List<MapSpawner> spawners;
        

        public PlayScreen screen;

        public bool paused;

        public ExtraLife E_Life;
        public Bonus bonus;

        public int Dead_reset;

        public Map(LevelData data, PlayScreen screen)
        {
            Dead_reset = 0;
            this.screen = screen;
            image_horz = new Image("horz");
            image_vert = new Image("vert");
            allNotes = new List<Note>();
            NoteProj = new List<NoteProjectile>();
            AllEnemies = new List<Enemy>();
            spawners = new List<MapSpawner>();
            points = new List<PopupPoints>();

            myData = data;

            currentTime = 0;
            timePause = false;

            player = new Player(new Point(data.PlayerSpawn.X * Globals.stdTile, data.PlayerSpawn.Y * Globals.stdTile), this);

            for(int x = 0; x < 14; x++)
            {
                for(int y = 0; y < 13; y++)
                {
                    if(data.Notes[x,y])
                    {
                        allNotes.Add(new Note(new Point(x * Globals.stdTile, y * Globals.stdTile)));
                    }
                }
            }

            foreach(Spawner s in data.spawners)
            {
                spawners.Add(new MapSpawner(s, this));
            }

            paused = false;
        }

        public bool isMainEnemy(Enemy e)
        {
            return e is Enemy1 || e is Enemy2 || e is Enemy3 || e is Enemy4 || e is Enemy5 || e is Enemy6;
        }

        public void ActivateBonus()
        {
            currentBonus = MAX_BONUS;
            foreach (Enemy e in AllEnemies)
                if(isMainEnemy(e))
                    e.Freeze(MAX_BONUS);
        }

        public void Update()
        {
            if (KeyboardInputManager.isKeyPressed(Microsoft.Xna.Framework.Input.Keys.NumPad9))
                Globals.DEBUG = !Globals.DEBUG;
            if (currentBonus > 0)
            {
                currentBonus--;
            }

            if(E_Life == null)
            {
                if(GlobalRandom.random.NextDouble() < ELIFE_SPAWN_CHANCE)
                {
                    Point pos = GlobalRandom.RandomFrom<Point>(myData.espawn_possible);
                    E_Life = new ExtraLife(pos, Globals.ETIME);
                    myData.espawn_possible.Remove(pos);
                }
            }
            else
            {
                Point tmp = E_Life.currentPos;
                E_Life.Update(this);
                if (E_Life == null)
                    myData.espawn_possible.Add(tmp);
            }

            if(bonus == null)
            {
                if (GlobalRandom.random.NextDouble() < ELIFE_SPAWN_CHANCE * 1.5)
                {
                    Point pos = GlobalRandom.RandomFrom<Point>(myData.espawn_possible);
                    bonus = new Bonus(pos, Globals.ETIME * 2);
                    myData.espawn_possible.Remove(pos);
                }
            }
            else
            {
                Point tmp = bonus.currentPos;
                bonus.Update(this);
                if (bonus == null)
                    myData.espawn_possible.Add(tmp);
            }

            if(allNotes.Count == 0)
            {
                screen.AdvanceLevel();
            }

            foreach (PopupPoints p in points)
                p.update();
            points.RemoveAll(x => !x.isAlive);
            if (!paused)
            {
                if (!timePause)
                    currentTime++;

                foreach (SpawnOrder order in myData.spawnOrders)
                {
                    if (order.time == currentTime)
                    {
                        foreach (MapSpawner spawn in spawners)
                        {
                            if (spawn.spawn.name.Equals(order.from))
                            {
                                spawn.AddToQueue(order.type);
                            }
                        }
                    }
                }

                foreach (MapSpawner spawn in spawners)
                {
                    spawn.Update(this);
                }

                player.Update(this);
                foreach (NoteProjectile proj in NoteProj)
                {
                    proj.Update(this);
                }

                if (!player.isGhost)
                {
                    for (int i = allNotes.Count - 1; i >= 0; i--)
                    {
                        if (Entity.EntitiesIntersect_Rect(player, allNotes[i], 16))
                        {
                            screen.score += 20;
                            NoteProj.Add(new NoteProjectile(allNotes[i].currentPos, player.direction));
                            allNotes.RemoveAt(i);
                        }
                    }
                }

                for (int i = NoteProj.Count - 1; i >= 0; i--)
                {
                    if (NoteProj[i].currentPos.X < -16 || NoteProj[i].currentPos.Y > 14 * 16 || NoteProj[i].currentPos.Y < -32 || NoteProj[i].currentPos.X >= 15 * 16)
                    {
                        NoteProj.RemoveAt(i);
                    }
                    else
                    {
                        for (int j = AllEnemies.Count - 1; j >= 0; j--)
                        {
                            if (Entity.EntitiesIntersect_Rect(NoteProj[i], AllEnemies[j]))
                            {
                                points.Add(new PopupPoints(AllEnemies[j].PointValue * NoteProj[i].multiplier++, new Point(AllEnemies[j].currentPos.X + 16, AllEnemies[j].currentPos.Y + 6), 64));
                                screen.score += AllEnemies[j].PointValue * NoteProj[i].multiplier++;
                                if (AllEnemies[j].respawns)
                                {
                                    MapSpawner spawnfrom = spawners[GlobalRandom.random.Next(spawners.Count)];
                                    spawnfrom.AddDeadCreep(AllEnemies[j].etype, AllEnemies[j].respawntime);
                                }
                                AllEnemies.RemoveAt(j);
                            }
                        }
                    }
                }
                for (int j = AllEnemies.Count - 1; j >= 0; j--)
                {
                    if (AllEnemies[j].freeze == 0 || !isMainEnemy(AllEnemies[j]))
                        AllEnemies[j].Update(this);
                    else
                        AllEnemies[j].freeze--;
                }
                if (!player.isGhost)
                {
                    for (int j = AllEnemies.Count - 1; j >= 0; j--)
                    {
                        if (Entity.EntitiesIntersect_Rect(AllEnemies[j], player, 8 * 16))
                            HitEnemy(AllEnemies[j]);
                    }
                }
            }

            if(Dead_reset > 0 && paused)
            {
                Dead_reset--;
                if(Dead_reset == 1)
                {
                    screen.PlayerDied();
                    paused = false;
                    AllEnemies.Clear();
                    player = new Player(new Point(myData.PlayerSpawn.X * Globals.stdTile, myData.PlayerSpawn.Y * Globals.stdTile), this);
                    NoteProj.Clear();
                    foreach (MapSpawner spawn in spawners)
                    {
                        spawn.reset();
                    }
                    foreach (SpawnOrder sorder in myData.spawnOrders)
                    {
                        if(sorder.time < currentTime)
                        {
                            foreach(MapSpawner spawn in spawners)
                            {
                                if(spawn.spawn.name.Equals(sorder.from))
                                {
                                    spawn.AddToQueue(sorder.type);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void HitEnemy(Enemy e)
        {
            if(e.freeze > 0)
            {
                if(e is Enemy4 || !isMainEnemy(e))
                {
                    paused = true;
                    Dead_reset = 96;
                }
                else
                {
                    points.Add(new PopupPoints(e.PointValue, new Point(e.currentPos.X + 16, e.currentPos.Y + 6), 64));
                    screen.score += e.PointValue;
                    if (e.respawns)
                    {
                        MapSpawner spawnfrom = spawners[GlobalRandom.random.Next(spawners.Count)];
                        spawnfrom.AddDeadCreep(e.etype, e.respawntime);
                    }
                    AllEnemies.Remove(e);
                }
            }
            else if(!paused)
            { 
                paused = true;
                Dead_reset = 96;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle loc = new Rectangle(0, 0, Globals.stdTile, Globals.stdTile);
            for(int x = 0; x < 14; x++)
            {
                loc.X =x * Globals.stdTile;
                for (int y = 0; y < 13; y++)
                {
                    loc.Y = y * Globals.stdTile;
                    if(myData.Map[x,y] == 1)
                    {
                        Camera.draw(sb, image_vert, loc);
                    }
                    else if(myData.Map[x,y] == 2)
                    {
                        Camera.draw(sb, image_horz, loc);
                    }
                }
            }
      
            Camera.drawGeneric(sb, Globals.FullScreen, Color.Black * .3f);

            foreach(Note n in allNotes)
            {
                n.Draw(sb);
            }

            foreach (NoteProjectile proj in NoteProj)
            {
                proj.Draw(sb);
            }

            foreach (Enemy e in AllEnemies)
            {
                e.Draw(sb);
                if (e.freeze > 0 && 
                    (((Globals.GlobalTime / 20) % 2 == 0 && e.freeze > 90)||
                    (e.freeze <= 90 && (Globals.GlobalTime / 10 ) % 2 == 0)))
                {
                    e.Draw_Froze(sb);
                }
            }

            foreach (MapSpawner spawn in spawners)
                spawn.Draw(sb);

            if(Dead_reset == 0 || (Dead_reset / 16) % 2 == 0 )
            {
                player.Draw(sb);
            }


            if (E_Life != null)
                E_Life.Draw(sb);
            if (bonus != null)
                bonus.Draw(sb);

            foreach (PopupPoints p in points)
                p.Draw(sb);
        }
    }
}
