
namespace MultiplayerGame.Networking.Messages{
    using Lidgren.Network;
    using Lidgren.Network.Xna;
    using Microsoft.Xna.Framework;
    using MultiplayerGame.Entities;

    public class VectorMessage : IGameMessage
    {
         public VectorMessage(NetIncomingMessage im)
        {
            this.Decode(im);
        }

        public VectorMessage(Vector2 shot) {
            Target = shot;
        }

        public Vector2 Target { get; set; }

        public GameMessageTypes MessageType {
            get
            {
                return GameMessageTypes.Vector;
            }
        }

        public void Decode(NetIncomingMessage im)
        {
            this.Target = im.ReadVector2();
        }

        public void Encode(NetOutgoingMessage om)
        {
            om.Write(this.Target);
        }

    }
}






          newAttack = Enemy.Shoot(shotRow, shotCol);

                    //game over when all players ships are destroyed
                    

                    //change player if the last hit was a miss/hit
                    if (newAttack.Value != ResultOfAttack.ShotAlready)
                    {
                         Player.EnemyGrid.Changed -= seaGridChanged;
                         _playerIndex = _otherPlayer;
                         _otherPlayer = (_playerIndex + 1) % 2;
                         Player.EnemyGrid.Changed += seaGridChanged;
                    }
     attacking = false;
