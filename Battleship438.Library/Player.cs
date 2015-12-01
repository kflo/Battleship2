using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship438.Library
{
     public class Player
     {
          public string Name { get; set; }
          public int X { get; set; }
          public int Y { get; set; }
          //public NetConnection Connection { get; set; }
          
          public Player(string name, int x, int y)
          {
               Name = name;
               X = x;
               Y = y;
          }

          public Player() { }
     }
}
