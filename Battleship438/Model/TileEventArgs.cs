using System;

namespace Battleship438Game.Model
{
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
   
     public class TileEventArgs : EventArgs
     {
          public int X;
          public int Y;
          public TileEventArgs(int x, int y)
          {
               X = x;
               Y = y;
          }
     }
}