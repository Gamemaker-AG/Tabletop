using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameMode _currentGameMode;
    public static Canvas _canvas;
	public static Player _player1, _player2;
	public Texture _healthBarTexture,
		_circleTexture,
        _actionCircleTexture,
		_healthBarBorder;

	// Use this for initialization
	void Start () 
	{
		_canvas = GetComponent<Canvas>();
		_player1 = new Player(_healthBarTexture, _healthBarBorder, _actionCircleTexture, 1);
		_player2 = new Player(_healthBarTexture, _healthBarBorder, _actionCircleTexture, 2);
		_currentGameMode = new CharacterPlacing(_player1, _circleTexture);//,GUI);
		GameManager._canvas.GetComponent<Sidebar>().enableSidebar(1);
	}

	void OnGUI()
	{
		_currentGameMode.OnGUI();
		_player1.OnGUI();
		_player2.OnGUI();
		/*
		GUI.color = Color.white;
		if( == _player1) GUI.Label(new Rect(Screen.width/2-70, Screen.height - 70, 200, 100), _currentGameMode.getName() + " (Spieler 1)");
		else GUI.Label(new Rect(Screen.width/2-70, Screen.height - 20, 200, 100), _currentGameMode.getName() + " (Spieler 2)");
		*/
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
