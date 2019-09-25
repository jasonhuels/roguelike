using System;
using System.Collections.Generic;
using System.Threading;
using MapNS;

class Program
{
   

    static void Main()
  {  
    Map map = new Map();
    
    int[] playerPosition = {15, 35};
    
    map.DrawMap(playerPosition);  

    ConsoleKeyInfo cki = Console.ReadKey(true);
   
    while (cki.KeyChar != 'q')
        {
            //Thread.Sleep(250); // Loop until input is entered.
            cki = Console.ReadKey(true);
            if(!Console.KeyAvailable && cki.KeyChar == 'a') 
            {
                map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[1]-1 >= 0 && map[playerPosition[0], playerPosition[1]-1] == FLOOR) 
                {
                    playerPosition[1] -= 1;
                }
                
                map.DrawMap(playerPosition);
            }
            if(!Console.KeyAvailable && cki.KeyChar == 'd') 
            {
                map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[1]+1 < map.GetLength(1)-1 && map[playerPosition[0], playerPosition[1]+1] == FLOOR) 
                {
                    playerPosition[1] += 1;
                }
                
                map.DrawMap(playerPosition);
            }
            if(!Console.KeyAvailable && cki.KeyChar == 'w') 
            {
                map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[0]-1 >= 0&& map[playerPosition[0]-1, playerPosition[1]] == FLOOR) 
                {
                    playerPosition[0] -= 1;
                }
                
                map.DrawMap(playerPosition);
            }
            if(!Console.KeyAvailable && cki.KeyChar == 's') 
            {
                map[playerPosition[0], playerPosition[1]] = FLOOR;
                if(playerPosition[0]+1 < map.GetLength(0)&& map[playerPosition[0]+1, playerPosition[1]] == FLOOR) 
                {
                    playerPosition[0] += 1;
                }
                
                map.DrawMap(playerPosition);
            }
        }
  }

  
}
