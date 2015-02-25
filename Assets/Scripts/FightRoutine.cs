using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightRoutine : GameMode 
{
	private Player _player;
	private string _name = "Im Kampf";

	public FightRoutine(Player player)
	{
		_player = player;
	}
	public string getName()
	{
		return _name;
  	}
	public void OnGUI()
	{

	}
	public void Update()
	{
		_player.fightCharacter();
	}
}
