using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {

	public int _ammo;
	public float _health;
	public bool _moving;
	public Actions _actions;

	void Awake()
	{
		//sets variables one awake
		_health = 5.0f;

		_ammo = 10;

		_actions = GetComponent<Actions> ();
	}

	void Update ()
	{
		//checks ammo on update
		AmmoCheck ();
	}

	public void DamagePlayer(float damage)
	{
		//checks if invincibility isn't active
		if (!GetComponent<PlayerAbilities> ().getInvincibility ()) 
		{
			//lowers health if invincibility isn't active
			_health -= damage;
		}

		//checks if health is less than or equal to 0
		if (_health <= 0) 
		{
			//loads the main menu
			SceneManager.LoadScene ("Menu");
		}
	}

	//function used when player enters and object with a trigger
	private void OnTriggerEnter(Collider _obj)
	{
		//checks for collision object tag
		if (_obj.gameObject.tag == "Ammo") 
		{
			//adds the ammo capacity in the ammo box to the player ammo
			_ammo += _obj.gameObject.GetComponent<AmmoCollect> ()._ammoCapacity;

			//destroys the ammo box
			Destroy (_obj.gameObject);
		}
	}

	//checks whether player ammo is greater than 30, reudcing it to 30 if so
	private void AmmoCheck ()
	{
		if(_ammo > 30)
		{
			_ammo = 30;
		}
	}

	//sets animations for player object
	#region Player Animation

	public void PlayerMoving()
	{
		_actions.Run ();
	}

	public void PlayerIdle()
	{
		_actions.Stay ();
	}

	public void PlayerAiming ()
	{
		_actions.Aiming ();	
	}

	public void PlayerShooting ()
	{
		_actions.Attack ();
	}

	#endregion
}
