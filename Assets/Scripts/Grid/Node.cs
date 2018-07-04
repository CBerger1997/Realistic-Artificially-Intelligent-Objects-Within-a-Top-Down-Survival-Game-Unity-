using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{

	public GameObject _obj;
	public int _floorVal;
	public bool _changed;
	public int _gridX;
	public int _gridZ;
	public bool _playerNode;
	public bool _enemyNode;
	public int _enemyNo;
	public Node _child;
	public bool _walkable;

	//class constructor
	public Node( GameObject _gridObj, string _name, int _floorNo, int _gridPosX, int _gridPosZ)
	{
		//sets node values to referenced node values
		_obj = _gridObj;
		_obj.name = (_name);
		_floorVal = _floorNo;
		_gridX = _gridPosX;
		_gridZ = _gridPosZ;
		_child = null;
		_walkable = true;
		_playerNode = false;
		_enemyNode = false;
	}
}