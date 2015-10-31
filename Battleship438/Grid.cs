using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438
{
     class Grid
     {
          int tileSize = 30;
          public Vector2 position = new Vector2(30,30);
          Texture2D gridTexture;
          const int width = 10;
          const int height = 10;
          int[,] grid = new int[width, height];

          public void Initialize(Texture2D texture)
          {
               gridTexture = texture;
               for (int i = 0; i <= width - 1; i++)
               {
                    for (int j = 0; j <= height - 1; j++)
                    {
                         grid[i, j] = 1;
                    }
               }
          }

          public void Draw(SpriteBatch spriteBatch)
          {
               for (int i = 0; i <= grid.GetUpperBound(0); i++)
               {
                    for (int j = 0; j <= grid.GetUpperBound(1); j++)
                    {
                         int textureId = grid[i, j];
                         if (textureId != 0)
                         {
                              Vector2 texturePosition = new Vector2(i * tileSize, j * tileSize) + position;

                              //Here you would typically index to a Texture based on the textureId.
                              spriteBatch.Draw(gridTexture, texturePosition, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                         }
                    }
               }
          }
     }
}
