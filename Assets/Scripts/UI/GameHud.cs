using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour {

	private GameObject _turnText; 
	private GameObject _waveText;
	private GameObject _noteText;
	private GameObject _gameManager;
	private GameObject _endBTN;
	private GameObject _playerPanel;
	private GameObject _abilityPanel;
	private GameObject _enemiesLeftText;
	private GameObject _abilityText;
	private GameObject _abilityTurnText;
	private GameObject _abilityBtn;

	private GameObject _menuHud;

	private GameObject _player;

	private GameObject _enemySpawnManager;


	private float _noteTimer;

	void Awake()
	{
		//finds and sets game objects
		_turnText = GameObject.Find ("TurnText");
		_waveText = GameObject.Find ("WaveText");
		_gameManager = GameObject.Find ("GameManager");
		_noteText = GameObject.Find ("NoteText");
		_endBTN = GameObject.Find ("EndTurnBtn");
		_playerPanel = GameObject.Find ("PlayerPanel");
		_abilityPanel = GameObject.Find ("AbilityPanel");
		_menuHud = GameObject.Find ("SettingsHud");
		_player = GameObject.FindGameObjectWithTag("Player");
		_enemiesLeftText = GameObject.Find ("EnemiesText");
		_enemySpawnManager = GameObject.Find ("SpawnManager");
		_abilityBtn = GameObject.Find ("AbilityBtn");
		_abilityText = GameObject.Find ("AbilityText");
		_abilityTurnText = GameObject.Find ("AbilityTurnText");
	}

	void Start () 
	{
		//sets variable
		_noteTimer = 0.0f;
	}
	
	void Update () 
	{
		//checks if note timer is greater than 0
		if (_noteTimer > 0.0f) 
		{
			//decreases note timer
			_noteTimer -= Time.deltaTime;
		}
		//checks if note timer is less than 0
		else if (_noteTimer <= 0.0f)
		{
			//clears note text
			ClearNoteText ();
		}

		//checks if player is not null
		if (_player != null) 
		{
			//hides or unhides player HUDS
			if (_player.GetComponent<PlayerManager> ()._hidePlayerHud == false) 
			{
				_playerPanel.gameObject.SetActive (true);
			}
			else 
			{
				_playerPanel.gameObject.SetActive (false);
			}

			if (_player.GetComponent<PlayerManager> ()._hideAbilityHud == false) 
			{
				_abilityPanel.gameObject.SetActive (true);
			}
			else 
			{
				_abilityPanel.gameObject.SetActive (false);
			}

			if (_player.GetComponent<PlayerAbilities> ().AbilityActive ())
			{
				_abilityBtn.GetComponent<Button> ().interactable = false;
			}
			else
			{
				_abilityBtn.GetComponent<Button> ().interactable = true;
			}

			//sets ability text
			SetAbilityText ();

			//sets ability turn text
			SetAbilityTurnText ();
		}
		else
		{
			//displays that player is null
			Debug.Log ("Game Hud Player object Null");
		}

		//sets enemies left text
		_enemiesLeftText.GetComponent<Text> ().text = "Enemies Left: " + _enemySpawnManager.GetComponent<EnemySpawnManager> ()._enemiesLeft;
	}

	#region GameHud UI

	//UI Functions

	public void SetTurnText()
	{
		string _text  = "";

		if (_gameManager.GetComponent<GameManager> ()._gameState == 0) 
		{
			_text = "Player";
		} 
		else if (_gameManager.GetComponent<GameManager> ()._gameState == 1) 
		{
			_text = "Enemy";
		}

		_turnText.GetComponent<Text> ().text = "Turn: " + _text;
	}

	public void SetNoteText(string _message)
	{
		_noteText.GetComponent<Text> ().text = _message;

		_noteTimer = 3.0f;
	}

	private void ClearNoteText()
	{
		_noteText.GetComponent<Text> ().text = "";
	}

	public void ClickShootBTN()
	{
		_player.GetComponent<PlayerManager>().ShootSelected ();
	}

	public void ClickMoveBTN()
	{
		_player.GetComponent<PlayerManager>().MoveSelected ();
	}

	public void ClickAbilityBtn()
	{
		_player.GetComponent<PlayerManager> ().AbilitySelected ();
	}

	public void ClickEndTurnBTN()
	{
		_gameManager.GetComponent<GameManager> ().EndTurn ();
	}

	public void ClickCancelBtn()
	{
		_player.GetComponent<PlayerManager>().PlayerUnselected ();
	}

	public void ClickAbilityCancelBtn()
	{
		_player.GetComponent<PlayerManager>().PlayerUnselected ();
	}

	public void ClickMenuBtn()
	{
		MenuOpenClose ();
	}

	public void ClickTurretBtn()
	{
		_player.GetComponent<PlayerManager>().PlayerUnselected ();

		Node _node;

		_node = GridSystem.FindSelectedObject (_player, MonteCarloBoard._playerVal);

		_player.GetComponent<PlayerAbilities> ().FindTurretPlaceableTiles (_node);
	}

	public void ClickInvincibleBtn()
	{
		_player.GetComponent<PlayerManager>().PlayerUnselected ();

		_player.GetComponent<PlayerAbilities> ().ActivateInvincibility ();
	}

	public void EnableEndTurnBtn()
	{
		_endBTN.GetComponent<Button> ().enabled = true;
	}

	public void DisableEndTurnBtn()
	{
		_endBTN.GetComponent<Button> ().enabled = false;
	}

	public void MenuOpenClose()
	{
		if (_menuHud.GetComponent<Canvas> ().enabled == false) 
		{
			GetComponent<Canvas> ().enabled = false;
			_player.GetComponent<PlayerManager>().PlayerUnselected ();
			_menuHud.GetComponent<Canvas> ().enabled = true;
		} 
		else 
		{
			_menuHud.GetComponent<Canvas> ().enabled = false;
			GetComponent<Canvas> ().enabled = true;
		}
	}

	public void SetWaveText()
	{
		_waveText.GetComponent<Text> ().text = "Wave: " + _gameManager.GetComponent<GameManager> ()._wave;
	}

	public void SetAbilityText()
	{
		_abilityText.GetComponent<Text> ().text = "Current Ability: " + _player.GetComponent<PlayerAbilities> ().GetActiveAbility ();
	}

	public void SetAbilityTurnText()
	{
		_abilityTurnText.GetComponent<Text> ().text = "Ability Turns Left: " + _player.GetComponent<PlayerAbilities> ()._abilityTimer;
	}

	#endregion
}
