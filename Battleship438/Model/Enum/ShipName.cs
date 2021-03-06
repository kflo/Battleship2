using System.ComponentModel;

namespace Battleship438Game.Model.Enum
{
     /// The names of all of the ships in the game
     public enum ShipName
     {
          None = 0,
          [Description("Tug Boat")] TugBoat = 1,
          Submarine = 2,
          Destroyer = 3,
          Battleship = 4,
          [Description("Aircraft Carrier")] AircraftCarrier = 5

     }
}
