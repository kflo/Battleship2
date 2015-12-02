using System;
using Battleship438Game.Model.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438Game.Model
{
     /// SeaGridAdapter allows for tile view change, into a sea tile instead of a ship tile.
     public class SeaGridAdapter : ISeaGrid {

          private readonly SeaGrid _myGrid;
          public bool Cheat = false;

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public SeaGridAdapter(SeaGrid grid) {
               _myGrid = grid;
               _myGrid.Changed += OriginalSeaGridChanged;
          }

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public int Width => _myGrid.Width;

          public int Height => _myGrid.Height;

          public Rectangle Rect => _myGrid.Rect;

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public event EventHandler<TileEventArgs> Changed;

          private void AdapterChanged(object sender, TileEventArgs e) {
               Changed?.Invoke(this, e);
          }

          private void OriginalSeaGridChanged(object sender, TileEventArgs e) {
               AdapterChanged(this, e);
          }

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Changes the discovery grid. Where there is a ship we will sea water
          public TileView TileView(int x, int y){
               TileView result = _myGrid.TileView(x, y);
               if (result == Enum.TileView.Ship)
                    if (Cheat == false)
                         return Enum.TileView.Sea;
               return result;
          }

          public AttackResult HitTile(int row, int col){
               return _myGrid.HitTile(row, col);
          }

          public void TvAssign(int row, int col, TileView tileView)
          {
               _myGrid.TvAssign(row, col, tileView);
          }

          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void Initialize(){
               _myGrid.Initialize();
          }

          public void Update(){
               _myGrid.Update();
          }

          public void Draw(SpriteBatch spriteBatch){
               for (int i = 0; i < Width; i++){
                    for (int j = 0; j < Height; j++){
                         switch (TileView(i, j)){
                              case Enum.TileView.Sea:
                                   spriteBatch.Draw(_myGrid.Water, _myGrid.TileRect(i, j), Color.White);
                                   break;
                              case Enum.TileView.Miss:
                                   spriteBatch.Draw(_myGrid.White, _myGrid.TileRect(i, j), Color.White);
                                   break;
                              case Enum.TileView.Ship:
                                   spriteBatch.Draw(_myGrid.ShipTex, _myGrid.TileRect(i, j), Color.White);
                                   break;
                              case Enum.TileView.Hit:
                                   spriteBatch.Draw(_myGrid.Red, _myGrid.TileRect(i, j), Color.White);
                                   break;
                              default:
                                   spriteBatch.Draw(_myGrid.Water, _myGrid.TileRect(i, j), Color.White);
                                   break;
                         }
                    }
               }
          }
     }
}