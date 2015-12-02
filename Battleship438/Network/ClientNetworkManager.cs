using Lidgren.Network;
using Battleship438;
using Battleship438.Model;

namespace Battleship438.Network
{
     public class ClientNetworkManager : INetworkManager   {
          //=======================================================================//
          #region Constants and Fields

          private bool _isDisposed;
          public bool Running { get; set; } = false;
          public NetClient Client { get; private set; }
          public Player Player { get; set; }

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
               Running = true;
               
               var outMsg = Client.CreateMessage();
               outMsg.Write((byte)PacketType.Login);
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
               NetOutgoingMessage outX = CreateMessage();
               NetOutgoingMessage outY = CreateMessage();
               //Vector2 target = new Vector2(x,y);
               outX.Write(x);
               this.Client.SendMessage(outX, NetDeliveryMethod.ReliableOrdered);
               outY.Write(y);
               this.Client.SendMessage(outY, NetDeliveryMethod.ReliableOrdered);
          }

          #endregion
          //=======================================================================//
          #region Methods

          private void Dispose(bool disposing){
               if (this._isDisposed) return;
               if (disposing)
                    this.Disconnect();
               Running = false;
               this._isDisposed = true;
          }
          #endregion
     }
}