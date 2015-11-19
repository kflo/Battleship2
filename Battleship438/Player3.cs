﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Battleship438
{
     class Player : IEnumerable<Ship>
     {
          protected static Random _Random = new Random();
          private static Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();
          private SeaGrid _playerGrid = new SeaGrid(_Ships);
          private ISeaGrid _enemyGrid;

          protected BattleshipGame _game;
          private int _shots;
          private int _hits;

          private int _misses;
          /// Returns the game that the player is part of.
          /// <value>The game</value>
          /// <returns>The game that the player is playing</returns>
          public BattleshipGame Game
          {
               get { return _game; }
               set { _game = value; }
          }

          /// Sets the grid of the enemy player
          /// <value>The enemy's sea grid</value>
          public ISeaGrid Enemy
          {
               set { _enemyGrid = value; }
          }

          public Player(BattleshipGame controller)
          {
               _game = controller;

               //for each ship add the ships name so the seagrid knows about them
               foreach (ShipName name in Enum.GetValues(typeof(ShipName)))
               {
                    if (name != ShipName.None)
                    {
                         _Ships.Add(name, new Ship(name));
                    }
               }

               RandomizeDeployment();
          }

          /// The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
          public ISeaGrid EnemyGrid
          {
               get { return _enemyGrid; }
               set { _enemyGrid = value; }
          }

          /// The PlayerGrid is just a normal SeaGrid where the players ships can be deployed and seen
          public SeaGrid PlayerGrid
          {
               get { return _playerGrid; }
          }

          /// ReadyToDeploy returns true if all ships are deployed
          public bool ReadyToDeploy
          {
               get { return _playerGrid.AllDeployed; }
          }

          public bool IsDestroyed
          {
               //Check if all ships are destroyed... -1 for the none ship
               get { return _playerGrid.ShipsKilled == Enum.GetValues(typeof(ShipName)).Length - 1; }
          }

          /// Returns the Player's ship with the given name.
          /// <param name="name">the name of the ship to return</param>
          /// <value>The ship</value>
          /// <returns>The ship with the indicated name</returns>
          /// <remarks>The none ship returns nothing/null</remarks>
          public Ship Ship(ShipName name)
          {
               if (name == ShipName.None)
                    return null;
               return _Ships[name];
          }

          /// <summary>
          /// The number of shots the player has made
          /// </summary>
          /// <value>shots taken</value>
          /// <returns>teh number of shots taken</returns>
          public int Shots
          {
               get { return _shots; }
          }

          public int Hits
          {
               get { return _hits; }
          }

          /// Total number of shots that missed
          /// <value>miss count</value>
          /// <returns>the number of shots that have missed ships</returns>
          public int Missed
          {
               get { return _misses; }
          }

          public int Score
          {
               get
               {
                    if (IsDestroyed)
                    {
                         return 0;
                    }
                    else
                    {
                         return (Hits * 12) - Shots - (PlayerGrid.ShipsKilled * 20);
                    }
               }
          }

          /// Makes it possible to enumerate over the ships the player has.
          /// <returns>A Ship enumerator</returns>
          public IEnumerator<Ship> GetShipEnumerator()
          {
               Ship[] result = new Ship[_Ships.Values.Count + 1];
               _Ships.Values.CopyTo(result, 0);
               List<Ship> lst = new List<Ship>();
               lst.AddRange(result);

               return lst.GetEnumerator();
          }

          IEnumerator<Ship> IEnumerable<Ship>.GetEnumerator()
          {
               return GetShipEnumerator();
          }


          /// <summary>
          /// Makes it possible to enumerate over the ships the player
          /// has.
          /// </summary>
          /// <returns>A Ship enumerator</returns>
          public IEnumerator GetEnumerator()
          {
               Ship[] result = new Ship[_Ships.Values.Count + 1];
               _Ships.Values.CopyTo(result, 0);
               List<Ship> lst = new List<Ship>();
               lst.AddRange(result);

               return lst.GetEnumerator();
          }

          /// Virtual Attack allows the player to shoot
          public virtual AttackResult Attack()
          {
               //human does nothing here...
               return null;
          }

          /// Shoot at a given row/column
          /// <param name="row">the row to attack</param>
          /// <param name="col">the column to attack</param>
          /// <returns>the result of the attack</returns>
          internal AttackResult Shoot(int row, int col)
          {
               _shots += 1;
               AttackResult result = default(AttackResult);
               result = EnemyGrid.HitTile(row, col);

               switch (result.Value)
               {
                    case ResultOfAttack.Destroyed:
                    case ResultOfAttack.Hit:
                         _hits += 1;
                         break;
                    case ResultOfAttack.Miss:
                         _misses += 1;
                         break;
               }
               return result;
          }

          public virtual void RandomizeDeployment()
          {
               bool placementSuccessful = false;
               Direction heading = default(Direction);

               //for each ship to deploy in shipist

               foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName)))
               {
                    if (shipToPlace == ShipName.None)
                         continue;

                    placementSuccessful = false;

                    //generate random position until the ship can be placed
                    do
                    {
                         int dir = _Random.Next(2);
                         int x = _Random.Next(0, 11);
                         int y = _Random.Next(0, 11);


                         if (dir == 0)
                         {
                              heading = Direction.UpDown;
                         }
                         else
                         {
                              heading = Direction.LeftRight;
                         }

                         //try to place ship, if position unplaceable, generate new coordinates
                         try
                         {
                              //PlayerGrid.MoveShip(x, y, shipToPlace, heading);
                              placementSuccessful = true;
                         }
                         catch
                         {
                              placementSuccessful = false;
                         }
                    } while (!placementSuccessful);
               }
          }

          public void Initialize(Texture2D texture, Vector2 position) {
               PlayerTexture = texture;
               Position = position;
          }

          public void Update() {
          }

          public void Draw(SpriteBatch spriteBatch) {
               spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
          }
     }
}
