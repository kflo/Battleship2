﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Battleship438
{
     /// This is the main type for your game.
     public class BattleshipGame : Game
     {
          GraphicsDeviceManager graphics;
          SpriteBatch spriteBatch;
          Player player;
          Grid grid;
          //Ship ship;
          Dictionary<ShipName, Ship> shipList;
          SeaGrid seaGrid;
          //movement speed, player
          float playerMoveSpeed;


          // keyboard states
          KeyboardState currentKeyboardState;
          KeyboardState previousKeyboardState;
          // mouse states
          MouseState currentMouseState;
          MouseState previousMouseState;


          public BattleshipGame() {
               graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               this.IsMouseVisible = true;
               //graphics.IsFullScreen = true;
          }

          /// Allows the game to perform any initialization it needs to before starting to run.
          /// This is where it can query for any required services and load any non-graphic related content.  
          /// Calling base.Initialize will enumerate through any components and initialize them as well.
          protected override void Initialize() { 
               player = new Player();
               grid = new Grid();
               shipList = new Dictionary<ShipName, Ship>();
               shipList.Add(ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier));
               //shipList.Add(ShipName.Battleship, new Ship(ShipName.Battleship));
               //shipList.Add(ShipName.Destroyer, new Ship(ShipName.Destroyer));
               //shipList.Add(ShipName.Submarine, new Ship(ShipName.Submarine));
               //shipList.Add(ShipName.Tug, new Ship(ShipName.Tug));

               seaGrid = new SeaGrid(shipList);
               seaGrid.Initialize();

               playerMoveSpeed = 4.0f;
               TouchPanel.EnabledGestures = GestureType.FreeDrag;
               base.Initialize();
          }

          /// LoadContent will be called once per game and is the place to load all of your content.
          protected override void LoadContent() {
               // Create a new SpriteBatch, which can be used to draw textures.
               spriteBatch = new SpriteBatch(GraphicsDevice);
               Vector2 pos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
               player.Initialize(Content.Load<Texture2D>("Graphics\\ship2"), pos);
               grid.Initialize(Content.Load<Texture2D>("Graphics\\gridTexShip"));
          }

          /// UnloadContent will be called once per game and is the place to unload game-specific content.
          protected override void UnloadContent() {
               // TODO: Unload any non ContentManager content here
          }



          /// Allows the game to run logic such as updating the world,
          /// checking for collisions, gathering input, and playing audio.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Update(GameTime gameTime) {
               if (Keyboard.GetState().IsKeyDown(Keys.Escape))
               {
                    graphics.IsFullScreen = false;
                    Exit();
               }
               // Save the previous state of the keyboard and game pad so we can determine single key/button presses
               previousKeyboardState = currentKeyboardState;
               // Read the current state of the keyboard and gamepad and store it
               currentKeyboardState = Keyboard.GetState();

               previousMouseState = currentMouseState;
               currentMouseState = Mouse.GetState();

               //Update the player
               UpdatePlayer(gameTime);
               //update the grid
               updateGrid(gameTime);
               // -=========-
               base.Update(gameTime);
          }



          private void updateGrid(GameTime gameTime)
          {
          }
          

          private void UpdatePlayer(GameTime gameTime) {
               // Use Touch input
               while (TouchPanel.IsGestureAvailable)
               {
                    GestureSample gesture = TouchPanel.ReadGesture();
                    if (gesture.GestureType == GestureType.FreeDrag)
                    {
                         gesture.Delta.Normalize();
                         player.Position += gesture.Delta;
                    }
               }

               // Use Mouse input
               Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
               if (currentMouseState.LeftButton == ButtonState.Pressed)
               {
                    Vector2 posDelta = mousePosition - player.Position;
                    posDelta.Normalize();
                    posDelta = posDelta * playerMoveSpeed;
                    //player.Position += posDelta;
               }

               // Use the Keyboard / Dpad
               if (currentKeyboardState.IsKeyDown(Keys.Left))               {
                    player.Position.X -= playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Right))               {
                    player.Position.X += playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Up))               {
                    player.Position.Y -= playerMoveSpeed;
               }
               if (currentKeyboardState.IsKeyDown(Keys.Down))               {
                    player.Position.Y += playerMoveSpeed;
               }

               // Make sure that the player does not go out of bounds
               player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
               player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
          }




          /// This is called when the game should draw itself.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Draw(GameTime gameTime)          {
               GraphicsDevice.Clear(Color.SlateGray);

               // starts Drawing
               spriteBatch.Begin();
               // draw the player
               player.Draw(spriteBatch);
               grid.Draw(spriteBatch);
               //stop drawing
               spriteBatch.End();

               base.Draw(gameTime);
          }
     }
}
