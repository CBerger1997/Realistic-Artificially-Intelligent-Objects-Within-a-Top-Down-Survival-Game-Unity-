using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float _health;
	public buttonControl_script _actions;
	private GameObject _gridSystem;
	private GameObject _spawnManager;

	void Awake()
	{
		//sets variables
		_health = 3.0f;

		_actions = GetComponent<buttonControl_script> ();

		_spawnManager = GameObject.Find ("SpawnManager");
	}

	//function used to damage enemy object
	public void DamageEnemy(float damage)
	{
		//reduces the enemy health
		_health -= damage;

		//checks if player health is less than or equal to 0
		if (_health <= 0) 
		{			
			//loops through all grid nodes
			for (int z = 0; z < GridSystem._gridSizeZ; z++) 
			{
				for (int x = 0; x < GridSystem._gridSizeX; x++) 
				{
					//checks if enemy position is equal to grid position
					if (GridSystem._grid [x, z]._obj.transform.position.x ==transform.position.x && GridSystem._grid [x, z]._obj.transform.position.z == transform.position.z) 
					{		
						//sets enemy node in grid position to false
						GridSystem._grid [x, z]._enemyNode = false;
					}
				}
			}

			//reduces enemiets left
			_spawnManager.GetComponent<EnemySpawnManager> ()._enemiesLeft--;

			//reduces current enemies
			_spawnManager.GetComponent<EnemySpawnManager> ()._curEnemies--;

			//destroys game object
			Destroy (gameObject);
		}
	}

	#region Enemy Animation

	//functions used for enemy animations
	public void EnemyMoving()
	{
		_actions.CrippledWalk ();
	}

	public void EnemyIdle()
	{
		_actions.Idle ();
	}

	#endregion
}
