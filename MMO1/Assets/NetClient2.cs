using UnityEngine;
using System.Collections;

public class NetClient2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    string Tf1 = "";
    string Tf2 = "";
 
    void OnGUI (){
        if (Network.peerType != NetworkPeerType.Disconnected && NetClient.connected == 1)
        {
        Tf1 = GUI.TextField (new Rect(10, 70, 200, 20), Tf1);
        if (GUI.Button(new Rect(10, 95, 100, 20), "Chat"))
        {
            networkView.RPC("BroadcastCL", RPCMode.Others, (NetClient.cliname + ": " + Tf1));
        }
        Tf2 = GUI.TextField(new Rect(10, 120, 200, 20), Tf2);
        }
    }
    [RPC]
    void BroadcastCL(string received){
        Tf2 = received;
    }
}
