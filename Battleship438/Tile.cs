using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;


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

/// Tile knows its location on the grid, if it is a ship and if it has been shot before
public class Tile
{
     private Rectangle rect;
     //the row value of the tile
     private readonly int _RowValue;
     //the column value of the tile
     private readonly int _ColumnValue;
     //the ship the tile belongs to
     private Ship _Ship;
     //the tile has been shot at
     private bool _Shot = false;
     private bool shipBool = false;
     private Texture2D texture;

     public event EventHandler<TileEventArgs> Changed;

     private void tileHandler(object sender, TileEventArgs e) {
          if (Changed != null) 
               Changed(this, e);
     }

     /// The tile constructor will know where it is on the grid, and is its a ship
     public Tile(int row, int col, Ship ship, Texture2D tex, Rectangle Rectangle) {
          _RowValue = row;
          _ColumnValue = col;
          _Ship = ship;
          Texture = tex;
          Rect = Rectangle;
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// <value>indicate if the tile has been shot</value>
     public bool Shot {
          get { return _Shot; }
          set { _Shot = value; }
     }

     public bool hasShip {
          get { return shipBool; }
     }
     
     /// The row of the tile in the grid
     public int Row {
          get { return _RowValue; }
     }

     /// The column of the tile in the grid
     public int Column {
          get { return _ColumnValue; }
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// Ship allows for a tile to check if there is ship and add a ship to a tile
     public Ship Ship {
          get { return _Ship; }
          set {
               if (_Ship == null) {
                    _Ship = value;
                    shipBool = true;
                    if (value != null) {
                         _Ship.AddTile(this);
                    }
               } else {
                    throw new InvalidOperationException("There is already a ship at [" + Row + ", " + Column + "]");
               }
          }
     }
     
     /// Clearship will remove the ship from the tile
     public void ClearShip() {
          _Ship = null;
          shipBool = false;
     }

     /// View is able to tell the grid what the tile is
     public TileView View {
          get {
               //if there is no ship in the tile
               if (_Ship == null) {
                    //and the tile has been hit
                    if (_Shot) 
                         return TileView.Miss;
                    else 
                         return TileView.Sea;
               }
               else {
                    //if there is a ship and it has been hit
                    if ((_Shot))
                         return TileView.Hit;
                    else 
                         return TileView.Ship;
               }
          }
     }

     public Texture2D Texture {
          get { return texture; }
          set { texture = value; }
     }

     public Rectangle Rect {
          get { return rect; }
          set { rect = value; }
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// Shoot allows a tile to be shot at; if tile was hit before, gives an error
     internal void Shoot() {
          if ((Shot == false)) {
               Shot = true;
               if (_Ship != null)
                    _Ship.Hit();
          }
          else
               throw new ApplicationException("You have already shot this square");
          }

     public void Update() {
          if (Rect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
               tileHandler(this, new TileEventArgs(this.Row, this.Column));
          //   Shoot();
     }
}