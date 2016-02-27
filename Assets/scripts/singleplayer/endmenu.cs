using UnityEngine;
using System.Collections;

public class endmenu : MonoBehaviour 
{
	Gamecontroller controller;

	public Transform titletext;
	public Transform scoretext;

	Transform button;
	Color color;

	void Start () 
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
		button = this.transform;
		color = button.GetComponent<TextMesh>().color;
	}

	void Update () 
	{
		if(controller.enemywin)
			titletext.GetComponent<TextMesh>().text = "You Lose";
		else if(controller.playerwin)
			titletext.GetComponent<TextMesh>().text = "You win";
		else
			titletext.GetComponent<TextMesh>().text = "";

		scoretext.GetComponent<TextMesh>().text = "You Score: "+ controller.playermoney.ToString();
	}

	void OnMouseDown()
	{
		Destroy(controller.gameObject);
		Application.LoadLevel(0);
	}

	void OnMouseEnter () 
	{
		button.GetComponent<TextMesh>().color = Color.black;
	}
	
	void OnMouseExit () 
	{
		button.GetComponent<TextMesh>().color = color;
	}
}
