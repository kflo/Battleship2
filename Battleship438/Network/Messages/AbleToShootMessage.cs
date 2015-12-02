using System.Runtime.InteropServices;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class AbleToShootMessage : IGameMessage
     {

          private bool turn;

          public AbleToShootMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public AbleToShootMessage()
          {
          }

          public GameMessageTypes MessageType => GameMessageTypes.AbleToShoot;

          public void Decode(NetIncomingMessage im)
          {
               turn = im.ReadBoolean();
          }

          public void Encode(NetOutgoingMessage om)
          {
               om.Write(turn);
          }

     }
}