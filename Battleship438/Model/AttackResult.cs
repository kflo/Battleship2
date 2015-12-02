using Battleship438Game.Model.Enum;

namespace Battleship438Game.Model
{
     public class AttackResult {
          public ResultOfAttack Value { get; set; }
          public int Ship { get; }
          public string Text { get; set; }
          public int Row { get; set; }
          public int Column { get; set; }

          public AttackResult(ResultOfAttack value, string text, int row, int column) {
               Value = value;
               Text = text;
               Ship = 0;
               Row = row;
               Column = column;
          }

          public AttackResult(ResultOfAttack value, int ship, string text, int row, int column) : this(value, text, row, column) {
               Ship = ship;
          }

          public override string ToString() {
               if (Ship == 0)
                    return Text;
               return Text + " " + Ship;
          }
     }
}