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
        static Image image_horz;
        static Image image_vert;
        
        int currentTime;
        public LevelData myData;

        bool timePause;

        public Player player;
        public List<Note> allNotes;
        List<NoteProjectile> NoteProj;
        public List<Enemy> AllEnemies;
        public List<PopupPoints> points;

        public List<MapSpawner> spawners;
        

        PlayScreen screen;

        public bool paused;

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

            player = new Player(new Point(data.PlayerSpawn.X * Globals.stdTile, data.PlayerSpawn.Y * Globals.stdTile));

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

        public void Update()
        {
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
                    AllEnemies[j].Update(this);
                bool hitEnemy = false;
                if (!player.isGhost)
                {
                    foreach (Enemy e in AllEnemies)
                    {
                        if (Entity.EntitiesIntersect_Rect(e, player, 8 * 16))
                            hitEnemy = true;
                    }
                    if (hitEnemy)
                        HitEnemy();
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
                    player = new Player(new Point(myData.PlayerSpawn.X * Globals.stdTile, myData.PlayerSpawn.Y * Globals.stdTile));
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

        public void HitEnemy()
        {
            paused = true;
            Dead_reset = 96;
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
                e.Draw(sb);

            foreach (MapSpawner spawn in spawners)
                spawn.Draw(sb);

            if(Dead_reset == 0 || (Dead_reset / 16) % 2 == 0 )
            {
                player.Draw(sb);
            }

            foreach (PopupPoints p in points)
                p.Draw(sb);
        }
    }
}
