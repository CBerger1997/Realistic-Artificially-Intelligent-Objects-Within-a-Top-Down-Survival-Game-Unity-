using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject _player;
	private Vector3 _offset;
	public float _cameraDist;

	// Use this for initialization
	void Start () 
	{
		//sets camera position above player object
		transform.position = new Vector3 (_player.transform.position.x, 
										  _player.transform.position.y + 1.0f, 
										  _player.transform.position.z);

		//sets camera offset from player position
		_offset = transform.position - _player.transform.position;

		//sets camera distance
		_cameraDist = 20.0f;
	}
		
	void LateUpdate()
	{
		//checks if player is no null
		if (_player != null)
		{
			//moves camera
			Movement ();
		} 
		else 
		{
			//displays player object is null
			Debug.Log ("Camera Player Null");
		}
	}

	void Movement()
	{
		//sets object position to player position plus the offset multiplied by the camera distance
		transform.position = _player.transform.position + _offset.normalized * _cameraDist;

		//sets camera offset from player position
		_offset = transform.position - _player.transform.position;
	}
}
