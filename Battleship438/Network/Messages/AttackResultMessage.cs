using Battleship438Game.Model;
using Battleship438Game.Model.Enum;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Battleship438Game.Network.Messages
{
     public class AttackResultMessage : IGameMessage
     {

          public ResultOfAttack Value { get; set; }
          public string Text { get; set; }
          public int Row { get; set; }
          public int Col { get; set; }


          public AttackResultMessage(NetIncomingMessage im){
               this.Decode(im);
          }

          public AttackResultMessage(AttackResult shot){
               Value = shot.Value;
               Text = shot.Text;
               Row = shot.Row;
               Col = shot.Column;
          }

          public GameMessageTypes MessageType => GameMessageTypes.AttackResult;

          public void Decode(NetIncomingMessage im)
          {
               this.Value = (ResultOfAttack) im.ReadInt32();
               this.Text = im.ReadString();
               this.Row = im.ReadInt32();
               this.Col = im.ReadInt32();
          }

          public void Encode(NetOutgoingMessage om)
          {
               om.Write((int) Value);
               om.Write(Text);
               om.Write(Row);
               om.Write(Col);
          }

     }
}