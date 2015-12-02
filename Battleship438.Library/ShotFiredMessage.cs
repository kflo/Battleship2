
namespace MultiplayerGame.Networking.Messages{
    using Lidgren.Network;
    using Lidgren.Network.Xna;
    using Microsoft.Xna.Framework;
    using MultiplayerGame.Entities;

    public class ShotFiredMessage : IGameMessage
    {
        public ShotFiredMessage(Vector2 shot)
        {
            this.X = shot.X;
            this.Y = shot.Y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public GameMessageTypes MessageType {
            get           
                return GameMessageTypes.ShotFired;
        }

    }
}