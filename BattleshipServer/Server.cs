using System;
using System.Collections.Generic;
using Lidgren.Network;
using Battleship438.Library;

namespace Battleship438.Server
{
     class Server {
          public static NetServer _server { get; private set; }
          private static List<Player> _players = new List<Player>();

          static void Main(string[] args) {
               var config = new NetPeerConfiguration("Battleship438")
               {
                    Port = 14241,
                    MaximumConnections = 2,
                    ConnectionTimeout = 5
               };
               config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
               config.EnableMessageType(NetIncomingMessageType.Data);
               config.EnableMessageType(NetIncomingMessageType.StatusChanged);
               config.EnableMessageType(NetIncomingMessageType.Receipt);
               _server = new NetServer(config);
               _server.Start();
               Console.WriteLine("Server started...");
               while (true)
               {
                    NetIncomingMessage im;



                    while ((im = _server.ReadMessage()) != null)
                    {
                         switch (im.MessageType)
                         {
                              case NetIncomingMessageType.VerboseDebugMessage:
                              case NetIncomingMessageType.DebugMessage:
                              case NetIncomingMessageType.WarningMessage:
                              case NetIncomingMessageType.ErrorMessage:
                                   Console.WriteLine(im.ReadString());
                                   break;
                              case NetIncomingMessageType.StatusChanged:
                                   Console.WriteLine("********" + NetIncomingMessageType.StatusChanged);
                                   Console.WriteLine("SENDER STATUS: " + im.SenderConnection.Status);
                                   switch ((NetConnectionStatus)im.ReadByte())
                                   {
                                        case NetConnectionStatus.Connected:
                                             Console.WriteLine(NetConnectionStatus.Connected + " -- Adding PLAYER");
                                             AddPlayer(im);
                                             break;
                                        case NetConnectionStatus.Disconnected:
                                             Console.WriteLine("{0} Disconnected");
                                             break;
                                        case NetConnectionStatus.RespondedAwaitingApproval:
                                             NetOutgoingMessage hailMessage = _server.CreateMessage("hail: APPROVE");
                                             im.SenderConnection.Approve(hailMessage);
                                             break;
                                   }
                                   break;
                              case NetIncomingMessageType.Error:
                                   break;
                              case NetIncomingMessageType.UnconnectedData:
                                   break;
                              case NetIncomingMessageType.ConnectionApproval:
                                   Console.WriteLine("********" + NetIncomingMessageType.ConnectionApproval);
                                   ConnectionApproval(im, _server);
                                   break;
                              case NetIncomingMessageType.Data:
                                   Console.WriteLine("********" + NetIncomingMessageType.Data);
                                   var number = im.ReadInt32();
                                   Console.WriteLine(number);
                                   break;
                              case NetIncomingMessageType.Receipt:
                                   Console.WriteLine("********" + NetIncomingMessageType.Receipt);
                                   break;
                              case NetIncomingMessageType.DiscoveryRequest:
                                   Console.WriteLine("********" + NetIncomingMessageType.DiscoveryRequest);
                                   break;
                              case NetIncomingMessageType.DiscoveryResponse:
                                   Console.WriteLine("********" + NetIncomingMessageType.DiscoveryResponse);
                                   break;
                              case NetIncomingMessageType.NatIntroductionSuccess:
                                   Console.WriteLine("********" + NetIncomingMessageType.NatIntroductionSuccess);
                                   break;
                              case NetIncomingMessageType.ConnectionLatencyUpdated:
                                   Console.WriteLine("********" + NetIncomingMessageType.ConnectionLatencyUpdated);
                                   break;
                              default:
                                   Console.WriteLine("default");
                                   throw new ArgumentOutOfRangeException();
                         }

                    }
               }
          }

          private static void ConnectionApproval (NetIncomingMessage inc, NetServer server) {
               Console.WriteLine("======== CONNECTION APPROVAL METHOD ========");
               Console.WriteLine("\nNew Connection Incoming...");
               var data = inc.ReadByte();
               if (data == (byte)PacketType.Login)
               {
                    var outMsg = server.CreateMessage();
                    outMsg.Write((byte)PacketType.Login);
                    outMsg.Write(true);
                    Console.WriteLine(inc.SenderConnection);
                    Console.WriteLine("Server sending reply...");
                    server.SendMessage(outMsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    //Console.WriteLine(inc.SenderConnection.Status);
                    //Console.WriteLine("Connected: " + server.ConnectionsCount);
                    Console.WriteLine("======== CONNECTION APPROVAL METHOD ======== \n");
               }
               else
               {
                    inc.SenderConnection.Deny("Didn't send correct information.");
               }
          }

          private static Player AddPlayer (NetIncomingMessage inc)
          {
               Player player = new Player(inc.ReadString(), inc.ReadInt32(), inc.ReadInt32());
               _players.Add(player);
               return player;
          }
     }
}



