using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleController : MonoBehaviour 
{
	private int _invincibleTimer;

	private void Start()
	{
		//sets variable
		_invincibleTimer = 3;
	}

	//function used to reduce the ability timer
	public void reduceAbilityTimer()
	{
		//decreases invincibility timer
		_invincibleTimer--;

		//checks if timer is less than or equal to 0
		if (_invincibleTimer <= 0)
		{
			//destorys game object
			Destroy (transform.gameObject);
		}
	}
}
