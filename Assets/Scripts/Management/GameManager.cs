using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	private GameObject _gameHud;
	private GameObject _player;
	private GameObject _ammoCrate;
	public GameObject _ammoCratePef;
	public int _wave;
	private GameObject _enemyManager;
	private GameObject _enemySpawnManager;

	private int _ammoRespawnTimer;
	public int _gameState;

	void Awake()
	{
		//sets variables
		_gameState = 0;

		_wave = 1;
	}

	void Start () 
	{
		//finds game objects and sets additional object variables
		_gameHud = GameObject.Find ("GameHud");

		_player = GameObject.Find ("Player");

		_enemyManager = GameObject.Find ("EnemyManager");

		_enemySpawnManager = GameObject.Find ("SpawnManager");

		_ammoCrate = Instantiate (_ammoCratePef);

		_gameHud.GetComponent<GameHud> ().SetTurnText ();

		_gameHud.GetComponent<GameHud> ().SetWaveText ();

		SetAllObjectNodes ();
	}
	
	void Update () 
	{
		//checks if escape button is pressed
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			//opens or closes the menu
			_gameHud.GetComponent<GameHud> ().MenuOpenClose ();
		}
	}

	//function used to begin player turn
	public void PlayerTurn()
	{
		//sets all object nodes
		SetAllObjectNodes ();

		//checks if ammo crate object is null
		if (_ammoCrate == null) 
		{
			//adds to ammo respawn timer
			_ammoRespawnTimer++;
		}

		//checks if ammo respawn timer is 3
		if (_ammoRespawnTimer == 3) 
		{
			int respawn;

			//sets respawn to a random number between 0 and 1
			respawn = (int)Random.Range (0, 2);

			//checks if respawn is 0
			if (respawn == 0) 
			{
				//instantiates ammo crate
				_ammoCrate = Instantiate (_ammoCratePef);

				//sets amm respawn timer to 0
				_ammoRespawnTimer = 0;
			}
		} 

		//checks if ammo respawn timer is 4
		else if (_ammoRespawnTimer == 4) 
		{
			//instantiates ammo crate
			_ammoCrate = Instantiate (_ammoCratePef);

			//sets ammo respawn timer to 0
			_ammoRespawnTimer = 0;
		}

		//unselects player object
		_player.GetComponent<PlayerManager>().PlayerUnselected ();

		//enables end turn button
		_gameHud.GetComponent<GameHud> ().EnableEndTurnBtn ();

		//sets player moved to false
		_player.GetComponent<PlayerManager>()._playerMoved = false;

		//enables player manager script
		_player.GetComponent<PlayerManager> ().enabled = true;
	}

	//function used to begin ability turn
	public void AbilityTurn()
	{
		//checks if player abilty active
		if (_player.GetComponent<PlayerAbilities> ().AbilityActive ())
		{
			//checks if player abilty active is turret
			if (_player.GetComponent<PlayerAbilities> ().GetActiveAbility () == "Turret")
			{
				//performs turret action
				_player.GetComponent<PlayerAbilities> ().TurretAction ();

				//lowers ability timer
				_player.GetComponent<PlayerAbilities> ().LowerAbilityTimer ();
			}
			//checks if player abilty active is Invincibility
			else if (_player.GetComponent<PlayerAbilities> ().GetActiveAbility () == "Invincibility")
			{
				//performs invinciblity action
				_player.GetComponent<PlayerAbilities> ().InvincibleAction ();

				//lowers ability timer
				_player.GetComponent<PlayerAbilities> ().LowerAbilityTimer ();
			}
			//checks if player abilty active is none
			else if (_player.GetComponent<PlayerAbilities> ().GetActiveAbility () == "None")
			{
				//changes turn
				ChangeTurn ();
			}
		}
		else 
		{
			//changes turn
			ChangeTurn ();
		}
	}

	//function user to begin enemy turn
	public void EnemyTurn()
	{
		//sets all object nodes
		SetAllObjectNodes ();

		//checks if player object exists
		if (_player != null) 
		{
			//unselects player object
			_player.GetComponent<PlayerManager>().PlayerUnselected ();

			//disables player manager script
			_player.GetComponent<PlayerManager> ().enabled = false;
		} 
		else 
		{
			//displays player object is null
			Debug.Log ("Game Manager Player Null");
		}

		//disables end turn button
		_gameHud.GetComponent<GameHud> ().DisableEndTurnBtn ();

		//performs enemy turn
		_enemyManager.GetComponent<EnemyManager> ().EnemyTurn ();
	}

	//function used to end player turn
	public void EndTurn()
	{
		//checks player object exists
		if (_player != null) 
		{
			//begins ability turn
			AbilityTurn ();
		} 
		else 
		{
			//shows player object is null
			Debug.Log ("Game Manager Player Null");

			//changes turn
			ChangeTurn ();

		}
	}

	//function used to change turn
	public void ChangeTurn()
	{
		//checks if game state is 0
		if (_gameState == 0)
		{
			//sets game state as 1
			_gameState = 1;

			//sets turn text
			_gameHud.GetComponent<GameHud> ().SetTurnText ();

			//sets wave text
			_gameHud.GetComponent<GameHud> ().SetWaveText ();

			//performs enemy turn
			EnemyTurn ();
		}
		//checks if game state is 1
		else if(_gameState == 1)
		{
			//sets game state as 0
			_gameState = 0;

			//sets turn text
			_gameHud.GetComponent<GameHud> ().SetTurnText ();

			//sets wave text
			_gameHud.GetComponent<GameHud> ().SetWaveText ();

			//performs player turn
			PlayerTurn ();
		}
	}

	//function used to start new wave
	public void NewWave()
	{
		//increases wave value
		_wave++;

		//sets up new wave with spawners
		_enemySpawnManager.GetComponent<EnemySpawnManager> ().NewWaveSetup ();

		//sets up new wave for player object
		_player.GetComponent<PlayerAbilities> ().NewWaveSetup ();

		//sets game state to 0
		_gameState = 0;

		//sets turn text
		_gameHud.GetComponent<GameHud> ().SetTurnText ();

		//sets wave text
		_gameHud.GetComponent<GameHud> ().SetWaveText ();

		//performs player turn
		PlayerTurn ();
	}

	//function used to set all object nodes
	public void SetAllObjectNodes()
	{
		//finds player object within grid
		GridSystem.FindSelectedObject (_player, MonteCarloBoard._playerVal);

		GameObject[] _enemies;

		//sets enemy array to enemy objects on grid
		_enemies = _enemyManager.GetComponent<EnemyManager> ().FindEnemies ();

		//loops for each enemy object within enemies array
		foreach (GameObject _enemy in _enemies) 
		{
			//finds each selected enemy object
			GridSystem.FindSelectedObject (_enemy, MonteCarloBoard._enemyVal);
		}
	}
}
