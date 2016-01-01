using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Lidgren.Network;
using ThemeParkCommon;
using System.Threading;

namespace ThemeParkConsoleClient
{
    class Program
    {

        private static NetClient m_client;
        private static IPEndPoint m_masterServer;
        private static Dictionary<long, IPEndPoint[]> m_hostList;
        private static volatile bool isRunning = true;

        static void Main(string[] args)
        {

            Console.WriteLine(" ===========================");
            Console.WriteLine(" Theme Park - Console Client");
            Console.WriteLine(" ===========================");
            Console.WriteLine();

            m_hostList = new Dictionary<long, IPEndPoint[]>();

            Console.WriteLine("Server started; waiting 10 seconds...");
            System.Threading.Thread.Sleep(10000);

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            m_client = new NetClient(config);
            m_client.Start();


            Thread networkingHandler = new Thread(HandleIncominging);
            networkingHandler.Start();

            while (isRunning)
            {
                Console.WriteLine("What would you like to do?");
                performAction(Console.ReadLine());
                Thread.Sleep(1000);
            }
        }

        static void performAction(String action) {
            switch (action) {
                case "View Commands":
                    ViewCommands();
                    break;
                case "Get Server List":
                    GetServerList(CommonConstants.MasterServerIP);
                    break;
                case "Request NAT Intro":
                    Console.WriteLine("What is the hostid?");
                    long hostID = long.Parse(Console.ReadLine());
                    RequestNATIntroduction(hostID);
                    break;
                case "Quit":
                    isRunning = false;
                    break;
                default:
                    break;
            }
        }

        static void HandleIncominging()
        {
            while (isRunning)
            {
                NetIncomingMessage inc;
                while ((inc = m_client.ReadMessage()) != null)
                {
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine( inc.ReadString());
                            break;
                        case NetIncomingMessageType.UnconnectedData:
                            if (inc.SenderEndPoint.Equals(m_masterServer))
                            {
                                // it's from the master server - must be a host
                                var id = inc.ReadInt64();
                                var hostInternal = inc.ReadIPEndPoint();
                                var hostExternal = inc.ReadIPEndPoint();

                                m_hostList[id] = new IPEndPoint[] { hostInternal, hostExternal };

                                // update combo box
                                foreach (var kvp in m_hostList)
                                    Console.WriteLine(kvp.Key.ToString() + " (" + kvp.Value[1] + ")");

                            }
                            break;
                        case NetIncomingMessageType.NatIntroductionSuccess:
                            string token = inc.ReadString();
                            Console.WriteLine("Nat introduction success to " + inc.SenderEndPoint + " token is: " + token);
                            break;
                    }
                }
            }
        }

        public static void GetServerList(string masterServerAddress)
        {
            //
            // Send request for server list to master server
            //
            m_masterServer = new IPEndPoint(NetUtility.Resolve("localhost"), 3000);

            NetOutgoingMessage listRequest = m_client.CreateMessage();
            listRequest.Write((byte)MasterServerMessageType.RequestHostList);
            m_client.SendUnconnectedMessage(listRequest, m_masterServer);
        }

        public static void RequestNATIntroduction(long hostid)
        {
            if (hostid == 0)
            {
                Console.WriteLine("Select a host in the list first");
                return;
            }

            if (m_masterServer == null)
                throw new Exception("Must connect to master server first!");

            NetOutgoingMessage om = m_client.CreateMessage();
            om.Write((byte)MasterServerMessageType.RequestIntroduction);

            // write my internal ipendpoint
            IPAddress mask;
            om.Write(new IPEndPoint(NetUtility.GetMyAddress(out mask), m_client.Port));

            // write requested host id
            om.Write(hostid);

            // write token
            om.Write("mytoken");

            m_client.SendUnconnectedMessage(om, m_masterServer);
        }

        static void ViewCommands() {
            Console.WriteLine("View Commands");
            Console.WriteLine("Get Server List");
            Console.WriteLine("Request NAT Intro");
            Console.WriteLine("Quit");
        }
    }
}
