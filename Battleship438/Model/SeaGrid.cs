using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438.Model
{
     /// The SeaGrid is the grid upon which the ships are deployed. The grid is 
     /// viewable via the ISeaGrid interface as a read only grid. This can be used 
     /// in conjuncture with the SeaGridAdapter to mask the position of the ships.
     public class SeaGrid : ISeaGrid
     {
          int tileSize = 28;
          private const int _WIDTH = 10;
          private const int _HEIGHT = 10;
          private int _shipsKilled = 0;

          private Random rand;
          private Rectangle tileRect;
          private Tile[,] _GameTiles;
          private Dictionary<ShipName, Ship> _shipList;
          public Texture2D Water, ShipTex, Red, White;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// SeaGrid constructor, a seagrid has a number of tiles stored in an array
          public SeaGrid(Dictionary<ShipName, Ship> ships, Vector2 vector, Texture2D water, Texture2D red, Texture2D white, Texture2D shipTex)     {
               Water = water;
               ShipTex = shipTex;
               Red = red;
               White = white;
               Rect = new Rectangle((int)vector.X, (int)vector.Y, tileSize * 10, tileSize * 10);
               _GameTiles = new Tile[_WIDTH, _HEIGHT];
               //fill array with empty Tiles
               for (int i = 0; i <= _WIDTH - 1; i++)          {
                    for (int j = 0; j <= _HEIGHT - 1; j++)               {
                         tileRect = new Rectangle(i * (tileSize) + Rect.X, j * (tileSize) + Rect.Y, water.Width, water.Height);
                         _GameTiles[i, j] = new Tile(i, j, null, tileRect);
                         _GameTiles[i, j].Changed += tileChanged;
                    }
               }
               _shipList = ships;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          ///set Texture of ALL tiles in seaGrid
          /*
          public void texturize(Texture2D texture) {
               for (int i = 0; i <= _WIDTH - 1; i++) {
                    for (int j = 0; j <= _HEIGHT - 1; j++) {
                         _GameTiles[i, j].Texture = texture;
                    }
               }
          }

          public void shipTexturize(Texture2D shipTex) {
               foreach (var item in _shipList)
                    item.Value.texturize(shipTex);
          }*/

          /// randomly initializes the SHIPS from the Dictionary with TEXTURE shipTex
          public void Initialize() {
               Direction heading = Direction.LeftRight;
               int row = 0;
               int col = 0;
               foreach (var item in _shipList) { 
                    //ITEM is the ship Dictionary pair (KEY = SHIPNAME, VALUE = SHIP)
                    Randomize(ref heading, ref row, ref col, item.Value);
                    MoveShip(row, col, item.Key, heading);
               }
          }

          private void Randomize(ref Direction heading, ref int row, ref int col, Ship ship)     {
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

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// The sea grid has changed and should be redrawn.
          public event EventHandler<TileEventArgs> Changed;

          private void gridChanged(object sender, TileEventArgs e)
          {
               Changed?.Invoke(this, e);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// <returns>The width of the sea grid.</returns>
          public int Width => _WIDTH;

          /// <returns>The height of the sea grid</returns>
          public int Height => _HEIGHT;

          /// ShipsKilled returns the number of ships killed
          public int ShipsKilled => _shipsKilled;




          public TileView TileView(int x, int y) {
               return _GameTiles[x, y].View;
          }

          public Rectangle TileRect(int x, int y) {
               return _GameTiles[x, y].Rect;
          }



          /// AllDeployed checks if all the ships are deployed
          public bool AllDeployed     {
               get {
                    foreach (Ship s in _shipList.Values) {
                         if (!s.IsDeployed)
                              return false;
                    }
                    return true;
               }
          }

          public Rectangle Rect { get; }

          public void Reset(){
               foreach (var item in _shipList)
                    item.Value.Remove();

               for (int i = 0; i <= _WIDTH - 1; i++) {
                    for (int j = 0; j <= _HEIGHT - 1; j++) {
                         _GameTiles[i, j].Shot = false;
                    }
               }
               //texturize(Water);
               _shipsKilled = 0;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// MoveShips allows for ships to be placed on the seagrid
          public void MoveShip(int row, int col, ShipName shipName, Direction direction)     {
               Ship shipToMove = _shipList[shipName];
               shipToMove.Remove();
               AddShip(row, col, direction, shipToMove);
          }

          /// AddShip add a ship to the SeaGrid
          private void AddShip(int row, int col, Direction direction, Ship ship)     {
               try {
                    int size = ship.Size;
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
                         _GameTiles[currentRow, currentCol].Ship = ship;
                         //_GameTiles[currentRow, currentCol].Texture = texture;
                         currentCol += dCol;
                         currentRow += dRow;
                    }
                    ship.Deployed(direction, row, col);
               }
               catch (Exception e) {
                    ship.Remove();
                    //if fails remove the ship
                    throw new ApplicationException(e.Message);
               }
               finally {
                    //gridChanged(this, new TileEventArgs(row, col));
               }
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// HitTile hits a tile at a row/col, and whatever tile has been hit, a result will be displayed.
          /// <returns>An attackResult (hit, miss, sunk, shotalready)</returns>
          public AttackResult HitTile(int row, int col)     {
               try {
                    //tile is already hit
                    if (_GameTiles[row, col].Shot) {
                         return new AttackResult(ResultOfAttack.ShotAlready, "already attacked", row, col);
                    }

                    _GameTiles[row, col].Shoot();

                    //there is no ship on the tile
                    if (_GameTiles[row, col].Ship == null)
                    {
                         //_GameTiles[row, col].Texture = White;
                         return new AttackResult(ResultOfAttack.Miss, "missed!", row, col);
                    }
                    else if (_GameTiles[row, col].Ship.IsDestroyed)
                    {
                         //_GameTiles[row, col].Shot = true;
                         //_GameTiles[row, col].Texture = Red;
                         _shipsKilled += 1;
                         return new AttackResult(ResultOfAttack.Destroyed, _GameTiles[row, col].Ship, "destroyed the enemy's " + _GameTiles[row, col].Ship.Name + "(" + _GameTiles[row, col].Ship.Size + ")" + "!", row, col);
                    }
                    else
                    {
                         //_GameTiles[row, col].Shot = true;
                         //_GameTiles[row, col].Texture = Red;
                         return new AttackResult(ResultOfAttack.Hit, _GameTiles[row, col].Ship, "hit something!", row, col);
                    }
               }
               finally {
                    //     gridChanged(this, new TileEventArgs(row, col));
               }
          }

          private void tileChanged(object sender, TileEventArgs e)     {
               gridChanged(this, e);
               //AttackResult result = HitTile(e.X, e.Y);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void Update()     {
               //     gridChanged(this, EventArgs.Empty);
               for (int i = 0; i <= _GameTiles.GetUpperBound(0); i++) {
                    for (int j = 0; j <= _GameTiles.GetUpperBound(1); j++) {
                         _GameTiles[i, j].Update();
                    }
               }

          }

          public void Draw(SpriteBatch spriteBatch) {  
               for (int i = 0; i <= _GameTiles.GetUpperBound(0); i++) {
                    for (int j = 0; j <= _GameTiles.GetUpperBound(1); j++) {
                         switch (TileView(i,j))
                         {
                              case Model.TileView.Sea:
                                   spriteBatch.Draw(Water, TileRect(i,j), Color.White);
                                   break;
                              case Model.TileView.Miss:
                                   spriteBatch.Draw(White, TileRect(i,j), Color.White);
                                   break;
                              case Model.TileView.Ship:
                                   spriteBatch.Draw(ShipTex, TileRect(i,j), Color.White);
                                   break;
                              case Model.TileView.Hit:
                                   spriteBatch.Draw(Red, TileRect(i,j), Color.White);
                                   break;
                              default:
                                   spriteBatch.Draw(Water, TileRect(i,j), Color.White);
                                   break;
                         }
                    }
               }
          }
     }
}
