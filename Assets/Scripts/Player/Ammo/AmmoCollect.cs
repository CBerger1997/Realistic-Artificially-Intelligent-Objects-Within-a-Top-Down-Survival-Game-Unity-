using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollect : MonoBehaviour {

	public int _ammoCapacity;
	private int _x;
	private int _z;

	// Use this for initialization

	void Start () 
	{
		//randomly sets ammo capacity between 5 and 10
		_ammoCapacity = (int)Random.Range (5, 11);

		//sets ammo object position
		SetPosition ();

		//checks if ammo is not within a walkable node
		while (!GridSystem._grid [_x, _z]._walkable) 
		{
			//sets object position
			SetPosition ();
		}

	}

	void Update()
	{
		//rotates game object
		transform.Rotate(0,6.0f*30.0f*Time.deltaTime,0);
	}

	//function to set object position
	void SetPosition()
	{
		//sets a random x and z value on the grid
		_x = (int)Random.Range (0, GridSystem._gridSizeX);
		_z = (int)Random.Range (0, GridSystem._gridSizeZ);

		//sets object on random x and z position
		transform.position = new Vector3 (_x * 2.25f, 0.5f, _z * 2.25f);
	}
}
