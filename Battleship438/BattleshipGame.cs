using System.Collections.Generic;
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
          public PB1 pbNewGame, pbExit, pbConnect, pbViewHide;

          private SpriteFont fontBody, fontH1, fontH2, motorOil, museo, museo29, museoSans;
          public Texture2D water, white, red, background, cursor, ship;
          private Dictionary<ShipName, Ship> shipList1, shipList2;
          private SeaGridAdapter clicker;
          private Player player1, player2;
          private Vector2 player1Grid, enemyGrid;

          private MouseState currMouseState;
          private Rectangle vp;
          private string str, currentPlayer;
          AttackResult newAttack;
          private int otherPlayer, shotRow, shotCol;
          private bool attacking;
          private Player[] _players = new Player[3];
          private int _playerIndex;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public BattleshipGame() {
               graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               Window.AllowUserResizing = true;
               graphics.PreferredBackBufferWidth = 900;
               graphics.PreferredBackBufferHeight = 500;
               //graphics.IsFullScreen = true;
          }

          public event AttackCompletedHandler AttackCompleted;

          public delegate void AttackCompletedHandler(object sender, AttackResult result);
          
          public Player Player {
               get { return _players[_playerIndex]; }
          }

          private void AddDeployedPlayer(Player p)
          {
               if (_players[0] == null)
                    _players[0] = p;
               else if (_players[1] == null) {
                    _players[1] = p;
                    CompleteDeployment();
               } else 
                    throw new ApplicationException("You cannot add another player, the game already has two players.");
          }

          private void CompleteDeployment()
          {
               _players[0].Enemy = new SeaGridAdapter(_players[1].PlayerGrid);
               _players[1].Enemy = new SeaGridAdapter(_players[0].PlayerGrid);
          }

          /// player turn toggler -- taking turns shooting
          /*
          public AttackResult Shoot(int row, int col)          {
               AttackResult newAttack = default(AttackResult);
               otherPlayer = (_playerIndex + 1) % 2;

               newAttack = Player.Shoot(row, col);
               //Will exit the game when all players ships are destroyed
               if (_players[otherPlayer].allDestroyed)
                    newAttack = new AttackResult(ResultOfAttack.GameOver, newAttack.Ship, newAttack.Text, row, col);

               if (AttackCompleted != null)
                    AttackCompleted(this, newAttack);

               //change player if the last hit was a miss
               if (newAttack.Value == ResultOfAttack.Miss)
                    _playerIndex = otherPlayer;

               return newAttack;
          } */

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #


          /// Allows the game to perform any initialization it needs to before starting to run.
          /// This is where it can query for any required services and load any non-graphic related content.  
          /// Calling base.Initialize will enumerate through any components and initialize them as well.
          protected override void Initialize() {
               vp = GraphicsDevice.Viewport.TitleSafeArea;
               shipList1 = new Dictionary<ShipName, Ship>();
               shipList1.Add(ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier));
               shipList1.Add(ShipName.Battleship, new Ship(ShipName.Battleship));
               shipList1.Add(ShipName.Destroyer, new Ship(ShipName.Destroyer));
               shipList1.Add(ShipName.Submarine, new Ship(ShipName.Submarine));
               shipList1.Add(ShipName.TugBoat, new Ship(ShipName.TugBoat));
               shipList2 = new Dictionary<ShipName, Ship>();
               shipList2.Add(ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier));
               shipList2.Add(ShipName.Battleship, new Ship(ShipName.Battleship));
               shipList2.Add(ShipName.Destroyer, new Ship(ShipName.Destroyer));
               shipList2.Add(ShipName.Submarine, new Ship(ShipName.Submarine));
               shipList2.Add(ShipName.TugBoat, new Ship(ShipName.TugBoat));

               moveTestShip = new MoveableShip();
               player1Grid = new Vector2(vp.Width / 14, 60);
               enemyGrid = new Vector2( 8 * vp.Width / 13, 60);
               str = "Guess where the enemy's ships are on the grid!";
               TouchPanel.EnabledGestures = GestureType.FreeDrag;

               newAttack = default(AttackResult);
               attacking = false;
               _playerIndex = 0;
               otherPlayer = (_playerIndex + 1) % 2;

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
               museo = Content.Load<SpriteFont>("Fonts\\museo");
               museo29 = Content.Load<SpriteFont>("Fonts\\museo29");
               museoSans = Content.Load<SpriteFont>("Fonts\\museoSans");

               background = Content.Load<Texture2D>("Graphics\\background");
               water = Content.Load<Texture2D>("Graphics\\gridTexWater");
               white = Content.Load<Texture2D>("Graphics\\gridTex");
               red = Content.Load<Texture2D>("Graphics\\gridTexRed");
               cursor = Content.Load<Texture2D>("Graphics\\cursor50");
               ship = Content.Load<Texture2D>("Graphics\\ship");

               Vector2 testShipPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, 397);
               moveTestShip.Initialize(Content.Load<Texture2D>("Graphics\\ship2"), testShipPos);

               player1 = new Player(shipList1, player1Grid, water, red, white);
               player1.PlayerGrid.Initialize(ship);
               player2 = new Player(shipList2, enemyGrid, water, red, white);
               player2.PlayerGrid.Initialize(ship);

               AddDeployedPlayer(player1);
               AddDeployedPlayer(player2);
               Player.EnemyGrid.Changed += seaGridChanged;
               _players[otherPlayer].EnemyGrid.Changed += seaGridChanged;


               pbNewGame = new PB1("Graphics\\newGame");
               pbNewGame.LoadContent(Content, new Rectangle(0, 0, vp.Width, 2* vp.Height /12));
               pbNewGame.ButtonDown += pbcD;
               pbNewGame.ButtonUp += pbcU;

               pbConnect = new PB1("Graphics\\connect");
               pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
               pbConnect.ButtonDown += pbcD;
               pbConnect.ButtonUp += pbcU;

               pbViewHide = new PB1("Graphics\\view");
               pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
               pbViewHide.ButtonDown += pbcD;
               pbViewHide.ButtonUp += pbcU;

               pbExit = new PB1("Graphics\\exit");
               pbExit.LoadContent(Content, new Rectangle(0,0,vp.Width, 8 * vp.Height / 12));
               pbExit.ButtonDown += pbcD;
               pbExit.ButtonUp += pbcU;

               
          }
          /// UnloadContent will be called once per game and is the place to unload game-specific content.
          protected override void UnloadContent() {// TODO: Unload any non ContentManager content here 
          }


          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          private void seaGridChanged(object sender, TileEventArgs e)
          {
               clicker = (SeaGridAdapter)sender;
               shotRow = e.X;
               shotCol = e.Y;
          }

          private void pbcD(object sender, EventArgs e)
          {
               if (sender.Equals((object)pbExit))
                    Exit();
               else if (sender.Equals((object)pbConnect))
               {

               }

               else if (sender.Equals((object)pbViewHide) && pbViewHide.Asset == "Graphics\\view")
               { /// HIDE THE SHIPS
                    pbViewHide.Asset = "Graphics\\hide";
                    pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
                    Player.EnemyGrid.shipTexturize(water);
               }
               else if (sender.Equals((object)pbViewHide) && pbViewHide.Asset == "Graphics\\hide")
               { /// VIEW THE SHIPS
                    pbViewHide.Asset = "Graphics\\view";
                    pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
                    Player.EnemyGrid.shipTexturize(ship);
               }

               else if (sender.Equals((object)pbNewGame))
               {
                    pbNewGame.Texture = Content.Load<Texture2D>("Graphics\\randomize");
                    player1.Reset(ship);
                    str = "RANDOMIZING...";
                    newAttack = default(AttackResult);
                    //******************
                    //TODO: make reset not switch grid sides (due to "Player.")
                    //was: Player.Reset(ship);
                    //******************
               }
          }
          private void pbcU(object sender, EventArgs e)
          {
               if (sender.Equals((object)pbNewGame))
               {
                    pbNewGame.Texture = Content.Load<Texture2D>("Graphics\\newGame");
                    player2.Reset(ship);
                    str = "Guess where the enemy's ships are on the grid!";
                    //******************
                    //TODO: make reset not switch grid sides
                    //was: _players[otherPlayer].Reset(ship);
                    //******************
               }
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
               moveTestShip.update(gameTime, graphics.GraphicsDevice);

               if (Player.EnemyGrid.Rect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && attacking == false && !str.Contains("OVER")) {

                    attacking = true;
                    
                    Player.Update();  //This sets the global shotROW and shotCOL
                    //new shotRow collected
                    //new shotCol collected
                    newAttack = Player.Shoot(shotRow, shotCol);
                    
                    //Will exit the game when all players ships are destroyed
                    if (_players[otherPlayer].allDestroyed)
                         newAttack = new AttackResult(ResultOfAttack.GameOver, newAttack.Ship, "PLAYER " + (_playerIndex+1) + " WINS! GAME OVER!", shotRow, shotCol);

                    /*
                    if (clicker == Player.EnemyGrid)
                         str = "You ";
                    else
                         str = "They ";
                    str += newAttack.Text;
                    */
                    str = newAttack.Text;

                    //change player if the last hit was a miss/hit
                    if (newAttack.Value != ResultOfAttack.ShotAlready) {
                         Player.EnemyGrid.Changed -= seaGridChanged;
                         _playerIndex = otherPlayer;
                         otherPlayer = (_playerIndex + 1) % 2;
                         Player.EnemyGrid.Changed += seaGridChanged;
                    }
               }

               attacking = false;

               pbNewGame.Update();
               pbConnect.Update();
               pbViewHide.Update();
               pbExit.Update();

               base.Update(gameTime);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// This is called when the game should draw itself.
          protected override void Draw(GameTime gameTime) {

               graphics.GraphicsDevice.Clear(Color.SlateGray);

               // starts Drawing
               spriteBatch.Begin();

               spriteBatch.Draw(background, new Rectangle(0, 0, 900, 900), Color.SlateGray);
               pbNewGame.Draw(spriteBatch);
               pbConnect.Draw(spriteBatch);
               pbViewHide.Draw(spriteBatch);
               pbExit.Draw(spriteBatch);

               player1.Draw(spriteBatch);
               player2.Draw(spriteBatch);

               moveTestShip.Draw(spriteBatch);
               spriteBatch.Draw(cursor, new Vector2(currMouseState.X, currMouseState.Y), Color.White);

               spriteBatch.DrawString(motorOil, "PLAYER 1", new Vector2(vp.Width / 14, 30), Color.White);
               spriteBatch.DrawString(motorOil, "PLAYER 2", new Vector2(11 * vp.Width / 14 - 7, 30), Color.White);

               spriteBatch.DrawString(museoSans, player1.shipsLeft().ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(player1.shipsLeft().ToString()).X, 200), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, "Ships", new Vector2(vp.Width / 2 - museoSans.MeasureString("Ships").X / 2, 200), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, player2.shipsLeft().ToString(), new Vector2(vp.Width / 2 + 50, 200), Color.Goldenrod);

               spriteBatch.DrawString(museoSans, player1.Shots.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(player1.Shots.ToString()).X, 235), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, "Shots", new Vector2(vp.Width / 2 - museoSans.MeasureString("Shots").X / 2, 235), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, player2.Shots.ToString(), new Vector2(vp.Width / 2 + 50, 235), Color.Goldenrod);

               spriteBatch.DrawString(museoSans, player1.Hits.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(player1.Hits.ToString()).X, 270), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, " Hits ", new Vector2(vp.Width / 2 - museoSans.MeasureString(" Hits ").X / 2, 270), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, player2.Hits.ToString(), new Vector2(vp.Width / 2 + 50, 270), Color.Goldenrod);

               spriteBatch.DrawString(museoSans, player1.Misses.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(player1.Misses.ToString()).X, 305), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, "Misses", new Vector2(vp.Width / 2 - museoSans.MeasureString("Misses").X / 2, 305), Color.Goldenrod);
               spriteBatch.DrawString(museoSans, player2.Misses.ToString(), new Vector2(vp.Width / 2 + 50, 305), Color.Goldenrod);

               spriteBatch.DrawString(museo, str, new Vector2(vp.Width / 2 - museo.MeasureString(str).X / 2, 360), Color.Goldenrod);
               if (newAttack != null)
                    spriteBatch.DrawString(museo, shotRow+1 + ", " + (shotCol+1), new Vector2(vp.Width / 2 - museo.MeasureString(shotRow + 1 + ", " + (shotCol+1)).X / 2, 397), Color.Goldenrod);


               currentPlayer = "PLAYER " + (_playerIndex+1) + "'S TURN";
               if (str.Contains("OVER"))
                    currentPlayer = "PLAY AGAIN?";
               spriteBatch.DrawString(museo29, currentPlayer, new Vector2(vp.Width / 2 - museo29.MeasureString(currentPlayer).X / 2, 435), Color.Gold);

               //stop drawing
               spriteBatch.End();

               base.Draw(gameTime);
          }
     }
}
