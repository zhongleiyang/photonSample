using UnityEngine;
using System.Collections;

public class Net : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    string msg = "Click to Start";

    void OnGUI()
    {
        msg = GUI.TextField(new Rect(10, 10, 300, 20), msg);

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GUI.Button(new Rect(10, 35, 100, 30), "Start"))
            {
                Network.InitializeServer(10, 25000, false);
            }
        }

        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            msg = "Server OK!";
            if (GUI.Button(new Rect(10, 35, 100, 30), "Off"))
            {
                Network.Disconnect();
                msg = "Click to   start";
            }
        }

    } 

    [RPC]
    void EchoSV (string receivedS , NetworkMessageInfo info){
     var msgS= receivedS + "  connected!";
     networkView.RPC("EchoCL",info.sender,msgS);
    }

}
