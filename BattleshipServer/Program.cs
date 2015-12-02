using Battleship438Game;
using Battleship438Game.Network;

namespace Server
{
     internal class Program
     {
          private static void Main(string[] args) {
               using (var game = new BattleshipGame()){
                    game.Run();
               }
          }
     }
}
