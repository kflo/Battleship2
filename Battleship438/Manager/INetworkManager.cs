using System;
using Lidgren.Network;

namespace Battleship438.Manager
{
    public interface INetworkManager : IDisposable
    {
        #region Public Methods and Operators

        void Connect();

        NetOutgoingMessage CreateMessage();

        void Disconnect();

        NetIncomingMessage ReadMessage();

        void Recycle(NetIncomingMessage im);

        void SendMessage(int x, int y);

        #endregion
    }
}