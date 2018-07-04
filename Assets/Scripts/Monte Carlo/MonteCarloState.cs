using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State
public class MonteCarloState
{
	private MonteCarloBoard _board;
	private int _visits;
	private int _score;

	//class constructors
	public MonteCarloState()
	{
		_board = new MonteCarloBoard();
	}

	public MonteCarloState(MonteCarloState _state) 
	{
		this._board = new MonteCarloBoard(_state.GetBoard());
		this._visits = _state.GetVisits();
		this._score = _state.GetScore();
	}

	public MonteCarloState(MonteCarloBoard _board)
	{
		this._board = new MonteCarloBoard(_board);
	}

	//getters and setters
	public MonteCarloBoard GetBoard() 
	{
		return _board;
	}

	public void SetBoard(MonteCarloBoard _board)
	{
		this._board = _board;
	}

	public int GetVisits() 
	{
		return _visits;
	}

	public void SetVisits(int _visitsNo) 
	{
		this._visits = _visitsNo;
	}

	public int GetScore() 
	{
		return _score;
	}

	public void SetScore(int _scoreNo)
	{
		this._score = _scoreNo;
	}

	//function that returns all legal states available for current action
	public List<MonteCarloState> GetLegalStates()
	{
		//creates a list to store the legal states
		List<MonteCarloState> _legalStates = new List<MonteCarloState>();

		//creates a list to store all legal positions and stores it with all legal positions returned from a function
		List<MonteCarloPosition> _legalPositions = this._board.GetLegalPositions(MonteCarloBoard._enemyVal);

		//loops through each position in the legal positions list
		foreach(MonteCarloPosition _p in _legalPositions)
		{
			//creates a new state with the current board
			MonteCarloState _newState = new MonteCarloState(this._board);

			//performs the enemy action within the new state
			_newState.GetBoard().performEnemyMove(_p);

			//adds the new state to the list of legal states
			_legalStates.Add(_newState);
		}

		return _legalStates;
	}

	//increments the visit count by 1
	public void incrementVisit()
	{
		this._visits++;
	}

	//adds a value to the current score variable as long as the current score is not equal to the integer minimum value
	public void addScore(int _addScore) 
	{
		if (this._score != int.MinValue) 
		{
			this._score += _addScore;
		}
	}

	//function that randomly selects an action to play
	public void randomPlay(int _turn)
	{
		//creates a list of all legal positions for the current player turn
		List<MonteCarloPosition> _legalPositions = this._board.GetLegalPositions(_turn);

		//creates an integer value to store the number of legal positions found
		int _totalLegalPossibilities = _legalPositions.Count;

		//selects a random integer value between 0 and the number of legal positions
		int _selectRandom = (int)(Random.Range (0, _totalLegalPossibilities));

		//checks for the player's turn
		if (_turn == MonteCarloBoard._enemyVal) 
		{
			//performs enemy move in randomly selected position
			this._board.performEnemyMove(_legalPositions[_selectRandom]);
		}
		else 
		{
			//performs player move in randomly selected position
			this._board.performPlayerMove(_legalPositions[_selectRandom]);
		}
	}
}
