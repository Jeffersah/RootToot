﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLoader;
using Microsoft.Xna.Framework;
using NCodeRiddian;
using Microsoft.Xna.Framework.Graphics;

namespace RootToot
{
    class LevelData
    {
        public static Image HorzMap;
        public static Image VertMap;

        public static List<LevelData> AllData;

        public static Image MapVertical()
        {
            return VertMap;
        }
        public static Image MapHorizontal()
        {
            return HorzMap;
        }

        public Image fromTex;

        public static void Load()
        {
            HorzMap = new Image("horz");
            VertMap = new Image("vert");
            PropertyObject complete = PropertyObject.Load("maps\\mapdata.txt");
            AllData = new List<LevelData>();
            foreach(PropertyObject o in complete.GetAllPropertyObjects())
            {
                AllData.Add(new LevelData(o));
            }
        }

        public int[,] Map;
        public bool[,] Notes;

        public List<Spawner> spawners = new List<Spawner>();
        public List<SpawnOrder> spawnOrders = new List<SpawnOrder>();
        public List<Point> espawn_possible;
        
        public Point PlayerSpawn;

        public LevelData(PropertyObject loadFrom)
        {
            espawn_possible = new List<Point>();

            string img = "maps\\" + loadFrom["fileloc"].value<string>();
            Texture2D colisdata = new Image(img).getTexture();
            fromTex = new Image(colisdata);
            Map = new int[14, 13];
            Notes = new bool[14, 13];
            PlayerSpawn = loadFrom["plrspawn"].value<Point>();
            foreach(PropertyDefinition obj in loadFrom.GetPropertyObject("spawnsqr").GetAllFields())
            {
                spawners.Add(new Spawner(obj));
            }
            foreach(PropertyObject obj in loadFrom.GetPropertyObject("spawns").GetAllPropertyObjects())
            {
                spawnOrders.Add(new SpawnOrder(obj));
            }

            Color[] data = new Color[14 * 13];
            colisdata.GetData<Color>(data);
            int x = 0;
            int y = 0;
            foreach(Color dat in data)
            {
                Map[x, y] = 0;
                if (dat.R == 255)
                {
                    Map[x, y] = 1;
                }
                if(dat.G == 255)
                {
                    Map[x, y] = 2;
                }

                if (dat.B == 200)
                {
                    Notes[x, y] = true;
                }
                else
                { 
                    Notes[x, y] = false;
                }

                x++;

                if(x >= 14)
                {
                    x = 0;
                    y++;
                }
            }

            for(int xi = 0; xi < 14; xi++)
            {
                for(int yi = 0; yi < 13; yi++)
                {
                    if(Map[xi, yi] != 0)
                    {
                        for(int d = 0; d < 4; d++)
                        {
                            int nx = Helper.adjx(xi, d);
                            int ny = Helper.adjy(yi, d);
                            if(nx >= 0 && nx < 14 && ny >= 0 && ny < 13 && Map[nx, ny] == 0)
                            {
                                espawn_possible.Add(new Point(nx * Globals.stdTile, ny * Globals.stdTile));
                            }
                        }
                    }
                }
            }
        }
    }

    class Spawner
    {
        public Point location;
        public string name;
        public Point SpawnOrigin;

        public Spawner(PropertyDefinition pd)
        {
            name = pd.name;
            location = (Point)pd.value.value<Point>();
            SpawnOrigin = new Point(location.X * 16, location.Y * 16);
            
        }
    }

    class SpawnOrder
    {
        public int type;
        public int time;
        public string from;
        public SpawnOrder(PropertyObject obj)
        {
            type = obj["type"].value<int>();
            time = obj["time"].value<int>();
            from = obj["from"].value<string>();
        }
    }
}
