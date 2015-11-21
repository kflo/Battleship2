﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Battleship438
{
     public class BattleshipGame : Game
     {
          GraphicsDeviceManager graphics;
          SpriteBatch spriteBatch; 
          MoveableShip moveTestShip;
          public PB1 pb1;

          private SpriteFont fontBody, fontH1, fontH2, motorOil;
          public Texture2D water, white, red, background, cursor;
          private Dictionary<ShipName, Ship> shipList, shipList2;
          private SeaGrid seaGrid, seaGrid2;
          private Vector2 playerGrid, enemyGrid;

          private MouseState currMouseState;
          private Rectangle vp;
          private string str;

          private Player[] _players = new Player[3];
          private int _playerIndex = 0;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public BattleshipGame() {
               graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               Window.AllowUserResizing = true;
               //graphics.IsFullScreen = true;
          }

          public delegate void AttackCompletedHandler(object sender, AttackResult result);
          public event AttackCompletedHandler AttackCompleted;
          
          public Player Player {
               get { return _players[_playerIndex]; }
          }

          public void AddDeployedPlayer(Player p)
          {
               if (_players[0] == null)
                    _players[0] = p;
               else if (_players[1] == null) {
                    _players[1] = p;
                    //CompleteDeployment();
               } else 
                    throw new ApplicationException("You cannot add another player, the game already has two players.");
          }
/*
          private void CompleteDeployment()
          {
               _players[0].Enemy = new SeaGridAdapter(_players[1].PlayerGrid);
               _players[1].Enemy = new SeaGridAdapter(_players[0].PlayerGrid);
          }
*/
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          private void seaGridChanged(object sender, EventArgs e)
          {
               str = sender.ToString();
          }

          private void pbcD(object sender, EventArgs e)
          {
               pb1.Texture = Content.Load<Texture2D>("Graphics\\randomize");
               seaGrid.texturize(water);
               seaGrid.Initialize(red);
               str = "CHANGE HAPPENED";
          }
          private void pbcU(object sender, EventArgs e)
          {
               pb1.Texture = Content.Load<Texture2D>("Graphics\\newGame");
               seaGrid2.texturize(water);
               seaGrid2.Initialize(red);
               str = "---";
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #


          /// Allows the game to perform any initialization it needs to before starting to run.
          /// This is where it can query for any required services and load any non-graphic related content.  
          /// Calling base.Initialize will enumerate through any components and initialize them as well.
          protected override void Initialize() {
               shipList = new Dictionary<ShipName, Ship>();
               shipList.Add(ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier));
               shipList.Add(ShipName.Battleship, new Ship(ShipName.Battleship));
               shipList.Add(ShipName.Destroyer, new Ship(ShipName.Destroyer));
               shipList.Add(ShipName.Submarine, new Ship(ShipName.Submarine));
               shipList.Add(ShipName.Tug, new Ship(ShipName.Tug));
               shipList2 = shipList;
               moveTestShip = new MoveableShip();
               playerGrid = new Vector2(50, 50);
               enemyGrid = new Vector2(470, 50);
               str = "-----------";
               TouchPanel.EnabledGestures = GestureType.FreeDrag;
               graphics.PreferredBackBufferWidth = 1900;
               graphics.PreferredBackBufferHeight = 1600;
               vp = GraphicsDevice.Viewport.TitleSafeArea;
               base.Initialize();
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  L O A D C O N T E N T   L O A D C O N T E N T   L O A D C O N T E N T   L O A D C O N T E N T
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// LoadContent will be called once per game and is the place to load all of your content.
          protected override void LoadContent() {
               // Create a new SpriteBatch, which can be used to draw textures.
               spriteBatch = new SpriteBatch(GraphicsDevice);

               fontBody = Content.Load<SpriteFont>("Fonts\\myFontBody");
               fontH1 = Content.Load<SpriteFont>("Fonts\\myFontH1");
               fontH2 = Content.Load<SpriteFont>("Fonts\\myFontH2");
               motorOil = Content.Load<SpriteFont>("Fonts\\motorOil");

               background = Content.Load<Texture2D>("Graphics\\background");
               water = Content.Load<Texture2D>("Graphics\\gridTexWater");
               white = Content.Load<Texture2D>("Graphics\\gridTex");
               red = Content.Load<Texture2D>("Graphics\\gridTexRed");
               cursor = Content.Load<Texture2D>("Graphics\\cursor50");

               Vector2 testShipPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
               moveTestShip.Initialize(Content.Load<Texture2D>("Graphics\\ship2"), testShipPos);

               seaGrid = new SeaGrid(shipList, playerGrid, water);
               seaGrid2 = new SeaGrid(shipList2, enemyGrid, water);
               seaGrid.Initialize(red);
               seaGrid2.Initialize(white);
               seaGrid.Changed += seaGridChanged;
               seaGrid2.Changed += seaGridChanged;

               pb1 = new PB1("Graphics\\newGame");
               pb1.LoadContent(Content, vp);
               pb1.ButtonDown += pbcD;
               pb1.ButtonUp += pbcU;
          }

          /// UnloadContent will be called once per game and is the place to unload game-specific content.
          protected override void UnloadContent() {
               /// TODO: Unload any non ContentManager content here
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Allows the game to run logic (updating the world, checking for collisions, input, etc.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Update(GameTime gameTime) {
               if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                    graphics.IsFullScreen = false;
                    Exit();
               }
               currMouseState = Mouse.GetState();
               pb1.Update();
               seaGrid.Update();
               seaGrid2.Update();

               moveTestShip.update(gameTime, graphics.GraphicsDevice);
               base.Update(gameTime);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// This is called when the game should draw itself.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Draw(GameTime gameTime) {

               graphics.GraphicsDevice.Clear(Color.SlateGray);

               // starts Drawing
               spriteBatch.Begin();

               spriteBatch.Draw(background, new Rectangle(0, 0, 1000, 1000), Color.SlateGray);
               pb1.Draw(spriteBatch);

               seaGrid.Draw(spriteBatch);
               seaGrid2.Draw(spriteBatch);

               spriteBatch.DrawString(motorOil, "PLAYER", new Vector2(vp.Width/14, 20), Color.White);
               spriteBatch.DrawString(motorOil, "ENEMY", new Vector2(5 * vp.Width / 6 - 5, 20), Color.White);
               spriteBatch.DrawString(fontBody, "Ships: " + shipList.Count, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width / 4 - 40, 350), Color.White);
               spriteBatch.DrawString(fontBody, "Hits:", new Vector2(vp.Width/4 - 31, 370), Color.White);
               moveTestShip.Draw(spriteBatch);
               spriteBatch.Draw(cursor, new Vector2(currMouseState.X, currMouseState.Y), Color.White);
               spriteBatch.DrawString(fontBody, currMouseState.X.ToString(), new Vector2(340, 50), Color.White);
               spriteBatch.DrawString(fontBody, currMouseState.Y.ToString(), new Vector2(340, 70), Color.White);
               spriteBatch.DrawString(fontH1, str, new Vector2(340, 90), Color.White);
               spriteBatch.DrawString(fontBody, moveTestShip.Position.ToString(), new Vector2(340, 110), Color.White);
               
               //stop drawing
               spriteBatch.End();

               base.Draw(gameTime);
          }
     }
}
