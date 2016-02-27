using UnityEngine;
using System.Collections;

public class NetworkController : MonoBehaviour 
{
	private const string TypeName = "Cube War";
	private const string GameName = "war";
	private string nickname = "youre name";
	private HostData[] HostList;
	private static string[] nicknames;
	public Transform playerprefab;
	private Transform player;

	private bool start = false;
	private int conections = 1;

	private multiplayerGamecontroller controller;
	
	void Start()
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<multiplayerGamecontroller>();
	}

	void Update () 
	{
		if(Network.isServer)
		{
			if(Network.connections.Length == conections)
			{
				if(!start)
				{
					GetComponent<NetworkView>().RPC("spawnplayer",RPCMode.All);
				}

				if(controller.serverhealth <= 0)
				{
					controller.clientwin = true;
				}

				if(controller.clienthealth <= 0)
				{
					controller.serverwin = true;
				}
			}
		}
	}

	void StartServer()
	{
		Network.InitializeServer(conections,2500,!Network.HavePublicAddress());
		MasterServer.RegisterHost(TypeName,GameName);
	}

	void RefreshHostList()
	{
		MasterServer.RequestHostList(TypeName);
	}

	void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnConnectedToServer()
	{
		print("Server Joined");
	}

	void OnServerInitialized()
	{
		print("server initialized");
	}

	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse == MasterServerEvent.RegistrationSucceeded)
			print("server registered");

		if(mse == MasterServerEvent.HostListReceived)
			HostList = MasterServer.PollHostList();
	}

	[RPC]
	void spawnplayer()
	{
		if(Network.isClient)
		{
			player = Network.Instantiate(playerprefab,new Vector3(100,20,50),Quaternion.Euler(45,90,0),0) as Transform;
			player.tag = "clientcamera";
			Camera.main.gameObject.SetActive(false);
		}
		else
		{
			Transform player = Network.Instantiate(playerprefab,new Vector3(150,20,200),Quaternion.Euler(45,270,0),0) as Transform;
			player.tag = "servercamera";
			Camera.main.gameObject.SetActive(false);
		}

		start = true;
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();

			nickname = GUI.TextField(new Rect(100, 400, 50, 25), nickname, 10);
			
			if (HostList != null)
			{
				for (int i = 0; i < HostList.Length; i++)
				{
					if(HostList[i].connectedPlayers < HostList[i].playerLimit)
					{
						if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), HostList[i].gameName))
							JoinServer(HostList[i]);
					}
				}
			}
		}
		else if(!start)
		{
			GUI.Box(new Rect(50,50,300,50),"WAIT FOR OTHER PLAYER");
		}
	}
}
