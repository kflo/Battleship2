using System;
using Lidgren.Network;

namespace Battleship438.Network
{
    public interface INetworkManager : IDisposable
    {
         bool Running { get; set; }

         void Connect();

        NetOutgoingMessage CreateMessage();

        void Disconnect();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

        void SendMessage(int x, int y);
          
    }
}