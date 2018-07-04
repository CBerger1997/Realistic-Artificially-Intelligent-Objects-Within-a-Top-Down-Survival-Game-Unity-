using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour 
{

	private float _health;
	Quaternion _rotation;
	float _positionDif;

	void Awake()
	{
		//sets rotation
		_rotation = transform.rotation;
	}

	// Use this for initialization
	void Start () 
	{
		//sets position difference
		_positionDif = 3.0f;

		//checks if the objects parent's parent's tag is enemy
		if (transform.parent.parent.tag == "Enemy") 
		{
			//sets slider value
			GetComponent<Slider>().maxValue =  transform.parent.parent.GetComponent<EnemyController> ()._health;
		} 
		//checks if the objects parent's parent's tag is enemy
		else if (transform.parent.parent.tag == "Player") 
		{
			//sets slider value
			GetComponent<Slider>().maxValue =  transform.parent.parent.GetComponent<Player_Controller> ()._health;
		}

	}

	// Update is called once per frame
	void Update () 
	{
		//checks if the objects parent's parent's tag is enemy
		if (transform.parent.parent.tag == "Enemy") 
		{
			//sets health to current enemy health
			_health = transform.parent.parent.GetComponent<EnemyController> ()._health;

			//sets slider value to current health
			GetComponent<Slider> ().value = _health;
		} 
		//checks if the objects parent's parent's tag is enemy
		else if (transform.parent.parent.tag == "Player") 
		{
			//sets health to current player health
			_health = transform.parent.parent.GetComponent<Player_Controller> ()._health;

			//sets slider value to current health
			GetComponent<Slider> ().value = _health;
		}
	}

	void LateUpdate()
	{
		//sets transform rotation
		transform.parent.rotation = _rotation;

		//sets transform postion
		transform.parent.position = new Vector3 (transform.parent.parent.position.x, transform.parent.parent.position.y + _positionDif, transform.parent.parent.position.z);
	}
}
