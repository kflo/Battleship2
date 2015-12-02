// ReSharper disable once CheckNamespace
namespace Battleship438.Server
{
     using System;
     using System.Threading;
     using Battleship438;
     using Battleship438.Network;
     internal class Program
     {
          private static void Main(string[] args)
          {
               using (var game = new BattleshipGame(new ServerNetworkManager()))
               {
                    game.Run();
               }
          }
     }
}
