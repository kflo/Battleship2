using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship438
{
     /// The GameController is responsible for controlling the game,
     /// managing user input, and displaying the current state of the game.
     public static class GameController
     {

          private static BattleshipGame _theGame;
          private static Player player1;
          private static Player player2;
          private Vector2 playerGrid, enemyGrid;

          //private static Stack<GameState> _state = new Stack<GameState>();
          //private static AIOption _aiSetting;

          /*
          /// Returns the current state of the game, indicating which screen is currently being used
          public static GameState CurrentState
          {
               get { return _state.Peek(); }
          }
          */

          /// Returns the human player.
          public static Player Player1
          {
               get { return player1; }
          }

          /// Returns the computer player.z
          public static Player Player2
          {
               get { return player2; }
          }

          /*
          public GameController()
          {
               //bottom state will be quitting. If player exits main menu then the game is over
               _state.Push(GameState.Quitting);

               //at the start the player is viewing the main menu
               _state.Push(GameState.ViewingMainMenu);
          }
          */

          /// Starts a new game.
          public static void StartGame()
          {
               if (_theGame != null)
                    EndGame();

               //Create the game
               _theGame = new BattleshipGame();

               //create the players
               player1 = new Player(_theGame);
               player2 = new Player(_theGame);

               //AddHandler _human.PlayerGrid.Changed, AddressOf GridChanged
               player2.PlayerGrid.Changed += GridChanged;
               _theGame.AttackCompleted += AttackCompleted;

               //AddNewState(GameState.Deploying);
          }

          /// Stops listening to the old game once a new game is started
          private static void EndGame()
          {
               //RemoveHandler _human.PlayerGrid.Changed, AddressOf GridChanged
               player2.PlayerGrid.Changed -= GridChanged;
               _theGame.AttackCompleted -= AttackCompleted;
          }

          /// Listens to the game grids for any changes and redraws the screen when the grids change
          private static void GridChanged(object sender, EventArgs args)
          {
               DrawScreen();
               //SwinGame.RefreshScreen();
          }

          private static void PlayHitSequence(int row, int column, bool showAnimation)
          {
               if (showAnimation)
               {
                    AddExplosion(row, column);
               }

               Audio.PlaySoundEffect(GameSound("Hit"));

               DrawAnimationSequence();
          }

          private static void PlayMissSequence(int row, int column, bool showAnimation)
          {
               if (showAnimation)
               {
                    AddSplash(row, column);
               }

               Audio.PlaySoundEffect(GameSound("Miss"));

               DrawAnimationSequence();
          }

          /// Listens for attacks to be completed. Displays a message, plays sound and redraws the screen
          private static void AttackCompleted(object sender, AttackResult result)
          {
               bool isHuman = false;
               isHuman = object.ReferenceEquals(_theGame.Player, Player1);

               if (isHuman)
               {
                    Message = "You " + result.ToString();
               }
               else
               {
                    Message = "The AI " + result.ToString();
               }

               switch (result.Value)
               {
                    case ResultOfAttack.Destroyed:
                         PlayHitSequence(result.Row, result.Column, isHuman);
                         Audio.PlaySoundEffect(GameSound("Sink"));

                         break;
                    case ResultOfAttack.GameOver:
                         PlayHitSequence(result.Row, result.Column, isHuman);
                         Audio.PlaySoundEffect(GameSound("Sink"));

                         while (Audio.SoundEffectPlaying(GameSound("Sink")))
                         {
                              SwinGame.Delay(10);
                              SwinGame.RefreshScreen();
                         }

                         if (Player1.IsDestroyed)
                         {
                              Audio.PlaySoundEffect(GameSound("Lose"));
                         }
                         else
                         {
                              Audio.PlaySoundEffect(GameSound("Winner"));
                         }

                         break;
                    case ResultOfAttack.Hit:
                         PlayHitSequence(result.Row, result.Column, isHuman);
                         break;
                    case ResultOfAttack.Miss:
                         PlayMissSequence(result.Row, result.Column, isHuman);
                         break;
                    case ResultOfAttack.ShotAlready:
                         Audio.PlaySoundEffect(GameSound("Error"));
                         break;
               }
          }

          /// Completes the deployment phase of the game and switches to the battle mode (Discovering state)
          /// This adds the players to the game before switching state.
          public static void EndDeployment()
          {
               //deploy the players
               _theGame.AddDeployedPlayer(player1);
               _theGame.AddDeployedPlayer(_ai);

               SwitchState(GameState.Discovering);
          }

          /// Gets the player to attack the indicated row and column. Checks the attack result once the attack is complete
          public static void Attack(int row, int col)
          {
               AttackResult result = default(AttackResult);
               result = _theGame.Shoot(row, col);
               CheckAttackResult(result);
          }

          /// Gets the AI to attack.
          private static void AIAttack()
          {
               AttackResult result = default(AttackResult);
               result = _theGame.Player.Attack();
               CheckAttackResult(result);
          }

          /// Checks the results of the attack and switches to Ending the Game if the result was game over.
          /// <remarks>Gets the AI to attack if the result switched to the AI player.</remarks>
          private static void CheckAttackResult(AttackResult result)
          {
               switch (result.Value)
               {
                    case ResultOfAttack.Miss:
                         if (object.ReferenceEquals(_theGame.Player, ComputerPlayer))
                              AIAttack();
                         break;
                    case ResultOfAttack.GameOver:
                         SwitchState(GameState.EndingGame);
                         break;
               }
          }

          /// Handles the user SwinGame. Reads key and mouse input and converts these into
          /// actions for the game to perform. The actions performed depend upon the state of the game.
          public static void HandleUserInput()
          {
               //Read incoming input events
               SwinGame.ProcessEvents();

               switch (CurrentState)
               {
                    case GameState.ViewingMainMenu:
                         HandleMainMenuInput();
                         break;
                    case GameState.ViewingGameMenu:
                         HandleGameMenuInput();
                         break;
                    case GameState.AlteringSettings:
                         HandleSetupMenuInput();
                         break;
                    case GameState.Deploying:
                         HandleDeploymentInput();
                         break;
                    case GameState.Discovering:
                         HandleDiscoveryInput();
                         break;
                    case GameState.EndingGame:
                         HandleEndOfGameInput();
                         break;
                    case GameState.ViewingHighScores:
                         HandleHighScoreInput();
                         break;
               }

               UpdateAnimations();
          }

          /// Draws the current state of the game to the screen. What is drawn depends upon the state of the game.
          public static void DrawScreen()
          {
               DrawBackground();

               switch (CurrentState)
               {
                    case GameState.ViewingMainMenu:
                         DrawMainMenu();
                         break;
                    case GameState.ViewingGameMenu:
                         DrawGameMenu();
                         break;
                    case GameState.AlteringSettings:
                         DrawSettings();
                         break;
                    case GameState.Deploying:
                         DrawDeployment();
                         break;
                    case GameState.Discovering:
                         DrawDiscovery();
                         break;
                    case GameState.EndingGame:
                         DrawEndOfGame();
                         break;
                    case GameState.ViewingHighScores:
                         DrawHighScores();
                         break;
               }

               DrawAnimations();

               SwinGame.RefreshScreen();
          }

          /// Move the game to a new state. The current state is maintained so that it can be returned to.
          /// <param name="state">the new game state</param>
          public static void AddNewState(GameState state)
          {
               _state.Push(state);
               Message = "";
          }

          /// End the current state and add in the new state.
          /// <param name="newState">the new state of the game</param>
          public static void SwitchState(GameState newState)
          {
               EndCurrentState();
               AddNewState(newState);
          }

          /// Ends the current state, returning to the prior state
          public static void EndCurrentState()
          {
               _state.Pop();
          }

          /// Sets the difficulty for the next level of the game.
          /// <param name="setting">the new difficulty level</param>
          public static void SetDifficulty(AIOption setting)
          {
               _aiSetting = setting;
          }

     }
}