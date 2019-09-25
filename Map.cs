using System;
using System.Threading;

namespace MapNS
{
    public class Map
    {
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
        
        public void MovePlayer(char input)
        {
             if(input == 'a') 
            {
                char leftCheck = _Map[playerPosition[0], playerPosition[1]-1];
                _Map[playerPosition[0], playerPosition[1]] = LastSquare;
                if(playerPosition[1]-1 >= 0)
                {
                    if(leftCheck == EXIT && _DoorOpen)
                    {
                        //move to next map
                    } else if (leftCheck == EXIT || leftCheck == FLOOR)
                    {
                        playerPosition[1] -= 1;
                    } else if(leftCheck == ENEMY) 
                    {
                        _EnemyCount--;
                        if(_EnemyCount == 0)
                        {
                            _Map[playerPosition[0], playerPosition[1]-1] = KEY;
                        } else 
                        {
                            _Map[playerPosition[0], playerPosition[1]-1] = FLOOR;
                        } 
                    } else if (leftCheck == SPIKEPIT)
                    {
                        //add code
                    } else if (leftCheck == KEY)
                    {
                        _DoorOpen = true;
                        _Map[playerPosition[0], playerPosition[1]-1] = FLOOR;
                        playerPosition[1] -= 1;
                    }
                }  
            }
            if(input == 'd') 
            {
                char rightCheck =_Map[playerPosition[0], playerPosition[1]+1];
                _Map[playerPosition[0], playerPosition[1]] = LastSquare;
                if(playerPosition[1]+1 < _Map.GetLength(1)-1)
                {
                    if( rightCheck == EXIT && _DoorOpen)
                    {
                        //move to next map
                    } else if (rightCheck == EXIT || rightCheck == FLOOR)
                    {
                        playerPosition[1] += 1; 
                    } else if (rightCheck == ENEMY)
                    {
                        _EnemyCount--;
                        if(_EnemyCount == 0)
                        {
                            _Map[playerPosition[0], playerPosition[1]+1] = KEY;
                        } else 
                        {
                            _Map[playerPosition[0], playerPosition[1]+1] = FLOOR;
                        }
                    } else if (rightCheck == SPIKEPIT)
                    {
                        //add code
                    } else if (rightCheck == KEY)
                    {
                        _DoorOpen = true;
                        _Map[playerPosition[0], playerPosition[1]+1] = FLOOR;
                        playerPosition[1] += 1;
                    }
                }     
            }
            if(input == 'w') 
            {
                char upCheck = _Map[playerPosition[0]-1, playerPosition[1]];
                _Map[playerPosition[0], playerPosition[1]] = LastSquare;
                if(playerPosition[0]-1 >= 0 && (upCheck == FLOOR || upCheck == EXIT)) 
                {
                    playerPosition[0] -= 1;
                } 
                else if(upCheck == ENEMY)
                {
                    _EnemyCount--;
                    if(_EnemyCount == 0)
                    {
                        _Map[playerPosition[0]-1, playerPosition[1]] = KEY;
                    }
                    else 
                    {
                         _Map[playerPosition[0]-1, playerPosition[1]] = FLOOR;
                    }
                    
                }
            }
            if(input == 's') 
            {
                char downCheck = _Map[playerPosition[0]+1, playerPosition[1]];
                _Map[playerPosition[0], playerPosition[1]] = LastSquare;
                if(playerPosition[0]+1 < _Map.GetLength(0) && (downCheck == FLOOR || downCheck == EXIT))
                {
                    playerPosition[0] += 1;
                } 
                else if (downCheck == ENEMY)
                {
                    _EnemyCount--;
                    if(_EnemyCount == 0)
                    {
                        _Map[playerPosition[0]+1, playerPosition[1]] = KEY;
                    }
                    else 
                    {
                         _Map[playerPosition[0]+1, playerPosition[1]] = FLOOR;
                    }
                   
                }
            }
            LastSquare = _Map[playerPosition[0], playerPosition[1]];
            DrawMap(playerPosition);
        }

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
                            _Map[i, j] = ENEMY;
                            _EnemyCount++;
                        }
                    }
                }
            }
        }
    }
}