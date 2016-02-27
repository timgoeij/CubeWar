using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class multiplayercastle : MonoBehaviour 
{
	public List<string> waitingarmy = new List<string>();
	public List<Transform> army = new List<Transform>();
	List<Transform> temparmy = new List<Transform>();

	public Transform spawner;

	public Transform zwordprefab;
	public Transform lanceprefab;
	public Transform bowprefab;

	public float spawntime = 0;
	public float zwordspawn = 5f;
	public float lancespawn = 7.5f;
	public float bowspawn = 10f;
		
	// Update is called once per frame
	void Update () 
	{
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
						fighter = Network.Instantiate(zwordprefab,spawner.position,spawner.rotation,0) as Transform;

						if(Network.isClient)
						{
							fighter.tag = "enemyfighter";
							fighter.name = "swordfighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Server,fighter.GetComponent<NetworkView>().viewID);
						}
						else
						{
							fighter.tag = "playerfighter";
							fighter.name = "swordfighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Others,fighter.GetComponent<NetworkView>().viewID);
						}
						break;
					case "lancefighter":
						fighter = Network.Instantiate(lanceprefab,spawner.position,spawner.rotation,0) as Transform;

						if(Network.isClient)
						{
							fighter.tag = "enemyfighter";
							fighter.name = "lancefighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Server,fighter.GetComponent<NetworkView>().viewID);
						}
						else
						{
							fighter.tag = "playerfighter";
							fighter.name = "lancefighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Others,fighter.GetComponent<NetworkView>().viewID);
						}
						break;
					case "bowfighter":
						fighter = Network.Instantiate(bowprefab,spawner.position,spawner.rotation,0) as Transform;
						
						if(Network.isClient)
						{
							fighter.tag = "enemyfighter";
							fighter.name = "bowfighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Server,fighter.GetComponent<NetworkView>().viewID);
						}
						else
						{
							fighter.tag = "playerfighter";
							fighter.name = "bowfighter";
							army.Add(fighter);
							GetComponent<NetworkView>().RPC("updatearmy",RPCMode.Others,fighter.GetComponent<NetworkView>().viewID);
						}
						break;
					}

					if(i < waitingarmy.Count - 1)
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

	[RPC]
	void updatearmy(NetworkViewID viewID)
	{
		if(Network.isClient)
		{
			Transform fighter = NetworkView.Find(viewID).transform;

			if(fighter.name.Contains("swordfighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "swordfighter";
			
			if(fighter.name.Contains("lancefighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "lancefighter";
			
			if(fighter.name.Contains("bowfighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "bowfighter";

			fighter.tag = "playerfighter";
			temparmy.Add(fighter);
		}
		else
		{
			Transform fighter = NetworkView.Find(viewID).transform;

			if(fighter.name.Contains("swordfighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "swordfighter";
			
			if(fighter.name.Contains("lancefighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "lancefighter";
			
			if(fighter.name.Contains("bowfighter") && fighter.name.EndsWith("(Clone)"))
				fighter.name = "bowfighter";

			fighter.tag = "enemyfighter";
			temparmy.Add(fighter);
		}
	}
}
