using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionHud : MonoBehaviour 
{
	public void ClickBackBtn()
	{
		SceneManager.LoadScene ("Menu");
	}
}
