using System;
using System.Collections.Generic;
using Battleship438Game.Model.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438Game.Model
{
     /// The SeaGrid is the grid upon which the ships are deployed. The grid is viewable via the ISeaGrid 
     /// interface as a read only grid. Can be used with the SeaGridAdapter to mask the position of the ships.
     public class SeaGrid : ISeaGrid
     {
          private const int TileSize = 28;
          private const int _WIDTH = 10;
          private const int _HEIGHT = 10;

          private Random _rand;
          private readonly Tile[,] _gameTiles;
          private readonly Dictionary<ShipName, Ship> _shipList;
          public Texture2D Water, ShipTex, Red, White;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// SeaGrid constructor, a seagrid has a number of tiles stored in an array
          public SeaGrid(Dictionary<ShipName, Ship> ships, Vector2 vector, Texture2D water, Texture2D red, Texture2D white, Texture2D shipTex) {
               Water = water;
               ShipTex = shipTex;
               Red = red;
               White = white;
               Rect = new Rectangle((int)vector.X, (int)vector.Y, TileSize * 10, TileSize * 10);
               _gameTiles = new Tile[_WIDTH, _HEIGHT];
               //fill array with empty Tiles
               for (int i = 0; i <= _WIDTH - 1; i++)          {
                    for (int j = 0; j <= _HEIGHT - 1; j++)               {
                         var tileRect = new Rectangle(i * (TileSize) + Rect.X, j * (TileSize) + Rect.Y, water.Width, water.Height);
                         _gameTiles[i, j] = new Tile(i, j, null, tileRect);
                         _gameTiles[i, j].Changed += TileChanged;
                    }
               }
               _shipList = ships;
          }

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
               _rand = new Random(System.DateTime.Now.Millisecond);
               int dir;
               int dRow;
               int dCol;
               int currRow;
               int currCol;
               bool blocked;
               repeat:
               blocked = false;
               dir = _rand.Next(2);
               if (dir == 0) {
                    heading = Direction.UpDown;
                    col = _rand.Next(10);
                    row = _rand.Next(6);
               } else {
                    heading = Direction.LeftRight;
                    col = _rand.Next(6);
                    row = _rand.Next(10);
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
                    if (_gameTiles[currRow, currCol].HasShip) {
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

          private void GridChanged(object sender, TileEventArgs e) {
               Changed?.Invoke(this, e);
          }

          private void TileChanged(object sender, TileEventArgs e){
               GridChanged(this, e);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public Rectangle Rect { get; }

          public int Width => _WIDTH;

          public int Height => _HEIGHT;

          public int ShipsKilled { get; set; }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public TileView TileView(int x, int y) {
               return _gameTiles[x, y].View;
          }

          public Rectangle TileRect(int x, int y) {
               return _gameTiles[x, y].Rect;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// AllDeployed checks if all the ships are deployed
          public bool AllDeployed {
               get {
                    foreach (Ship s in _shipList.Values) {
                         if (!s.IsDeployed)
                              return false;
                    }
                    return true;
               }
          }

          public void Reset(){
               foreach (var item in _shipList)
                    item.Value.Remove();

               for (int i = 0; i <= _WIDTH - 1; i++) {
                    for (int j = 0; j <= _HEIGHT - 1; j++) {
                         _gameTiles[i, j].Shot = false;
                    }
               }
               ShipsKilled = 0;
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
                         _gameTiles[currentRow, currentCol].Ship = ship;
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
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// HitTile hits a tile at a row/col, and whatever tile has been hit, a result will be displayed.
          /// <returns>An attackResult (hit, miss, sunk, shotalready)</returns>
          public AttackResult HitTile(int row, int col)     {
               if (_gameTiles[row, col].Shot) {
                    return new AttackResult(ResultOfAttack.ShotAlready, "already attacked", row, col);
               }

               _gameTiles[row, col].Shoot();

               if (_gameTiles[row, col].Ship == null) {
                    return new AttackResult(ResultOfAttack.Miss, "missed!", row, col);
               }
               else if (_gameTiles[row, col].Ship.IsDestroyed)
               {
                    ShipsKilled += 1;
                    return new AttackResult(ResultOfAttack.Destroyed, _gameTiles[row, col].Ship.Size, "destroyed the enemy's " + _gameTiles[row, col].Ship.Name + "(" +
                         _gameTiles[row, col].Ship.Size + ")" + "!", row, col);
               }
               else
                    return new AttackResult(ResultOfAttack.Hit, _gameTiles[row, col].Ship.Size, "hit something!", row, col);
          }

          public void TvAssign(int row, int col, TileView tileView)
          {
               _gameTiles[row,col].TakeView(tileView);
          }
          
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void Update()     {
               for (int i = 0; i <= _gameTiles.GetUpperBound(0); i++) {
                    for (int j = 0; j <= _gameTiles.GetUpperBound(1); j++) {
                         _gameTiles[i, j].Update();
                    }
               }

          }

          public void Draw(SpriteBatch spriteBatch) {  
               for (int i = 0; i <= _gameTiles.GetUpperBound(0); i++) {
                    for (int j = 0; j <= _gameTiles.GetUpperBound(1); j++) {
                         switch (TileView(i,j))
                         {
                              case Enum.TileView.Sea:
                                   spriteBatch.Draw(Water, TileRect(i,j), Color.White);
                                   break;
                              case Enum.TileView.Miss:
                                   spriteBatch.Draw(White, TileRect(i,j), Color.White);
                                   break;
                              case Enum.TileView.Ship:
                                   spriteBatch.Draw(ShipTex, TileRect(i,j), Color.White);
                                   break;
                              case Enum.TileView.Hit:
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
