using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;

namespace HelloWorld2Photon
{
    class Program : IPhotonPeerListener
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        PhotonPeer peer;

        public Program()
        {
            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        }

        void Run()
        {
            //DebugLevel should usally be ERROR or Warning - ALL lets you "see" more details of what the sdk is doing.
            //Output is passed to you in the DebugReturn callback
            peer.DebugOut = DebugLevel.ALL;
            if (peer.Connect("localhost:5055", "Lite"))
            {
                do
                {
                    Debug.Write("."); //allows you to "see" the game loop is working, check your output-tab when running from within VS
                    peer.Service();
                    System.Threading.Thread.Sleep(50);
                }
                while (!Console.KeyAvailable);
            }
            else
                Console.WriteLine("Unknown hostname!");
            Console.WriteLine("Press any key to end program!");
            Console.ReadKey();
            //peer.Disconnect(); //<- uncomment this line to see a faster disconnect/leave on the other clients.
        }

        #region IPhotonPeerListener Members
        public void DebugReturn(DebugLevel level, string message)
        {
            // level of detail depends on the setting of peer.DebugOut
            Debug.WriteLine("\nDebugReturn:" + message); //check your output-tab when running from within VS
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            if (operationResponse.ReturnCode == 0)
                Console.WriteLine("\n---OnOperationResponse: OK - " + (OpCodeEnum)operationResponse.OperationCode + "(" + operationResponse.OperationCode + ")");
            else
            {
                Console.WriteLine("\n---OnOperationResponse: NOK - " + (OpCodeEnum)operationResponse.OperationCode + "(" + operationResponse.OperationCode + ")\n ->ReturnCode=" + operationResponse.ReturnCode
                  + " DebugMessage=" + operationResponse.DebugMessage);
                return;
            }

            switch (operationResponse.OperationCode)
            {
                case LiteOpCode.Join:
                    int myActorNr = (int)operationResponse.Parameters[LiteOpKey.ActorNr];
                    Console.WriteLine(" ->My PlayerNr (or ActorNr) is:" + myActorNr);

                    Console.WriteLine("Calling OpRaiseEvent ...");
                    Dictionary<byte, object> opParams = new Dictionary<byte, object>();
                    opParams[LiteOpKey.Code] = (byte)101;
                    //opParams[LiteOpKey.Data] = "Hello World!"; //<- returns an error, server expects a hashtable

                    Hashtable evData = new Hashtable();
                    evData[(byte)1] = "Hello Wolrd!";
                    opParams[LiteOpKey.Data] = evData;
                    peer.OpCustom((byte)LiteOpCode.RaiseEvent, opParams, true);
                    break;
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            Console.WriteLine("\n---OnStatusChanged:" + statusCode);
            switch (statusCode)
            {
                case StatusCode.Connect:
                    Console.WriteLine("Calling OpJoin ...");
                    Dictionary<byte, object> opParams = new Dictionary<byte, object>();
                    opParams[LiteOpKey.GameId] = "MyRoomName";
                    peer.OpCustom((byte)LiteOpCode.Join, opParams, true);
                    break;
                default:
                    break;
            }
        }

        public void OnEvent(EventData eventData)
        {
            Console.WriteLine("\n---OnEvent: " + (EvCodeEnum)eventData.Code + "(" + eventData.Code + ")");

            switch (eventData.Code)
            {
                case LiteEventCode.Join:
                    int actorNrJoined = (int)eventData.Parameters[LiteEventKey.ActorNr];
                    Console.WriteLine(" ->Player" + actorNrJoined + " joined!");

                    int[] actorList = (int[])eventData.Parameters[LiteEventKey.ActorList];
                    Console.Write(" ->Total num players in room:" + actorList.Length + ", Actornr List: ");
                    foreach (int actorNr in actorList)
                    {
                        Console.Write(actorNr + ",");
                    }
                    Console.WriteLine("");
                    break;

                case 101:
                    int sourceActorNr = (int)eventData.Parameters[LiteEventKey.ActorNr];
                    Hashtable evData = (Hashtable)eventData.Parameters[LiteEventKey.Data];
                    Console.WriteLine(" ->Player" + sourceActorNr + " say's: " + evData[(byte)1]);
                    break;

                case LiteEventCode.Leave:
                    int actorNrLeft = (int)eventData.Parameters[LiteEventKey.ActorNr];
                    Console.WriteLine(" ->Player" + actorNrLeft + " left!");
                    break;
            }
        }

        #endregion
    }

    enum OpCodeEnum : byte
    {
        Join = 255,
        Leave = 254,
        RaiseEvent = 253,
        SetProperties = 252,
        GetProperties = 251
    }

    enum EvCodeEnum : byte
    {
        Join = 255,
        Leave = 254,
        PropertiesChanged = 253
    }
}
