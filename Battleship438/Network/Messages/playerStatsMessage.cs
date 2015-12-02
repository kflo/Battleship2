using Battleship438Game.Model;
using Battleship438Game.Model.Enum;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class PlayerStatsMessage : IGameMessage
     {

          public int Shots { get; set; }
          public int Hits { get; set; }
          public int Misses { get; set; }
          public int ShipsKilled { get; set; }


          public PlayerStatsMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public PlayerStatsMessage(int shots, int hits, int misses, int shipsKilled){
               Shots = shots;
               Hits = hits;
               Misses = misses;
               ShipsKilled = shipsKilled;
          }

          public GameMessageTypes MessageType => GameMessageTypes.PlayerStats;

          public void Decode(NetIncomingMessage im)
          {
               Shots = im.ReadInt32();
               Hits = im.ReadInt32();
               Misses = im.ReadInt32();
               ShipsKilled = im.ReadInt32();
          }

          public void Encode(NetOutgoingMessage om)
          {
               om.Write(Shots);
               om.Write(Hits);
               om.Write(Misses);
               om.Write(ShipsKilled);
          }

     }
}