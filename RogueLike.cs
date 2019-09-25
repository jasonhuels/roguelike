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
        if(!Console.KeyAvailable)
        {
            map.MovePlayer(cki.KeyChar);
        }
    }        
   }
 }

