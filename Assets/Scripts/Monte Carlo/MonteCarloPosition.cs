using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonteCarloPosition
{
	int _x;
	int _z;


	//class constructor
	public MonteCarloPosition()
	{
		
	}

	public MonteCarloPosition(int _x, int _z)
	{
		this._x = _x;
		this._z = _z;
	}

	//getters and setters
	public int GetX()
	{
		return _x;
	}

	public void SetX(int _x)
	{
		this._x = _x;
	}

	public int GetZ() 
	{
		return _z;
	}

	public void SetZ(int _z)
	{
		this._z = _z;
	}
}