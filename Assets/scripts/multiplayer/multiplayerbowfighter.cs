using UnityEngine;
using System.Collections;

public class multiplayerbowfighter : multiplayerarmy
{
	CharacterController charactercontroller;
	
	Transform[] playerpoints = new Transform[2];
	Transform[] enemypoints = new Transform[2];
	
	public Transform shootingpoint;
	public Transform arrowprefab;
	
	float attacktime = 3f;
	float reloadtime = 0;
	
	multiplayerweapon arrowscript;
	public AudioClip clip;
	
	int currentpoint = 0;
	
	bool reload = false;

	public override void Start() 
	{
		charactercontroller = this.GetComponent<CharacterController>();
		
		playerpoints[0] = GameObject.Find("playerpoint").transform;
		playerpoints[1] = GameObject.Find("playerpoint1").transform;
		
		enemypoints[0] = GameObject.Find("enemypoint").transform;
		enemypoints[1] = GameObject.Find("enemypoint1").transform;
		
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
			if(this.arrowtarget != null)
			{
				if(reload)
					firstreload();
				
				if(Time.time > reloadtime)
					fire();
			}
			else
				animState = animationstate.walk;
		}
		
		base.Update();
	}

	void firstreload()
	{
		reloadtime = attacktime + Time.time;
		reload = false;
	}
	
	void fire()
	{
		if(GetComponent<NetworkView>().isMine)
		{
			GetComponent<AudioSource>().PlayOneShot(clip);
			Transform arrow = Network.Instantiate(arrowprefab,shootingpoint.position,this.transform.rotation,0) as Transform;
			arrowscript = arrow.GetComponent<multiplayerweapon>();
			arrowscript.target = this.arrowtarget;
			arrowscript.damage = this.damage;
		}

		reloadtime = attacktime + Time.time;
	}
}