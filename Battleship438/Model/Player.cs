using System;
using System.Collections;
using System.Collections.Generic;
using Battleship438Game.Model.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleship438Game.Model
{
     /// Player has its own _PlayerGrid, and can see an _EnemyGrid; it can also check if all ships are deployed/detroyed.
     public class Player : IEnumerable<Ship>
     {
          protected static Random _Random = new Random();
          private readonly Dictionary<ShipName, Ship> _ships;
          private Texture2D Water, Red, White, ShipTex;
          public TileView Tv { get; set; }


          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          // # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public Player(Dictionary<ShipName, Ship> shipList, Vector2 gridVector, Texture2D water, Texture2D red, Texture2D white, Texture2D shipTex) {
               Water = water;
               Red = red;
               White = white;
               ShipTex = shipTex;
               _ships = shipList;
               PlayerGrid = new SeaGrid(_ships, gridVector, water, red, white, shipTex);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
     
          /// Sets the grid of the enemy player 
          public SeaGridAdapter Enemy {
               set { EnemyGrid = value; }
          }

          /// The EnemyGrid is a ISeaGrid because you shouldn't be allowed to see the enemies ships
          public SeaGridAdapter EnemyGrid { get; private set; }

          /// The PlayerGrid is just a normal SeaGrid where the players ships can be deployed and seen
          public SeaGrid PlayerGrid { get; }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// returns true if all ships are deployed
          public bool ReadyToDeploy => PlayerGrid.AllDeployed;

          public bool AllDestroyed => PlayerGrid.ShipsKilled == 5;

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          public int Shots { get;  set; }

          public int Hits { get;  set; }

          public int Misses { get;  set; }

          public int ShipsLeft() { return _ships.Count - PlayerGrid.ShipsKilled; }

          public int Score {
               get {
                    if (AllDestroyed)
                         return 0;
                    else
                         return (Hits * 12) - Shots - (PlayerGrid.ShipsKilled * 20);
               }
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Makes it possible to enumerate over the ships the player has.
          public IEnumerator<Ship> GetShipEnumerator()     {
               Ship[] result = new Ship[_ships.Values.Count + 1];
               _ships.Values.CopyTo(result, 0);
               List<Ship> lst = new List<Ship>();
               lst.AddRange(result);

               return lst.GetEnumerator();
          }
     
          IEnumerator<Ship> IEnumerable<Ship>.GetEnumerator()     {
               return GetShipEnumerator();
          }

          /// Makes it possible to enumerate over the ships the player has.
          public IEnumerator GetEnumerator()     {
               Ship[] result = new Ship[_ships.Values.Count + 1];
               _ships.Values.CopyTo(result, 0);
               List<Ship> lst = new List<Ship>();
               lst.AddRange(result);

               return lst.GetEnumerator();
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #

          /// Shoot at a given row/column
          internal AttackResult Shoot(int row, int col) {
               AttackResult result = default(AttackResult);
               result = PlayerGrid.HitTile(row, col);
               //result = EnemyGrid.HitTile(row, col);

               switch (result.Value) {
                    case ResultOfAttack.Destroyed:
                         Tv = TileView.Hit;
                         break;
                    case ResultOfAttack.Hit:
                         Hits += 1;
                         Shots += 1;
                         Tv = TileView.Hit;
                         break;
                    case ResultOfAttack.Miss:
                         Misses += 1;
                         Shots += 1;
                         Tv = TileView.Miss;
                         break;
                    case ResultOfAttack.ShotAlready:
                         break;
                    case ResultOfAttack.GameOver:
                         break;
                    case ResultOfAttack.None:
                         break;
                    default:
                         throw new ArgumentOutOfRangeException();
               }
               return result;
          }

          public TileView ItemTileView(int x, int y){
               return PlayerGrid.TileView(x, y);
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          public void Reset()
          {
               PlayerGrid.Reset();
               PlayerGrid.Initialize();
               Shots = 0;
               Hits = 0;
               Misses = 0;
          }

          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          /// # # # # # # # # # # # # # # # # # # # # # # # # # # # # #
          public void Update()
          {
               //PlayerGrid.Update();
               EnemyGrid.Update();
          }

          public void Draw(SpriteBatch spriteBatch)
          {
               PlayerGrid.Draw(spriteBatch);
               EnemyGrid.Draw(spriteBatch);
          }
     }
}