namespace Battleship438
{
     partial class PB1
     {
          /// <summary> 
          /// Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary> 
          /// Clean up any resources being used.
          /// </summary>
          /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
          protected override void Dispose(bool disposing)
          {
               if (disposing && (components != null))
               {
                    components.Dispose();
               }
               base.Dispose(disposing);
          }

          #region Component Designer generated code

          /// <summary> 
          /// Required method for Designer support - do not modify 
          /// the contents of this method with the code editor.
          /// </summary>
          private void InitializeComponent()
          {
               this.SuspendLayout();
               // 
               // PB1
               // 
               this.AccessibleDescription = "pushbutton1";
               this.AccessibleName = "PB1";
               this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
               this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
               this.Cursor = System.Windows.Forms.Cursors.Hand;
               this.Location = new System.Drawing.Point(1, 1);
               this.Name = "PB1";
               this.Size = new System.Drawing.Size(283, 72);
               this.Tag = "pushbutton";
               this.Click += new System.EventHandler(this.PB1_Load);
               this.ResumeLayout(false);

          }

          #endregion
     }
}
