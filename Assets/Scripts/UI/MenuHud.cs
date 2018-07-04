using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHud : MonoBehaviour {

	private GameObject _zoomSlider;

	private GameObject _camera;

	private GameObject _gameHud;

	void Start () 
	{
		//finds game objects
		_camera = GameObject.Find ("Main Camera");

		_zoomSlider = GameObject.Find ("CameraSlider");

		_gameHud = GameObject.Find ("GameHud");
	}

	#region MenuHud UI

	//function used to set camera distance
	public void SetCameraDist()
	{
		//sets camera distance based on slider value
		_camera.GetComponent<CameraController>()._cameraDist = _zoomSlider.GetComponent<Slider> ().value;
	}

	//function used to return back to game
	public void ClickReturnBTN()
	{
		_gameHud.GetComponent<GameHud> ().MenuOpenClose ();
	}

	//function used to return back to main menu
	public void ClickMainMenu()
	{
		SceneManager.LoadScene ("Menu");
	}

	#endregion
}
