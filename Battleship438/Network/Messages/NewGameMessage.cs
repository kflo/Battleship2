using System.Runtime.InteropServices;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class NewGameMessage : IGameMessage
     {

          private string _str;

          public NewGameMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public NewGameMessage()
          {
          }

          public GameMessageTypes MessageType => GameMessageTypes.NewGame;

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