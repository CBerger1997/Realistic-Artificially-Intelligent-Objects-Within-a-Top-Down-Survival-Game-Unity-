using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The tree
public class MonteCarloTree
{
	MonteCarloNode _root;

	public bool learningPhase;

	//class constructors
	public MonteCarloTree()
	{
		_root = new MonteCarloNode ();

		learningPhase = false;
	}

	public MonteCarloTree(MonteCarloNode _node)
	{
		this._root = _node;
	}

	//getters and setters
	public MonteCarloNode getRoot()
	{
		return _root;
	}

	public void setRoot(MonteCarloNode _node)
	{
		this._root = _node;
	}

	public void addChild(MonteCarloNode _parent, MonteCarloNode _child)
	{
		_parent.GetChildren ().Add (_child);
	}
}
