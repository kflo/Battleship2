using System;
using Battleship438Game;
using Battleship438Game.Network;

namespace Battleship438GameCLIENT
{
     public static class ProgramClient
     {
          [STAThread]
          private static void Main()
          {
               using (var game = new BattleshipGame(new ClientNetworkManager()))
                    game.Run();
          }
     }
}
