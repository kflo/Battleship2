using System;
using System.Threading;
using Battleship438Game;
using Battleship438Game.Network;

namespace Client
{
     public static class Program
     {
          [STAThread]
          private static void Main() {
               Thread.Sleep(1000);
               using (var game = new BattleshipGame()){
                    game.Run();
               }
          }
     }
}
