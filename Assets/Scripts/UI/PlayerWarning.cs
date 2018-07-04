using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWarning : MonoBehaviour 
{
	private float _timer;
	private bool _showing;

	void Awake()
	{
		//disables object image component
		this.gameObject.GetComponent<Image>().enabled = false;
	}

	void Update()
	{
		//checks if showing is true
		if (_showing) 
		{
			//reduces timer
			_timer -= Time.deltaTime;

			//checks if timer is less than or equal to 0
			if (_timer <= 0) 
			{
				//removes warning
				RemoveWarning ();
			}
		}
	}

	//function used to show warning
	public void ShowWarning()
	{
		//enables object image component
		this.gameObject.GetComponent<Image>().enabled = true;

		//sets showing to true
		_showing = true;

		//sets timer to 3
		_timer = 3.0f;
	}

	//function used to remove warning
	public void RemoveWarning()
	{
		//disables object image component
		this.gameObject.GetComponent<Image>().enabled = false;

		//sets showing to false
		_showing = false;
	}

}
