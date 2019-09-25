using System;
using System.Threading;

namespace MapNS
{
    public class Map
    {
        const char FLOOR = (char)9617;
        const char WALL = (char)9608;
        const char FOG = (char)9619;
        const char SPIKEPIT = (char)5775;
        private bool[,] _Fog;
        private char[,] _Map;
        private int[] playerPosition = {15,35};


        public Map()
        {
            int[] diggerStart = {15, 35};
            char[,] map = new char[30,71];
            bool[,] fog = new bool[30,71];
            Console.Clear();
            for (int i=0; i<30; i++)
            {
                for(int j=0;j<70; j++)
                {
                    map[i,j] = WALL;
                    fog[i,j] = true;
                }
                map[i,70] = '\n';
            }
            Digger(map, diggerStart, 1500);
            _Fog = fog;
            _Map = map;
            CleanMap(7);
            //CleanMap(8);
            AddSpikePits();
        }
        public void DrawMap(int[] playerPosition) 
        {
            Thread.Sleep(5);
            Console.Clear();
            string mapString = "\r";
            _Map[playerPosition[0], playerPosition[1]] = (char)9792;
            _Fog[playerPosition[0], playerPosition[1]] = false;
            for(int i=-4; i<=4; i++)
            {
                for(int j=-4; j<=4; j++)
                {
                    //relative to player position
                    if (Math.Abs(i) + Math.Abs(j) <= 4)
                    {
                        if((playerPosition[0]+ i >= 0 && playerPosition[0]+i < _Map.GetLength(0)) && (playerPosition[1]+j >= 0 && playerPosition[1]+j < _Map.GetLength(1)-1)) {
                        _Fog[playerPosition[0]+i, playerPosition[1]+j] = false;
                        }
                    }
                    // if(playerPosition[0]+i > 0 && playerPosition[0]+i < _Map.GetLength(0)-1)
                    // {
                    //     _Fog[playerPosition[0]+i, playerPosition[1]] = false;
                    // }
                    
                    //  if(playerPosition[1]+i > 0 && playerPosition[1]+i < _Map.GetLength(1)-1)
                    // {
                    //     _Fog[playerPosition[0], playerPosition[1]+i] = false;
                    // }
                }
            }
            for(int i=0; i<30; i++)
            {
                for(int j=0; j< 71; j++)
                {
                    //Console.Write(map[i,j]);
                    if(_Fog[i,j])
                    {
                        mapString += FOG;
                    } else{
                        mapString += _Map[i,j];
                    }
                }
            }
            Console.WriteLine(mapString);
        }
        private int CountNeighbors(int i, int j)
        {
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
            return neighbors;
        }
        private void AddSpikePits()
        {
            char[,] newMap = new char[30,71];
            newMap[0,70] = '\n';
            for( int i=1; i<29; i++)
            {
                for(int j=1; j<70; j++)
                {  
                    if (_Map[i,j] == FLOOR){
                        newMap[i,j] = FLOOR;
                        continue;
                    }
                    int neighbors = CountNeighbors(i,j);
                    if(neighbors == 8)
                    {
                        newMap[i,j] = SPIKEPIT;
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
        private void CleanMap(int neighborLimit)
        {
            char[,] newMap = new char[30,71];
            newMap[0, 70] = '\n';
            for( int i=1; i<29; i++)
            {
                for(int j=1; j<70; j++)
                {
                    if (_Map[i,j] == FLOOR){
                        newMap[i,j] = FLOOR;
                        continue;
                    }
                    int neighbors = CountNeighbors(i,j);
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
        
        public void MovePlayer(char input)
        {
             if(input == 'a') 
            {
                _Map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[1]-1 >= 0 && _Map[playerPosition[0], playerPosition[1]-1] == FLOOR) 
                {
                    playerPosition[1] -= 1;
                }
                
                DrawMap(playerPosition);
            }
            if(input == 'd') 
            {
                _Map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[1]+1 < _Map.GetLength(1)-1 && _Map[playerPosition[0], playerPosition[1]+1] == FLOOR) 
                {
                    playerPosition[1] += 1;
                }
                
                DrawMap(playerPosition);
            }
            if(input == 'w') 
            {
                _Map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[0]-1 >= 0&& _Map[playerPosition[0]-1, playerPosition[1]] == FLOOR) 
                {
                    playerPosition[0] -= 1;
                }
                
                DrawMap(playerPosition);
            }
            if(input == 's') 
            {
                _Map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[0]+1 < _Map.GetLength(0)&& _Map[playerPosition[0]+1, playerPosition[1]] == FLOOR) 
                {
                    playerPosition[0] += 1;
                }
                
                DrawMap(playerPosition);
            }
        }

        public int[] GetPlayerPosition()
        {
            return playerPosition;
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