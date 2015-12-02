using System;
using Battleship438Game.Model.Enum;

namespace Battleship438Game.Model
{
     /// The ISeaGrid defines the read only interface of a Grid. This allows each player to see and attack their opponents grid.
     public interface ISeaGrid {

          int Width { get; }

          int Height { get; }

          event EventHandler<TileEventArgs> Changed;

          TileView TileView(int row, int col);

          AttackResult HitTile(int row, int col);

     }
}