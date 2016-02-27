using UnityEngine;
using System.Collections;

public class Menubuttons : MonoBehaviour 
{
	Transform button;
	Color color;
	Gamecontroller controller;
	
	void Start () 
	{
		button = this.transform;
		color = button.GetComponent<TextMesh>().color;
		controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
	}

	void OnMouseDown () 
	{
		if(button.CompareTag("startbutton"))
		{
			Application.LoadLevel(1);
		}

		if(button.CompareTag("helpbutton"))
		{
			controller.help = true;
		}

		if(button.CompareTag("backbutton"))
		{
			controller.back = true;
		}

		if(button.CompareTag("quitbutton"))
		{
			Application.Quit();
		}

		if(button.CompareTag("multiplayerbutton"))
		{
			Destroy(controller.gameObject);
			Application.LoadLevel(3);
		}
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
