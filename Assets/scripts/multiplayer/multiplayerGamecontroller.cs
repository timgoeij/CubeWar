using UnityEngine;
using System.Collections;

public class multiplayerGamecontroller : MonoBehaviour 
{
	public int serverhealth = 10000;
	public int clienthealth = 10000;

	public int servermoney = 1500;
	public int clientmoney = 1500;

	public bool serverwin = false;
	public bool clientwin = false;

	// Use this for initialization
	void Start () 
	{
	
	}

	public void updatehealth(int damage) 
	{
		if(Network.isClient)
		{
			serverhealth -= damage;
			GetComponent<NetworkView>().RPC("updatehealthforplayers",RPCMode.Server,serverhealth);
		}
		else
		{
			clienthealth -= damage;
			GetComponent<NetworkView>().RPC("updatehealthforplayers",RPCMode.Others,clienthealth);
		}
	}

	public void updatemoney(int money, bool less, int forother)
	{
		if(Network.isClient)
		{
			if(forother == 1)
			{
				if(less)
				{
					servermoney -= money;
				}
				else
				{
					servermoney += money;
				}

				GetComponent<NetworkView>().RPC("updatemoneyforplayers", RPCMode.Server, servermoney, forother);
			}
			else
			{
				if(less)
				{
					clientmoney -= money;
				}
				else
				{
					clientmoney += money;
				}

				GetComponent<NetworkView>().RPC("updatemoneyforplayers", RPCMode.Server, clientmoney, forother);
			}
		}
		else
		{
			if(forother == 1)
			{
				if(less)
				{
					clientmoney -= money;
				}
				else
				{
					clientmoney += money;
				}

				GetComponent<NetworkView>().RPC("updatemoneyforplayers", RPCMode.Others, clientmoney, forother);
			}
			else
			{
				if(less)
				{
					servermoney -= money;
				}
				else
				{
					servermoney += money;
				}

				GetComponent<NetworkView>().RPC("updatemoneyforplayers", RPCMode.Others, servermoney, forother);
			}
		}
	}

	[RPC]
	void updatehealthforplayers(int newhealth)
	{
		if(Network.isServer)
			serverhealth = newhealth;
		else
			clienthealth = newhealth;
	}

	[RPC]
	void updatemoneyforplayers(int newmoney, int forother)
	{
		if(Network.isServer)
		{
			if(forother == 1)
				servermoney = newmoney;
			else
				clientmoney = newmoney;
		}
		else
		{
			if(forother == 1)
				clientmoney = newmoney;
			else
				servermoney = newmoney;
		}
	}
}