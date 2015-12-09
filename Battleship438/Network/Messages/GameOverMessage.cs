using System.Runtime.InteropServices;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class GameOverMessage : IGameMessage
     {

          private string _str;

          public GameOverMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public GameOverMessage()
          {
          }

          public GameMessageTypes MessageType => GameMessageTypes.GameOver;

          public void Decode(NetIncomingMessage im)
          {
               _str = im.ReadString();
          }

          public void Encode(NetOutgoingMessage om)
          {
               om.Write(_str);
          }

     }
}