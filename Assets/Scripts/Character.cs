using UnityEngine;
using System.Collections;

public class Character
{
	private int _maxHealth,
		_currentHealth,
		_weaponType,
		_accuracy, // balistik
		_resistance,
		_armor;
	private Rect _position;
	//private static GUI _gui;
	private bool _visibility = true;
	private Player _player;

	public Character(int health, int weaponType, int accuracy, int resistance, int armor, Rect position, Player player)
	{
		_maxHealth = health;
		_currentHealth = health;
		_weaponType = weaponType;
		_accuracy = accuracy;
		_resistance = resistance;
		_armor = armor;
		_position = position;
		_player = player;
	}
	public void setGUI(GUI gui)
	{
		//_gui = gui;
	}

	public void getDamage(int amountOfDamage)
	{
		_currentHealth -= amountOfDamage;
		if(_currentHealth<0)_currentHealth=0;
	}
	public bool isDead()
	{
		return _currentHealth <= 0;
	}
	public void draw(Texture healthBarElement)
	{
		if(!_visibility)return; // if NOT visible

		Vector2 pivotPoint = new Vector2(_position.x, _position.y - 100);
		int degree = (int)(360f*(_currentHealth/(float)_maxHealth));
		GUI.color = new Color(((1f-_currentHealth/(float)_maxHealth)*509)/255, ((_currentHealth/(float)_maxHealth)*509)/255, 0);
		for(int i = 0; i <= degree; i++)
		{
			GUIUtility.RotateAroundPivot(1, pivotPoint);
			GUI.DrawTexture(new Rect(_position.x - 50, _position.y - 150, 100, 100), healthBarElement);//, ScaleMode.ScaleToFit, true, 0);
		}
		GUI.color = Color.white;
		GUIUtility.RotateAroundPivot(0-(degree+1), pivotPoint);
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
}