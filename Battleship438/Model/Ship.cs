using System.Collections.Generic;
using Battleship438Game.Model.Enum;

namespace Battleship438Game.Model
{
     /// A Ship has all the details about itself: shipname, size, # of hits, and location. Its able to add tiles, remove, 
     /// hits taken and if its deployed and destroyed. Deployment information is supplied to allow ships to be drawn.
     public class Ship
     {

          private readonly ShipName _shipName;
          private readonly List<Tile> _tiles;

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public Ship(ShipName shipName){
               _shipName = shipName;
               _tiles = new List<Tile>();
               Size = (int)_shipName;
          }

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public ShipName Name => _shipName;

          public int Size { get; }

          public int Hits { get; private set; }

          public int Row { get; private set; }

          public int Column { get; private set; }

          public Direction Direction { get; private set; }

          public bool IsDeployed => _tiles.Count > 0;

          public bool IsDestroyed => Hits == Size;

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void AddTile(Tile tile) {
               _tiles.Add(tile);
          }

          public void Remove() {
               Hits = 0;
               foreach (Tile tile in _tiles)
                    tile.ClearShip();
               _tiles.Clear();
          }

          public void Hit() {
               Hits = Hits + 1;
          }
          
          internal void Deployed(Direction direction, int row, int col) {
               Row = row;
               Column = col;
               Direction = direction;
          }

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     }
}
