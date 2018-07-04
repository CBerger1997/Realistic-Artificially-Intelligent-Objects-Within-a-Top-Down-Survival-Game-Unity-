using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHud : MonoBehaviour 
{

	public void PlayButtonClicked()
	{
		SceneManager.LoadScene ("Floor 1");
	}

	public void instructionButtonClicked()
	{
		SceneManager.LoadScene ("Instructions");
	}

}
