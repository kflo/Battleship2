using System;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Battleship438.Manager
{
     public class ServerNetworkManager : INetworkManager{
          //=======================================================================//
          #region Constants and Fields

          private bool _isDisposed;
          public NetServer Server { get; private set; }

          #endregion
          //=======================================================================//
          #region Public Methods and Operators

          public void Connect(){
               var config = new NetPeerConfiguration("Battleship438"){
                    Port = Convert.ToInt32("14242"),
                    // SimulatedMinimumLatency = 0.2f, 
                    // SimulatedLoss = 0.1f 
               };
               config.EnableMessageType(NetIncomingMessageType.WarningMessage);
               config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
               config.EnableMessageType(NetIncomingMessageType.Error);
               config.EnableMessageType(NetIncomingMessageType.Data);
               config.EnableMessageType(NetIncomingMessageType.DebugMessage);
               config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               this.Server = new NetServer(config);
               this.Server.Start();
          }

          public NetOutgoingMessage CreateMessage(){
               return this.Server.CreateMessage();
          }

          public void Disconnect(){
               this.Server.Shutdown("Bye");
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

          public void SendMessage(int x, int y){
               NetOutgoingMessage om = this.Server.CreateMessage();
               om.Write(x);
               om.Write(y);
               this.Server.SendToAll(om, NetDeliveryMethod.ReliableUnordered);
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