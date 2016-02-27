using UnityEngine;
using System.Collections;

public class camerascript : MonoBehaviour 
{
	float speed = 15f;
	
	void Update () 
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
}
