using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438.Model
{
     /// The SeaGridAdapter allows for the change in a sea grid view. Whenever a ship is
     /// presented it changes the view into a sea tile instead of a ship tile.
     public class SeaGridAdapter : ISeaGrid
     {

          private SeaGrid _MyGrid;
          public bool Cheat = false;
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
               adapterChanged(this, e);
          }


          /*public void shipTexturize(Texture2D shipTex) {
               _MyGrid.shipTexturize(shipTex);
          }*/

          /// Changes the discovery grid. Where there is a ship we will sea water
          /// <returns>a tile, either what it actually is, or if it was a ship then return a sea tile</returns>
          public TileView TileView(int x, int y)
          {
               TileView result = _MyGrid.TileView(x, y);
               if (result == Model.TileView.Ship)
                    if (Cheat == false)
                         return Model.TileView.Sea;
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

          /*public void texturize(Texture2D tex) {
               _MyGrid.texturize(tex);
          }*/
          public void Initialize(){
               _MyGrid.Initialize();
          }

          public void Update()
          {
               _MyGrid.Update();
          }

          public void Draw(SpriteBatch spriteBatch){
               for (int i = 0; i < Width; i++){
                    for (int j = 0; j < Height; j++){
                         switch (TileView(i, j)){
                              case Model.TileView.Sea:
                                   spriteBatch.Draw(_MyGrid.Water, _MyGrid.TileRect(i, j), Color.White);
                                   break;
                              case Model.TileView.Miss:
                                   spriteBatch.Draw(_MyGrid.White, _MyGrid.TileRect(i, j), Color.White);
                                   break;
                              case Model.TileView.Ship:
                                   spriteBatch.Draw(_MyGrid.ShipTex, _MyGrid.TileRect(i, j), Color.White);
                                   break;
                              case Model.TileView.Hit:
                                   spriteBatch.Draw(_MyGrid.Red, _MyGrid.TileRect(i, j), Color.White);
                                   break;
                              default:
                                   spriteBatch.Draw(_MyGrid.Water, _MyGrid.TileRect(i, j), Color.White);
                                   break;
                         }
                    }
               }
          }
     }
}