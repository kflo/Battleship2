namespace Battleship438.Client
{
     using System;
     using System.Threading;
     using Battleship438;
     using Battleship438.Network;

     internal class Program
     {
          private static void Main(string[] args) {
               Thread.Sleep(1000);
               using (var game = new BattleshipGame(new ClientNetworkManager())){
                    game.Run();
               }
          }
     }
}
