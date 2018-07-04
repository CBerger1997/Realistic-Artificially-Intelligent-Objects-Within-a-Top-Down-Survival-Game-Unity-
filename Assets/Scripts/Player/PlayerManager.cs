using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour 
{
	//Player Vars
	private Vector3 _playerPos;
	private int _playerX;
	private int _playerZ;
	public bool _playerMoved;
	private bool _moving;
	private bool _nodes2;
	private bool _nodes1;
	private Vector3 _movePos1;
	private Vector3 _movePos2;
	private float _shootWaitTimer;
	private float _speed;
	private RaycastHit _mouseHit;
	private Ray _mouseRay;
	public bool _hidePlayerHud;
	public bool _hideAbilityHud;

	private GameObject _gameManager;
	private GameObject _warning;
	private GameObject _gameHud;

	private GameObject _gridSystem;

	public GameObject _bullet;
	private GameObject _bulletInst;
	private bool _shootingActive = false;

	void Start()
	{
		//finds game objects and sets variables
		_hidePlayerHud = true;

		_hideAbilityHud = true;

		_gridSystem = GameObject.Find ("Grid");

		_gameManager = GameObject.Find ("GameManager");

		_playerMoved = false;

		_gameHud = GameObject.Find ("GameHud");

		_warning = GameObject.FindGameObjectWithTag ("Warning");
	}

	void Update()
	{
		//checks if the game state is 0
		if (_gameManager.GetComponent<GameManager> ()._gameState == 0)
		{
			//performs checks
			MouseCollisionCheck ();

			MovingPlayerChecks ();

			ShootingChecks ();

			if (_shootingActive) 
			{
				FindShootTiles ();
			}
		} 
	}

	#region Public Functions

	//function used to unselect the player
	public void PlayerUnselected()
	{
		//sets player animation
		GetComponent<Player_Controller>().PlayerIdle ();

		//unselects all grid tiles
		_gridSystem.GetComponent<GridSystem>().UnselectTiles ();

		//sets all boolean variables
		_shootingActive = false;

		_hidePlayerHud = true;

		_hideAbilityHud = true;
	}

	//function used to select shootable tiles
	public void ShootSelected()
	{
		//hides player hud
		_hidePlayerHud = true;

		//sets player animation
		GetComponent<Player_Controller> ().PlayerAiming ();

		//unselects all grid tiles
		_gridSystem.GetComponent<GridSystem>().UnselectTiles ();

		//sets shooting to active
		_shootingActive = true;
	}

	//function used to select moveable tiles
	public void MoveSelected()
	{
		//sets shooting to false and hides player HUD
		_shootingActive = false;

		_hidePlayerHud = true;

		//sets player animation
		GetComponent<Player_Controller> ().PlayerIdle ();

		//unselects all grid tiles
		_gridSystem.GetComponent<GridSystem>().UnselectTiles ();

		//find all adjacent tiles to the player object
		FindPlayerAdjacentTiles (GridSystem._grid[_playerX, _playerZ]);
	}

	//function used to show ability menu
	public void AbilitySelected()
	{
		//sets all other actions to false and hides other
		_shootingActive = false;

		_hideAbilityHud = false;

		_hidePlayerHud = true;

		//sets player animation
		GetComponent<Player_Controller>().PlayerIdle ();

		//unselects all grid tiles
		_gridSystem.GetComponent<GridSystem>().UnselectTiles ();
	}

	#endregion

	#region Private Functions

	//function used to find all adjacent nodes to the player obect within a 2 node distance
	private void FindPlayerAdjacentTiles(Node node)
	{
		int _gridX;
		int _gridZ;

		_gridX = node._gridX;
		_gridZ = node._gridZ;

		//loops through checking player adjacent nodes, except diagonal nodes
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (x == 0 || z == 0)
				{

					//sets the check x and z variables
					int _checkX = _gridX + x;
					int _checkZ = _gridZ + z;

					//checks if the check x and z varables are within the grid
					if (_checkX >= 0 && _checkX < GridSystem._gridSizeX && _checkZ >= 0 && _checkZ < GridSystem._gridSizeZ) 
					{

						//checks if the node is walkable and doesn't contain an enemy object						
						if (GridSystem._grid[_checkX, _checkZ]._walkable && GridSystem._grid[_checkX, _checkZ]._enemyNode == false) 
						{
							//changes the floor tile material
							_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (1, _checkX, _checkZ);

							//loops through checking player adjacent nodes, except diagonal nodes
							for (int x2 = -1; x2 <= 1; x2++) 
							{
								for (int z2 = -1; z2 <= 1; z2++)
								{
									if (x2 == 0 || z2 == 0)
									{

										//sets the check2 x and z variables
										int _checkX2 = _checkX + x2;
										int _checkZ2 = _checkZ + z2;

										//checks if the check x and z varables are within the grid
										if (_checkX2 >= 0 && _checkX2 < GridSystem._gridSizeX && _checkZ2 >= 0 && _checkZ2 < GridSystem._gridSizeZ) 
										{

											//checks if the node is walkable and doesn't contain an enemy object						
											if (GridSystem._grid[_checkX2, _checkZ2]._walkable && GridSystem._grid[_checkX2, _checkZ2]._enemyNode == false) 
											{
												//sets the first grid position to the child of the second grid position
												GridSystem._grid [_checkX2, _checkZ2]._child = GridSystem._grid [_checkX, _checkZ];

												//changes the floor tile material
												_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (1, _checkX2, _checkZ2);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (0, _gridX, _gridZ);
	}

	//finds the shootable tiles for the player object
	private void FindShootTiles()
	{
		//boolean variables used to check whether a certain direction has hit a wall yet
		bool _positiveXSearch = true;
		bool _positiveZSearch = true;
		bool _negativeXSearch = true;
		bool _negativeZSearch = true;

		//iterates through based upon the size of the grid
		for (int i = 0; i < GridSystem._gridSizeX; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveXSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_playerX + i, _playerZ]._walkable == true)
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (2, _playerX + i, _playerZ);
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveXSearch = false;
				}

				//checks if the current grid node contains an enemy
				if (GridSystem._grid [_playerX + i, _playerZ]._enemyNode == true) 
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (3, _playerX + i, _playerZ);
				}

			}
			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeXSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_playerX - i, _playerZ]._walkable == true)
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (2, _playerX - i, _playerZ);
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeXSearch = false;
				}

				//checks if the current grid node contains an enemy
				if (GridSystem._grid [_playerX - i, _playerZ]._enemyNode == true) 
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (3, _playerX - i, _playerZ);
				}
			}
		}

		//iterates through based upon the size of the grid
		for (int i = 0; i < GridSystem._gridSizeZ; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveZSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_playerX, _playerZ + i]._walkable == true)
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (2, _playerX, _playerZ + i);
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveZSearch = false;
				}

				//checks if the current grid node contains an enemy
				if (GridSystem._grid [_playerX, _playerZ + i]._enemyNode == true) 
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (3, _playerX, _playerZ + i);
				}
			}
			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeZSearch) 
			{
				//checks if the current grid node is walkable
				if (GridSystem._grid[_playerX, _playerZ - i]._walkable == true)
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (2, _playerX, _playerZ - i);
				}
				else
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeZSearch = false;
				}

				//checks if the current grid node contains an enemy
				if (GridSystem._grid [_playerX, _playerZ - i]._enemyNode == true) 
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem> ().ChangeFloorMat (3, _playerX, _playerZ - i);
				}
			}
		}

		//changes the floor node material
		_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (0, _playerX, _playerZ);
	}

	//function used to reset the player after a move
	private void ResetAfterPlayerMove()
	{
		//sets player animation
		GetComponent<Player_Controller> ().PlayerIdle ();

		//loops through the whole grid
		for (int z = 0; z < GridSystem._gridSizeZ; z++) 
		{
			for (int x = 0; x < GridSystem._gridSizeX; x++) 
			{
				//checks if the grid contains a chnaged node
				if (GridSystem._grid [x, z]._changed == true) 
				{
					//changes the floor node material
					_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (0, x, z);
				}

				//sets each grid child node to null
				GridSystem._grid [x, z]._child = null;
			}
		}

		//finds the player object within the grid
		GridSystem.FindSelectedObject (transform.gameObject, MonteCarloBoard._playerVal);
	}

	//function used to check for player movement
	private void MovingPlayerChecks()
	{
		//checks if the player can move and the first node has not been reached
		if (_moving == true && _nodes1 == false) 
		{
			//sets current player node to false
			GridSystem._grid [_playerX, _playerZ]._playerNode = false;

			_nodes1 = false;

			//makes player object look at it's moving position
			transform.LookAt (_movePos1);

			//makes the payer object move towards it's moving position
			transform.position = Vector3.MoveTowards (transform.position, _movePos1, _speed * Time.deltaTime);

			//speed increased by it's value multiplied by delta time
			_speed += _speed * Time.deltaTime;

			//checks if the player position has reached it's move position
			if (transform.position == _movePos1) 
			{
				//checks if there is a second node position
				if (_nodes2 == false) 
				{
					//sets moving to false
					_moving = false;

					//enables the end turn button
					_gameHud.GetComponent<GameHud> ().EnableEndTurnBtn ();

					//resets the player after moving
					ResetAfterPlayerMove ();
				} 
				else 
				{
					//sets the node 1 to true
					_nodes1 = true;
				}
			}
		}

		//checks if there is a second node to move too and the first node has been reached
		if (_nodes2 == true && _nodes1 == true) 
		{
			//makes player object look at it's moving position
			transform.LookAt (_movePos2);

			//makes the payer object move towards it's moving position
			transform.position = Vector3.MoveTowards (transform.position, _movePos2, _speed * Time.deltaTime);

			//speed increased by it's value multiplied by delta time
			_speed += _speed * Time.deltaTime;

			//checks if the player position has reached it's move position
			if (transform.position == _movePos2) 
			{
				//sets all move booleans to false
				_nodes1 = false;

				_nodes2 = false;

				_moving = false;

				//enables the end turn button
				_gameHud.GetComponent<GameHud> ().EnableEndTurnBtn ();

				//resets the player after moving
				ResetAfterPlayerMove ();
			}
		}
	}

	//function used to check for shooting
	private void ShootingChecks()
	{
		//checks if the shoot timer is greater than 0
		if(_shootWaitTimer > 0)
		{
			//reduces the shoot timer
			_shootWaitTimer -= Time.deltaTime;
		}
	}

	//function used when the player is selected
	private void PlayerSelected()
	{
		Node _playerNode;

		//finds the player object within the grid
		_playerNode = GridSystem.FindSelectedObject(transform.gameObject, MonteCarloBoard._playerVal);

		//sets the player x and z coordinates to the player's position on the grid
		_playerX = _playerNode._gridX;
		_playerZ = _playerNode._gridZ;

		//checks if the player node is found
		if (_playerNode != null) 
		{
			//sets the player node on the grid to true
			GridSystem._grid [_playerNode._gridX, _playerNode._gridZ]._playerNode = true;

			//shows the player hud
			_hidePlayerHud = false;
		}
	}

	//function used to allow the player to shoot
	private void PlayerShoot(Vector3 _direction, Vector3 _lookPos)
	{
		//checks if the shoot timer is less than or equal to 0
		if (_shootWaitTimer <= 0) 
		{
			//sets the player objects y look position
			_lookPos.y = 0.05f;

			//sets the player animation
			GetComponent<Player_Controller> ().PlayerShooting ();

			//makes the object look in the direction shooting
			transform.LookAt (_lookPos);

			//instantiates the bullet on the player's position
			_bulletInst = Instantiate (_bullet, new Vector3 (transform.position.x, 1.0f, transform.position.z), transform.rotation);

			//sets the bullets direction
			_bulletInst.GetComponent<BulletMovement> ()._direction = _direction;

			//sets the bullets owner to the player
			_bulletInst.GetComponent<BulletMovement> ()._owner = "Player";

			//reduces the player's ammo
			GetComponent<Player_Controller> ()._ammo--;

			//sets the shoot timer
			_shootWaitTimer = 0.5f;
		}
	}

	//function used to move the player
	private void MovePlayer(GameObject _targetObj)
	{
		int _floorPos;
		int _x;
		int _z;

		//changes the floor tile name to an integer value
		int.TryParse (_targetObj.name, out _floorPos);

		//sets the x value of the grid
		_x = _floorPos%GridSystem._gridSizeX;

		//sets the y value of the grid
		_z = _floorPos / GridSystem._gridSizeX;

		//sets the speed of movement
		_speed = 1.5f;

		//disables the end turn button
		_gameHud.GetComponent<GameHud> ().DisableEndTurnBtn ();

		//checks if the grid child is not null
		if (GridSystem._grid [_x, _z]._child != null) 
		{
			//sets the 1st move position to the nodes child node
			_movePos1 = GridSystem._grid [_x, _z]._child._obj.transform.position;

			//sets the 1st move position y component
			_movePos1.y = 0.05f;

			//sets the 2nd move position to the nodes child node
			_movePos2 = GridSystem._grid [_x, _z]._obj.transform.position;

			//sets the 2nd move position y component
			_movePos2.y = 0.05f;

			//sets the grid child node to null
			GridSystem._grid [_x, _z]._child = null;

			//sets the nodes 2 to true and moving to tue
			_nodes2 = true;
			_moving = true;

			//sets player animation
			GetComponent<Player_Controller> ().PlayerMoving ();
		} 
		else 
		{
			//sets the 1st move position to the nodes child node
			_movePos1 = GridSystem._grid [_x, _z]._obj.transform.position;

			//sets the 1st move position y component
			_movePos1.y = 0.05f;

			//sets the nodes 2 to false and moving to tue
			_nodes2 = false;
			_moving = true;

			//sets player animation
			GetComponent<Player_Controller> ().PlayerMoving ();
		}

	}

	//function used to check mouse collisions
	private void MouseCollisionCheck()
	{
		//sets mouse ray to where the mouse is clicked
		_mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//checks if the mouse ray collides with an object
		if (Physics.Raycast (_mouseRay, out _mouseHit)) 
		{
			//checks if the left mouse button is clicked
			if (Input.GetMouseButtonDown (0)) 
			{
				//checks if the mouse collides with the player object
				if (_mouseHit.collider.tag == ("Player")) 
				{	
					//hides ability and player hud
					_hideAbilityHud = true;

					_hidePlayerHud = true;

					//selects the player
					PlayerSelected ();					
				}
				if (!EventSystem.current.IsPointerOverGameObject ()) 
				{
					//checks if the mouse collides with the floor move tiles and player moved is false
					if (_mouseHit.collider.tag == ("FloorMove") && _playerMoved == false) 
					{
						//unselects the player
						PlayerUnselected ();

						//moves the player
						MovePlayer (_mouseHit.collider.gameObject);

						//sets player moved to true
						_playerMoved = true;
					} 
					//checks if the mouse collides with the floor move tiles and player moved is true
					else if (_mouseHit.collider.tag == ("FloorMove") && _playerMoved == true) 
					{
						//unselects the player
						PlayerUnselected ();

						//shows warning symbol to user
						_warning.GetComponent<PlayerWarning> ().ShowWarning ();

						//sets the text for the warning text
						_gameHud.GetComponent<GameHud> ().SetNoteText ("Player has already moved");
					} 
					//checks if the mouse collides with the floor shoot tiles and player ammo is greater than 0
					else if (_mouseHit.collider.tag == ("FloorShoot") && GetComponent<Player_Controller>()._ammo > 0)
					{		
						//performs player shoot
						PlayerShoot ((_mouseHit.collider.transform.position - transform.position), _mouseHit.collider.transform.position);
					}
					//checks if the mouse collides with the floor shoot tiles and player ammo is equal to 0
					else if (_mouseHit.collider.tag == ("FloorShoot") && GetComponent<Player_Controller>()._ammo == 0)
					{
						//shows warning symbol to user
						_warning.GetComponent<PlayerWarning>().ShowWarning ();

						//sets the text for the warning text
						_gameHud.GetComponent<GameHud> ().SetNoteText ("Player has run out of ammo");

						//unselects the player object
						PlayerUnselected ();
					}
					//checks if the mouse collides with the floor turret tile
					else if(_mouseHit.collider.tag == "FloorTurret")
					{
						//unselects the player object
						PlayerUnselected ();

						//places player turret
						GetComponent<PlayerAbilities> ().PlaceTurret (_mouseHit.collider.gameObject);
					}
				}
			} 
		}
	}

	#endregion
}