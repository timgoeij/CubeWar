using UnityEngine;
using System.Collections;

public class multiplayerweapon : MonoBehaviour 
{
	public Transform target;
	public Transform raypoint;
	public int damage;
	float speed = 10f;
	bool damagehit = false;

	void Update () 
	{
		Ray ray;
		RaycastHit hit;
		
		if(transform.CompareTag("zword"))
		{
			ray = new Ray(raypoint.position,transform.TransformDirection(Vector3.left));
		}
		else
		{
			ray = new Ray(raypoint.position,transform.TransformDirection(Vector3.forward));
		}
		
		if(Physics.Raycast(ray, out hit, 1.5f))
		{
			if(hit.transform.name == "swordfighter")
			{
				if(!damagehit)
				{
					multiplayerswordfighter fighter = hit.transform.GetComponent<multiplayerswordfighter>();
					fighter.health -= damage;
					damagehit = true;
				}
				
				if(this.transform.CompareTag("arrow"))
				{
					if(GetComponent<NetworkView>().isMine)
					Network.Destroy(this.gameObject);
				}
			}
			else if(hit.transform.name == "lancefighter")
			{
				if(!damagehit)
				{
					multiplayerlancefighter fighter = hit.transform.GetComponent<multiplayerlancefighter>();
					
					fighter.health -= damage;
					damagehit = true;
				}
				
				if(this.transform.CompareTag("arrow"))
				{
					if(GetComponent<NetworkView>().isMine)
						Network.Destroy(this.gameObject);
				}
			}
			else if(hit.transform.name == "bowfighter")
			{
				if(!damagehit)
				{
					multiplayerbowfighter fighter = hit.transform.GetComponent<multiplayerbowfighter>();
					fighter.health -= damage;
					damagehit = true;
				}
				
				if(this.transform.CompareTag("arrow"))
				{
					if(GetComponent<NetworkView>().isMine)
						Network.Destroy(this.gameObject);
				}
			}
			else if(hit.transform.name == "gate")
			{
				if(!damagehit)
				{
					multiplayerGamecontroller controller = GameObject.FindWithTag("GameController").GetComponent<multiplayerGamecontroller>();

					if(GetComponent<NetworkView>().isMine)
						controller.updatehealth(damage);

					damagehit = true;
				}
				
				if(this.transform.CompareTag("arrow"))
				{
					if(GetComponent<NetworkView>().isMine)
						Network.Destroy(this.gameObject);
				}
			}
		}
		else
			damagehit = false;

		if(target != null)
		{
			Vector3 direction = target.position - this.transform.position;
			Vector3 rotdir = Vector3.RotateTowards(transform.forward,direction,speed * Time.deltaTime,0.0f);
			
			this.transform.position += direction.normalized * speed * Time.deltaTime;
			this.transform.rotation = Quaternion.LookRotation(rotdir);
		}
		else
		{
			if(this.transform.CompareTag("arrow"))
			{
				if(GetComponent<NetworkView>().isMine)
					Network.Destroy(this.gameObject);
			}
		}
	}
}