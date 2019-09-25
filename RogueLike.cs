using System;
using System.Collections.Generic;
using System.Threading;
using MapNS;
using PlayerNS;

class Program
{
    static void Main()
  {  
    Map map = new Map();
    Player player = new Player(20);
    
    map.DrawMap(player.GetPosition()); 
    player.DrawHealth();

    ConsoleKeyInfo cki = Console.ReadKey(true);
   
    while (cki.KeyChar != 'q')
    {
        cki = Console.ReadKey(true);
        if(!Console.KeyAvailable)
        {
            map.MovePlayer(cki.KeyChar);
            player.SetPosition(map.GetPlayerPosition());
            player.DrawHealth();
        }
        
    }        
   }
 }

