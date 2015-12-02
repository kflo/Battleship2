using System;
using Battleship438Game.Model.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Battleship438Game.Model
{
     /// Tile knows its location on the grid, if it is a ship and if it has been shot before
     public class Tile
     {
          private Ship _ship;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public event EventHandler<TileEventArgs> Changed;

          private void tileHandler(object sender, TileEventArgs e){
               Changed?.Invoke(this, e);
          }

          /// The tile constructor will know where it is on the grid, and is its a ship
          public Tile(int row, int col, Ship ship, Rectangle rectangle) {
               Row = row;
               Column = col;
               _ship = ship;
               Rect = rectangle;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// <value>indicate if the tile has been shot</value>
          public bool Shot { get; set; }

          public bool HasShip { get; private set; }

          public int Row { get; }

          public int Column { get; }

          public Rectangle Rect { get; set; }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public Ship Ship {
               get { return _ship; }
               set {
                    if (_ship == null) {
                         _ship = value;
                         HasShip = true;
                         if (value != null) {
                              _ship.AddTile(this);
                         }
                    } else {
                         throw new InvalidOperationException("There is already a ship at [" + Row + ", " + Column + "]");
                    }
               }
          }
     
          /// Clearship will remove the ship from the tile
          public void ClearShip() {
               _ship = null;
               HasShip = false;
               Shot = false;
          }

          /// View is able to tell the grid what the tile is
          public TileView View {
               get {
                    if (HasShip == false)
                         return Shot ? TileView.Miss : TileView.Sea;
                    else
                         return Shot ? TileView.Hit : TileView.Ship;
               }
          }

          public void TakeView(TileView tv)
          {
               switch (tv)
               {
                         case TileView.Hit:
                         Shot = true;
                         HasShip = true;
                         break;
                         case TileView.Miss:
                         Shot = true;
                         HasShip = false;
                         break;
                         case TileView.Sea:
                         Shot = false;
                         break;
                         case TileView.Ship:
                         HasShip = true;
                         break;
               }
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Shoot allows a tile to be shot at; if tile was hit before, gives an error
          internal void Shoot() {
               if ((Shot == false)) {
                    Shot = true;
                    _ship?.Hit();
               }
               else
                    throw new ApplicationException("You have already shot this square");
          }

          public void Update() {
               if (Rect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    tileHandler(this, new TileEventArgs(this.Row, this.Column));
          }
     }
}