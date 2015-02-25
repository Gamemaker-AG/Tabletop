using UnityEngine;
using System.Collections;

public class CharacterPlacing : GameMode {
	public Texture _circle_Texture;
	private int _characterSetCounter = 0; // amount of placed characters
	private Player _player;
	//private GUI _gui;
	private bool[] _characterset = new bool[5];

	public CharacterPlacing(Player player, Texture circle)//, GUI gui)
	{
		_player = player;
		//_gui = gui;
		_circle_Texture = circle;
	}
	public void OnGUI()
	{
		if(_player.ID == 1) OnGUI1 ();
		else OnGUI2();
	}
	public void OnGUI1()
	{
		for(int i = 0; i < 5; i++)
		{
			GUI.DrawTexture(new Rect(650, 150 + 150*i, 100, 100), _circle_Texture);
		}
	}
	public void OnGUI2()
	{
		for(int i = 0; i < 5; i++)
		{
			GUI.DrawTexture(new Rect(Screen.width - 650, 150 + 150*i, 100, 100), _circle_Texture);
    	}
  	}
	public void Update()
	{
		if(_player.ID == 1) Update1 ();
		else Update2();
	}
	public void Update1 () 
	{
		foreach(TouchField f in TouchManager._touchList)
		{
			for(int i = 0; i < 5; i++)
			{
				if((new Rect(650, 100 + 150*i, 100, 100)).Contains(f.getPosition()) && !_characterset[i]) // Touch is in circle
				{
					//Debug.Log("Set Character");
                    //System.Random r = new System.Random();
                    //Character c = new Character(r.Next() % 100, 0, 3, 3, 3, new Rect(f.getPosition().x, Screen.height - f.getPosition().y + 100, 100, 100), _player); // some values
                    Character c = new Character(5, 0, 6, 0, 6, new Rect(f.getPosition().x, Screen.height - f.getPosition().y + 100, 100, 100), _player); // some values
					c.getDamage(4);
					f.setCharacter(c);
					_player.addCharacter(c);
					_characterSetCounter++;
					_characterset[i] = true;
					break;
				}
			}
		}
		if(_characterSetCounter == 5) // all characters set
		{
			GameManager._currentGameMode = new CharacterPlacing(GameManager._player2, _circle_Texture);
			Debug.Log (GameManager._canvas);
            GameManager._canvas.GetComponent<Sidebar>().resetResults(1);
			_player.resetCharacterOrder();
			GameManager._canvas.GetComponent<Sidebar>().enableSidebar(2);
		}
	}
	public void Update2 () 
	{
		foreach(TouchField f in TouchManager._touchList)
		{
			for(int i = 0; i < 5; i++)
			{
				if((new Rect(Screen.width - 650, 100 + 150*i, 100, 100)).Contains(f.getPosition()) && !_characterset[i]) // Touch is in circle
				{
					//Debug.Log("Set Character");
					Character c = new Character(5, 0, 6, 0, 6, new Rect(f.getPosition().x, Screen.height - f.getPosition().y + 100, 100, 100), _player); // some values
					c.getDamage(4);
					f.setCharacter(c);
					_player.addCharacter(c);
					_characterSetCounter++;
					_characterset[i] = true;
					break;
				}
			}
		}
		if(_characterSetCounter == 5) // all characters set
		{
			GameManager._currentGameMode = new MoveRoutine(GameManager._player1);
			GameManager._canvas.GetComponent<Sidebar>().resetResults(2);
			GameManager._player1.resetCharacterOrder();
			GameManager._canvas.GetComponent<Sidebar>().enableSidebar(1);
		}
	}
}
