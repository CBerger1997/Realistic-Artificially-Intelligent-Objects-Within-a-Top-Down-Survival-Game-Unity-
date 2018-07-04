using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteCarloBoard
{
	int[,] _boardValues;
	int _totalMoves;

	//variables used to define board states
	public static int _inProgress = -1;
	public static int _playerVal = 1;
	public static int _enemyVal = 2;

	public float _originalPlayerHealth;

	public float _originalEnemyHealth;

	public float _playerHealth = 5;

	public float _enemyHealth = 5;

	public MonteCarloPosition _movedPosition;

	//class contructors
	public MonteCarloBoard() 
	{
		_boardValues = new int[GridSystem._gridSizeX, GridSystem._gridSizeZ];
	}

	public MonteCarloBoard(MonteCarloBoard _board) 
	{
		this._boardValues = new int[GridSystem._gridSizeX, GridSystem._gridSizeZ];

		int[,] _boardValues = _board.getBoardValues();

		this._movedPosition = _board._movedPosition;

		this._playerHealth = _board._playerHealth;

		this._enemyHealth = _board._enemyHealth;

		this._originalPlayerHealth = _board._originalPlayerHealth;

		this._originalEnemyHealth = _board._originalEnemyHealth;

		for (int x = 0; x < GridSystem._gridSizeX; x++) 
		{
			for (int z = 0; z < GridSystem._gridSizeZ; z++) 
			{
				this._boardValues [x, z] = _boardValues [x, z];
			}
		}
	}

	//functions used to set specific values of the board
	public void SetSpecificBoardValues(int _val, int _x, int _z)
	{
		this._boardValues [_x, _z] = _val;
	}

	//function used to perform the move of the player
	public void performPlayerMove(MonteCarloPosition _p)
	{
		//increments the total moves
		this._totalMoves++;

		//finds the shootable tiles for the player to check for collision with the enemy object, returning true if an enemy object is in line of sight
		bool _opponentCheck = FindPlayerShootTiles ();

		//checks if the opponent check value is true
		if (_opponentCheck)
		{
			//reduces the enemy health by 1
			this._enemyHealth--;
		}
		//performs player action if opponent check value is false
		else
		{
			//sets old player position to current player position
			MonteCarloPosition _oldPos = FindPlayer (_playerVal);

			//randomly decides whether to move the player 1 or 2 places
			int move = (int)Random.Range (0, 2);

			//if move value is 0, moves the player 1 position
			if (move == 0) 
			{
				//sets board value of the old player position to 0, whilst setting the new player position to the player value 
				_boardValues [_oldPos.GetX (), _oldPos.GetZ ()] = 0;
				_boardValues [_p.GetX (), _p.GetZ ()] = _playerVal;

				//finds the shootable tiles for the player to check for collision with the enemy object, returning true if an enemy object is in line of sight
				bool _opponentCheck2 = FindPlayerShootTiles ();

				//checks if the opponent check value is true
				if (_opponentCheck2)
				{
					//reduces the enemy health by 1
					this._enemyHealth--;
				}
			} 
			//if move value is not 0, moves the player 2 positions
			else 
			{
				//sets board value of the old player position to 0, whilst setting the new player position to the player value 
				_boardValues [_oldPos.GetX (), _oldPos.GetZ ()] = 0;
				_boardValues [_p.GetX (), _p.GetZ ()] = _playerVal;

				//finds the legal positions for the player
				List<MonteCarloPosition> _legalPositions = this.GetLegalPositions(_playerVal);

				//sets a new int to the number of legal positions found
				int _totalLegalPossibilities = _legalPositions.Count;

				//selects a random number between 0 and the number of legal positions
				int _selectRandom = (int)(Random.Range (0, _totalLegalPossibilities));

				//sets the second move position to that of the randomly selected legal position
				MonteCarloPosition _p2 = _legalPositions [_selectRandom];

				//sets board value of the old player position to 0, whilst setting the new player position to the player value 
				_boardValues [_p.GetX (), _p.GetZ ()] = 0;
				_boardValues [_p2.GetX (), _p2.GetZ ()] = _playerVal;


				//finds the shootable tiles for the player to check for collision with the enemy object, returning true if an enemy object is in line of sight
				bool _opponentCheck2 = FindPlayerShootTiles ();

				//checks if the opponent check value is true
				if (_opponentCheck2)
				{
					//reduces the enemy health by 1
					this._enemyHealth--;
				}
			}
		}
	}

	//function used to perform the move of the enemy
	public void performEnemyMove(MonteCarloPosition _p)
	{
		//increments the total moves
		this._totalMoves++;

		//sets old enemy position to current enemy position
		MonteCarloPosition _oldPos = FindPlayer (_enemyVal);

		//finds the adjacent tiles for the enemy to check for collision with the player object, returning true if the player object is adjacent to the enemy object
		bool _opponentCheck = CheckOpponentAdjacent ();

		//checks if the opponent check value is true
		if (_opponentCheck) 
		{
			//reduces the player health by 1
			this._playerHealth--;
		}
		else
		{
			//sets board value of the old enemy position to 0, whilst setting the new enemy position to the player value 
			_boardValues [_oldPos.GetX (), _oldPos.GetZ ()] = 0;
			_boardValues [_p.GetX (), _p.GetZ ()] = _enemyVal;

			//finds the adjacent tiles for the enemy to check for collision with the player object, returning true if the player object is adjacent to the enemy object
			bool _opponentCheck2 = CheckOpponentAdjacent ();

			//checks if the opponent check value is true
			if (_opponentCheck2) 
			{					
				//reduces the player health by 1
				this._playerHealth--;				
			}
		}
	}

	//function that checks the current status of the board, returning the value of the current state
	public int CheckStatus() 
	{
		//checks if the player has been damaged
		if (this._playerHealth < _originalPlayerHealth) 
		{
			//returns enemy state value
			return _enemyVal;
		} 
		//checks if the enemy has been damaged
		else if (this._enemyHealth < _originalEnemyHealth) 
		{
			//returns player state value
			return _playerVal;
		}
		//if neither player nor enemy is damaged this is reached
		else
		{
			//returns in progress state value
			return _inProgress;
		}
	}

	//function that gets all legal positions for a selected player
	public List<MonteCarloPosition> GetLegalPositions(int _playerNo) 
	{
		//creates list to store legal positions
		List<MonteCarloPosition> _legalPositions = new List<MonteCarloPosition> ();
	
		MonteCarloPosition _pos;

		//finds the object on the board dependant on the player number referenced
		_pos = FindPlayer (_playerNo);

		//sets the board x and z to the current object x and z position
		int _boardX = _pos.GetX();
		int _boardZ = _pos.GetZ();

		int _checkX;
		int _checkZ;

		//loops used to check all adjacent tiles, except diagonal tiles
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (x == 0 || z == 0) 
				{
					if (x == 0 && z == 0) 
					{
					} 
					else 
					{
						//sets the check x and z to the current board x and z position plus the current x and z iteration values
						_checkX = _boardX + x;
						_checkZ = _boardZ + z;

						//checks if the check x and check z values are within the grid
						if (_checkX >= 0 && _checkX < GridSystem._gridSizeX && _checkZ >= 0 && _checkZ < GridSystem._gridSizeZ)
						{			
							//checks if the board values are set to 0 (being empty)
							if (_boardValues [_checkX, _checkZ] == 0) 							
							{

								MonteCarloPosition _position = new MonteCarloPosition ();

								//sets the position values to the current check x and check z values
								_position.SetX (_checkX);
								_position.SetZ (_checkZ);

								//adds the position to the list of legal positions
								_legalPositions.Add (_position);
							}
						}					
					}
				}
			}
		}

		//returns the list of legal positions
		return _legalPositions;
	}

	//function that checks the adjacent tiles to the opponent currents positions
	public bool CheckOpponentAdjacent() 
	{
		MonteCarloPosition _pos;

		//finds the position of the current enemy object
		_pos = FindPlayer (_enemyVal);

		//sets the board x and z to the current object x and z position
		int _boardX = _pos.GetX();
		int _boardZ = _pos.GetZ();

		int _checkX = _boardX;
		int _checkZ = _boardZ;

		//loops used to check all adjacent tiles, except diagonal tiles
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (x == 0 || z == 0) 
				{
					if (x == 0 && z == 0) 
					{
					} 
					else 
					{
						_checkX = _boardX + x;
						_checkZ = _boardZ + z;

						//checks if the check x and check z values are within the grid
						if (_checkX >= 0 && _checkX < GridSystem._gridSizeX && _checkZ >= 0 && _checkZ < GridSystem._gridSizeZ)
						{			
							//checks if the current board value is equal to that of the player value
							if (_boardValues [_checkX, _checkZ] == _playerVal) 							
							{
								//returns true showing the player object has been found
								return true;
							}
						}					
					}
				}
			}
		}

		//returns false showing that the player object was not found
		return false;
	}

	//finds the shootable tiles for the player object
	private bool FindPlayerShootTiles()
	{
		//boolean variables used to check whether a certain direction has hit a wall yet
		bool _positiveXSearch = true;
		bool _positiveZSearch = true;
		bool _negativeXSearch = true;
		bool _negativeZSearch = true;

		MonteCarloPosition pos;

		//finds the position of the current enemy object
		pos = FindPlayer (_playerVal);

		//iterates through based upon the size of the grid
		for (int i = 0; i < GridSystem._gridSizeX; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveXSearch) 
			{
				//checks if the current board value contains a wall
				if (this._boardValues [pos.GetX() + i, pos.GetZ()] == 10)
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveXSearch = false;
				}
				//checks if the current board value contains an enemy object
				else if (this._boardValues [pos.GetX() + i, pos.GetZ()] == _enemyVal) 
				{
					//returns true, showing that an enemy object has been found
					return true;
				}
			}

			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeXSearch) 
			{
				//checks if the current board value contains a wall
				if (this._boardValues [pos.GetX() - i, pos.GetZ()] == 10)
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeXSearch = false;
				}

				//checks if the current board value contains an enemy object
				if (this._boardValues [pos.GetX() - i, pos.GetZ()] == _enemyVal) 
				{
					//returns true, showing that an enemy object has been found
					return true;
				}
			}
		}

		for (int i = 0; i < GridSystem._gridSizeZ; i++) 
		{
			//check as to whether a wall has been collided with or not within this certain direction
			if (_positiveZSearch) 
			{
				//checks if the current board value contains a wall
				if (this._boardValues [pos.GetX(), pos.GetZ() + i] == 10)
				{
					//sets the boolean variable to false, stopping searches within this direction
					_positiveZSearch = false;
				}

				//checks if the current board value contains an enemy object
				if (this._boardValues [pos.GetX(), pos.GetZ() + i] == _enemyVal) 
				{
					//returns true, showing that an enemy object has been found
					return true;
				}
			}

			//check as to whether a wall has been collided with or not within this certain direction
			if (_negativeZSearch) 
			{
				//checks if the current board value contains a wall
				if (this._boardValues [pos.GetX(), pos.GetZ() - i] == 10)
				{
					//sets the boolean variable to false, stopping searches within this direction
					_negativeZSearch = false;
				}

				//checks if the current board value contains an enemy object
				if (this._boardValues [pos.GetX(), pos.GetZ() - i] == _enemyVal) 
				{
					//returns true, showing that an enemy object has been found
					return true;
				}
			}
		}

		return false;
	}

	//function used to find an object on the board
	public MonteCarloPosition FindPlayer(int _playerNo)
	{
		MonteCarloPosition pos = new MonteCarloPosition();

		int _player = 0;

		//checks for the player no, setting player as the referenced player value
		if (_playerNo == _enemyVal)
		{
			_player = _enemyVal;
		} 
		else if (_playerNo == _playerVal)
		{
			_player = _playerVal;
		}

		//loops through all board values on the board
		for (int x = 0; x < GridSystem._gridSizeX; x++)
		{
			for (int z = 0; z < GridSystem._gridSizeZ; z++)
			{
				//checks if the current board value is equal to the player defined by the player no
				if (_boardValues [x, z] == _player)
				{
					//sets the position x and z values to the current board x and z values
					pos.SetX (x);
					pos.SetZ (z);

					//returns the position
					return pos; 
				}
			}
		}

		//returns null if no object found
		return null;
	}

	//getters and setters
	public int[,] getBoardValues() 
	{
		return _boardValues;
	}

	public void setBoardValues(int[,] _boardValues) 
	{
		this._boardValues = _boardValues;
	}
}
