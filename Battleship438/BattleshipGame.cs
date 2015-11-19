using System.Collections.Generic;
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

          private SpriteFont font;
          public Texture2D water;
          public Texture2D white;
          public Texture2D red;
          private Texture2D background;
          private Texture2D cursor;

          //public PB1 pb1;

          Dictionary<ShipName, Ship> shipList;
          MoveableShip moveTestShip;
          SeaGrid seaGrid;
          SeaGrid seaGrid2;
          Vector2 playerGrid;
          Vector2 enemyGrid;

          // mouse states
          Vector2 mousePos;
          MouseState currMouseState;

          

          public BattleshipGame()
          {
               graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               this.IsMouseVisible = true;
               this.Window.AllowUserResizing = true;

               //graphics.IsFullScreen = true;
          }

          /// Allows the game to perform any initialization it needs to before starting to run.
          /// This is where it can query for any required services and load any non-graphic related content.  
          /// Calling base.Initialize will enumerate through any components and initialize them as well.
          protected override void Initialize()
          {
               shipList = new Dictionary<ShipName, Ship>();
               shipList.Add(ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier));
               shipList.Add(ShipName.Battleship, new Ship(ShipName.Battleship));
               shipList.Add(ShipName.Destroyer, new Ship(ShipName.Destroyer));
               shipList.Add(ShipName.Submarine, new Ship(ShipName.Submarine));
               shipList.Add(ShipName.Tug, new Ship(ShipName.Tug));
               seaGrid = new SeaGrid(shipList);
               seaGrid2 = new SeaGrid(shipList);
               moveTestShip = new MoveableShip();
               playerGrid = new Vector2(50, 50);
               enemyGrid = new Vector2(470, 50);

               TouchPanel.EnabledGestures = GestureType.FreeDrag;
               //graphics.PreferredBackBufferWidth = 1900;
               //graphics.PreferredBackBufferHeight = 1600;
               base.Initialize();
          }

          /// LoadContent will be called once per game and is the place to load all of your content.
          protected override void LoadContent()
          {
               // Create a new SpriteBatch, which can be used to draw textures.
               spriteBatch = new SpriteBatch(GraphicsDevice);

               font = Content.Load<SpriteFont>("Fonts\\myFont");
               background = Content.Load<Texture2D>("Graphics\\background");
               water = Content.Load<Texture2D>("Graphics\\gridTexWater");
               white = Content.Load<Texture2D>("Graphics\\gridTex");
               red = Content.Load<Texture2D>("Graphics\\gridTexRed");
               cursor = Content.Load<Texture2D>("Graphics\\cursor50");

               Vector2 testShipPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
               moveTestShip.Initialize(Content.Load<Texture2D>("Graphics\\ship2"), testShipPos);

              // pb1 = new PB1("Graphics\\newGame");
              // pb1.LoadContent(Content);

               seaGrid.texturize(water);
               seaGrid.Initialize(red);
               seaGrid2.texturize(water);
               seaGrid2.Initialize(red);
               //seaGrid.Changed += seaGridChanged;
          }


          /// UnloadContent will be called once per game and is the place to unload game-specific content.
          protected override void UnloadContent() {
               // TODO: Unload any non ContentManager content here
          }




          /// Allows the game to run logic such as updating the world,
          /// checking for collisions, gathering input, and playing audio.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
           
          protected override void Update(GameTime gameTime) {
               if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                    graphics.IsFullScreen = false;
                    Exit();
               }

               //previousMouseState = currentMouseState;
               currMouseState = Mouse.GetState();

               //Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
               mousePos.X = currMouseState.X + 50;
               mousePos.Y = currMouseState.Y + 50;

               if (currMouseState.LeftButton == ButtonState.Pressed) { 
                    Vector2 posDelta = new Vector2(mousePos.X, mousePos.Y);
                    posDelta.Normalize();
                    posDelta = posDelta * 3;
                    mousePos = mousePos + posDelta;
               }               

               moveTestShip.update(gameTime, graphics.GraphicsDevice);
               base.Update(gameTime);


          }

          /*
          static void seaGridChanged(object sender, EventArgs e) {
               Console.WriteLine("The SEAGRID was changed.");
               Environment.Exit(0);
          }


          private void updateGrid(GameTime gameTime) {
               seaGrid.grid_Changed();
          }
          */

          /// This is called when the game should draw itself.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Draw(GameTime gameTime)
          {
               graphics.GraphicsDevice.Clear(Color.SlateGray);

               
               // starts Drawing
               spriteBatch.Begin();

               spriteBatch.Draw(background, new Rectangle(0, 0, 1000, 1000), Color.SlateGray);

               // draw the grid
               seaGrid.Draw(spriteBatch, playerGrid);
               seaGrid2.Draw(spriteBatch, enemyGrid);

               spriteBatch.DrawString(font, "Player", new Vector2(170, 20), Color.White);
               spriteBatch.DrawString(font, "Enemy", new Vector2(588, 20), Color.White);
               spriteBatch.DrawString(font, "Ships: " + shipList.Count, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 4 - 40, 350), Color.White);
               spriteBatch.DrawString(font, "Hits:", new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width/4 - 31, 370), Color.White);

               //stop drawing
               moveTestShip.Draw(spriteBatch);
               //pb1.Draw(spriteBatch);
               spriteBatch.Draw(cursor, new Vector2(mousePos.X, mousePos.Y), Color.White);
               spriteBatch.DrawString(font, mousePos.X.ToString(), new Vector2(340, 10), Color.White);
               spriteBatch.DrawString(font, mousePos.Y.ToString(), new Vector2(340, 30), Color.White);
               spriteBatch.DrawString(font, currMouseState.X.ToString(), new Vector2(340, 50), Color.White);
               spriteBatch.DrawString(font, currMouseState.Y.ToString(), new Vector2(340, 70), Color.White);
               spriteBatch.DrawString(font, moveTestShip.gesture.GestureType.ToString(), new Vector2(340, 90), Color.White);
               spriteBatch.DrawString(font, moveTestShip.Position.ToString(), new Vector2(340, 110), Color.White);

               spriteBatch.End();

               base.Draw(gameTime);
          }
     }
}
