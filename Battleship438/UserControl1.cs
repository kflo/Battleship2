using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Battleship438
{
     public partial class PB1 : UserControl
     {
          private Texture2D texture;
          private Rectangle rect;
          private string str;

          public event EventHandler ButtonClick;

          private void PB1_Load(object sender, EventArgs e)          {
          }

          public PB1(string str) {
               this.str = str;
               InitializeComponent();
          }

          public void LoadContent(ContentManager content)
          {
               texture = content.Load<Texture2D>(str);
               rect = new Rectangle(0, 0, texture.Width, texture.Height);
          }

          public void Draw(SpriteBatch spriteBatch) {
               spriteBatch.Draw(texture, rect, Color.White);
          }
         
     }
}
