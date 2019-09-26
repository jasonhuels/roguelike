using System;
using System.Threading;
using PlayerNS;
using EnemyNS;

namespace MapNS
{
    public class Map
    {
        const int HEIGHT = 30;
        const int WIDTH = 70;
        const char FLOOR = (char)9617;
        const char WALL = (char)9608;
        const char FOG = (char)9619;
        const char EXIT = (char)9636;
        const char SPIKEPIT = (char)5774;
        const char ENEMY = (char)8621;
        private bool[,] _Fog;
        private char[,] _Map;
        private int[] playerPosition = {15,35};
        private char KEY = (char)8613;
        private char LastSquare = FLOOR;
        private int _EnemyCount = 0;
        private bool _DoorOpen = false;
        private Enemy[] enemies;
        public Map()
        {
            int[] diggerStart = {15, 35};
            char[,] map = new char[30,71];
            bool[,] fog = new bool[30,71];
            LastSquare = FLOOR;
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
            CleanMap(4);
            AddSpikePits();
            PopulateMap();
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
                    if (Math.Abs(i) + Math.Abs(j) <= 4)
                    {
                        if((playerPosition[0]+ i >= 0 && playerPosition[0]+i < _Map.GetLength(0)) && (playerPosition[1]+j >= 0 && playerPosition[1]+j < _Map.GetLength(1)-1)) {
                        _Fog[playerPosition[0]+i, playerPosition[1]+j] = false;
                        }
                    }
                }
            }
            for(int i=0; i<30; i++)
            {
                for(int j=0; j< 71; j++)
                {
                    if(_Fog[i,j])
                    {
                        //mapString += FOG;
                        mapString += _Map[i,j];         
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
                    if(neighbors == 8 && _Map[i,j] != EXIT)
                    {
                        newMap[i,j] = SPIKEPIT;
                    } else if(_Map[i,j] != EXIT)
                    {
                        newMap[i,j] = WALL;
                    }else if(_Map[i,j] == EXIT)
                    {
                        newMap[i,j] = EXIT;
                    }
                }
                newMap[i,70] = '\n';
            }
            for(int i=0; i<=69; i++)
            {
                newMap[0, i] = WALL;
                newMap[29,i] = WALL;
            }
            for(int j=0; j<29; j++)
            {
                newMap[j, 0] = WALL;
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
                    if(neighbors >=neighborLimit && _Map[i,j] != EXIT)
                    {
                        newMap[i,j] = FLOOR;
                    } else if(_Map[i,j] != EXIT)
                    {
                        newMap[i,j] = WALL;
                    } else if(_Map[i,j] == EXIT)
                    {
                        newMap[i,j] = EXIT;
                    }
                }
                newMap[i,70] = '\n';
            }
            for(int i=0; i<=69; i++)
            {
                newMap[29,i] = WALL;
            }
            newMap[29, 70] = '\n';
            _Map = newMap;
        }
        
        public int[] CheckMove(char input, int[] startingPosition)
        {   
            int moveX = 0;
            int moveY = 0;
            int[] outputPosition = startingPosition;
            char myChar = _Map[startingPosition[0], startingPosition[1]];
            switch(input)
            {
                case 'w':
                    moveX = -1;
                    break;
                case 'a':
                    moveY = -1;
                    break;
              
                case 's':
                    moveX = 1;
                    break;
                case 'd':
                    moveY = 1;
                    break;
            }
            int [] nextPos = {startingPosition[0] + moveX, startingPosition[1] + moveY};
            char nextChar = _Map[nextPos[0],nextPos[1]];
             if(nextPos[0] >= 1 && nextPos[0] < HEIGHT - 1 && nextPos[1] >= 1 && nextPos[1] < WIDTH -1)
            {
                if( nextChar == EXIT && _DoorOpen)
                {
                    //go to next level
                } else if (nextChar == EXIT || nextChar == FLOOR )
                {
                    LastSquare = nextChar;
                    _Map[startingPosition[0],startingPosition[1]] = FLOOR; //makes door to floor
                    outputPosition = nextPos;
                    playerPosition = nextPos;
                    _Map[startingPosition[0],startingPosition[1]] = LastSquare;
                } else if (nextChar == ENEMY)
                {
                    _EnemyCount--;
                    if (_EnemyCount == 0)
                    {
                        _Map[nextPos[0],nextPos[1]] = KEY;
                    } else
                    {
                        nextChar = FLOOR;
                        _Map[startingPosition[0],startingPosition[1]] = FLOOR;
                        outputPosition = nextPos;
                        playerPosition = nextPos;
                    }
                } else if ( nextChar == SPIKEPIT)
                {
                    //player.ReduceHealth();

                } else if( nextChar == KEY)
                {
                    _DoorOpen = true;
                    _Map[startingPosition[0],startingPosition[1]] = FLOOR;
                    outputPosition = nextPos;
                    playerPosition = nextPos;
                }                
            }
            DrawMap(playerPosition);
            return outputPosition;
        }
        // public void MovePlayer(char input)
        // {
        //     int moveX = 0;
        //     int moveY = 0;
        //     switch(input)
        //     {
        //         case 'w':
        //             moveX = -1;
        //             break;
        //         case 'a':
        //             moveY = -1;
        //             break;
              
        //         case 's':
        //             moveX = 1;
        //             break;
        //         case 'd':
        //             moveY = 1;
        //             break;
        //     }
        //     int [] startingPos = {playerPosition[0], playerPosition[1]};
        //     int [] nextPos = {playerPosition[0] + moveX, playerPosition[1] + moveY};
        //     char nextChar = _Map[nextPos[0],nextPos[1]];
        //     if(nextPos[0] >= 1 && nextPos[0] < HEIGHT - 1 && nextPos[1] >= 1 && nextPos[1] < WIDTH -1)
        //     {
        //         if( nextChar == EXIT && _DoorOpen)
        //         {
                    
        //         } else if (nextChar == EXIT || nextChar == FLOOR )
        //         {
        //             LastSquare = nextChar;
        //             _Map[startingPos[0],startingPos[1]] = FLOOR; //makes door to floor
        //             playerPosition = nextPos;
        //             _Map[startingPos[0],startingPos[1]] = LastSquare;
        //         } else if (nextChar == ENEMY)
        //         {
        //             _EnemyCount--;
        //             if (_EnemyCount == 0)
        //             {
        //                 _Map[nextPos[0],nextPos[1]] = KEY;
        //             } else
        //             {
        //                 nextChar = FLOOR;
        //                 _Map[startingPos[0],startingPos[1]] = FLOOR;
        //                 playerPosition = nextPos;
        //             }
        //         } else if ( nextChar == SPIKEPIT)
        //         {
        //             //player.ReduceHealth();

        //         } else if( nextChar == KEY)
        //         {
        //             _DoorOpen = true;
        //             _Map[startingPos[0],startingPos[1]] = FLOOR;
        //             playerPosition = nextPos;
        //         }                
        //     }
        //     DrawMap(playerPosition);
        // }

        public int[] GetPlayerPosition()
        {
            return playerPosition;
        }

        private void Digger(char[,] map, int[] position, int lifeSpan) 
        {
            int [] myPos = position;
            Random rand = new Random();
            int dir = rand.Next(1,5);
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
                }
            }
            if(lifeSpan <= 10)
            {
                map[myPos[0], myPos[1]] = EXIT;
                lifeSpan = 0;
            }
        }

        private void PopulateMap()
        {
            Random rand = new Random();
            int chance;
            for(int i=0; i<_Map.GetLength(0)-1; i++)
            {
                for(int j=0; j<_Map.GetLength(1)-1; j++)
                {
                    if(_Map[i,j] == FLOOR) {
                        chance = rand.Next(1,101);
                        if(chance == 1) 
                        {
                            int[] pos = {i, j};
                            Enemy enemy = new Enemy(_EnemyCount, 5, pos);
                            _Map[i, j] = ENEMY;
                            _EnemyCount++;
                        }
                    }
                }
            }
        }
    }
}