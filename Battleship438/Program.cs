using System;

namespace Battleship438
{
#if WINDOWS || LINUX
     /// The main class.
     public static class Program
     {
          /// The main entry point for the application.
          [STAThread]
          static void Main()
          {
               using (var game = new BattleshipGame())
                    game.Run();
          }
     }
#endif
}
