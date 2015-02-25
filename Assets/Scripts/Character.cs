using UnityEngine;
using System.Collections;

public class Character
{
	private int _maxHealth,
		_currentHealth,
		_ballistik,
		_resistance,
		_armor;
	private Rect _position;
	//private static GUI _gui;
	private bool _visibility = true;
	private Player _player;
	private static Weapon[] _weapon = new Weapon[2];
	private Weapon _currentWeapon;
	public static int _counter = 0;
	private int _privateCounter;

	public Character(int health, int weaponType, int ballistik, int resistance, int armor, Rect position, Player player)
	{
		_maxHealth = health;
		_currentHealth = health;
		_ballistik = ballistik;
		_resistance = resistance;
		_armor = armor;
		_position = position;
		_player = player;

		_privateCounter = _counter;
		_counter++;

		switch(weaponType)
		{
		case 0:
			_currentWeapon = new Weapon(4, 2);
			break;
		case 1:
			_currentWeapon = new Weapon(8, 4);
			break;
		}
	}
	
	public void setGUI(GUI gui)
	{
		//_gui = gui;
	}
	public bool isDead()
	{
		return _currentHealth == 0;
	}
	public void getDamage(int amountOfDamage)
	{
		_currentHealth -= amountOfDamage;
		if(_currentHealth<0)
		{
			_visibility = false;
		}
	}
	public void draw(Texture healthBarElement, Texture healthBarBorder)
	{
		if(!_visibility)return; // if NOT visible

		Vector2 pivotPoint = new Vector2(_position.x, _position.y - 100);
		int degree = (int)(360f*(_currentHealth/(float)_maxHealth));
		if(_player.ID == 1)
		{
			GUI.color = Color.cyan;
			GUI.DrawTexture(new Rect(_position.x - 50, _position.y - 150, 100, 100), healthBarBorder);
			if(GameManager._player2.getSelectedEnemy() == this)
			{
				GUI.color = Color.blue;
			}
			else
			{
				GUI.color = new Color(((1f-_currentHealth/(float)_maxHealth)*509)/255, ((_currentHealth/(float)_maxHealth)*509)/255, 0);
			}
		}
		else if(_player.ID == 2)
		{
			GUI.color = Color.magenta;
			GUI.DrawTexture(new Rect(_position.x - 50, _position.y - 150, 100, 100), healthBarBorder);
			if(GameManager._player1.getSelectedEnemy() == this)
			{
				GUI.color = Color.blue;
			}
			else
			{
				GUI.color = new Color(((1f-_currentHealth/(float)_maxHealth)*509)/255, ((_currentHealth/(float)_maxHealth)*509)/255, 0);
			}
		}
		   
		for(int i = 0; i <= degree; i++)
		{
			GUIUtility.RotateAroundPivot(1, pivotPoint);
			GUI.DrawTexture(new Rect(_position.x - 50, _position.y - 150, 100, 100), healthBarElement);//, ScaleMode.ScaleToFit, true, 0);
		}
		GUI.color = Color.white;
		GUIUtility.RotateAroundPivot(0-(degree+1), pivotPoint);

		GUI.Label(new Rect(_position.x, _position.y - 100, 100, 100), ""+_currentWeapon.getRange());
	}
	public void changeVisibility()
	{
		_visibility = false == _visibility; // true -> false, false -> true
	}
	public Player getPlayer()
	{
		return _player;
	}
	public Vector2 getPositionVector()
	{
		return new Vector2(_position.x, _position.y);
	}
	public void setPosition(Rect newPosition)
	{
		_position = newPosition;
	}
	public int getWeaponRadius()
	{
		return 100*_currentWeapon.getRange();
	}
	public int getWeaponStrength()
	{
		return _currentWeapon.getStrength();
	}
	public int getBallistic()
	{
		return _ballistik;
	}
	public int getResistance()
	{
		return _resistance;
	}
	public int getArmor()
	{
		return _armor;
	}
}

public class Weapon{

	private int _range,
		_strength;
	
	public Weapon (int range, int strength) {
		_range = range;
		_strength = strength;
	}
	
	public int getRange() 
	{
		return _range;
	}
	public int getStrength()
	{
		return _strength;
	}
}