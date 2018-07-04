using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteCarloEnemy : MonoBehaviour 
{
	
	MonteCarloTreeSearch _mcts = new MonteCarloTreeSearch ();

	//function used to update a board
	public MonteCarloBoard UpdateBoard(float _playerHealth, float _enemyHealth, Node _objNode)
	{
		MonteCarloBoard _board = new MonteCarloBoard();

		//loops through all values of the board
		for (int z = 0; z < GridSystem._gridSizeZ; z++) 
		{
			for (int x = 0; x < GridSystem._gridSizeX; x++) 
			{
				//checks for the enemy node
				if (GridSystem._grid [x, z] == _objNode)
				{
					//sets value of current board x and z to enemy value
					_board.SetSpecificBoardValues (MonteCarloBoard._enemyVal, x, z);
				}
				//checks for the player node
				else if (GridSystem._grid [x, z]._playerNode == true)
				{
					//sets value of current board x and z to player value
					_board.SetSpecificBoardValues (MonteCarloBoard._playerVal, x, z);
				}
				//checks for unwalkable nodes
				else if (GridSystem._grid [x, z]._walkable == false)
				{
					//sets value of current board x and z to 10 setting the value as unwalkable
					_board.SetSpecificBoardValues (10, x, z);
				}
				else
				{
					//sets value of current board x and z to 0, making it empty
					_board.SetSpecificBoardValues (0, x, z);
				}
			}
		}

		//sets player and enemy health values
		_board._playerHealth = _playerHealth;
		_board._enemyHealth = _enemyHealth;
		_board._originalPlayerHealth = _playerHealth;
		_board._originalEnemyHealth = _enemyHealth;

		//returns the updated board
		return _board;
	}

	//function used to perform MCTS
	public MonteCarloPosition StartMCTS(float _curPlayerHealth, float _curEnemyHealth, Node _objNode)
	{
		//updates the board to fit the current grid
		MonteCarloBoard _board = UpdateBoard (_curPlayerHealth, _curEnemyHealth, _objNode);				

		//performs MCTS on the updated board
		_board = _mcts.findNextMove (_board);

		MonteCarloPosition _enemyPos = new MonteCarloPosition();

		//sets the new enemy pos to the positions returned from MCTS
		_enemyPos = _board.FindPlayer (MonteCarloBoard._enemyVal);

		//returns the enemy position
		return _enemyPos;
	}
}
