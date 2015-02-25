using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
	private List<Character> _characterList = new List<Character>();
	private List<Character> _lastRemovedCharacters = new List<Character>();
    private List<Character> _characterOrder = new List<Character>(); // order of character for e.g. move routine; works like a stack
	private List<Character> _enemyInRange = new List<Character>();
	private int _enemyIndex;
	private Texture _healthBarElement, _actionCircleTexture, _healthBarBorder;
    private Rect _actionCircleRect;
    private bool _characterTouchWasAway; // for moving a character
	public int ID;

	public Player(Texture healthBarElement,Texture healthBarBorder, Texture actionCircleTexture, int ID)
	{
		_healthBarElement = healthBarElement;
		_healthBarBorder = healthBarBorder;
        _actionCircleTexture = actionCircleTexture;
		this.ID = ID;
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
            c.draw(_healthBarElement, _healthBarBorder);
        }
        foreach (Character c in _lastRemovedCharacters)
        {
			c.draw(_healthBarElement, _healthBarBorder);
        }
	}
	public bool addCharacter(Character c)
	{
		// for move routine:
        if (_characterTouchWasAway // character was taken away to set new position
		    && _characterOrder[0].Equals(c))
		{
            int r = GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[0];

            if (Mathf.Pow(TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().x - _actionCircleRect.center.x, 2)
                + Mathf.Pow(Screen.height - TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().y - _actionCircleRect.center.y, 2) 
                < Mathf.Pow(r * 50, 2)) // If new touch is in circle ...
            {
                _characterOrder.RemoveAt(0); // to have stack-feeling
                _characterTouchWasAway = false;
				GameManager._canvas.GetComponent<Sidebar>().resetResults(ID);
				_actionCircleRect = new Rect(0, 0, 0, 0);

                TouchManager._touchList[TouchManager._touchList.Count - 1].setCharacter(c);
                c.setPosition(new Rect(
                    TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().x,
                    Screen.height - TouchManager._touchList[TouchManager._touchList.Count - 1].getPosition().y + 100,
                    100,
                    100));

                _characterList.Add(c);
                _lastRemovedCharacters.Remove(c);

				Debug.Log (_characterOrder.Count);

				if(_characterOrder.Count == 0) // every character has been set
				{
					Debug.Log("ID: " + ID);
					if(ID == 1)
					{
						GameManager._currentGameMode = new MoveRoutine(GameManager._player2);
						GameManager._canvas.GetComponent<Sidebar>().resetResults(1);
						GameManager._player2.resetCharacterOrder();
						GameManager._canvas.GetComponent<Sidebar>().enableSidebar(2);
					}
					else
					{
						Debug.Log ("OOOOOOKKKKKK");
						GameManager._currentGameMode = new FightRoutine(GameManager._player1);
						GameManager._canvas.GetComponent<Sidebar>().resetResults(2);
						GameManager._player1.resetCharacterOrder();
						GameManager._player1._enemyIndex = 0;
						GameManager._player1.calcEnemiesInRange();
						GameManager._canvas.GetComponent<Sidebar>().enableSidebar(1);
					}
				}

                return true;
            }
        }
        // TouchManager._touchList[TouchManager._touchList.Count - 1] is the last added field
        if (TouchManager._touchList[TouchManager._touchList.Count - 1].getFieldRect().Contains(c.getPositionVector()) // is new char in near of old char
		    && _characterList.Count < 5 && !_characterList.Contains(c))
        {
            //Debug.Log("Moved Character B");
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
	public void removeCharacter(Character c, bool permanent = false)
	{
		if(!permanent) _lastRemovedCharacters.Add(c);
		else
		{
			_lastRemovedCharacters.Remove(c);
			_characterOrder.Remove(c);		}
		_characterList.Remove(c);
	}
    public void resetCharacterOrder()
    {
		_characterOrder.Clear();
        foreach (Character c in _characterList)
        {
            if(!_characterOrder.Contains(c)) _characterOrder.Add(c);
        }
        foreach (Character c in _lastRemovedCharacters)
        {
			if(!_characterOrder.Contains(c)) _characterOrder.Add(c);
        }
    }
    public void moveCharacter()
    {
		if (GameManager._canvas.GetComponent<Sidebar>().getResults(ID).Count == 0) return; // if no dice-value has been set
        if (!_characterList.Contains(_characterOrder[0]))
        {
            _characterTouchWasAway = true;
		}
		int radius = GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[0];
        _actionCircleRect = new Rect(_characterOrder[0].getPositionVector().x - 50 * radius, _characterOrder[0].getPositionVector().y - 100 - 50 * radius, radius * 100, radius * 100);
    }
	public void fightCharacter()
	{
		// Würfel 1: Treffer (Ja, Nein?): Ergebnis > (7-Ballistik)
		// Würfel 2: Verwundung:
		// 		Waffe.strength == resistance -> 4, 5, 6 zum verwunden
		// 		Waffe.strength == resistance +1 -> 3, 4, 5, 6 zum verwunden
		// 		Waffe.strength == resistande +2/+3/... -> 2, 3, 4, 5, 6 zum verwunden
		// 		Waffe.strength +1 == resistance -> 5, 6
		// 		Waffe.strength +1 > resistance -> 6
		// Würfel 3: Rüstung vom Ziel
		// 		Beispiel: Bei Rüstung 4 ist bei Würfel-Ergebnis 4, 5, 6 keinen Treffer
		//		Beispiel: Bei Rüstung 3 ist bei Würfel-Ergebinis 3, 4, 5, 6 keinen Treffer

		if(_characterOrder.Count == 0) return;
		
		int radius = _characterOrder[0].getWeaponRadius();
		//Debug.Log ("FIGHT"+ radius);
		_actionCircleRect = new Rect(_characterOrder[0].getPositionVector().x - radius, _characterOrder[0].getPositionVector().y - 100 - radius, radius * 2, radius * 2);

		if (GameManager._canvas.GetComponent<Sidebar>().getResults(ID).Count >= 3)
		{
			Debug.Log("G0");
      		if(GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[0] > (7-_characterOrder[0].getBallistic()))
			{
				Debug.Log("G1");
				if(		(_characterOrder[0].getWeaponStrength() == _characterOrder[0].getResistance()
						&& GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[1] >= 4)
					|| 
						(_characterOrder[0].getWeaponStrength() == _characterOrder[0].getResistance() + 1
						&& GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[1] >= 3)
					||	
						(_characterOrder[0].getWeaponStrength() >= _characterOrder[0].getResistance() + 2
			        	&& GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[1] >= 2)
					||	
						(_characterOrder[0].getWeaponStrength() + 1 == _characterOrder[0].getResistance()
				    	&& GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[1] >= 5)
					||	
						(_characterOrder[0].getWeaponStrength() +2 >= _characterOrder[0].getResistance()
				    	&& GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[1] == 6))
				{
					Debug.Log ("G2");
					if(_enemyInRange.Count > _enemyIndex 
					   && GameManager._canvas.GetComponent<Sidebar>().getResults(ID)[2] < _enemyInRange[_enemyIndex].getArmor())
					{
						Debug.Log ("Damage");
						_enemyInRange[_enemyIndex].getDamage(_characterOrder[0].getWeaponStrength());
						if(ID==1)
						{
							GameManager._player2._characterList.Remove(_enemyInRange[_enemyIndex]);
							GameManager._player2._characterOrder.Remove(_enemyInRange[_enemyIndex]);
							GameManager._player2._lastRemovedCharacters.Remove(_enemyInRange[_enemyIndex]);
						}
						else 
						{
							GameManager._player1._characterList.Remove(_enemyInRange[_enemyIndex]);
							GameManager._player1._characterOrder.Remove(_enemyInRange[_enemyIndex]);
							GameManager._player1._lastRemovedCharacters.Remove(_enemyInRange[_enemyIndex]);
						}
						_enemyInRange.RemoveAt(_enemyIndex);
					}
				}
			}
			_characterOrder.RemoveAt(0);
			GameManager._canvas.GetComponent<Sidebar>().resetResults(ID);
			
			calcEnemiesInRange();
			_enemyIndex = 0;
			_actionCircleRect = new Rect(0,0,0,0);

			if(_characterOrder.Count == 0)
			{
				if(ID == 1)
				{
					_enemyInRange.Clear();
					_enemyIndex = 0;
					GameManager._player1._enemyInRange.Clear();
					GameManager._currentGameMode = new FightRoutine(GameManager._player2);
					GameManager._canvas.GetComponent<Sidebar>().resetResults(1);
					GameManager._player2.resetCharacterOrder();
					GameManager._player2.calcEnemiesInRange();
					GameManager._canvas.GetComponent<Sidebar>().enableSidebar(2);
				}
				else
				{
					GameManager._currentGameMode = new MoveRoutine(GameManager._player1);
					GameManager._canvas.GetComponent<Sidebar>().resetResults(2);
					GameManager._player1.resetCharacterOrder();
					GameManager._player2._enemyInRange.Clear();
					GameManager._canvas.GetComponent<Sidebar>().enableSidebar(1);
				}
			}
		}
	}
	public void calcEnemiesInRange()
	{
		//Debug.Log (_characterOrder.Count);
		if(_characterOrder.Count == 0) return;

		//resetCharacterOrder();

		//_enemyIndex = 0;

		Player enemy;
		if(ID==1) enemy = GameManager._player2;
		else enemy = GameManager._player1;

		enemy.resetCharacterOrder(); // put all characters in one list
		_enemyInRange.Clear();

		foreach(Character c in enemy._characterOrder)
		{
			if(Mathf.Pow(c.getPositionVector().x - _characterOrder[0].getPositionVector().x, 2) 
			   + Mathf.Pow(c.getPositionVector().y - _characterOrder[0].getPositionVector().y, 2) 
			   < Mathf.Pow(_characterOrder[0].getWeaponRadius(), 2))
			{
				_enemyInRange.Add(c);
			}
		}

		if(_enemyInRange.Count == 0)
		{
			_characterOrder.RemoveAt(0);
			if(_characterOrder.Count == 0)
			{
				if(ID == 1)
				{
					GameManager._currentGameMode = new FightRoutine(GameManager._player2);
					GameManager._canvas.GetComponent<Sidebar>().resetResults(1);
					GameManager._player2.resetCharacterOrder();
					GameManager._player2.calcEnemiesInRange();
					GameManager._canvas.GetComponent<Sidebar>().enableSidebar(2);
				}
				else
				{
					GameManager._currentGameMode = new MoveRoutine(GameManager._player1);
					GameManager._canvas.GetComponent<Sidebar>().resetResults(2);
					GameManager._player1.resetCharacterOrder();
					GameManager._canvas.GetComponent<Sidebar>().enableSidebar(1);
				}
				return;
			}
			calcEnemiesInRange();
		}
	}
	public void nextEnemy()
	{
		if(_enemyInRange.Count == 0) return;
		_enemyIndex++;
		_enemyIndex%=_enemyInRange.Count;
	}
	public void prevEnemy()
	{
		if(_enemyInRange.Count == 0) return;
		_enemyIndex--;
		_enemyIndex%=_enemyInRange.Count;
		if(_enemyIndex < 0) _enemyIndex+=_enemyInRange.Count;
	}
	public Character getSelectedEnemy()
	{
		
		//Debug.Log ("GetSelected: " + _enemyIndex + " - " + _enemyInRange.Count);
		if(_enemyInRange.Count == 0 || _enemyIndex >= _enemyInRange.Count || _enemyIndex < 0) return new Character(0, 0, 0, 0, 0, new Rect(0, 0, 0, 0), this);
		return _enemyInRange[_enemyIndex];
	}
}
