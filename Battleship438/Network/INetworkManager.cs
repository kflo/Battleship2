using System;
using Battleship438Game.Network.Messages;
using Lidgren.Network;

namespace Battleship438Game.Network
{
     public interface INetworkManager : IDisposable
     {
          bool Running { get; set; }

          int Connection();
          
          void Connect();

          void Disconnect();

          NetOutgoingMessage CreateMessage();

          NetIncomingMessage ReadMessage();

          void Recycle(NetIncomingMessage im);

          void SendMessage(IGameMessage gameMessage);

     }
}