using System;
using System.Collections.Generic;
using Battleship438Game.Model;
using Battleship438Game.Model.Enum;
using Battleship438Game.Network;
using Battleship438Game.Network.Messages;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship438Game
{
     public class BattleshipGame : Game
     {
          readonly GraphicsDeviceManager _graphics;
          private SpriteBatch _spriteBatch;
          private MoveableShip _moveTestShip;
          private readonly INetworkManager _networkManager;
          private SpriteFont motorOil, museo, museo29, museoSans;
          private Texture2D water, white, red, background, background0, background1, cursor, ship;
          private Dictionary<ShipName, Ship> shipList1, shipList2;
          private PB1 pbNewGame, pbExit, pbConnect, pbViewHide;
          private Rectangle vp;
          private SeaGridAdapter clicker;
          // PLAYERS
          private readonly Player[] _players = new Player[3];
          private int _playerIndex, _otherPlayer;
          private Player player1, enemyPlayer;
          private Vector2 player1Grid, enemyGrid;

          // EVENT use
          private int shotRow, shotCol;
          private bool ableToShoot, easterEgg;

          // MISC
          private MouseState currMouseState;
          private string str, currentPlayer, shotCoordinate;
          private AttackResult newAttack;


          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L A S S F U N C T I O N S   C L
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public BattleshipGame(INetworkManager networkManager){
               _graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               Window.AllowUserResizing = true;
               _graphics.PreferredBackBufferWidth = 900;
               _graphics.PreferredBackBufferHeight = 500;
               this._networkManager = networkManager;
               //_graphics.IsFullScreen = true;
          }

          public BattleshipGame(){
               _graphics = new GraphicsDeviceManager(this);
               Content.RootDirectory = "Content";
               Window.AllowUserResizing = true;
               _graphics.PreferredBackBufferWidth = 900;
               _graphics.PreferredBackBufferHeight = 500;
               //_graphics.IsFullScreen = true;
          }

          private bool IsHost => this._networkManager is ServerNetworkManager;

          public Player Player => _players[_playerIndex];

          public Player Enemy => _players[_otherPlayer];

          private void AddDeployedPlayer(Player p){
               if (_players[0] == null)
                    _players[0] = p;
               else {
                    _players[1] = p;
                    CompleteDeployment();
               }
          }

          private void CompleteDeployment(){
               _players[0].Enemy = new SeaGridAdapter(_players[1].PlayerGrid);
               _players[1].Enemy = new SeaGridAdapter(_players[0].PlayerGrid);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I T I A L I Z E   I N I
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Allows the game to perform any initialization it needs to before starting to run. can query for required services and 
          /// load any non-graphic related content. Calling base.Initialize will enumerate through any components and initialize them as well.
          protected override void Initialize(){
               vp = GraphicsDevice.Viewport.TitleSafeArea;
               shipList1 = new Dictionary<ShipName, Ship> {
                    {ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier)},
                    {ShipName.Battleship, new Ship(ShipName.Battleship)},
                    {ShipName.Destroyer, new Ship(ShipName.Destroyer)},
                    {ShipName.Submarine, new Ship(ShipName.Submarine)},
                    {ShipName.TugBoat, new Ship(ShipName.TugBoat)}
               };
               shipList2 = new Dictionary<ShipName, Ship>{
                    {ShipName.AircraftCarrier, new Ship(ShipName.AircraftCarrier)},
                    {ShipName.Battleship, new Ship(ShipName.Battleship)},
                    {ShipName.Destroyer, new Ship(ShipName.Destroyer)},
                    {ShipName.Submarine, new Ship(ShipName.Submarine)},
                    {ShipName.TugBoat, new Ship(ShipName.TugBoat)}
               };

               _moveTestShip = new MoveableShip();
               player1Grid = new Vector2(vp.Width / 14, 60);
               enemyGrid = new Vector2(8 * vp.Width / 13, 60);
               str = "Hit the CONNECT button to find an oppenent.\nThen hit the NEW GAME button to start playing!";
               shotCoordinate = "";
               currentPlayer = "BATTLESHIP";

               newAttack = default(AttackResult);
               ableToShoot = false;
               easterEgg = false;
               _playerIndex = 0;
               _otherPlayer = (_playerIndex + 1) % 2;

               base.Initialize();
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  L O A D C O N T E N T   L O A D C O N T E N T   L O A D C O N T E N T   L O A D C O N T E N T
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// LoadContent will be called once per game and is the place to load all of your content.
          protected override void LoadContent(){
               // Create a new SpriteBatch, which can be used to draw textures.
               _spriteBatch = new SpriteBatch(GraphicsDevice);

               motorOil = Content.Load<SpriteFont>("Fonts\\motorOil");
               museo = Content.Load<SpriteFont>("Fonts\\museo");
               museo29 = Content.Load<SpriteFont>("Fonts\\museo29");
               museoSans = Content.Load<SpriteFont>("Fonts\\museoSans");

               background0 = Content.Load<Texture2D>("Graphics\\background0");
               background = background0;
               background1 = Content.Load<Texture2D>("Graphics\\background");
               water = Content.Load<Texture2D>("Graphics\\gridTexWater");
               white = Content.Load<Texture2D>("Graphics\\gridTex");
               red = Content.Load<Texture2D>("Graphics\\gridTexRed");
               cursor = Content.Load<Texture2D>("Graphics\\cursor50");
               ship = Content.Load<Texture2D>("Graphics\\ship");

               var testShipPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + 10, 450);
               _moveTestShip.Initialize(Content.Load<Texture2D>("Graphics\\ship2"), testShipPos);

               player1 = new Player(shipList1, player1Grid, water, red, white, ship);
               enemyPlayer = new Player(shipList2, enemyGrid, water, red, white, ship);
               //TODO: initialize only local player...

               AddDeployedPlayer(player1);
               AddDeployedPlayer(enemyPlayer);

               player1.EnemyGrid.Changed += SeaGridChanged;
               //_players[_otherPlayer].EnemyGrid.Changed += SeaGridChanged;

               pbNewGame = new PB1("Graphics\\newGame");
               pbNewGame.LoadContent(Content, new Rectangle(0, 0, vp.Width, 2 * vp.Height / 12));
               pbNewGame.ButtonDown += pbcD;
               pbNewGame.ButtonUp += pbcU;

               pbConnect = IsHost ? new PB1("Graphics\\host") : new PB1("Graphics\\connect");
               pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
               pbConnect.ButtonDown += pbcD;
               pbConnect.ButtonUp += pbcU;

               pbViewHide = new PB1("Graphics\\hide");
               pbViewHide.ButtonDown += pbcD;
               pbViewHide.ButtonUp += pbcU;

               pbExit = new PB1("Graphics\\exit");
               pbExit.LoadContent(Content, new Rectangle(0, 0, vp.Width, 8 * vp.Height / 12));
               pbExit.ButtonDown += pbcD;
               pbExit.ButtonUp += pbcU;
          }

          /// UnloadContent will be called once per game and is the place to unload game-specific content.
          protected override void UnloadContent(){ }
          
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V E N T F U N C T I O N S   E V
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          private void SeaGridChanged(object sender, TileEventArgs e){
               clicker = (SeaGridAdapter)sender;
               shotRow = e.X;
               shotCol = e.Y;
          }

          private void pbcD(object sender, EventArgs e){
               if (sender.Equals(pbExit)){
                    Exit();
               }
               else if (sender.Equals(pbConnect) && IsHost && pbConnect.Asset == "Graphics\\host"){
                    pbConnect.Asset = "Graphics\\disconnect";
                    pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
                    if (_networkManager.Running)
                         _networkManager.Disconnect();
                    str = _networkManager.Running.ToString();
               }
               else if (sender.Equals(pbConnect) && IsHost && pbConnect.Asset == "Graphics\\disconnect"){
                    pbConnect.Asset = "Graphics\\host";
                    pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
                    if (_networkManager.Running)
                         _networkManager.Disconnect();
                    str = _networkManager.Running.ToString();
               }
               else if (sender.Equals(pbConnect) && pbConnect.Asset == "Graphics\\connect"){
                    pbConnect.Asset = "Graphics\\disconnect";
                    pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
                    if (_networkManager.Running)
                         _networkManager.Disconnect();
                    str = _networkManager.Running.ToString();
               }
               else if (sender.Equals(pbConnect) && pbConnect.Asset == "Graphics\\disconnect"){
                    pbConnect.Asset = "Graphics\\connect";
                    pbConnect.LoadContent(Content, new Rectangle(0, 0, vp.Width, 4 * vp.Height / 12));
                    if (_networkManager.Running)
                         _networkManager.Disconnect();
                    str = _networkManager.Running.ToString();
               }
               else if (sender.Equals(pbViewHide) && pbViewHide.Asset == "Graphics\\view"){ 
                    // HIDE THE SHIPS
                    pbViewHide.Asset = "Graphics\\hide";
                    pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
                    player1.EnemyGrid.Cheat = false;
               }
               else if (sender.Equals(pbViewHide) && pbViewHide.Asset == "Graphics\\hide"){ 
                    // VIEW THE SHIPS
                    pbViewHide.Asset = "Graphics\\view";
                    pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
                    player1.EnemyGrid.Cheat = true;
               }
               else if (sender.Equals(pbNewGame)){
                    pbNewGame.Texture = Content.Load<Texture2D>("Graphics\\randomize");
                    player1.Reset();
                    str = "RANDOMIZING...";
                    newAttack = default(AttackResult); // SETS TO NULL
               }
          }
          private void pbcU(object sender, EventArgs e){
               if (sender.Equals(pbConnect) && IsHost && pbConnect.Asset == "Graphics\\disconnect"){
                    _networkManager.Connect();
                    str = "CONNECTING...";
               }
               else if (sender.Equals(pbConnect) && IsHost && pbConnect.Asset == "Graphics\\host"){
                    str = "DISCONNECTED -- NOT HOSTING...";
               }

               else if (sender.Equals(pbConnect) && pbConnect.Asset == "Graphics\\disconnect"){
                    _networkManager.Connect();
                    str = "CONNECTING...";
               }

               else if (sender.Equals(pbConnect) && pbConnect.Asset == "Graphics\\connect"){
                    str = "DISCONNECTED.";
               }

               else if (sender.Equals(pbNewGame)){
                    pbNewGame.Texture = Content.Load<Texture2D>("Graphics\\newGame");
                    _networkManager.SendMessage(new NewGameMessage());
                    enemyPlayer.Reset();
                    str = "Guess where the enemy's ships are on the grid!";
                    currentPlayer = "YOUR TURN!";
                    ableToShoot = true;
               }
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A T E   U P D A
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Allows the game to run logic (updating the world, checking for collisions, input, etc.
          /// <param name="gameTime">Provides a snapshot of timing values.</param>
          protected override void Update(GameTime gameTime){
               if (Keyboard.GetState().IsKeyDown(Keys.Escape)){
                    _graphics.IsFullScreen = false;
                    Exit();
               }

               if (newAttack == null && _networkManager.Running)
                    background = _networkManager.Connection() == 0 ? background0 : background1;

               // LOCAL PLAYER, PROCESS INCOMING NETWORK MESSAGES
               this.ProcessNetworkMessages();

               currMouseState = Mouse.GetState();
               _moveTestShip.update(gameTime, _graphics.GraphicsDevice);

               if (_moveTestShip.Position == new Vector2(GraphicsDevice.Viewport.Width - _moveTestShip.Width, 0) && !easterEgg){
                    pbViewHide.LoadContent(Content, new Rectangle(0, 0, vp.Width, 6 * vp.Height / 12));
                    easterEgg = true;
               }

               if (player1.EnemyGrid.Rect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && ableToShoot == true && !str.Contains("OVER")){
                    ableToShoot = false;
                    player1.Update(); //This sets the global shotROW and shotCOL
                    if (_networkManager.Connection() > 0){
                         _networkManager.SendMessage(new VectorMessage(shotRow, shotCol));
                         shotCoordinate = (shotRow + 1) + ", " + (shotCol + 1);
                    }

                    //attacking = true;
               }

               if (_networkManager.Running)
                    pbNewGame.Update();

               pbConnect.Update();
               pbViewHide.Update();
               pbExit.Update();

               base.Update(gameTime);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  N E T W O R K I N G F U N C T I O N S   N E T W O R K I N G F U N C T I O N S   N E T W O R K 
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void ProcessNetworkMessages(){
               if (!_networkManager.Running) return;
               NetIncomingMessage im;
               while ((im = this._networkManager.ReadMessage()) != null)
               {
                    switch (im.MessageType)
                    {
                         case NetIncomingMessageType.VerboseDebugMessage:
                         case NetIncomingMessageType.DebugMessage:
                         case NetIncomingMessageType.WarningMessage:
                         case NetIncomingMessageType.ErrorMessage:
                              str = im.ReadString();
                              break;
                         case NetIncomingMessageType.StatusChanged:
                              switch ((NetConnectionStatus) im.ReadByte())
                              {
                                   case NetConnectionStatus.Connected:
                                        if (this.IsHost)
                                             str = "CONNECTED TO " + im.SenderConnection;
                                        else
                                             str = "CONNECTED TO" + im.SenderConnection;
                                        break;
                                   case NetConnectionStatus.Disconnected:
                                        str = !this.IsHost
                                             ? "{0} Disconnected"
                                             : "Disconnected from " + im.SenderEndPoint;
                                        break;
                                   case NetConnectionStatus.RespondedAwaitingApproval:
                                        NetOutgoingMessage hailMessage = this._networkManager.CreateMessage();
                                        hailMessage.Write("APPROVED");
                                        im.SenderConnection.Approve(hailMessage);
                                        break;
                              }
                              break;
                         case NetIncomingMessageType.Data:
                              var gameMessageType = (GameMessageTypes) im.ReadByte();
                              switch (gameMessageType)
                              {
                                   case GameMessageTypes.Vector:
                                        this.HandleVector(im);
                                        break;
                                   case GameMessageTypes.TileView:
                                        this.HandleTileView(im);
                                        break;
                                   case  GameMessageTypes.NewGame:
                                        this.HandleNewGame(im);
                                        break;
                                   case GameMessageTypes.AttackResult:
                                        this.HandleAttackResult(im);
                                        break;
                                   case GameMessageTypes.PlayerStats:
                                        this.HandlePlayerStats(im);
                                        break;
                                   case GameMessageTypes.GameOver:
                                        this.HandleGameOver();
                                        break;
                              }
                              break;
                    }
                    this._networkManager.Recycle(im);
               }
          }

          private void HandleVector(NetIncomingMessage im){
               var message = new VectorMessage(im);
               int row = message.X;
               int col = message.Y;

               newAttack = player1.Shoot(row, col);
               TileView tileView = player1.ItemTileView(row, col);
               _networkManager.SendMessage(new TileViewMessage(row, col, tileView));
               _networkManager.SendMessage(new PlayerStatsMessage(player1.Shots, player1.Hits, player1.Misses, player1.PlayerGrid.ShipsKilled));
               _networkManager.SendMessage(new AttackResultMessage(newAttack));

               if (newAttack.Value == ResultOfAttack.Destroyed)
                    str = "Enemy destroyed your " + newAttack.Ship + "(" + (int)newAttack.Ship + ")" + "!";
               else
                    str = "Enemy " + newAttack.Text;

               shotCoordinate = (row + 1) + ", " + (col + 1);

               //change player turn if the last hit was a miss
               if (newAttack.Value != ResultOfAttack.ShotAlready){
                    currentPlayer = "YOUR TURN...";
                    ableToShoot = true;
               }
               else
                    ableToShoot = false;
          }

          private void HandleTileView(NetIncomingMessage im){
               var message = new TileViewMessage(im);
               int x = message.X;
               int y = message.Y;
               TileView tv = message.View;
               enemyPlayer.PlayerGrid.TvAssign(x, y, tv);
          }

          private void HandleNewGame(NetIncomingMessage im){
               var message = new NewGameMessage(im);
               this.player1.Reset();
               str = "Guess where the enemy's ships are on the grid!";
               currentPlayer = "ENEMY'S TURN...";
               enemyPlayer.Reset();
               ableToShoot = false;
          }

          private void HandleAttackResult(NetIncomingMessage im){
               var message = new AttackResultMessage(im);
               AttackResult ar = new AttackResult((ResultOfAttack) 1, " ", 0, 0){
                    Value = message.Value,
                    Text = message.Text,
                    Row = message.Row,
                    Column = message.Col
               };
               if (_players[_otherPlayer].AllDestroyed){
                    ar = new AttackResult(ResultOfAttack.GameOver, ar.Ship, "WIN! GAME OVER!", shotRow, shotCol);
                    _networkManager.SendMessage(new GameOverMessage());
               }

               str = Player == player1 ? "YOU " : "ENEMY ";
               str += ar.Text;

               //change player if the last hit was a miss
               if (ar.Value != ResultOfAttack.ShotAlready)
                    currentPlayer = "ENEMY'S TURN...";
               else
                    ableToShoot = true;
          }

          private void HandlePlayerStats(NetIncomingMessage im){
               var message = new PlayerStatsMessage(im);
               enemyPlayer.Shots = message.Shots;
               enemyPlayer.Hits = message.Hits;
               enemyPlayer.Misses = message.Misses;
               enemyPlayer.PlayerGrid.ShipsKilled = message.ShipsKilled;
          }

          private void HandleGameOver()
          {
               AttackResult ar = new AttackResult((ResultOfAttack) 1, " ", 0, 0);
               if (player1.AllDestroyed)
                    ar = new AttackResult(ResultOfAttack.GameOver, ar.Ship, "WINS! GAME OVER!", shotRow, shotCol);
               str = "ENEMY " + ar.Text;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          ///  D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D R A W   D
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// This is called when the game should draw itself.
          protected override void Draw(GameTime gameTime){
               // starts Drawing
               _spriteBatch.Begin();

               _spriteBatch.Draw(background, new Rectangle(0, 0, 900, 900), Color.SlateGray);

               pbNewGame.Draw(_spriteBatch);
               pbConnect.Draw(_spriteBatch);
               if (easterEgg)
                    pbViewHide.Draw(_spriteBatch);
               pbExit.Draw(_spriteBatch);

               player1.Draw(_spriteBatch); //This draws both the player and their enemy's grid, locally

               _moveTestShip.Draw(_spriteBatch);
               _spriteBatch.Draw(cursor, new Vector2(currMouseState.X, currMouseState.Y), Color.White);

               _spriteBatch.DrawString(motorOil, "PLAYER", new Vector2(vp.Width / 14, 30), Color.White);
               _spriteBatch.DrawString(motorOil, "ENEMY ", new Vector2(12 * vp.Width / 14 - 30, 30), Color.White);

               _spriteBatch.DrawString(museoSans, player1.ShipsLeft().ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(player1.ShipsLeft().ToString()).X, 200), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, "Ships", new Vector2(vp.Width / 2 - museoSans.MeasureString("Ships").X / 2, 200), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, enemyPlayer.ShipsLeft().ToString(), new Vector2(vp.Width / 2 + 50, 200), Color.Goldenrod);

               _spriteBatch.DrawString(museoSans, enemyPlayer.Shots.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(enemyPlayer.Shots.ToString()).X, 235), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, "Shots", new Vector2(vp.Width / 2 - museoSans.MeasureString("Shots").X / 2, 235), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, player1.Shots.ToString(), new Vector2(vp.Width / 2 + 50, 235), Color.Goldenrod);

               _spriteBatch.DrawString(museoSans, enemyPlayer.Hits.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(enemyPlayer.Hits.ToString()).X, 270), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, " Hits ", new Vector2(vp.Width / 2 - museoSans.MeasureString(" Hits ").X / 2, 270), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, player1.Hits.ToString(), new Vector2(vp.Width / 2 + 50, 270), Color.Goldenrod);

               _spriteBatch.DrawString(museoSans, enemyPlayer.Misses.ToString(), new Vector2(vp.Width / 2 - 50 - museoSans.MeasureString(enemyPlayer.Misses.ToString()).X, 305), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, "Misses", new Vector2(vp.Width / 2 - museoSans.MeasureString("Misses").X / 2, 305), Color.Goldenrod);
               _spriteBatch.DrawString(museoSans, player1.Misses.ToString(), new Vector2(vp.Width / 2 + 50, 305), Color.Goldenrod);

               _spriteBatch.DrawString(museo, str, new Vector2(vp.Width / 2 - museo.MeasureString(str).X / 2, 360), Color.Goldenrod);

               _spriteBatch.DrawString(museo, shotCoordinate, new Vector2(vp.Width / 2 - museo.MeasureString(shotCoordinate).X / 2, 397), Color.Goldenrod);

               if (str.Contains("OVER"))
                    currentPlayer = "PLAY AGAIN?";
               
               _spriteBatch.DrawString(museo29, currentPlayer, new Vector2(vp.Width / 2 - museo29.MeasureString(currentPlayer).X / 2, 435), Color.Gold);

               //stop drawing
               _spriteBatch.End();

               base.Draw(gameTime);
          }
     }
}
