using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

	public GameObject _bullet;
	private GameObject _bulletInst;
	private int _turretTimer;
	public bool _shotTaken;
	public bool _turretActionPerformed;
	private int _turretX;
	private int _turretZ;

	private void Start()
	{
		//sets variables
		_turretTimer = 3;
		_shotTaken = false;
		_turretActionPerformed = false;
	}

	void Update()
	{
		//checks if bullet isn't instantiated and shot has been taken
		if (_bulletInst == null && _shotTaken == true) 
		{
			//sets turret action performed to true
			_turretActionPerformed = true;
		}

		//checks if bullet instance is null
		if (_bulletInst == null) 
		{
			//sets shot taken to false
			_shotTaken = false;
		}
	}

	//function used to shoot with turret
	private void TurretShoot(Vector3 _direction, Vector3 _lookPos)
	{
		//makes turret look in direction shooting
		transform.LookAt (_lookPos);

		//instantiates bullet for turret
		_bulletInst = Instantiate (_bullet, new Vector3(transform.position.x, 0.4f, transform.position.z), transform.rotation);

		//sets bullet direction
		_bulletInst.GetComponent<BulletMovement> ()._direction = _direction;

		//sets bullet owner as turret
		_bulletInst.GetComponent<BulletMovement> ()._owner = "Turret";

		//sets shot taken as true
		_shotTaken = true;
	}

	//function used to reduce ability timer
	private void reduceAbilityTimer()
	{
		_turretTimer--;
	}

	//function used to find shoot tiles for the turret
	private List<Node> FindTurretShootTiles()
	{
		//boolean variables used to check whether a certain direction has hit a wall yet
		bool _positiveXSearch = true;
		bool _positiveZSearch = true;
		bool _negativeXSearch = true;
		bool _negativeZSearch = true;

		List<Node> _enemyNodes = new List<Node> ();

		//iterates through based upon the size of the grid
		for (int i = 0; i < GridSystem._gridSizeX; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveXSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_turretX + i, _turretZ]._walkable == true)
				{
					//checks if the current grid node's enemy node is true
					if (GridSystem._grid [_turretX + i, _turretZ]._enemyNode == true)
					{
						//adds node to enemy nodes list
						_enemyNodes.Add (GridSystem._grid [_turretX + i, _turretZ]);
					}
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveXSearch = false;
				}
			}
			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeXSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_turretX - i, _turretZ]._walkable == true)
				{
					//checks if the current grid node's enemy node is true
					if (GridSystem._grid [_turretX - i, _turretZ]._enemyNode == true)
					{
						//adds node to enemy nodes list
						_enemyNodes.Add (GridSystem._grid [_turretX - i, _turretZ]);
					}
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeXSearch = false;
				}
			}
		}

		for (int i = 0; i < GridSystem._gridSizeZ; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveZSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_turretX, _turretZ + i]._walkable == true)
				{
					//checks if the current grid node's enemy node is true
					if (GridSystem._grid [_turretX, _turretZ + i]._enemyNode == true)
					{
						//adds node to enemy nodes list
						_enemyNodes.Add (GridSystem._grid [_turretX, _turretZ + i]);
					}
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveZSearch = false;
				}
			}
			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeZSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_turretX, _turretZ - i]._walkable == true)
				{
					//checks if the current grid node's enemy node is true
					if (GridSystem._grid [_turretX, _turretZ - i]._enemyNode == true) 
					{
						//adds node to enemy nodes list
						_enemyNodes.Add (GridSystem._grid [_turretX, _turretZ - i]);
					}
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeZSearch = false;
				}
			}
		}

		//returns list of enemy nodes
		return _enemyNodes;
	}

	//function used to perform turret action
	public void PerformTurretAction()
	{
		Node _turretNode;

		//sets list of shootable nodes to null
		List<Node> _shootableNodes = null;

		//finds turret node on grid
		_turretNode = GridSystem.FindSelectedObject (transform.gameObject, 0);

		//sets turret x and z values
		_turretX = _turretNode._gridX;
		_turretZ = _turretNode._gridZ;

		//find shootable nodes
		_shootableNodes = FindTurretShootTiles ();

		//checks if shootable nodes size is not equal to 0
		if (_shootableNodes.Count != 0) 
		{
			//sets node to shoot as random value between 0 and shootable nodes size
			int _nodeToShoot = Random.Range (0, _shootableNodes.Count);

			//performs turret shoot action
			TurretShoot (_shootableNodes [_nodeToShoot]._obj.transform.position - transform.position, _shootableNodes [_nodeToShoot]._obj.transform.position);

			//reduces abilty timer
			reduceAbilityTimer ();
		} 
		else 
		{
			//reduces ability timer
			reduceAbilityTimer ();

			//sets turret action performed to true
			_turretActionPerformed = true;
		}

	}

	//function used to return turret timer
	public int GetTurretTimer()
	{
		return _turretTimer;
	}

	//function used to destroy turret
	public void DestroyTurret()
	{
		Destroy (transform.gameObject);
	}
}
