using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameMode _currentGameMode;
	public static Player _currentPlayer;
    public static Canvas _canvas;
	public static Player _player1, _player2;
	public Texture _healthBarTexture,
		_circleTexture,
        _actionCircleTexture;

	// Use this for initialization
	void Start () 
	{
		_player1 = new Player(_healthBarTexture, _actionCircleTexture, 1);
		_player2 = new Player(_healthBarTexture, _actionCircleTexture, 2);
		_currentGameMode = new CharacterPlacing(_player1, _circleTexture);//,GUI);
        _canvas = GetComponent<Canvas>();
	}

	void OnGUI()
	{
		_currentGameMode.OnGUI();
		_player1.OnGUI();
		_player2.OnGUI();
  	}
	
	// Update is called once per frame
	void Update ()
	{
		_player1.Update();
		_player2.Update();
		_currentGameMode.Update();
        //_player.Update(); // old position
	}

    public void setGameMode(GameMode newGameMode)
    {
        _currentGameMode = newGameMode;
    }
}
