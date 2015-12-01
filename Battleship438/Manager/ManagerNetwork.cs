using System;
using Battleship438.Library;
using Lidgren.Network;

namespace Battleship438.Manager
{
     class ManagerNetwork
     {
          public NetClient Client;
          public Library.Player Player { get; set;}
          
          public bool Start()
          {
               Client = new NetClient(new NetPeerConfiguration("Battleship438"));
               Client.Configuration.EnableMessageType(NetIncomingMessageType.Data);
               Client.Configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               Client.Start();
               Player = new Library.Player("Kevin F", 20, 10);  
               //TODO: NEED TO ACQUIRE PLAYER1/2 FROM SERVER
               var outMsg = Client.CreateMessage();
               outMsg.Write((byte)PacketType.Login);
               outMsg.Write(Player.Name);
               outMsg.Write(Player.X);
               outMsg.Write(Player.Y);
               Client.Connect("localhost", 14241, outMsg);
               return EstablishInfo();
          }

          public bool Active { get; set; }

          public void Send(string msg)
          {
               var outMsg = Client.CreateMessage(msg);
               Client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
          }


          private bool EstablishInfo(){
               var time = DateTime.Now;
               NetIncomingMessage inc;
               while (true){
                    if(DateTime.Now.Subtract(time).Seconds > 2) return false;
                    
                    if ((inc = Client.ReadMessage()) == null) continue;

                    switch (inc.MessageType)
                    {
                         case NetIncomingMessageType.Data:
                              var data = inc.ReadByte();
                              if (data == (byte)PacketType.Login){
                                   Active = inc.ReadBoolean();
                                   if (Active){
                                        Player.X = inc.ReadInt32();
                                        Player.Y = inc.ReadInt32();
                                        return true;
                                   }
                                   return false;
                              }
                              return false;
                    }
               }
          }

          public void Update()
          {
               NetIncomingMessage inc;
               
          }
     }
}
