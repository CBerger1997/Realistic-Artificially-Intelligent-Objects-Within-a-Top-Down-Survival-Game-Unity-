using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {

	public GameObject _turret;
	private GameObject _curTurret;
	public GameObject _invincible;
	private GameObject _curInvincible;
	private bool _turretActive;
	private bool _invincibleActive;
	private bool _abilityUsed;
	private GameObject _gridSystem;
	private GameObject _gameManager;
	public int _abilityTimer;
	public bool _turnOver;

	void Start ()
	{
		//sets variables
		_invincibleActive = false;

		_turretActive = false;

		_gridSystem = GameObject.Find ("Grid");

		_gameManager = GameObject.Find ("GameManager");

		_abilityTimer = 0;

		_turnOver = false;
	}
	
	void Update () 
	{
		//checks if current turret is null
		if (_curTurret == null)
		{
			//removs turret
			removeTurret ();
		}

		//checks if current invincibility is null
		if (_curInvincible == null)
		{
			//removes invincibility
			removeInvincibility ();
		}

		//checks if current turret is not null and turret action has been performed
		if (_curTurret != null && _curTurret.GetComponent<TurretController>()._turretActionPerformed == true)
		{
			//sets action performed to false
			_curTurret.GetComponent<TurretController> ()._turretActionPerformed = false;

			//checks if current turret timer is less than or equal to 0
			if (_curTurret.GetComponent<TurretController> ().GetTurretTimer () <= 0) 
			{
				//destroys current turret
				_curTurret.GetComponent<TurretController> ().DestroyTurret ();
			}

			//changes game turn
			_gameManager.GetComponent<GameManager> ().ChangeTurn ();
		}
	}
		
	//function used to find placeable turret nodes
	public void FindTurretPlaceableTiles(Node node)
	{
		int _gridX;
		int _gridZ;

		_gridX = node._gridX;
		_gridZ = node._gridZ;

		//loops through checking player adjacent nodes, except diagonal nodes
		for (int x = -1; x <= 1; x++) 
		{
			for (int z = -1; z <= 1; z++)
			{
				if (x == 0 || z == 0)
				{

					//sets the check x and z variables
					int _checkX = _gridX + x;
					int _checkZ = _gridZ + z;

					//checks if the check x and z varables are within the grid
					if (_checkX >= 0 && _checkX < GridSystem._gridSizeX && _checkZ >= 0 && _checkZ < GridSystem._gridSizeZ) 
					{

						//checks if the node is walkable					
						if (GridSystem._grid[_checkX, _checkZ]._walkable) 
						{
							//changes the floor tile material
							_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (4, _checkX, _checkZ);
						}
					}
				}
			}
		}

		//changes the floor tile material
		_gridSystem.GetComponent<GridSystem>().ChangeFloorMat (0, _gridX, _gridZ);
	}

	//function used to place the turret
	public void PlaceTurret(GameObject _obj)
	{
		//instantiates turret
		_curTurret = Instantiate (_turret, _obj.transform.position, _turret.transform.rotation);

		//sets the instantiated turret
		setTurret ();
	}

	//performs turret action
	public void TurretAction ()
	{
		//checks if current turret is not null
		if (_curTurret != null) 
		{
			//performs turret action
			_curTurret.GetComponent<TurretController> ().PerformTurretAction ();
		}
	}

	//performs invincibility action
	public void InvincibleAction()
	{
		//checks if current invincible object is not null
		if (_curInvincible != null)
		{
			//reduces invinciblity timer
			_curInvincible.GetComponent<InvincibleController> ().reduceAbilityTimer ();

			//changes game turn
			_gameManager.GetComponent<GameManager> ().ChangeTurn ();
		}
	}

	//function used to set up new wave
	public void NewWaveSetup()
	{
		//sets ability used to false
		_abilityUsed = false;
	}

	//function used to activate invinciblity
	public void ActivateInvincibility()
	{
		//instantiates invincibility object
		_curInvincible = Instantiate (_invincible, transform.position, transform.rotation);

		//sets invincibility object
		setInvincibitlity ();
	}

	//function used to reduce ability timer
	public void LowerAbilityTimer()
	{
		//reduces ability time
		_abilityTimer--;
	}

	//getters and setters
	public void setInvincibitlity()
	{
		_invincibleActive = true;
		_abilityUsed = true;
		_abilityTimer = 3;
	}

	public void removeInvincibility()
	{
		_invincibleActive = false;
	}

	public bool getInvincibility()
	{
		return _invincibleActive;
	}

	public void setTurret()
	{
		_turretActive = true;
		_abilityUsed = true;
		_abilityTimer = 3;
	}

	public void removeTurret()
	{
		_turretActive = false;
	}

	public bool getTurret()
	{
		return _turretActive;
	}

	public bool AbilityActive()
	{
		if (_turretActive == true || _invincibleActive == true || _abilityUsed == true) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public string GetActiveAbility()
	{
		if (_turretActive == true) 
		{
			return "Turret";
		}
		else if (_invincibleActive == true) 
		{
			return "Invincibility";
		}
		else 
		{
			return "None";
		}
	}
}
