using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
///using Microsoft.Xna.Framework.Input;

namespace Battleship438.Model
{
     /// Player has its own _PlayerGrid, and can see an _EnemyGrid, it can also check if
     /// all ships are deployed and if all ships are detroyed. A Player can also attach.
     public class Player : IEnumerable<Ship>
     {
          protected static Random _Random = new Random();

          private Dictionary<ShipName, Ship> _Ships = new Dictionary<ShipName, Ship>();
          private SeaGrid _playerGrid;
          private SeaGridAdapter _enemyGrid;
          private Texture2D Water, Red, White, ShipTex;

          private int _shots;
          private int _hits;
          private int _misses;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

   
          public Player(Dictionary<ShipName, Ship> shipList, Vector2 gridVector, Texture2D water, Texture2D red, Texture2D white, Texture2D shipTex) {
               Water = water;
               Red = red;
               White = white;
               ShipTex = shipTex;
               _Ships = shipList;
               _playerGrid = new SeaGrid(_Ships, gridVector, water, red, white, shipTex);

               //RandomizeDeployment();
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     
          /// Sets the grid of the enemy player
          /// <value>The enemy's sea grid</value>
          public SeaGridAdapter Enemy {
               set { _enemyGrid = value; }

          }

          /// The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
          public SeaGridAdapter EnemyGrid {
               get { return _enemyGrid; }
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

          public bool allDestroyed {
               get { return _playerGrid.ShipsKilled == 5; }
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public int Shots { get { return _shots; } }

          public int Hits { get { return _hits; } }

          public int Misses { get { return _misses; } }

          public int shipsLeft() { return _Ships.Count - PlayerGrid.ShipsKilled; }

          public int Score {
               get {
                    if (allDestroyed)
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

          /// Shoot at a given row/column
          internal AttackResult Shoot(int row, int col) {
               AttackResult result = default(AttackResult);
               result = EnemyGrid.HitTile(row, col);

               switch (result.Value) {
                    case ResultOfAttack.Destroyed:
                    case ResultOfAttack.Hit:
                         _hits += 1;
                         _shots += 1;
                         break;
                    case ResultOfAttack.Miss:
                         _misses += 1;
                         _shots += 1;
                         break;
               }
               return result;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void Reset(Texture2D tex)
          {
               PlayerGrid.Reset();
               PlayerGrid.Initialize();
               _shots = 0;
               _hits = 0;
               _misses = 0;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public void Update()
          {
               //PlayerGrid.Update();
               EnemyGrid.Update();
          }

          public void Draw(SpriteBatch spriteBatch){
               _playerGrid.Draw(spriteBatch);
               _enemyGrid.Draw(spriteBatch);
          }

     }
}