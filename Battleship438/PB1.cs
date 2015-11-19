using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Battleship438
{
     public partial class PB1
     {
          private Texture2D texture;
          private string asset;
          private Rectangle rect;
          private bool clickDone = false;

          public Texture2D Texture {
               get   {return texture;}
               set   {texture = value;}     
          }

          public event EventHandler ButtonDown;
          public event EventHandler ButtonUp;

          public PB1(string asset) {
               this.asset = asset;
          }

          private void pbDown(object sender, EventArgs e) {
               //bubble the event up to the parent
               if (this.ButtonDown != null)
                    this.ButtonDown(this, e);
          }
          private void pbUp(object sender, EventArgs e)
          {
               //bubble the event up to the parent
               if (this.ButtonUp != null)
                    this.ButtonUp(this, e);
          }

          public void LoadContent(ContentManager content)
          {
               Texture = content.Load<Texture2D>(asset);
               rect = new Rectangle(0, 0, Texture.Width, Texture.Height);

          }

          public void CenterPB(Rectangle window)
          {
               rect = new Rectangle(window.Width / 2 - Texture.Width / 2, window.Height / 2 - Texture.Height / 2, Texture.Width, Texture.Height);
          }

          public void Update()
          {
               if (rect.Contains(new Point(Mouse.GetState().X,Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && clickDone == false)
               {
                    pbDown(this, EventArgs.Empty);
                    clickDone = true;
               }
               if (Mouse.GetState().LeftButton == ButtonState.Released && clickDone == true)
               {
                    pbUp(this, EventArgs.Empty);
                    clickDone = false;
               }
          }

          public void Draw(SpriteBatch spriteBatch, Vector2 pos) {
               spriteBatch.Draw(Texture, rect, Color.White);
          }
         
     }
}
