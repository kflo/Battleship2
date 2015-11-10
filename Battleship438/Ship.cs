using System.Collections.Generic;

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
     /// <value>The type of ship</value>
     /// <returns>The type of ship</returns>
     public string Name     {
          get { return _shipName.ToString(); }
     }

     /// The number of cells that this ship occupies.
     /// <value>The number of hits the ship can take</value>
     /// <returns>The number of hits the ship can take</returns>
     public int Size     {
          get { return _sizeOfShip; }
     }

     /// The number of hits that the ship has taken.
     /// <value>The number of hits the ship has taken.</value>
     /// <returns>The number of hits the ship has taken</returns>
     /// <remarks>When this equals Size the ship is sunk</remarks>
     public int Hits     {
          get { return _hitsTaken; }
     }

     /// The row location of the ship
     /// <value>The topmost location of the ship</value>
     /// <returns>the row of the ship</returns>
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
     /// <param name="tile">one of the tiles the ship is on</param>
     public void AddTile(Tile tile)     {
          _tiles.Add(tile);
     }

     /// Remove clears the tile back to a sea tile
     public void Remove()     {
          foreach (Tile tile in _tiles)
          {
               tile.ClearShip();
          }
          _tiles.Clear();
     }

     /// increments HIT counter if ship is hit
     public void Hit()     {
          _hitsTaken = _hitsTaken + 1;
     }

     /// IsDeployed returns if the ships is deployed, 
     /// if its deplyed it has more than 0 tiles
     public bool IsDeployed     {
          get { return _tiles.Count > 0; }
     }

     /// ship destroyed when HIT counter is equal to its SIZE
     public bool IsDestroyed     {
          get { return Hits == Size; }
     }

     /// Record that the ship is now deployed.
     /// <param name="direction"></param>
     /// <param name="row"></param>
     /// <param name="col"></param>
     internal void Deployed(Direction direction, int row, int col)
     {
          _row = row;
          _col = col;
          _direction = direction;
     }
}
