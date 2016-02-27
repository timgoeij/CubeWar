using UnityEngine;
using System.Collections;

public class multiplayerswordfighter : multiplayerarmy
{
	CharacterController charactercontroller;
	
	Transform[] playerpoints = new Transform[2];
	Transform[] enemypoints = new Transform[2];
	
	public Transform zword;
	multiplayerweapon sword;
	
	public AudioClip clip;
	float musictime = 0;
	float nextmusic = 1;
	float firstmusic = 0.5f;
	bool first = true;
	
	int currentpoint = 0;

	public override void Start() 
	{
		charactercontroller = this.GetComponent<CharacterController>();
		
		playerpoints[0] = GameObject.Find("playerpoint").transform;
		playerpoints[1] = GameObject.Find("playerpoint1").transform;
		
		enemypoints[0] = GameObject.Find("enemypoint").transform;
		enemypoints[1] = GameObject.Find("enemypoint1").transform;
		
		sword = zword.GetComponent<multiplayerweapon>();
		
		base.Start();
	}
	
	public override void Update() 
	{
		if(animState == animationstate.walk)
		{
			Vector3 target;
			Vector3 direction;
			Vector3 velocity;
			Vector3 rotdir;
			
			if(transform.CompareTag("playerfighter"))
			{
				if(currentpoint < playerpoints.Length)
				{
					target = playerpoints[currentpoint].position;
					direction = target - transform.position;
					velocity = this.transform.TransformDirection(Vector3.forward);
					rotdir = Vector3.RotateTowards(transform.forward,direction,speed * Time.deltaTime,0.0f);
					
					if(direction.magnitude < 0.75f)
						currentpoint++;
					else
						velocity = direction.normalized * speed;
					
					charactercontroller.Move(velocity * Time.deltaTime);
					this.transform.rotation = Quaternion.LookRotation(rotdir);
				}
			}
			else if(transform.CompareTag("enemyfighter"))
			{
				if(currentpoint < enemypoints.Length)
				{
					target = enemypoints[currentpoint].position;
					direction = target - transform.position;
					velocity = this.transform.TransformDirection(Vector3.forward);
					rotdir = Vector3.RotateTowards(transform.forward,direction,speed * Time.deltaTime,0.0f);
					
					if(direction.magnitude < 0.75f)
						currentpoint++;
					else
						velocity = direction.normalized * speed;
					
					charactercontroller.Move(velocity * Time.deltaTime);
					this.transform.rotation = Quaternion.LookRotation(rotdir);
				}
			}
		}
		else if(animState == animationstate.attack)
		{
			sword.damage = this.damage;
			
			if(Time.time > musictime)
			{
				if(first)
				{
					GetComponent<AudioSource>().PlayOneShot(clip);
					musictime = Time.time + nextmusic;
					first = false;
				}
				else
				{
					GetComponent<AudioSource>().PlayOneShot(clip);
					musictime = Time.time + firstmusic;
					first = true;
				}
			}
		}
		
		base.Update();
	}
}