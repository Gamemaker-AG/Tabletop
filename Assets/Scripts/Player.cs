using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
	private List<Character> _characterList = new List<Character>();
	private List<Character> _lastRemovedCharacters = new List<Character>();
    private List<Character> _characterOrder = new List<Character>(); // order of character for e.g. move routine; works like a stack
    private Texture _healthBarElement, _actionCircleTexture;
    private Rect _actionCircleRect;
    private bool _characterTouchWasAway; // for moving a character

	public Player(Texture healthBarElement, Texture actionCircleTexture)
	{
		_healthBarElement = healthBarElement;
        _actionCircleTexture = actionCircleTexture;
	}

	public void Update()
	{
	}
	public void OnGUI()
	{
        if (_actionCircleRect.width != 0)
        {
            GUI.DrawTexture(_actionCircleRect, _actionCircleTexture);
        }
        foreach (Character c in _characterList)
        {
            c.draw(_healthBarElement);
        }
        foreach (Character c in _lastRemovedCharacters)
        {
            c.draw(_healthBarElement);
        }
	}
	public bool addCharacter(Character c)
    {
        if (_characterTouchWasAway && _characterOrder[0].Equals(c))
        {
            int r = GameManager._canvas.GetComponent<Sidebar>().getResults()[0];

            if (Mathf.Pow(TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().x - _actionCircleRect.center.x, 2)
                + Mathf.Pow(Screen.height - TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().y - _actionCircleRect.center.y, 2) 
                < Mathf.Pow(r * 50, 2)) // If new touch is in circle ...
            {

                _characterOrder.RemoveAt(0); // to have stack-feeling
                _characterTouchWasAway = false;
                GameManager._canvas.GetComponent<Sidebar>().resetResults();
                _actionCircleRect = new Rect(0, 0, 0, 0);

                TouchManager._touchList[TouchManager._touchList.Count - 1].setCharacter(c);
                c.setPosition(new Rect(
                    TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().x,
                    Screen.height - TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().y + 100,
                    100,
                    100));

                _characterList.Add(c);
                _lastRemovedCharacters.Remove(c);
                return true;
            }
        }
        // TouchManager._touchList[TouchManager._touchList.Count - 1] is the last added field
        if (TouchManager._touchList[TouchManager._touchList.Count - 1].getFieldRect().Contains(c.getPositionVector()) // is new char in near of old char
            && _characterList.Count < 5)
        {
            Debug.Log("Moved Character B");
            TouchManager._touchList[TouchManager._touchList.Count - 1].setCharacter(c);
            c.setPosition(new Rect(
                TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().x, 
                Screen.height - TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().y + 100, 
                100, 
                100));

            _characterList.Add(c);
            _lastRemovedCharacters.Remove(c);
            return true;
        }
        return false;
	}
	public void removeCharacter(Character c)
	{
		_lastRemovedCharacters.Add(c);
		_characterList.Remove(c);
	}
    public void resetCharacterOrder()
    {
        foreach (Character c in _characterList)
        {
            _characterOrder.Add(c);
        }
        foreach (Character c in _lastRemovedCharacters)
        {
            _characterOrder.Add(c);
        }
    }
    public void moveCharacter()
    {
        if (GameManager._canvas.GetComponent<Sidebar>().getResults().Count == 0) return; // wenn noch nicht gewürfelt wurde, macht nichts


        if (!_characterTouchWasAway && !_characterList.Contains(_characterOrder[0]))
        {
            _characterTouchWasAway = true;
        }
        int radius = GameManager._canvas.GetComponent<Sidebar>().getResults()[0];
        _actionCircleRect = new Rect(_characterOrder[0].getPositionVector().x - 50 * radius, _characterOrder[0].getPositionVector().y - 100 - 50 * radius, radius * 100, radius * 100);
    }
}
