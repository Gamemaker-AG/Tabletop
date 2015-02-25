using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightRoutine : GameMode 
{
	private Player _player;

	public FightRoutine(Player player)
	{
		_player = player;
	}

	public void OnGUI()
	{

	}
	public void Update()
	{
		_player.fightCharacter();
	}
}
