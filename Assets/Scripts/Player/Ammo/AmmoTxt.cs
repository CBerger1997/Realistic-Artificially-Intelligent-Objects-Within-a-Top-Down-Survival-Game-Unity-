using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTxt : MonoBehaviour 
{
	private float _ammo;

	// Use this for initialization
	void Start () 
	{
		//sets ammo text to current ammo
		GetComponent<Text>().text = "Ammo: " + transform.parent.parent.GetComponent<Player_Controller> ()._ammo;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//sets ammo text to current ammo
		GetComponent<Text>().text = "Ammo: " + transform.parent.parent.GetComponent<Player_Controller> ()._ammo;
	}
}
