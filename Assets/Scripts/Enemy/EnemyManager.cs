using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	private GameObject[] _enemies;
	private bool _enemyMoving;
	private int _curEnemy;
	private float _speed;
	private GameObject _gameManager;
	private GameObject _player;
	private int _enemyCount;
	private GameObject _gridSystem;
	private MonteCarloPosition _positionToMoveTo;

	void Start () 
	{
		//finds game objects and sets variables
		_enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		_gameManager = GameObject.Find ("GameManager");

		_player = GameObject.Find ("Player");

		_gridSystem = GameObject.Find ("Grid");

		_curEnemy = 0;
	}

	void Update () 
	{
		//checks if the game state is 1
		if (_gameManager.GetComponent<GameManager> ()._gameState == 1)
		{
			//checks if enemy moving is true
			if (_enemyMoving) 
			{
				//moves enemy object
				MoveEnemy (_enemies[_curEnemy]);
			}
		}			
	}

	//performs enemy turn
	public void EnemyTurn()
	{
		//checks if enemy player exists
		if (_player != null) 
		{
			//unselects any selected tiles
			_gridSystem.GetComponent<GridSystem>().UnselectTiles ();

			//finds all enemies on the grid
			_enemies = FindEnemies ();

			//checks if there are enemies within the enemies array
			if (_enemies.Length > 0) 
			{
				//checks if the current enemy array index is null
				if (_enemies [_curEnemy] == null)
				{
					//goes to the next enemy
					NextEnemy ();
				} 
				else 
				{		
					//finds the selected enemy in the array index
					Node _node = GridSystem.FindSelectedObject (_enemies [_curEnemy], 0);

					//finds the position to move to using MCTS
					_positionToMoveTo = _enemies[_curEnemy].GetComponent<MonteCarloEnemy> ().StartMCTS (_player.GetComponent<Player_Controller> ()._health, _enemies[_curEnemy].GetComponent<EnemyController>()._health, _node);

					//checks whether an action should be performed and which action
					CheckForAction ();
				}
			}
			else
			{
				//changes to player turn if no enemy objects in enemies array
				_gameManager.GetComponent<GameManager> ().ChangeTurn ();
			}
		} 
		else 
		{
			//displays if no player on grid
			Debug.Log ("Enemy Manager Player Null");
		}

	}

	//function to select next enemy
	public void NextEnemy()
	{
		//checks if current enemy index is within the enemies array size
		if (_curEnemy == _enemies.Length - 1) 
		{
			//checks if the current enemy is null
			if (_enemies [_curEnemy] != null) 
			{
				//sets enemy animation
				_enemies [_curEnemy].GetComponent<EnemyController> ().EnemyIdle ();
			}

			_curEnemy = 0;

			//changes to player turn
			_gameManager.GetComponent<GameManager> ().ChangeTurn ();
		} 
		else 
		{
			//checks if current enemy is not null
			if (_enemies [_curEnemy] != null) 
			{
				//sets enemy animation
				_enemies [_curEnemy].GetComponent<EnemyController> ().EnemyIdle ();
			}

			//increments current enemyvalue
			_curEnemy++;

			//performs enemy turn
			EnemyTurn ();
		}
	}
		
	//function used to find all tiles adjacent to the enemy
	public List<Node> FindEnemyAdjacentTiles(Node node)
	{
		//creates a list for the adjecent nodes
		List<Node> _adjacents = new List<Node> ();

		int _gridX;
		int _gridZ;

		//sets the grid x and z variables
		_gridX = node._gridX;
		_gridZ = node._gridZ;

		int _checkX = _gridX;
		int _checkZ = _gridZ;

		//loops through checking every adjacent nodes, except diagonal nodes
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (x == 0 || z == 0)
				{
					//sets the check x and z variables
					_checkX = _gridX + x;
					_checkZ = _gridZ + z;

					//checks if the check x and z varables are within the grid
					if (_checkX >= 0 && _checkX < GridSystem._gridSizeX && _checkZ >= 0 && _checkZ < GridSystem._gridSizeZ && GridSystem._grid[_checkX, _checkZ]._enemyNode == false) 
					{
						//checks if the node is walkable
						if (GridSystem._grid [_checkX, _checkZ]._walkable)
						{							
							//adds the node to the list of adjacent nodes
							_adjacents.Add (GridSystem._grid [_checkX, _checkZ]);
						} 
						//checks if the node contains the player
						else if (GridSystem._grid [_checkX, _checkZ]._playerNode)
						{
							//adds the node to the list of adjacent nodes
							_adjacents.Add (GridSystem._grid [_checkX, _checkZ]);
						}
					}
				}
			}
		}

		//returns the list of adjacent nodes
		return _adjacents;
	}

	//function used to damage the player
	public void DamagePlayer()
	{
		//damages the player object for 1
		_player.GetComponent<Player_Controller> ().DamagePlayer (1.0f);
	}

	//function used to move the enemy object
	public void MoveEnemy(GameObject _enemyObj)
	{
		//sets the speed of the enemy
		_speed = 2.0f;

		Node _enemyNode;

		//checks if the enemy moving variable is true
		if (_enemyMoving == true) 
		{			
			//sets the enemy object to look in the direction of movement
			_enemyObj.transform.LookAt (GridSystem._grid [_positionToMoveTo.GetX (), _positionToMoveTo.GetZ ()]._obj.transform.position);

			//sets the enemy object to move towards the position calulated in MCTS
			_enemyObj.transform.position = Vector3.MoveTowards (_enemyObj.transform.position, GridSystem._grid[_positionToMoveTo.GetX(),_positionToMoveTo.GetZ()]._obj.transform.position, _speed * Time.deltaTime);

			//speed increased by it's value multiplied by delta time
			_speed += _speed * Time.deltaTime;

			//checks if the enemy object has reached it's desired position
			if (_enemyObj.transform.position == GridSystem._grid[_positionToMoveTo.GetX(),_positionToMoveTo.GetZ()]._obj.transform.position)
			{
				//sets enemy moving to false
				_enemyMoving = false;

				//sets enemy node to the node moved to
				_enemyNode = GridSystem.FindSelectedObject (_enemyObj, MonteCarloBoard._enemyVal);

				//sets the node's enemy node value to true
				GridSystem._grid [_enemyNode._gridX, _enemyNode._gridZ]._enemyNode = true;

				//checks whether the enemy object can damage the player
				bool _damagePlayer = CheckForAttack ();

				//checks if the damage player variable is true
				if (_damagePlayer)
				{
					//damages the player
					DamagePlayer ();
				}

				//moves to next enemy in enemies array
				NextEnemy ();
			}
		}
	}

	//function used to find enemy objects on the board
	public GameObject[] FindEnemies()
	{
		GameObject[] _allEnemies;

		//sets all game objects with enemy tag to all enemies array
		_allEnemies = GameObject.FindGameObjectsWithTag ("Enemy");

		//returns all enemy array
		return _allEnemies;
	}

	//function used to lower enemy count
	public void LowerEnemyCount()
	{
		_enemyCount--;
	}

	//function used to check whether enemy can attack
	public bool CheckForAttack()
	{
		//find enemy object and sets to new node
		Node _node = GridSystem.FindSelectedObject (_enemies[_curEnemy], MonteCarloBoard._enemyVal);

		List<Node> _possibleAttackNodes;

		//sets list of all possible attack nodes
		_possibleAttackNodes = FindEnemyAdjacentTiles (_node);

		//loops through attack nodes, check whether any contain the player object
		for (int i = 0; i < _possibleAttackNodes.Count; i++)
		{
			//checks if node contains player
			if (_possibleAttackNodes [i]._playerNode) 
			{
				//returns true
				return true;
			}
		}

		//returns false if no player node found
		return false;
	}

	//checks for action for enemy object
	public void CheckForAction()
	{
		//checks whether the enemy object can attack
		bool _damagePlayer = CheckForAttack ();

		//checks if damage player is true
		if (_damagePlayer)
		{
			//damages the player
			DamagePlayer ();

			//moves to the player
			NextEnemy ();
		}
		else
		{		
			//sets the enemy moving to true	
			_enemyMoving = true;

			//finds the enemy object and sets the node to a new node
			Node _node = GridSystem.FindSelectedObject (_enemies[_curEnemy], MonteCarloBoard._enemyVal);

			//sets the current enemy node to false
			GridSystem._grid [_node._gridX, _node._gridZ]._enemyNode = false;

			//moves the enemy object
			_enemies[_curEnemy].GetComponent<EnemyController> ().EnemyMoving ();
		}
	}
}