using System;
using System.Net;
using Battleship438Game.Network.Messages;
using Lidgren.Network;

namespace Battleship438Game.Network
{
     public class ServerNetworkManager : INetworkManager{
          //=======================================================================//

          private bool _isDisposed;
          public NetServer Server { get; private set; }
          public bool Running { get; set; }

          //=======================================================================//

          public void Connect(){
               var config = new NetPeerConfiguration("Battleship438"){
                    Port = 14241, 
               };
               config.EnableMessageType(NetIncomingMessageType.WarningMessage);
               config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
               config.EnableMessageType(NetIncomingMessageType.Error);
               config.EnableMessageType(NetIncomingMessageType.StatusChanged);
               config.EnableMessageType(NetIncomingMessageType.Data);
               config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               this.Server = new NetServer(config);
               this.Server.Start();
               Running = true;
          }

          public int Connection(){
               return Server.ConnectionsCount;
          }

          public NetOutgoingMessage CreateMessage(){
               return this.Server.CreateMessage();
          }

          public void Disconnect(){
               this.Server.Shutdown("Bye");
               Running = false;
          }

          public void Dispose(){
               this.Dispose(true);
          }

          public NetIncomingMessage ReadMessage(){
               return this.Server.ReadMessage();
          }

          public void Recycle(NetIncomingMessage im){
               this.Server.Recycle(im);
          }

          //=======================================================================//

          public void SendMessage(IGameMessage gameMessage)
          {
               NetOutgoingMessage om = this.Server.CreateMessage();
               om.Write((byte)gameMessage.MessageType);
               gameMessage.Encode(om);

               this.Server.SendToAll(om, NetDeliveryMethod.ReliableOrdered);
          }

          //=======================================================================//

          private void Dispose(bool disposing){
               if (this._isDisposed) return;
               if (disposing)
                    this.Disconnect();
               this._isDisposed = true;
          }
     }
}