using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438.Model
{
     /// A Ship has all the details about itself. For example the shipname,
     /// size, number of hits taken and the location. Its able to add tiles,
     /// remove, hits taken and if its deployed and destroyed.
     /// 
     /// Deployment information is supplied to allow ships to be drawn.
     public class Ship
     {
          private ShipName _shipName;
          private int _sizeOfShip;
          private int _hitsTaken = 0;
          private List<Tile> _tiles;
          private int _row;
          private int _col;
          private Direction _direction;

          /// SHIP CONSTRUCTOR
          /// <param name="ship"></param>
          public Ship(ShipName shipName)
          {
               _shipName = shipName;
               _tiles = new List<Tile>();
               //gets the ship size from the enumerator
               _sizeOfShip = (int)_shipName;
          }

          /// The type of ship
          public string Name     {
               get { return _shipName.ToString(); }
          }

          /// The number of cells that this ship occupies.
          public int Size     {
               get { return _sizeOfShip; }
          }

          /// The number of hits that the ship has taken.
          public int Hits     {
               get { return _hitsTaken; }
          }

          /// The row location of the ship
          public int Row     {
               get { return _row; }
          }

          /// The column location of the ship
          public int Column     {
               get { return _col; }
          }

          /// The direction of the ship; L-to-R or U-and-D
          public Direction Direction     {
               get { return _direction; }
          }

          /// Add tile adds the ship tile
          public void AddTile(Tile tile)     {
               _tiles.Add(tile);
          }

          /// Remove clears the tile back to a sea tile
          public void Remove() {
               _hitsTaken = 0;
               foreach (Tile tile in _tiles) {
                    tile.ClearShip();
               }
               _tiles.Clear();
          }

          /// increments HIT counter if ship is hit
          public void Hit()     {
               _hitsTaken = _hitsTaken + 1;
          }

          /// IsDeployed returns if the ships is deployed, if its deplyed it has more than 0 tiles
          public bool IsDeployed     {
               get { return _tiles.Count > 0; }
          }

          /// ship destroyed when HIT counter is equal to its SIZE
          public bool IsDestroyed     {
               get { return Hits == Size; }
          }

          /// Record that the ship is now deployed.
          internal void Deployed(Direction direction, int row, int col)     {
               _row = row;
               _col = col;
               _direction = direction;
          }


          public void texturize(Texture2D tex) {
               foreach (Tile tile in _tiles)
                    if (!tile.Shot)
                         tile.Texture = tex; 
          }

     }
}
