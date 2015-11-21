using Battleship438;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
///using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

/// Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
/// all ships are deployed and if all ships are detroyed. A Player can also attach.
public class Player : IEnumerable<Ship>
{
     protected static Random _Random = new Random();
     protected BattleshipGame _game;

     private Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();
     private SeaGrid _playerGrid;
     private ISeaGrid _enemyGrid;
     private Texture2D red;
     private Texture2D water;

     private int _shots;
     private int _hits;
     private int _misses;

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// Returns the game that the player is part of.
     /// <returns>The game that the player is playing</returns>
     public BattleshipGame Game {
          get { return _game; }
          set { _game = value; }
     }

     public Player(BattleshipGame controller, Vector2 gridVector) {
          _game = controller;
          _playerGrid = new SeaGrid(_Ships, gridVector, water);
          //for each ship add the ships name so the seagrid knows about them
          foreach (ShipName name in Enum.GetValues(typeof(ShipName))) {
               if (name != ShipName.None) {
                    _Ships.Add(name, new Ship(name));
               }
          }
          RandomizeDeployment();
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     
     /// Sets the grid of the enemy player
     /// <value>The enemy's sea grid</value>
     public ISeaGrid Enemy {
          set { _enemyGrid = value; }
     }

     /// The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
     public ISeaGrid EnemyGrid {
          get { return _enemyGrid; }
          set { _enemyGrid = value; }
     }

     /// The PlayerGrid is just a normal SeaGrid where the players ships can be deployed and seen
     public SeaGrid PlayerGrid {
          get { return _playerGrid; }
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// returns true if all ships are deployed
     public bool ReadyToDeploy {
          get { return _playerGrid.AllDeployed; }
     }

     public bool IsDestroyed {
          //Check if all ships are destroyed... -1 for the none ship
          get { return _playerGrid.ShipsKilled == Enum.GetValues(typeof(ShipName)).Length - 1; }
     }

     /// Returns the Player's ship with the given name.
     /// <returns>The ship with the indicated name</returns>
     public Ship Ship {
          get {
               ShipName name = new ShipName();
               if (name == ShipName.None)
                    return null;
               return _Ships[name];
          }
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     public int Shots     {
          get { return _shots; }
     }

     public int Hits     {
          get { return _hits; }
     }

     public int Missed     {
          get { return _misses; }
     }
    
     public int Score     {
          get {
               if (IsDestroyed)
                    return 0;
               else
                    return (Hits * 12) - Shots - (PlayerGrid.ShipsKilled * 20);
          }
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     
     /// Makes it possible to enumerate over the ships the player has.
     /// <returns>A Ship enumerator</returns>
     public IEnumerator<Ship> GetShipEnumerator()     {
          Ship[] result = new Ship[_Ships.Values.Count + 1];
          _Ships.Values.CopyTo(result, 0);
          List<Ship> lst = new List<Ship>();
          lst.AddRange(result);

          return lst.GetEnumerator();
     }
     
     IEnumerator<Ship> IEnumerable<Ship>.GetEnumerator()     {
          return GetShipEnumerator();
     }

     /// Makes it possible to enumerate over the ships the player has.
     /// <returns>A Ship enumerator</returns>
     public IEnumerator GetEnumerator()     {
          Ship[] result = new Ship[_Ships.Values.Count + 1];
          _Ships.Values.CopyTo(result, 0);
          List<Ship> lst = new List<Ship>();
          lst.AddRange(result);

          return lst.GetEnumerator();
     }

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     /// Vitual Attack allows the player to shoot
     public virtual AttackResult Attack()     {
          //human does nothing here...
          return null;
     }

     /// Shoot at a given row/column
     /// <returns>the result of the attack</returns>
     internal AttackResult Shoot(int row, int col) {
          _shots += 1;
          AttackResult result = default(AttackResult);
          result = EnemyGrid.HitTile(row, col);

          switch (result.Value) {
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

     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

     public virtual void RandomizeDeployment() {
          bool placementSuccessful = false;
          Direction heading = default(Direction);

          //for each ship to deploy in shipist
          foreach (ShipName shipToPlace in Enum.GetValues(typeof(ShipName))) {
               if (shipToPlace == ShipName.None)
                    continue;
               placementSuccessful = false;
               //generate random position until the ship can be placed
               do {
                    int dir = _Random.Next(2);
                    int x = _Random.Next(0, 11);
                    int y = _Random.Next(0, 11);

                    if (dir == 0) 
                         heading = Direction.UpDown;
                    else
                         heading = Direction.LeftRight;

                    //try to place ship, if position unplaceable, generate new coordinates
                    try {
                         PlayerGrid.MoveShip(x, y, shipToPlace, heading, red);
                         placementSuccessful = true;
                    } catch {
                         placementSuccessful = false;
                    }
               } while (!placementSuccessful);
          }
     }
}