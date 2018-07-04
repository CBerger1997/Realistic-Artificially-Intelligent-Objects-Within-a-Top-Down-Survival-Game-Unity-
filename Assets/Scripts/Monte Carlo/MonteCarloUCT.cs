using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonteCarloUCT
{
	//performs UCT using a child nodes score and visit count, along with it's parent visit value
	public static double UCTValue(int _totalVisit, double _nodeWinScore, int _nodeVisit)
	{
		//checks if the node has been visited
		if (_nodeVisit == 0) 
		{
			//returns the integer max value if the node hasn't been visited yet
			return int.MaxValue;
		}

		//The UCT Formula, used to calculate the best child node
		return (_nodeWinScore / (double)_nodeVisit) + 1.41 * System.Math.Sqrt (System.Math.Log (_totalVisit) / (double)_nodeVisit);
	}

	//Finds max visit value within child nodes of referenced Node
	public static MonteCarloNode findBestUCTNode(MonteCarloNode _node)
	{
		//creates a new int parent visit, setting the value to that of the referenced node's state's visit count
		int _parentVisit = _node.GetState ().GetVisits ();

		//creates variables used to assist in finding the best UCT node
		int _childIndex = 0;

		double _uctValue;

		double _prevUctValue = int.MinValue;

		//loops which iterates for the number of children in the referenced node, finding the best child node with UCT
		for (int i = 0; i < _node.GetChildren().Count; i++)
		{
			//finds the UCT value of the current child node
			_uctValue = UCTValue(_parentVisit, _node.GetChildren()[i].GetState().GetScore(), _node.GetChildren()[i].GetState().GetVisits());

			//checks if the current UCT value is greater than that of the previous UCT value
			if (_uctValue > _prevUctValue) 
			{
				//sets the child index to the index of the current child node
				_childIndex = i;
			}

			//sets the previous UCT value to the current UCT value
			_prevUctValue = _uctValue;
		}

		//returns the child node at the index which produced the best UCT value
		return _node.GetChildren ()[_childIndex];
	}
		
}
