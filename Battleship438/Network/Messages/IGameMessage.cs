using Lidgren.Network;

namespace Battleship438Game.Network.Messages
{
     public interface IGameMessage{

        GameMessageTypes MessageType { get; }

        void Decode(NetIncomingMessage im);

        void Encode(NetOutgoingMessage om);

    }
}