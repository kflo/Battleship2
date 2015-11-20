using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

/// The SeaGrid is the grid upon which the ships are deployed.
/// 
/// The grid is viewable via the ISeaGrid interface as a read only
/// grid. This can be used in conjuncture with the SeaGridAdapter to 
/// mask the position of the ships.
namespace Battleship438
{
     public class SeaGrid : ISeaGrid
     {
          private Random rand;
          int tileSize = 30;
          private Rectangle rect, tileRect;
          private const int _WIDTH = 10;
          private const int _HEIGHT = 10;
          private Tile[,] _GameTiles;
          private Dictionary<ShipName, Ship> _shipList;
          private int _ShipsKilled = 0;
          private Texture2D red;


          /// SeaGrid constructor, a seagrid has a number of tiles stored in an array
          public SeaGrid(Dictionary<ShipName, Ship> ships, Vector2 vector, Texture2D tex)
          {
               rect = new Rectangle((int)vector.X, (int)vector.Y, tileSize * 10 - 20, tileSize * 10 - 20);
               _GameTiles = new Tile[_WIDTH, _HEIGHT];
               //fill array with empty Tiles
               for (int i = 0; i <= _WIDTH - 1; i++) {
                    for (int j = 0; j <= _HEIGHT - 1; j++) {
                         tileRect = new Rectangle(i * (tileSize - 2) + rect.X, j * (tileSize - 2) + rect.Y, tex.Width, tex.Height);
                         _GameTiles[i, j] = new Tile(i, j, null, tex, tileRect);
                         _GameTiles[i, j].Changed += tileChanged;
                    }
               }
               _shipList = ships;
          }

          //set Texture of ALL tiles in seaGrid
          public void texturize(Texture2D texture) {
               for (int i = 0; i <= _WIDTH - 1; i++) {
                    for (int j = 0; j <= _HEIGHT - 1; j++) {
                        _GameTiles[i, j].Texture = texture;
                    }
               }
          }

          /// randomly initializes the ships from the Dictionary
          public void Initialize(Texture2D texture) {
               red = texture;
               Direction heading = Direction.LeftRight;
               int row = 0;
               int col = 0;
               foreach (var item in _shipList) { //ITEM is the ship Dictionary pair (KEY = SHIPNAME, VALUE = SHIP)
                    randomize(ref heading, ref row, ref col, item.Value);
                    MoveShip(row, col, item.Key, heading, texture);
               }
          }

          private void randomize(ref Direction heading, ref int row, ref int col, Ship ship) {
               rand = new Random(System.DateTime.Now.Millisecond);
               int dir;
               int dRow;
               int dCol;
               int currRow;
               int currCol;
               bool blocked;
     repeat:
               blocked = false;
               dir = rand.Next(2);
               if (dir == 0) {
                    heading = Direction.UpDown;
                    col = rand.Next(10);
                    row = rand.Next(6);
               } else {
                    heading = Direction.LeftRight;
                    col = rand.Next(6);
                    row = rand.Next(10);
               }

               currRow = row;
               currCol = col;

               if (heading == Direction.LeftRight) {
                    dRow = 0;
                    dCol = 1;
               } else {
                    dRow = 1;
                    dCol = 0;
               }

               for (int j = 0; j < ship.Size; j++) {
                    if (_GameTiles[currRow, currCol].hasShip) { 
                         blocked = true;
                         break;
                    }
                    currCol += dCol;
                    currRow += dRow;
               }
               
               if (blocked) 
                    goto repeat;
          }

          /// The sea grid has changed and should be redrawn.
          
          public event EventHandler Changed;
          
          private void gridChanged(object sender, EventArgs e) {
               if (Changed != null) {
                    Changed(this, e);
               }
          }
          
          /// The width of the sea grid.
          /// <value>The width of the sea grid.</value>
          /// <returns>The width of the sea grid.</returns>
          public int Width {
               get { return _WIDTH; }
          }

          /// The height of the sea grid
          /// <value>The height of the sea grid</value>
          /// <returns>The height of the sea grid</returns>
          public int Height {
               get { return _HEIGHT; }
          }

          /// ShipsKilled returns the number of ships killed
          public int ShipsKilled {
               get { return _ShipsKilled; }
          }

          /// Show the tile view
          /// <param name="x">x coordinate of the tile</param>
          /// <param name="y">y coordiante of the tile</param>
          /// <returns>The TileView of the Tile item</returns>
          public TileView Item(int x, int y) {
               return _GameTiles[x, y].View;
          }

          /// AllDeployed checks if all the ships are deployed
          public bool AllDeployed {
               get {
                    foreach (Ship s in _shipList.Values) {
                         if (!s.IsDeployed) {
                              return false;
                         }
                    }
                    return true;
               }
          }

          /// MoveShips allows for ships to be placed on the seagrid
          /// <param name="row">the row selected</param>
          /// <param name="col">the column selected</param>
          /// <param name="ship">the ship selected</param>
          /// <param name="direction">the direction the ship is going</param>
          public void MoveShip(int row, int col, ShipName shipName, Direction direction, Texture2D texture) {
               Ship shipToMove = _shipList[shipName];
               shipToMove.Remove();
               AddShip(row, col, direction, shipToMove, texture);
          }

          /// AddShip add a ship to the SeaGrid
          /// <param name="row">row coordinate</param>
          /// <param name="col">col coordinate</param>
          /// <param name="direction">direction of ship</param>
          /// <param name="newShip">the ship</param>
          private void AddShip(int row, int col, Direction direction, Ship newShip, Texture2D texture) {
               try {
                    int size = newShip.Size;
                    int currentRow = row;
                    int currentCol = col;
                    int dRow = 0;
                    int dCol = 0;

                    if (direction == Direction.LeftRight) {
                         dRow = 0;
                         dCol = 1;
                    } else {
                         dRow = 1;
                         dCol = 0;
                    }
   
                    //place ship's tiles in array and into ship object
                         for (int i = 0; i < size; i++) {
                         if (currentRow < 0 | currentRow > _WIDTH | currentCol < 0 | currentCol > _HEIGHT) {
                              throw new InvalidOperationException("Ship can't fit on the board");
                         }
                         _GameTiles[currentRow, currentCol].Ship = newShip;
                         _GameTiles[currentRow, currentCol].Texture = texture;
                         currentCol += dCol;
                         currentRow += dRow;
                         
                    }
                    newShip.Deployed(direction, row, col);
               }
               catch (Exception e) {
                    newShip.Remove();
                    //if fails remove the ship
                    throw new ApplicationException(e.Message);
               }
               finally {
                    //grid_Changed();
               }
          }
                    

          /// HitTile hits a tile at a row/col, and whatever tile has been hit, a result will be displayed.
          /// <param name="row">the row at which is being shot</param>
          /// <param name="col">the cloumn at which is being shot</param>
          /// <returns>An attackResult (hit, miss, sunk, shotalready)</returns>
          public AttackResult HitTile(int row, int col) {
               try {
                    //tile is already hit
                    if (_GameTiles[row, col].Shot) {
                         return new AttackResult(ResultOfAttack.ShotAlready, "have already attacked [" + col + "," + row + "]!", row, col);
                    }

                    _GameTiles[row, col].Shoot();

                    //there is no ship on the tile
                    if (_GameTiles[row, col].Ship == null) {
                         return new AttackResult(ResultOfAttack.Miss, "missed", row, col);
                    }

                    //all ship's tiles have been destroyed
                    if (_GameTiles[row, col].Ship.IsDestroyed) {
                         _GameTiles[row, col].Shot = true;
                         _ShipsKilled += 1;
                         return new AttackResult(ResultOfAttack.Destroyed, _GameTiles[row, col].Ship, "destroyed the enemy's", row, col);
                    }

                    //else hit but not destroyed
                    return new AttackResult(ResultOfAttack.Hit, "hit something!", row, col);
               }
               finally {
                    //grid_Changed();
               }
          }

          private void tileChanged(object sender, TileEventArgs e)
          {
               _GameTiles[e.X, e.Y].Texture = red;
          }    


          public void Update()
          {
               //     gridChanged(this, EventArgs.Empty);
                    for (int i = 0; i <= _GameTiles.GetUpperBound(0); i++)
                    {
                         for (int j = 0; j <= _GameTiles.GetUpperBound(1); j++)
                         {
                              _GameTiles[i, j].Update();
                         }
                    }
               
          }


          public void Draw(SpriteBatch spriteBatch)
          {  /// this PLAYER's grid, at position "playerGrid"
               for (int i = 0; i <= _GameTiles.GetUpperBound(0); i++)
               {
                    for (int j = 0; j <= _GameTiles.GetUpperBound(1); j++)
                    {
                         spriteBatch.Draw(_GameTiles[i, j].Texture, _GameTiles[i,j].Rect, Color.White);
                    }
               }
          }
     }
}