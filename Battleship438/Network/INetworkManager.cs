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

          NetOutgoingMessage CreateMessage();

          void Disconnect();

          NetIncomingMessage ReadMessage();

          void Recycle(NetIncomingMessage im);

          void SendMessage(int x, int y);

          void SendMessage(IGameMessage gameMessage);

     }
}