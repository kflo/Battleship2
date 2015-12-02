using Battleship438Game.Model.Enum;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class TileViewMessage : IGameMessage
     {
          public int X;
          public int Y;
          public TileView View;


          public TileViewMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public TileViewMessage(int x, int y, TileView view)
          {
               X = x;
               Y = y;
               View = view;
          }

          public GameMessageTypes MessageType => GameMessageTypes.TileView;

          public void Decode(NetIncomingMessage im)
          {
               this.X = im.ReadInt32();
               this.Y = im.ReadInt32();
               this.View = (TileView) im.ReadInt32();
          }

          public void Encode(NetOutgoingMessage om) {
               om.Write(X);
               om.Write(Y);
               om.Write((int) View);
          }

     }
}