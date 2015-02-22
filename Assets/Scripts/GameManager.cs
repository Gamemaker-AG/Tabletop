using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameMode _currentGameMode;
	public static Player _currentPlayer;
    public static Canvas _canvas;
	Player _player;
	public Texture _healthBarTexture,
		_circleTexture,
        _actionCircleTexture;

	// Use this for initialization
	void Start () 
	{
        _player = new Player(_healthBarTexture, _actionCircleTexture);
		_currentGameMode = new CharacterPlacing(_player, _circleTexture);//,GUI);
        _canvas = GetComponent<Canvas>();
	}

	void OnGUI()
	{
		_currentGameMode.OnGUI();
		_player.OnGUI();
  	}
	
	// Update is called once per frame
	void Update ()
    {
        _player.Update();
        _currentGameMode.Update();
        //_player.Update(); // old position
	}

    public void setGameMode(GameMode newGameMode)
    {
        _currentGameMode = newGameMode;
    }
}
