using UnityEngine;
using System.Collections;

public class Player {
	private System.Collections.Generic.List<Character> _characterList = new System.Collections.Generic.List<Character>();
	private System.Collections.Generic.List<Character> _lastRemovedCharacters = new System.Collections.Generic.List<Character>();
	public Texture _healthBarElement;

	public Player(Texture healthBarElement)
	{
		_healthBarElement = healthBarElement;
	}

	public void Update()
	{
	}
	public void OnGUI()
	{
		foreach(Character c in _characterList)
		{
      		c.draw(_healthBarElement);
    	}
	}
	public void addCharacter(Character c)
	{
		if(_characterList.Count < 5) _characterList.Add(c);
	}
	public void removeCharacter(Character c)
	{
		_lastRemovedCharacters.Add(c);
		_characterList.Remove(c);
	}
}
