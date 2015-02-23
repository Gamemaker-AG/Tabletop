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
		for(int i = 0; i < 5; i++)
		{
			GUI.DrawTexture(new Rect(150, 150 + 150*i, 100, 100), _circle_Texture);
		}
	}

	public void Update () 
	{
		foreach(TouchField f in TouchManager._touchList)
		{
			for(int i = 0; i < 5; i++)
			{
				if((new Rect(150, 100 + 150*i, 100, 100)).Contains(f.getPosition()) && !_characterset[i]) // Touch is in circle
				{
					Debug.Log("Set Character");
                    //System.Random r = new System.Random();
                    //Character c = new Character(r.Next() % 100, 0, 3, 3, 3, new Rect(f.getPosition().x, Screen.height - f.getPosition().y + 100, 100, 100), _player); // some values
                    Character c = new Character(100, 0, 3, 3, 3, new Rect(f.getPosition().x, Screen.height - f.getPosition().y + 100, 100, 100), _player); // some values
					//c.setGUI(_gui);
					c.getDamage(30);
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
            GameManager._currentGameMode = new MoveRoutine(_player); // TODO: später soll andere seite dran sein
            GameManager._canvas.GetComponent<Sidebar>().resetResults();
            _player.resetCharacterOrder();
		}
	}
}
