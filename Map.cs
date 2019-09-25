using System;
using System.Threading;

namespace MapNS
{
    public class Map
    {
        const char FLOOR = (char)9617;
        const char WALL = (char)9608;
        private char[,] _Map;

        public Map()
        {
            int[] diggerStart = {15, 35};
            char[,] map = new char[30,71];
            Console.Clear();
            for (int i=0; i<30; i++)
            {
                for(int j=0;j<70; j++)
                {
                    map[i,j] = (char)9608;
                }
                map[i,70] = '\n'; 
            }
            Digger(map, diggerStart, 1500);
            _Map = map;
            CleanMap(7);
            CleanMap(8);
            _Map = map;
        }

        public void DrawMap(int[] playerPosition) 
        {
            Thread.Sleep(5);
            //Console.Clear();
            string mapString = "\r";
            
            _Map[playerPosition[0], playerPosition[1]] = (char)9792;
            //move to draw function
            for(int i=0; i<30; i++)
            {
                for(int j=0; j< 71; j++)
                {
                    //Console.Write(map[i,j]);
                    mapString += _Map[i,j];
                }
            }
            Console.WriteLine(mapString);
        }

        private void CleanMap(int neighborLimit)
        {
            char[,] newMap = new char[30,71];
            for( int i=1; i<29; i++)
            {
                for(int j=1; j<70; j++)
                {
                    if (_Map[i,j] == FLOOR){
                        newMap[i,j] = FLOOR;
                        continue;
                    }
                    //calculate neighbors
                    int neighbors = 0;
                    if (_Map[i-1,j] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i-1,j+1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i,j+1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i+1,j+1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i+1,j] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i+1,j-1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i,j-1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(_Map[i-1,j-1] == FLOOR)
                    {
                        neighbors++;
                    }
                    if(neighbors >=neighborLimit)
                    {
                        newMap[i,j] = FLOOR;
                    } else
                    {
                        newMap[i,j] = WALL;
                    }
                }
                newMap[i,70] = '\n';
            }
            for(int i=0; i<69; i++)
            {
                newMap[29,i] = WALL;
            }
            newMap[29, 70] = '\n';
            _Map = newMap;
        }

        private void Digger(char[,] map, int[] position, int lifeSpan) 
        {
            int [] myPos = position;
            // //pick a random direction
            Random rand = new Random();
            int dir = rand.Next(1,5);
            //pick a number of steps to go
            int dist = rand.Next(3,11);
            if(dir == 1)
            {
                //up
                if(myPos[0] - dist > 1)
                {
                    for(int i=0; i<dist;i++)
                    {
                        map[myPos[0],myPos[1]]= FLOOR;
                        myPos[0]--;
                    }
                    lifeSpan -= dist;
                    if (lifeSpan > 0)
                    {
                        Digger(map,myPos,lifeSpan);
                    }
                }
                else
                {
                Digger(map,myPos,lifeSpan);
                return;
                }
            } 
            else if(dir == 2) 
            {
                //right
                if(myPos[1] + dist < 69)
                {
                    for(int i=0; i<dist;i++)
                    {
                        map[myPos[0],myPos[1]]= FLOOR;
                        myPos[1]++;
                    }
                    lifeSpan -= dist;
                    if (lifeSpan > 0)
                    {
                        Digger(map,myPos,lifeSpan);
                    }
                }
                else
                {
                Digger(map,myPos,lifeSpan);
                return;
                }
            } 
            else if(dir == 3)
            {
                // down
                if(myPos[0] + dist < 29)
                {
                    for(int i=0; i<dist;i++)
                    {
                        map[myPos[0],myPos[1]]= FLOOR;
                        myPos[0]++;
                    }
                    lifeSpan -= dist;
                    if (lifeSpan > 0)
                    {
                        Digger(map,myPos,lifeSpan);
                    }
                }
                else
                {
                Digger(map,myPos,lifeSpan);
                return;
                }
                
            } 
            else 
            {
                //left
                if(myPos[1] - dist > 1)
                {
                    for(int i=0; i<dist;i++)
                    {
                        map[myPos[0],myPos[1]]= FLOOR;
                        myPos[1]--;
                    }
                    lifeSpan -= dist;
                    if (lifeSpan > 0)
                    {
                        Digger(map,myPos,lifeSpan);
                    }
                }
                else
                {
                Digger(map,myPos,lifeSpan);
                return;
                }
            }
        }
    }
}