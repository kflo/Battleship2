using System.Net;
using Battleship438Game.Model;
using Battleship438Game.Network.Messages;
using Lidgren.Network;

namespace Battleship438Game.Network
{
     public class ClientNetworkManager : INetworkManager   {
          //=======================================================================//

          private bool _isDisposed;
          public bool Running { get; set; }
          public NetClient Client { get; private set; }
          public Player Player { get; set; }

          //=======================================================================//

          public void Connect(){
               var config = new NetPeerConfiguration("Battleship438"){
                    //SimulatedMinimumLatency = 0.2f,
                    // SimulatedLoss = 0.1f
               };

               config.EnableMessageType(NetIncomingMessageType.WarningMessage);
               config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
               config.EnableMessageType(NetIncomingMessageType.Error);
               config.EnableMessageType(NetIncomingMessageType.StatusChanged);
               config.EnableMessageType(NetIncomingMessageType.Data);
               config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               this.Client = new NetClient(config);
               this.Client.Start();
               Running = true;
               this.Client.Connect("localhost", 14241);
          }

          public int Connection(){
               return Client.ConnectionsCount;
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

          //=======================================================================//

          public void SendMessage(IGameMessage gameMessage)
          {
               NetOutgoingMessage om = this.Client.CreateMessage();
               om.Write((byte)gameMessage.MessageType);
               gameMessage.Encode(om);

               this.Client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
          }

          //=======================================================================//

          private void Dispose(bool disposing){
               if (this._isDisposed) return;
               if (disposing)
                    this.Disconnect();
               Running = false;
               this._isDisposed = true;
          }
     }
}