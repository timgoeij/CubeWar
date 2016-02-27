using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playercastle : MonoBehaviour 
{
	Gamecontroller controller;
	
	float maxplayerhealth;
	float maxenemyhealth;

	public GUIStyle lancebutton;
	public GUIStyle bowbutton;
	public GUIStyle healthbar;

	public GUISkin skin;

	public List<string> waitingarmy = new List<string>();
	public List<Transform> army = new List<Transform>();

	public Transform spawner;

	public Transform bowprefab;
	public Transform zwordprefab;
	public Transform lanceprefab;

	float spawntime = 0;
	float zwordspawn = 2.5f;
	float lancespawn = 5f;
	float bowspawn = 7.5f;

	bool pause = false;

	void Start() 
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
		maxplayerhealth = controller.playerhealth;
		maxenemyhealth = controller.enemyhealth;
	}

	void Update() 
	{
		bool stopspawn = spawner.GetComponent<spawner>().stopspawning;

		if(Input.GetKeyDown(KeyCode.P))
		{
			if(pause)
				pause = false;
			else
				pause = true;
		}
		
		if(pause)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;

		for(int i = 0; i < army.Count; i++)
		{
			if(army[i] == null)
				army.RemoveAt(i);
		}

		if(!stopspawn)
		{
			for(int i = 0; i < waitingarmy.Count; i++)
			{
				if(Time.time > spawntime)
				{
					string waiting = waitingarmy[i];
					Transform fighter;
					
					switch(waiting)
					{
					case "swordfighter":
						fighter = Instantiate(zwordprefab,spawner.position,spawner.rotation) as Transform;
						fighter.tag = "playerfighter";
						fighter.name = "swordfighter";
						army.Add(fighter);
						break;
					case "lancefighter":
						fighter = Instantiate(lanceprefab,spawner.position,spawner.rotation) as Transform;
						fighter.tag = "playerfighter";
						fighter.name = "lancefighter";
						army.Add(fighter);
						break;
					case "bowfighter":
						fighter = Instantiate(bowprefab,spawner.position,spawner.rotation) as Transform;
						fighter.tag = "playerfighter";
						fighter.name = "bowfighter";
						army.Add(fighter);
						break;
					}

					if(i < waitingarmy.Count -1)
					{
						string waitingnext = waitingarmy[i+1];

						switch(waitingnext)
						{
						case "swordfighter":
							waitingarmy.RemoveAt(i);
							spawntime = Time.time + zwordspawn;
							break;
						case "lancefighter":
							waitingarmy.RemoveAt(i);
							spawntime = Time.time + lancespawn;
							break;
						case "bowfighter":
							waitingarmy.RemoveAt(i);
							spawntime = Time.time + bowspawn;
							break;
						}
					}
					else
						waitingarmy.RemoveAt(i);
				}
			}
		}
	}

	void OnGUI()
	{
		if(pause)
		{
			if(GUI.Button(viewportrect(0.425f,0.20f,0.15f,0.15f),"Resume"))
			{
				pause = false;
			}

			if(GUI.Button(viewportrect(0.425f,0.35f,0.15f,0.15f),"Quit"))
			{
				pause = false;
				controller.enemywin = true;
				Application.LoadLevel(2);
			}
		}


		GUI.skin = skin;

		GUI.Box(viewportrect(0f, 0.8f, 1f, 0.2f),"");

		float playerhealthbar = 0.5f * (controller.playerhealth / maxplayerhealth);
		float enemyhealthbar = 0.5f * (controller.enemyhealth / maxenemyhealth);

		GUI.Label(viewportrect(0.05f, 0.80f,0.2f,0.75f),"player health: "+controller.playerhealth.ToString());
		GUI.Box(viewportrect(0f, 0.85f, playerhealthbar, 0.02f),"",healthbar);

		GUI.Label(viewportrect(0.05f, 0.90f,0.2f,0.75f),"enemy health: "+controller.enemyhealth.ToString());
		GUI.Box(viewportrect(0f, 0.95f, enemyhealthbar, 0.02f),"",healthbar);

		GUI.Label(viewportrect(0.55f,0.95f,0.2f,0.075f),"Money: "+ controller.playermoney.ToString());

		GUI.Label(viewportrect(0.55f,0.9f,0.2f,0.075f),"100");
		GUI.Label(viewportrect(0.7f,0.9f,0.2f,0.075f),"200");
		GUI.Label(viewportrect(0.85f,0.9f,0.2f,0.075f),"300");

		if(GUI.Button(viewportrect(0.55f,0.8f,0.1f,0.1f),""))
		{
			if(controller.playermoney >= 100)
			{
				if(!pause)
				{
					if(waitingarmy.Count == 0)
						spawntime = Time.time + zwordspawn;

					waitingarmy.Add("swordfighter");
					controller.playermoney -= 100;
				}
			}
		}

		if(GUI.Button(viewportrect(0.7f,0.8f,0.1f,0.1f),"",lancebutton))
		{
			if(controller.playermoney >= 200)
			{
				if(!pause)
				{
					if(waitingarmy.Count == 0)
						spawntime = Time.time + lancespawn;

					waitingarmy.Add("lancefighter");
					controller.playermoney -= 200;
				}
			}
		}

		if(GUI.Button(viewportrect(0.85f,0.8f,0.1f,0.1f),"",bowbutton))
		{
			if(controller.playermoney >= 300)
			{
				if(!pause)
				{
					if(waitingarmy.Count == 0)
						spawntime = Time.time + bowspawn;

					waitingarmy.Add("bowfighter");
					controller.playermoney -= 300;
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
