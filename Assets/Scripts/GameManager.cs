using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	GameMode _currentGameMode;
	Player _player;
	public Texture _healthBarTexture,
		_circleTexture;

	// Use this for initialization
	void Start () 
	{
		_player = new Player(_healthBarTexture);
		_currentGameMode = new CharacterPlacing(_player, _circleTexture);//,GUI);
	}

	void OnGUI()
	{
		_currentGameMode.OnGUI();
		_player.OnGUI();
  	}
	
	// Update is called once per frame
	void Update () 
	{
		_currentGameMode.Update();
		_player.Update();
	}
}
