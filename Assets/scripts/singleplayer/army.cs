using UnityEngine;
using System.Collections;

public class army : MonoBehaviour 
{
	Gamecontroller controller;

	public enum animationstate
	{
		idle,
		walk,
		attack
	};

	public animationstate animState = animationstate.walk;
	public float maxhealth;

	public float health
	{
		get {return Health;}
		set {Health = value;}
	}

	protected float Health;

	public int damage;
	public float speed;
	public float attackdistance;
	public float idledistance;
	public int reward;
	public Transform arrowtarget;

	Transform fighter;
	Animator animator;

	public virtual void Start() 
	{
		controller = GameObject.FindWithTag("GameController").GetComponent<Gamecontroller>();
		fighter = this.transform;
		animator = this.GetComponent<Animator>();
		Health = maxhealth;
	}

	public virtual void Update() 
	{
		Vector3 rayvector;

		if(fighter.CompareTag("playerfighter"))
			rayvector = new Vector3(fighter.position.x - 2, fighter.position.y, fighter.position.z);
		else
			rayvector = new Vector3(fighter.position.x + 2, fighter.position.y, fighter.position.z);
		 
		Ray enemyray = new Ray(rayvector, fighter.TransformDirection(Vector3.forward));
		Ray playeray = new Ray(fighter.position, fighter.TransformDirection(Vector3.forward));
		RaycastHit hit;
		RaycastHit playerhit;

		if(Health <= 0)
		{
			dead();
		}

		if(fighter.name == "bowfighter")
		{
			if(Physics.Raycast(enemyray, out hit, attackdistance)) 
			{
				if(!hit.transform.CompareTag(fighter.tag))
				{
					animState = animationstate.attack;
					arrowtarget = hit.transform;
				}
				else
					animState = animationstate.walk;
			} 
			else
				animState = animationstate.walk;

			if(Physics.Raycast(playeray, out playerhit, idledistance))
			{
				if(playerhit.transform.CompareTag(fighter.tag))
					animState = animationstate.idle;
			}
		}
		else
		{
			if(Physics.Raycast(enemyray, out hit, attackdistance)) 
			{
				if(!hit.transform.CompareTag(fighter.tag))
				{
					animState = animationstate.attack;
					arrowtarget = hit.transform;
				}
				else
					animState = animationstate.walk;
			} 
			else
				animState = animationstate.walk;

			if(Physics.Raycast(playeray, out playerhit, attackdistance)) 
			{
				if(playerhit.transform.CompareTag(fighter.tag))
					animState = animationstate.idle;
			} 
		}

		switch(animState) 
		{
		case animationstate.idle:
			animator.CrossFade("idle",0.0f);
			break;

		case animationstate.attack:
			animator.CrossFade("attack",0.0f);
			break;

		case animationstate.walk:
			animator.CrossFade("walk",0.0f);
			break;
		}
	}

	void dead()
	{
		if(fighter.CompareTag("playerfighter"))
			controller.playerfighterdead(reward);
		else
			controller.enemyfighterdead(reward);
		Destroy(this.gameObject,0.05f);
	}
}
