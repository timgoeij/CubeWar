using UnityEngine;
using System.Collections;

public class Gamecontroller : MonoBehaviour 
{
	public int playerhealth = 10000;
	public int enemyhealth = 10000;
	public int playermoney = 1500;
	public int enemymoney = 1500;

	public bool playerwin = false;
	public bool enemywin = false;
	public bool help = false;
	public bool back = false;

	Camera cam;

	void Start () 
	{
		cam = Camera.main;
	}

	void Update () 
	{
		Object.DontDestroyOnLoad(this.gameObject);

		if(playerhealth <= 0)
		{
			enemywin = true;
			Application.LoadLevel(2);
		}

		if(enemyhealth <= 0)
		{
			playerwin = true;
			Application.LoadLevel(2);
		}

		if(help)
		{
			gotohelp();
		}

		if(back) 
		{
			gotostart();
		}
	}

	void gotohelp()
	{
		if(Mathf.Round(cam.transform.eulerAngles.y) != 180)
			cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,Quaternion.Euler(0,180,0),Time.deltaTime * 2);
		else
			help = false;
	}

	void gotostart()
	{
		if(Mathf.Round(cam.transform.eulerAngles.y) != 360)
			cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,Quaternion.identity,Time.deltaTime * 2);
		else
			back = false;
	}

	public void enemyfighterdead(int reward)
	{
		playermoney += reward;
	}

	public void playerfighterdead(int reward)
	{
		enemymoney += reward;
	}

	public void enemygatedamage(int damage)
	{
		enemyhealth -= damage;
	}

	public void playergatedamage(int damage)
	{
		playerhealth -= damage;
	}
}