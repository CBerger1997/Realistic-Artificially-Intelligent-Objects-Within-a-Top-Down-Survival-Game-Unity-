using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

	public int _maxEnemies;
	public int _maxEnemiesAtOnce;
	public int _curEnemies;
	public int _enemiesLeft;
	private GameObject _gameManager;
	private int _enemyNo = 1;

	private GameObject[] _spawners;

	void Awake()
	{
		//sets variables and finds game objects
		_gameManager = GameObject.Find ("GameManager");

		_spawners = GameObject.FindGameObjectsWithTag ("Spawner");

		_maxEnemiesAtOnce = _spawners.Length * 2;
	}

	void Start () 
	{
		//sets variable
		_curEnemies = 0;

		//sets the spawners for a new wave
		NewWaveSetup ();
	}

	void Update () 
	{
		//checks if current enemies is less than max enemies and enemies left is greater than or equal to max enemies left
		if (_curEnemies < _maxEnemiesAtOnce && _enemiesLeft >= _maxEnemiesAtOnce) 
		{
			//generates enemy with spawner
			GenerateEnemyWithSpawner ();
		}

		//checks if there are no enemies left
		if (_enemiesLeft == 0) 
		{
			//spawns a new wave of enemies
			_gameManager.GetComponent<GameManager> ().NewWave ();
		}
	}

	//function used to generate enemies with spawners
	void GenerateEnemyWithSpawner()
	{
		Node _adjacentNode = null;

		int _spawnerNo;

		//loop used while adjacent node is null
		do 
		{
			//sets spawner no to a randomly selected spawner
			_spawnerNo = SelectSpawner ();

			//sets adjacent node to the spawner's randomly selected adjacent node
			_adjacentNode =	SelectSpawnNode (_spawners [_spawnerNo]);
		} 
		while(_adjacentNode == null);

		//spawns enemy with selected spawner
		_spawners [_spawnerNo].GetComponent<EnemySpawnController> ().GenerateEnemy (_adjacentNode, _enemyNo);

		//increases enemy number
		_enemyNo++;

		//increases current enemies
		_curEnemies++;

	}

	//function used to select spawn node
	public Node SelectSpawnNode(GameObject _spawner)
	{
		Node _spawnerNode;

		//finds referenced spawner
		_spawnerNode = _spawner.GetComponent<EnemySpawnController> ().FindSpawner ();

		//creates adjacent nodes list
		List<Node> _adjacentNodes = new List<Node> ();

		//finds adjacent nodes for spawner
		_adjacentNodes = _spawner.GetComponent<EnemySpawnController> ().LocateSpawnTiles (GridSystem._grid [_spawnerNode._gridX, _spawnerNode._gridZ]);

		//checks if adjacent nodes count is 0
		if (_adjacentNodes.Count == 0) 
		{
			//returns null
			return null;
		}

		//randomly generates number between 0 and number of adjacent nodes
		int _nodeNo = Random.Range (0, _adjacentNodes.Count);
	
		//returns randomly selected adjacent node
		return _adjacentNodes [_nodeNo];
	}

	//function used to select spawner
	private int SelectSpawner()
	{
		int _spawnerNo;

		//randomly generates number between 0 and number of spawners
		_spawnerNo = (int)Random.Range (0, _spawners.Length);

		//returns spawner number
		return _spawnerNo;
	}

	//function used to set up a new wave
	public void NewWaveSetup()
	{
		//sets max enemies to wave number multiplied by 2
		_maxEnemies = _gameManager.GetComponent<GameManager> ()._wave * 2;

		//sets enemies left to wave number multiplied by 2
		_enemiesLeft = _gameManager.GetComponent<GameManager> ()._wave * 2;
			
		//sets amount of enemies to spawn to max enemies
		int _amountToSpawn = _maxEnemies;

		//checks if max enemies is greater than max enemies at once
		if (_maxEnemies > _maxEnemiesAtOnce) 
		{
			//sets amount to spawn to max enemies at once
			_amountToSpawn = _maxEnemiesAtOnce;
		}

		//loops through to generate enemies with spawner
		for (int i = 0; i < _amountToSpawn; i++) 
		{
			GenerateEnemyWithSpawner ();		
		}
	}


}
