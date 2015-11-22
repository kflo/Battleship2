using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

/// The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
/// presented it changes the view into a sea tile instead of a ship tile.
public class SeaGridAdapter : ISeaGrid
{

     private SeaGrid _MyGrid;
     /// Create the SeaGridAdapter, with the grid, and it will allow it to be changed
     /// <param name="grid">the grid that needs to be adapted</param>
     /// 
     public SeaGridAdapter(SeaGrid grid) {
          _MyGrid = grid;
          _MyGrid.Changed += originalSeaGridChanged;
     }

     /// Indicates that the grid has been changed
     public event EventHandler<TileEventArgs> Changed;

     /// MyGrid_Changed causes the grid to be redrawn by raising a changed event
     private void adapterChanged(object sender, TileEventArgs e) {
          if (Changed != null)
               Changed(this, e);
     }

     private void originalSeaGridChanged(object sender, TileEventArgs e) {
          adapterChanged(sender, e);
     }


     public void shipTexturize(Texture2D shipTex) {
          _MyGrid.shipTexurize(shipTex);
     }

     #region "ISeaGrid Members"

     /// Changes the discovery grid. Where there is a ship we will sea water
     /// <returns>a tile, either what it actually is, or if it was a ship then return a sea tile</returns>
     public TileView Item(int x, int y) {
          TileView result = _MyGrid.Item(x, y);

          if (result == TileView.Ship)
               return TileView.Sea;
          else 
               return result;
         }


     /// Get the width of a tile
     public int Width  {
          get { return _MyGrid.Width; }
     }

     /// Get the height of the tile
     public int Height {
          get { return _MyGrid.Height; }
     }


     public Rectangle Rect{
          get { return _MyGrid.Rect; }
          //set { rect = value; }
     }

     /// HitTile calls oppon _MyGrid to hit a tile at the row, col <returns>The result from hitting that tile</returns>
     public AttackResult HitTile(int row, int col) {
          return _MyGrid.HitTile(row, col);
     }
     #endregion

     public void texturize(Texture2D tex) {
          _MyGrid.texturize(tex);
     }
     public void Initialize(Texture2D tex){
          _MyGrid.Initialize(tex);
     }

     public void Update()
     {
          _MyGrid.Update();
     }

     public void Draw(SpriteBatch spriteBatch) {
          _MyGrid.Draw(spriteBatch);
     }
}