using UnityEngine;
using System.Collections;

public class MoveRoutine : GameMode 
{
	Player _player;
	private string _name = "Einheiten bewegen";

	public MoveRoutine(Player player)
	{
        _player = player;
	}
	public string getName()
	{
		return _name;
  	}
	public void OnGUI () 
	{
	    
	}
	public void Update () 
	{
        _player.moveCharacter();
	}
}
