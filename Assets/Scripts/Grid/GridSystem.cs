using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour {

	#region Vars

	public static int _gridSizeX = 15;
	public static int _gridSizeZ = 13;
	public static Node[,] _grid;

	private int _floorCount = 0;
	public GameObject _floorDark;
	public Material _darkMat;
	public Material _darkMatMove;
	public Material _darkMatShoot;
	public Material _darkMatEnemy;
	public GameObject _floorLight;
	public Material _lightMat;
	public Material _lightMatMove;
	public Material _lightMatShoot;
	public Material _lightMatEnemy;
	private int _counter = 0;

	public LayerMask _unwalkable;

	#endregion

	void Awake()
	{
		GameObject _gridObj;
		RaycastHit _objHit;
		Vector3 _pos;

		//sets grid array size
		_grid = new Node[_gridSizeX, _gridSizeZ];

		//loops through entire grid
		for (int z = 0; z < _gridSizeZ; z++) 
		{
			for (int x = 0; x < _gridSizeX; x++) 
			{
				//sets position of each grid node
				_pos = new Vector3 (x * 2.25f, 0.0f, z * 2.25f);

				//checks if floor count is 0
				if (_floorCount == 0) 
				{
					//instantiates floor tile in position
					_gridObj = Instantiate (_floorDark, _pos, transform.rotation);

					//sets parent of grid node to grid object
					_gridObj.transform.parent = transform;

					//sets grid node with specified values
					_grid [x, z] = new Node (_gridObj, _counter.ToString (), 0, x, z);

					//sets floor count to 1
					_floorCount = 1;

					//increases counter
					_counter++;
				}
				else 
				{
					//instantiates floor tile in position
					_gridObj = Instantiate (_floorLight, _pos, transform.rotation);

					//sets parent of grid node to grid object
					_gridObj.transform.parent = transform;

					//sets grid node with specified values
					_grid [x, z] = new Node (_gridObj, _counter.ToString (), 1, x, z);

					//sets floor count to 0
					_floorCount = 0;

					//increases counter
					_counter++;
				}

				//checks for walkable nodes using raycasts
				if (Physics.Raycast (_pos, transform.TransformDirection (Vector3.up), out _objHit)) 
				{
					//checks if collided game object layer is 9
					if (_objHit.collider.gameObject.layer == 9) 
					{
						//sets walkable node to false
						_grid [x, z]._walkable = false;
					} 
				}

			}
		}
			
	}

	//function used to unselect grid tiles
	public void UnselectTiles()
	{
		//loops through entire grid
		for (int z = 0; z < _gridSizeZ; z++) 
		{
			for (int x = 0; x < _gridSizeX; x++) 
			{
				//sets grid changed to true
				if (_grid [x, z]._changed == true) 
				{
					//changes floor node material
					ChangeFloorMat (0, x, z);
				}
			}
		}
	}

	//function used to change the material of a grid node
	public void ChangeFloorMat(int _matSelect, int x, int z)
	{
		//0 = DefaultFloor
		//1 = MoveFloor
		//2 = ShootFloor

		Material _darkFloor;
		Material _lightFloor;
		string _tag;

		//switch statement to change floor materials
		//sets the node material for dark and light floors
		//sets the node changed as true
		//changes the node tag
		switch (_matSelect) 
		{
		case 1:
			{
				_darkFloor = _darkMatMove;
				_lightFloor = _lightMatMove;
				_grid [x, z]._changed = true;
				_tag = "FloorMove";
				break;
			}
		case 2:
			{
				_darkFloor = _darkMatShoot;
				_lightFloor = _lightMatShoot;
				_grid [x, z]._changed = true;
				_tag = "FloorShoot";
				break;
			}
		case 3:
			{
				_darkFloor = _darkMatEnemy;
				_lightFloor = _lightMatEnemy;
				_grid [x, z]._changed = true;
				_tag = "FloorShoot";
				break;
			}
		case 4:
			{
				_darkFloor = _darkMatMove;
				_lightFloor = _lightMatMove;
				_grid [x, z]._changed = true;
				_tag = "FloorTurret";
				break;
			}
		default:
			{
				_darkFloor = _darkMat;
				_lightFloor = _lightMat;
				_grid [x, z]._changed = false;
				_tag = "Floor";
				break;
			}
		}

		//checks if x and z are within the grid
		if (x >= 0 && x < _gridSizeX && z >= 0 && z < _gridSizeZ) 
		{
			//checks if grid node val is 0
			if (_grid [x, z]._floorVal == 0) 
			{			
				//changes node material to dark	
				_grid [x, z]._obj.GetComponent<Renderer> ().material = _darkFloor;

				//sets node tag to tag variable
				_grid [x, z]._obj.tag = _tag;

			} 
			//checks if grid node val is 1
			else if (_grid [x, z]._floorVal == 1) 
			{
				//changes floor material to light	
				_grid [x, z]._obj.GetComponent<Renderer> ().material = _lightFloor;

				//sets node tag to tag variable
				_grid [x, z]._obj.tag = _tag;
			}
		}
	}

	//function used to find a selected object within the grid
	public static Node FindSelectedObject(GameObject _obj, int _objectVal)
	{		
		//loops through entire grid
		for (int z = 0; z < _gridSizeZ; z++) 
		{
			for (int x = 0; x < _gridSizeX; x++) 
			{
				//checks if referenced object position is equal to current grid node position
				if (_grid [x, z]._obj.transform.position.x == _obj.transform.position.x && _grid [x, z]._obj.transform.position.z == _obj.transform.position.z) 
				{
					//checks if object value is equal to player value
					if (_objectVal == MonteCarloBoard._playerVal) 
					{
						//sets player node as true within grid node
						_grid [x, z]._playerNode = true;
					} 
					//checks if object value is equal to enemy value
					else if (_objectVal == MonteCarloBoard._enemyVal) 
					{
						//sets enemy node as true within grid node
						_grid [x, z]._enemyNode = true;
					}

					//returns grid node
					return _grid [x, z];				
				}		
			}
		}

		//returns null
		return null;
	}

	//function used to change string to integer
	public static int GetNameToInt (string _name)
	{
		int _val = 0;

		//changes string to integer
		int.TryParse (_name, out _val);

		//returns integer value
		return _val;
	}
}