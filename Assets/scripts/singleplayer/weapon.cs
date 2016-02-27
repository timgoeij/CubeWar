using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour 
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
					swordfighter fighter = hit.transform.GetComponent<swordfighter>();
					fighter.health -= damage;
					damagehit = true;
				}

				if(this.transform.CompareTag("arrow"))
					Destroy(this.gameObject,0.1f);
			}
			else if(hit.transform.name == "lancefighter")
			{
				if(!damagehit)
				{
					lancefighter fighter = hit.transform.GetComponent<lancefighter>();

					fighter.health -= damage;
					damagehit = true;
				}

				if(this.transform.CompareTag("arrow"))
					Destroy(this.gameObject,0.1f);
			}
			else if(hit.transform.name == "bowfighter")
			{
				if(!damagehit)
				{
					bowfighter fighter = hit.transform.GetComponent<bowfighter>();
					fighter.health -= damage;
					damagehit = true;
				}

				if(this.transform.CompareTag("arrow"))
					Destroy(this.gameObject,0.1f);
			}
			else if(hit.transform.name == "gate")
			{
				if(!damagehit)
				{
					Gamecontroller controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
					if(hit.transform.CompareTag("playergate"))
						controller.playergatedamage(damage);
					else
						controller.enemygatedamage(damage);

					damagehit = true;
				}
				
				if(this.transform.CompareTag("arrow"))
					Destroy(this.gameObject,0.1f);
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
				Destroy(this.gameObject);
		}
	}
}