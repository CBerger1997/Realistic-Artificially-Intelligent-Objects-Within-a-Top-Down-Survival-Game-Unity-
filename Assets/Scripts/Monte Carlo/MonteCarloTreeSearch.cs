using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteCarloTreeSearch
{
	MonteCarloTree _tree;

	//Constructor for the class
	public MonteCarloTreeSearch() 
	{
		_tree = new MonteCarloTree ();
	}
		
	//function used to perform MCTS and return the best node's board
	public MonteCarloBoard findNextMove(MonteCarloBoard _board)
	{
		//gathers a start and end time, limiting the amount of MCTS cycles for the enemy to a timer
		float _start = Time.realtimeSinceStartup * 100;

		float _end = _start + 5;

		//gets the root of the tree
		MonteCarloNode _rootNode = _tree.getRoot ();

		//sets the root node to the current board
		_rootNode.GetState ().SetBoard (_board);

		//checks whether learning phase has been carried out
		if (!_tree.learningPhase) 
		{
			//performs 1000 iterations of MCTS
			for (int l = 0; l < 1000; l++) 
			{
				//function used to perform MCTS
				MCTSCycle (_rootNode);
			}

			//sets learning phase as true so that it does not occur again
			_tree.learningPhase = true;
		} 
		else 
		{
			//performs the MCTS for the limited amount of time, terminating on the last cycle once the time is reached.
			while (Time.realtimeSinceStartup * 100 < _end)
			{
				//function used to perform MCTS
				MCTSCycle (_rootNode);
			}

		}

		//finds the best child node of the root node
		MonteCarloNode _optimalNode = _rootNode.GetMaxChild ();

		//sets the root node to the new winner node
		_tree.setRoot (_optimalNode);

		//returns the board of the root's best child node
		return _optimalNode.GetState ().GetBoard ();
	}

	//function used to perform the MCTS Cycle
	public void MCTSCycle(MonteCarloNode _rootNode)
	{
		// Phase 1 - Selection
		//finds best node from root node using UCT (Upper Confidence Bound for Trees)
		MonteCarloNode _selectionNode = Selection (_rootNode);

		// Phase 2 - Expansion
		//Checks if the selected node produces a win state, otherwise expanding on the selected node
		if (_selectionNode.GetState ().GetBoard ().CheckStatus () == MonteCarloBoard._inProgress) 
		{
			//Performs expansion on the seleted node
			Expansion (_selectionNode);
		}

		// Phase 3 - Simulation
		//sets the node to explore to the selected node
		MonteCarloNode _nodeToExplore = _selectionNode;

		//checks if the node to explore contains any children
		if (_selectionNode.GetChildren ().Count > 0) 
		{
			//sets the node to explore to a random child node if any are available
			_nodeToExplore = _selectionNode.GetRandomChild ();
		}

		//simulates the node to explore and produces the number of the winner of the simulation, storing the result
		int _simulationWinner = Simulation (_nodeToExplore);

		// Phase 4 - Update
		//backpropagates up the tree, updating node scores from the node to explore to root based upon the winner of the simulation
		Backpropogation (_nodeToExplore, _simulationWinner);
	}

	//function used to perform the selection of the MCTS cycle
	private MonteCarloNode Selection(MonteCarloNode _rootNode) 
	{
		//sets a new node variable to the root node passed through
		MonteCarloNode _UCTnode = _rootNode;

		//checks if the node contains any child nodes
		while (_UCTnode.GetChildren().Count != 0) 
		{
			//finds the best child node using UCT (Upper Confidence Bound for Trees) of the current node, then updates the node
			//this is looped while the node contains children, finding the best child node with no more child nodes
			_UCTnode = MonteCarloUCT.findBestUCTNode(_UCTnode);
		}

		//the UCT node is returned
		return _UCTnode;
	}

	//function used to perform expansion of the MCTS cycle
	private void Expansion(MonteCarloNode _nodeToExpand) 
	{
		//finds all legal states for the current node
		List<MonteCarloState> _legalStates = _nodeToExpand.GetState().GetLegalStates();

		//loops through each legal state found
		foreach(MonteCarloState _state in _legalStates)
		{
			//creates a new node, setting it's state to a legal state found
			MonteCarloNode _newNode = new MonteCarloNode(_state);

			//sets the parent as the node to expand as passed through
			_newNode.SetParent(_nodeToExpand);

			//adds the new node to the child array of the node to expand
			_nodeToExpand.GetChildren().Add(_newNode);
		}
	}

	//function used to perform simulation of the MCTS cycle
	private int Simulation(MonteCarloNode _nodeToSimulate) 
	{
		//sets the turn number to the enemy value, so that the simulation begins within the right turn
		int _turn = MonteCarloBoard._enemyVal;

		//creates a temporary node, setting the values to that of the node to simulate
		MonteCarloNode _tempNode = new MonteCarloNode(_nodeToSimulate);

		//creates a tempporary state, setting the values to that of the temporary node's state
		MonteCarloState _tempState = _tempNode.GetState();

		//checks for the current board status of the temporary node
		int _boardStatus = _tempState.GetBoard().CheckStatus();

		//checks if the current board state produces a win for the player
		if (_boardStatus == MonteCarloBoard._playerVal) 
		{
			//sets the score of the temp node's parent's state to the minimum integer value
			_tempNode.GetParent().GetState().SetScore(int.MinValue);

			//returns the board status value
			return _boardStatus;
		}

		//loops while the board state is in progress
		while (_boardStatus == MonteCarloBoard._inProgress) 
		{
			//changes the turn between the player and the enemy
			_turn = 3 - _turn;

			//performs a random play from the current tempstate
			_tempState.randomPlay(_turn);

			//sets the board status to the current board state of the temp state board
			_boardStatus = _tempState.GetBoard().CheckStatus();
		}

		//returns the board status value
		return _boardStatus;
	}

	//function used to perform backpropagation of the MCTS cycle
	private void Backpropogation(MonteCarloNode _nodeToExplore, int _winningPlayerNo) 
	{
		//creates a backup node, setting the values to that of node to expore as passed through
		MonteCarloNode _backupNode = _nodeToExplore;

		//loops while the backup node is not null
		while (_backupNode != null) 
		{
			//increments the visit count of the backup node's state
			_backupNode.GetState().incrementVisit();

			//checks if the winner of the simulation was the enemy
			if (_winningPlayerNo == MonteCarloBoard._enemyVal)
			{
				//increases the backup node's state's score by 10
				_backupNode.GetState ().addScore (10);
			} 

			//checks if the winner of the simulation was the player
			else if(_winningPlayerNo == MonteCarloBoard._playerVal)
			{
				//decreases the backup node's state's score by 10
				_backupNode.GetState ().addScore (-10);
			}

			//sets the backup node to it's parent node
			_backupNode = _backupNode.GetParent();
		}
	}
}