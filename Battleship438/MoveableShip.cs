using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Battleship438
{
     class MoveableShip
     {
          // Animation representing the player
          public Texture2D PlayerTexture;

          // Position of the Player relative to the upper left side of the screen
          public Vector2 Position = new Vector2(10, 10);
          float playerMoveSpeed = 4.0f;
          public GestureSample gesture;

         // keyboard states
          KeyboardState currentKeyboardState;
          KeyboardState previousKeyboardState;

          MouseState currentMouseState;
          //MouseState previousMouseState;
          


          // Get the width of the player ship
          public int Width          {
               get { return PlayerTexture.Width; }
          }


          // Get the height of the player ship
          public int Height          {
               get { return PlayerTexture.Height; }
          }



          public void Initialize(Texture2D texture, Vector2 position)          {
               PlayerTexture = texture;
               Position = position;
          }



          public void update(GameTime gameTime, GraphicsDevice graphics)          {

               previousKeyboardState = currentKeyboardState;
               currentKeyboardState = Keyboard.GetState();

               currentMouseState = Mouse.GetState();
               
               /*// Use Touch input
               while (TouchPanel.IsGestureAvailable)               {
                    gesture = TouchPanel.ReadGesture();
                    if (gesture.GestureType == GestureType.FreeDrag)                    {
                         gesture.Delta.Normalize();
                    }
               }               */

               // Use Mouse input
               Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
               if (currentMouseState.LeftButton == ButtonState.Pressed)
               {
                    Vector2 posDelta = mousePosition - Position;
                    posDelta.Normalize();
                    posDelta = posDelta * playerMoveSpeed;
                    Position += posDelta;
               }
               // Use the Keyboard / Dpad
               if (currentKeyboardState.IsKeyDown(Keys.Left)) {
                    Position.X -= playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Right)) {
                    Position.X += playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Up)) {
                    Position.Y -= playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Down)) {
                    Position.Y += playerMoveSpeed;
               }
               // Make sure that the player does not go out of bounds
               Position.X = MathHelper.Clamp(Position.X, 0, graphics.Viewport.Width - Width);
               Position.Y = MathHelper.Clamp(Position.Y, 0, graphics.Viewport.Height - Height);
          }
          


          public void Draw(SpriteBatch spriteBatch)          {
               spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
          }
     }
}
