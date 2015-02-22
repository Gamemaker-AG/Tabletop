using UnityEngine;
using System.Collections;

public class MoveRoutine : GameMode 
{
	Player _player;

	public MoveRoutine(Player player)
	{
        _player = player;
	}
	public void OnGUI () 
	{
	    
	}
	public void Update () 
	{
        _player.moveCharacter();
	}
}
