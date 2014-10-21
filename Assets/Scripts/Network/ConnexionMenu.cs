using UnityEngine;
using System.Collections;

public class ConnexionMenu : MonoBehaviour {
	
	public string _ip = "127.0.0.1";
	public int _port = 6600;
	
	void OnGUI() {
		if(Network.peerType == NetworkPeerType.Disconnected) {
			if(GUI.Button(new Rect(100,100,100,25), "Start client")) {
				Network.Connect(_ip, _port);
                Screen.showCursor = true;
				
			}
			if(GUI.Button(new Rect(100,125,100,25), "Start server")) {
				Network.InitializeServer(10, _port, false);
                Screen.showCursor = true;
			}
		}else if(Network.peerType == NetworkPeerType.Client) {
			
			GUI.Label(new Rect(100,100,100,25), "Client");
			if(GUI.Button(new Rect(100,125,100,25), "Log Out")) {
				Network.Disconnect(250);
			}
		}else if(Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(100,100,100,25), "Server");
			GUI.Label(new Rect(100,125,100,25), "Connections " + Network.connections.Length);
			if(GUI.Button(new Rect(100,150,100,25), "Log Out")) {
				Network.Disconnect(250);
			}
		}
	}
}
