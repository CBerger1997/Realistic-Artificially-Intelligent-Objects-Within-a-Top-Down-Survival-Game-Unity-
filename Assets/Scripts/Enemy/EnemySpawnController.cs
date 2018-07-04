using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {

	public GameObject _enemyObj;
	private GameObject _obj;
	public int _maxEnemies;
	public int _curEnemies;

	//function used to generate enemies
	public void GenerateEnemy(Node _node, int _val)
	{
		//no integer value used to set name for enemy object
		int no = _val + 10;

		//sets enemy node to true for enemy spawn position
		GridSystem._grid [_node._gridX, _node._gridZ]._enemyNode = true;

		//sets enemy no to no integer value for enemy spawn position
		GridSystem._grid [_node._gridX, _node._gridZ]._enemyNo = no;

		//instantiates the enemy object in enemy spawn position
		_obj = Instantiate (_enemyObj, GridSystem._grid[_node._gridX, _node._gridZ]._obj.transform.position, transform.rotation);

		//set object name
		_obj.name = no.ToString ();
	}

	//function used to locate spawn tiles
	public List<Node> LocateSpawnTiles(Node node)
	{
		//creates a list of nodes
		List<Node> _nodes = new List<Node> ();

		int _gridX;
		int _gridZ;

		_gridX = node._gridX;
		_gridZ = node._gridZ;

		//loops through checking player adjacent nodes, including diagonal nodes
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (!(x == 0 && z == 0)) 
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
							//adds the node to the list of nodes
							_nodes.Add(GridSystem._grid[_checkX,_checkZ]);
						}
					}
				}
			}
		}

		//returns list of nodes
		return _nodes;
	}

	//function used to find a spawner
	public Node FindSpawner()
	{
		//loops through all grid nodes
		for (int z = 0; z < GridSystem._gridSizeZ; z++) 
		{
			for (int x = 0; x < GridSystem._gridSizeX; x++) 
			{
				//checks if the grid position is equal to the spawner position
				if (GridSystem._grid [x, z]._obj.transform.position.x == transform.position.x && GridSystem._grid [x, z]._obj.transform.position.z == transform.position.z) 
				{
					//returns grid position
					return GridSystem._grid [x, z];
				}
			}
		}

		//returns null if spawner not found
		return null;
	}

}
