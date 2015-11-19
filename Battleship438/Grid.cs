using System;
using System.Xaml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438
{
     class Grid
     {
          int tileSize = 30;
          public Vector2 playerGrid = new Vector2(50,50);
          public Vector2 enemyGrid = new Vector2(470, 50);
          //Texture2D gridTexture;
          const int width = 10;
          const int height = 10;
          Tile[,] grid = new Tile[width, height];
          

          //adds TILES into the underlying grid array
          public void Initialize(Texture2D texture)
          {
               
               for (int i = 0; i <= width - 1; i++)
               {
                    for (int j = 0; j <= height - 1; j++)
                    {
                              grid[i, j] = new Tile(i, j, null);
                              grid[i, j].Texture = texture;
                    }
               }
          }


          public void Draw(SpriteBatch spriteBatch)
          {  /// this PLAYER's grid, at position "playerGrid"
               for (int i = 0; i <= grid.GetUpperBound(0); i++) {
                    for (int j = 0; j <= grid.GetUpperBound(1); j++) {
                              Vector2 texturePosition = new Vector2(i * (tileSize -2), j * (tileSize -2)) + playerGrid;
                              spriteBatch.Draw(grid[i, j].Texture, texturePosition, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    }
               } /// the enemy's grid, at position "enemyGrid"
               for (int i = 0; i <= grid.GetUpperBound(0); i++) {
                    for (int j = 0; j <= grid.GetUpperBound(1); j++) {
                              Vector2 texturePosition = new Vector2(i * (tileSize - 2), j * (tileSize - 2)) + enemyGrid;
                              spriteBatch.Draw(grid[i, j].Texture, texturePosition, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    }
               }
          }
     }
}
