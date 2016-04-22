using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NCodeRiddian;

namespace RootToot
{
    class MapSpawner
    {
        public int current;

        public Spawner spawn;
        List<int> Queue;
        List<DeadQueue> Respawns;
        int cooldown;

        Image spawnoutline;
        AnimatedImage spawnanim;
        bool animRunning;
        bool canSpawn;
        Map myMap;

        public MapSpawner(Spawner spawn, Map m)
        {
            this.spawn = spawn;
            Queue = new List<int>();
            cooldown = 0;
            spawnoutline = new Image("box");
            spawnanim = new AnimatedImage("spawncirc", Globals.StdFrame, 16, 4);
            animRunning = false;
            myMap = m;
            spawnanim.onFinishAction += OnAnimFinish;
            Respawns = new List<DeadQueue>();
            canSpawn = true;
        }

        public void reset()
        {
            Queue.Clear();
            Respawns.Clear();
            animRunning = false;
            spawnanim.reset();
            canSpawn = true;
        }

        public void OnAnimFinish(AnimatedImage image)
        {
            switch(current)
            {
                case 1:
                    myMap.AllEnemies.Add(new Enemy1(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
                case 2:
                    myMap.AllEnemies.Add(new Enemy2(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
                case 3:
                    myMap.AllEnemies.Add(new Enemy3(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
                case 4:
                    myMap.AllEnemies.Add(new Enemy4(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
                case 5:
                    myMap.AllEnemies.Add(new Enemy5(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
                case 6:
                    myMap.AllEnemies.Add(new Enemy6(spawn.SpawnOrigin, myMap, myMap.player));
                    break;
            }
            animRunning = false;
            canSpawn = true;
            image.reset();
        }

        public void AddToQueue(int i)
        {
            Queue.Add(i);
        }

        public void AddDeadCreep(int type, int time)
        {
            Respawns.Add(new DeadQueue(type, time));
        }

        public void Update(Map m)
        {
            for(int i = Respawns.Count - 1; i >= 0; i--)
            {
                Respawns[i].time--;
                if(Respawns[i].time == 0)
                {
                    AddToQueue(Respawns[i].type);
                    Respawns.RemoveAt(i);
                }
            }

            if (animRunning)
            {
                spawnanim.applyStep();
            }

            if (canSpawn)
            {
                if(Queue.Count > 0)
                {
                    current = Queue[0];
                    Queue.RemoveAt(0);
                    animRunning = true;
                    canSpawn = false;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Camera.draw(sb, spawnoutline, new Rectangle(spawn.SpawnOrigin.X, spawn.SpawnOrigin.Y, Globals.stdTile, Globals.stdTile));
            if(animRunning)
                Camera.draw(sb, spawnanim, new Rectangle(spawn.SpawnOrigin.X, spawn.SpawnOrigin.Y, Globals.stdTile, Globals.stdTile));
        }
    }

    class DeadQueue
    {
        public int type;
        public int time;
        public DeadQueue(int type, int time)
        {
            this.type = type;
            this.time = time;
        }
    }
}
