using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tree Node
public class MonteCarloNode 
{
	MonteCarloState _state;
	MonteCarloNode _parent;
	List<MonteCarloNode> _children;

	//class constructors
	public MonteCarloNode() 
	{
		this._state = new MonteCarloState();
		_children = new List<MonteCarloNode>();
	}

	public MonteCarloNode(MonteCarloState _newState) 
	{
		this._state = _newState;
		_children = new List<MonteCarloNode>();
	}

	public MonteCarloNode(MonteCarloNode _newNode) 
	{
		this._children = new List<MonteCarloNode>();

		this._state = new MonteCarloState(_newNode.GetState());

		if (_newNode.GetParent() != null)
			this._parent = _newNode.GetParent();
		
		List<MonteCarloNode> _nodeChildren = _newNode.GetChildren();

		foreach(MonteCarloNode _child in _nodeChildren) 
		{
			this._children.Add(new MonteCarloNode(_child));
		}
	}

	//returns a random child node
	public MonteCarloNode GetRandomChild()
	{
		//creates integer and sets value to number of children in node child array
		int _noOfChildren = this._children.Count;

		//random selects a number between 0 and the number of children
		int _selectRandom = (int)Random.Range (0, _noOfChildren);

		//returns the randomly selected child node
		return this._children[_selectRandom];
	}

	//returns node of child array with maximum visit count
	public MonteCarloNode GetMaxChild()
	{
		//creates int child index and sets equal to 0
		int _childIndex = 0;

		//creates double score
		double _score;

		//creates double prev score and sets to min int value
		double _prevScore = int.MinValue;

		//loops through amount of times as child array size
		for (int i = 0; i < this._children.Count; i++)
		{
			//sets score equal to current loop child array index node visit count
			_score = this._children[i].GetState().GetVisits();

			//Debug.Log("Cur Score: " + _score);

			//Debug.Log ("Prev Score: " + _prevScore);

			//checks if score is greater than prev score
			if (_score > _prevScore) 
			{

				//changes child index to current loop index value
				_childIndex = i;
			}

			//sets prev score equal to current score
			_prevScore = _score;
		}


		//returns node at child array child index
		return this._children [_childIndex];
	}

	//getters and setters
	public MonteCarloState GetState() 
	{
		return _state;
	}

	public void SetState(MonteCarloState _newState) 
	{
		this._state = _newState;
	}

	public MonteCarloNode GetParent() 
	{
		return _parent;
	}

	public void SetParent(MonteCarloNode _newParent) 
	{
		this._parent = _newParent;
	}

	public List<MonteCarloNode> GetChildren() 
	{
		return _children;
	}

	public void SetChildren(List<MonteCarloNode> _newChildren) 
	{
		this._children = _newChildren;
	}
}
