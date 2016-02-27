using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemycastle : MonoBehaviour 
{
	Gamecontroller controller;

	enum Enemystate
	{
		idle,
		waiting,
		randomspawn,
		checkspawn
	};

	Enemystate enemystate = Enemystate.idle; 

	List<string> waitingarmy = new List<string>();

	public Transform spawner;
	
	public Transform bowprefab;
	public Transform zwordprefab;
	public Transform lanceprefab;
	
	float spawntime = 0;
	float zwordspawn = 2.5f;
	float lancespawn = 5f;
	float bowspawn = 7.5f;

	float waitingtime = 20f;
	float time = 0;
	bool wait = true;

	void Start () 
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
	}

	void Update () 
	{
		switch(enemystate)
		{
		case Enemystate.idle: getstate();
			break;
		case Enemystate.randomspawn: spawnrandom();
			break;
		case Enemystate.checkspawn: spawnrandom();
			break;
		case Enemystate.waiting: waitfornextspawn();
			break;
		}

		bool stopspawn = spawner.GetComponent<spawner>().stopspawning;

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
						fighter.tag = "enemyfighter";
						fighter.name = "swordfighter";
						break;
					case "lancefighter":
						fighter = Instantiate(lanceprefab,spawner.position,spawner.rotation) as Transform;
						fighter.tag = "enemyfighter";
						fighter.name = "lancefighter";
						break;
					case "bowfighter":
						fighter = Instantiate(bowprefab,spawner.position,spawner.rotation) as Transform;
						fighter.tag = "enemyfighter";
						fighter.name = "bowfighter";
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

	void getstate()
	{
		float randompoint = Random.value;

		if(randompoint < 0.4f)
			enemystate = Enemystate.checkspawn;
		else if(randompoint < 0.8f)
			enemystate = Enemystate.randomspawn;
		else
			enemystate = Enemystate.waiting;
	}

	void spawnrandom()
	{
		float randompoint = Random.value;

		if(randompoint < 0.33f)
		{
			int count = Random.Range(1,Mathf.FloorToInt(controller.enemymoney / 100)+1);

			if(controller.enemymoney >= 100)
			{
				int setinwait = 0;

				while(setinwait < count)
				{
					waitingarmy.Add("swordfighter");
					controller.enemymoney -= 100;
					setinwait++;
				}

				enemystate = Enemystate.waiting;
			}
			else
				enemystate = Enemystate.waiting;
		}
		else if(randompoint < 0.66f)
		{
			int count = Random.Range(1,Mathf.FloorToInt(controller.enemymoney / 200)+1);

			if(controller.enemymoney >= 200)
			{
				int setinwait = 0;
				
				while(setinwait < count)
				{
					waitingarmy.Add("lancefighter");
					controller.enemymoney -= 200;
					setinwait++;
				}

				enemystate = Enemystate.waiting;
			}
			else
				enemystate = Enemystate.waiting;
		}
		else
		{
			int count = Random.Range(1,Mathf.FloorToInt(controller.enemymoney / 300)+1);
			
			if(controller.enemymoney >= 300)
			{
				int setinwait = 0;
				
				while(setinwait < count)
				{
					waitingarmy.Add("bowfighter");
					controller.enemymoney -= 300;
					setinwait++;
				}

				enemystate = Enemystate.waiting;
			}
			else
				enemystate = Enemystate.waiting;
		}
	}

	void waitfornextspawn()
	{
		if(wait)
		{
			time = Time.time + waitingtime;
			wait = false;
		}
		else
		{
			if(Time.time > time)
			{
				enemystate = Enemystate.idle;
				wait = true;
			}
		}
	}
}
