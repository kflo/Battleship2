using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace extensionMethods
{
     public class ExtendedNetBuffer : NetBuffer
     {
          public Vector2 ReadVector2()
          {
               uint num1 = NetBitWriter.ReadUInt32(this.Data, 32, (int)this.Position);
               this.Position += 32;
               uint num2 = NetBitWriter.ReadUInt32(this.Data, 32, (int)this.Position);
               this.Position += 32;
               return new Vector2(num1, num2);
          }


          public void WriteVector2(Vector2 source)
          {
               this.EnsureBufferSize(this.LengthBits + 32);
               NetBitWriter.WriteUInt32((uint)source.X, 32, this.Data, this.LengthBits);
               this.LengthBits += 32;
               this.EnsureBufferSize(this.LengthBits + 32);
               NetBitWriter.WriteUInt32((uint)source.Y, 32, this.Data, this.LengthBits);
               this.LengthBits += 32;
          }
     }
}