using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship438Game
{
     public partial class PB1 {

          private Rectangle _rect;
          private bool _clickDone;

          public Texture2D Texture { get; set; }
          public string Asset { get; set; }

          public PB1(string asset) {
               this.Asset = asset;
          }

          //=======================================================================//
          //=======================================================================//

          public event EventHandler ButtonDown;
          public event EventHandler ButtonUp;

          private void pbDown(object sender, EventArgs e){
               //bubble the event up to the parent
               this.ButtonDown?.Invoke(this, e);
          }

          private void pbUp(object sender, EventArgs e){
               //bubble the event up to the parent
               this.ButtonUp?.Invoke(this, e);
          }

          //=======================================================================//
          //=======================================================================//

          public void LoadContent(ContentManager content, Rectangle window){
               Texture = content.Load<Texture2D>(Asset);
               _rect = new Rectangle(window.Width / 2 - Texture.Width / 2, window.Height / 2 - Texture.Height / 2, Texture.Width, Texture.Height);

          }
          
          public void Update(){
               if (_rect.Contains(new Point(Mouse.GetState().X,Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && _clickDone == false){
                    pbDown(this, EventArgs.Empty);
                    _clickDone = true;
               }
               if (Mouse.GetState().LeftButton == ButtonState.Released && _clickDone){
                    pbUp(this, EventArgs.Empty);
                    _clickDone = false;
               }
          }

          public void Draw(SpriteBatch spriteBatch) {
               spriteBatch.Draw(Texture, _rect, Color.White);
          }
         
     }
}
