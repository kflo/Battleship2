using Lidgren.Network;
using Microsoft.Xna.Framework;
using Battleship438.Library;

namespace Battleship438.Manager
{
     public class ClientNetworkManager : INetworkManager   {
          //=======================================================================//
          #region Constants and Fields

          private bool _isDisposed;
          public NetClient Client { get; private set; }
          public Library.Player Player { get; set; }

          #endregion
          //=======================================================================//
          #region Public Methods and Operators

          public void Connect(){
               var config = new NetPeerConfiguration("Battleship438"){
                    //SimulatedMinimumLatency = 0.2f,
                    // SimulatedLoss = 0.1f
               };

               config.EnableMessageType(NetIncomingMessageType.WarningMessage);
               config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
               config.EnableMessageType(NetIncomingMessageType.Error);
               config.EnableMessageType(NetIncomingMessageType.Data);
               config.EnableMessageType(NetIncomingMessageType.DebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               this.Client = new NetClient(config);
               this.Client.Start();
               Player = new Library.Player("Kevin F", 20, 10);

               var outMsg = Client.CreateMessage();
               outMsg.Write((byte)PacketType.Login);
               outMsg.Write(Player.Name);
               outMsg.Write(Player.X);
               outMsg.Write(Player.Y);
               this.Client.Connect("localhost", 14241, outMsg);
          }

          public NetOutgoingMessage CreateMessage(){
               return this.Client.CreateMessage();
          }

          public void Disconnect(){
               this.Client.Disconnect("Bye");
          }

          public void Dispose(){
               this.Dispose(true);
          }

          public NetIncomingMessage ReadMessage(){
               return this.Client.ReadMessage();
          }

          public void Recycle(NetIncomingMessage im){
               this.Client.Recycle(im);
          }

          public void SendMessage(int x, int y){
               NetOutgoingMessage om = this.Client.CreateMessage();
               om.Write(x);
               om.Write(y);
               this.Client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
          }

          #endregion
          //=======================================================================//
          #region Methods

          private void Dispose(bool disposing){
               if (this._isDisposed) return;
               if (disposing)
                    this.Disconnect();
               this._isDisposed = true;
          }
          #endregion
     }
}