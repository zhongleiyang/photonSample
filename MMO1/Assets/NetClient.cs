using UnityEngine;
using System.Collections;

public class NetClient : MonoBehaviour {

   static public int connected = 0;
   public string msg = "o<=Name";
   static public string cliname = "";
 
    void Start(){
     Network.Connect("127.0.0.1",25000);
    }
 
    void OnGUI(){ 
     if(Network.peerType == NetworkPeerType.Disconnected){
      msg = "o<=Name";
      if(GUI.Button(new Rect(10,35,100,30),"Conect")){
       Network.Connect("127.0.0.1",25000);
       connected = 0; 
      }
     }
 
     if(Network.peerType != NetworkPeerType.Disconnected && connected == 0){
      msg = GUI.TextField(new Rect(10,10,300,20),msg); 
      if(GUI.Button(new Rect(10,35,100,30),"Login")){
       cliname=msg;  
       networkView.RPC ("EchoSV", RPCMode.Server, msg);
      connected = 1; 
      }
     } 

     if(Network.peerType != NetworkPeerType.Disconnected && connected == 1){
      msg = GUI.TextField(new Rect(10,10,300,20),msg); 
      if(GUI.Button(new Rect(10,35,100,30),"Off")){
       Network.Disconnect();
       msg = "o<=Name";
       connected = 0; 
      }
     }
    } 

    [RPC]
    void EchoCL(string received){
      msg = received ;
    }	
}
