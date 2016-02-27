using UnityEngine;
using System.Collections;

public class multiplayercamerascript : MonoBehaviour 
{
	public GUIStyle lancebutton;
	public GUIStyle bowbutton;
	public GUIStyle healthbar;
	
	public GUISkin skin;

	private float speed = 15f;

	private multiplayerGamecontroller controller;
	private float maxserverhealth;
	private float maxclienthealth;

	private multiplayercastle mycastle;

	void Start()
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<multiplayerGamecontroller>();
		maxclienthealth = controller.clienthealth;
		maxserverhealth = controller.serverhealth;

		if(Network.isServer)
			mycastle = GameObject.FindWithTag("playercastle").GetComponent<multiplayercastle>();
		else
			mycastle = GameObject.FindWithTag("enemycastle").GetComponent<multiplayercastle>();
	}

	void Update () 
	{
		if(GetComponent<NetworkView>().isMine)
		{
			if(Input.GetKey(KeyCode.LeftArrow))
			{
				transform.position += transform.TransformDirection(Vector3.left) * speed * Time.deltaTime;
			}

			if(Input.GetKey(KeyCode.RightArrow))
			{
				transform.position += transform.TransformDirection(Vector3.right) * speed * Time.deltaTime;
			}
		}
		else
		{
			GetComponent<Camera>().enabled = false;
			gameObject.GetComponent<AudioListener>().enabled = false;
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		
		GUI.Box(viewportrect(0f, 0.8f, 1f, 0.2f),"");
		
		float serverhealthbar = 0.5f * (controller.serverhealth / maxserverhealth);
		float clienthealthbar = 0.5f * (controller.clienthealth / maxclienthealth);

		if(Network.isServer)
		{
			GUI.Label(viewportrect(0.05f, 0.80f,0.2f,0.75f),"player health: "+ controller.serverhealth.ToString());
			GUI.Box(viewportrect(0f, 0.85f, serverhealthbar, 0.02f),"",healthbar);
			
			GUI.Label(viewportrect(0.05f, 0.90f,0.2f,0.75f),"enemy health: "+ controller.clienthealth.ToString());
			GUI.Box(viewportrect(0f, 0.95f, clienthealthbar, 0.02f),"",healthbar);

			GUI.Label(viewportrect(0.55f,0.95f,0.2f,0.075f),"Money: "+ controller.servermoney);
			GUI.Label(viewportrect(0.75f,0.95f,0.2f,0.075f),"Enemy Money"+ controller.clientmoney);
		}
		else
		{
			GUI.Label(viewportrect(0.05f, 0.80f,0.2f,0.75f),"player health: "+ controller.clienthealth.ToString());
			GUI.Box(viewportrect(0f, 0.85f, clienthealthbar, 0.02f),"",healthbar);
			
			GUI.Label(viewportrect(0.05f, 0.90f,0.2f,0.75f),"enemy health: "+ controller.serverhealth.ToString());
			GUI.Box(viewportrect(0f, 0.95f, serverhealthbar, 0.02f),"",healthbar);

			GUI.Label(viewportrect(0.55f,0.95f,0.2f,0.075f),"Money: "+ controller.clientmoney);
			GUI.Label(viewportrect(0.75f,0.95f,0.2f,0.075f),"Enemy Money"+ controller.servermoney);
		}
		
		GUI.Label(viewportrect(0.55f,0.9f,0.2f,0.075f),"100");
		GUI.Label(viewportrect(0.7f,0.9f,0.2f,0.075f),"200");
		GUI.Label(viewportrect(0.85f,0.9f,0.2f,0.075f),"300");

		if(GUI.Button(viewportrect(0.55f,0.8f,0.1f,0.1f),""))
		{
			if(Network.isServer)
			{
				if(controller.servermoney >= 100)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.zwordspawn;

					mycastle.waitingarmy.Add("swordfighter");
					controller.updatemoney(100, true, 0);
				}

			}
			else
			{
				if(controller.clientmoney >= 100)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.zwordspawn;

					mycastle.waitingarmy.Add("swordfighter");
					controller.updatemoney(100, true, 0);
				}
			}
		}

		if(GUI.Button(viewportrect(0.7f,0.8f,0.1f,0.1f),"",lancebutton))
		{
			if(Network.isServer)
			{
				if(controller.servermoney >= 200)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.lancespawn;

					mycastle.waitingarmy.Add("lancefighter");
					controller.updatemoney(200, true, 0);
				}
			}
			else
			{
				if(controller.clientmoney >= 200)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.lancespawn;

					mycastle.waitingarmy.Add("lancefighter");
					controller.updatemoney(200, true, 0);
				}
			}
		}

		if(GUI.Button(viewportrect(0.85f,0.8f,0.1f,0.1f),"",bowbutton))
		{
			if(Network.isServer)
			{
				if(controller.servermoney >= 300)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.bowspawn;
					
					mycastle.waitingarmy.Add("bowfighter");
					controller.updatemoney(300, true, 0);
				}
			}
			else
			{
				if(controller.clientmoney >= 300)
				{
					if(mycastle.waitingarmy.Count == 0)
						mycastle.spawntime = Time.time + mycastle.bowspawn;
					
					mycastle.waitingarmy.Add("bowfighter");
					controller.updatemoney(300, true, 0);
				}
			}
		}
	}

	Rect viewportrect(float left,float top,float width,float height)
	{
		return new Rect (left * Screen.width, top * Screen.height,
		                 width * Screen.width, height * Screen.height);
	}
}
