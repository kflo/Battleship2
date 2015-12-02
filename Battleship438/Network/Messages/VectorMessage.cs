using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class VectorMessage : IGameMessage
     {

          public int X;
          public int Y;

          public VectorMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public VectorMessage(int x, int y){
               X = x;
               Y = y;
          }

          public GameMessageTypes MessageType => GameMessageTypes.Vector;

          public void Decode(NetIncomingMessage im)
          {
               this.X = im.ReadInt32();
               this.Y = im.ReadInt32();
          }

          public void Encode(NetOutgoingMessage om)
          {
               om.Write(this.X);
               om.Write(this.Y);
          }

     }
}