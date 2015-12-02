﻿using System;
using Battleship438Game.Network;

namespace Battleship438Game
{
     public static class Program
     {
          [STAThread]
          private static void Main()
          {
               using (var game = new BattleshipGame(new ServerNetworkManager()))
                    game.Run();
          }
     }
}
