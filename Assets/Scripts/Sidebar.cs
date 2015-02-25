using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sidebar : MonoBehaviour
{
	int _1_Animation = 0,
		// 0 = no animation
		// 1 = plane is moving up
		// 2 = no animation
		// 3 = plane is moving down
		_1_dice_numberToFind = -2, // -1 -> Do dice animation, everything else: No animation, number has been found
		_1_sibebar_yOffset = -70,
		
		_2_Animation = 0,
		// 0 = no animation
		// 1 = plane is moving up
		// 2 = no animation
		// 3 = plane is moving down
		_2_dice_numberToFind = -2, // -1 -> Do dice animation, everything else: No animation, number has been found
		_2_sibebar_yOffset = -70;
  	float _elapsed_time, 
		_1_elapsed_time_dice,
		_pixelPerSecond,
		_1_dice_counter, 
		_dice_speed = 10, 
		_1_dice_yPos,

		_2_elapsed_time_dice,
		_2_dice_counter, 
		_2_dice_yPos;
  	public Texture _dice_backgroundTexture,
		_dice_Texture,
		_dice_BorderTexture,
		_sidebar_ArrowUp,
		_dice_reload,
		_nextCharacterButtons;
	private bool _1_diceEnabled, 
		_2_diceEnabled,
		_touchOnArrows;
	private List<int> _1_results = new List<int>();
	private List<float> _1_diceResults = new List<float>();
	private List<int> _2_results = new List<int>();
	private List<float> _2_diceResults = new List<float>();
  
	// Use this for initialization
	void Start () 
	{
		_1_dice_yPos = Screen.height / 2 + 10;
		_2_dice_yPos = Screen.height / 2 + 10;
		resetDice ();
		
		_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
		_elapsed_time = Time.realtimeSinceStartup;
		Vector3 canvasPos = transform.position;
		
		print (transform.position.x);
		float relativePixel_X = 1/(transform.position.x*2);
	}
	void OnGUI()
	{
		OnGUI1 ();
		OnGUI2 ();
	}
	private void OnGUI1()
	{
		GUI.DrawTexture(new Rect(0, Screen.height - 130, 96, 130), _sidebar_ArrowUp);
		GUI.DrawTexture(new Rect(0, _1_dice_yPos + Screen.height/2, 120, Screen.height), _dice_backgroundTexture);
		GUI.DrawTextureWithTexCoords(new Rect(15, (int)_1_dice_yPos + _1_sibebar_yOffset + 80 + Screen.height/2, 80, 80), _dice_Texture, new Rect(0, (int)_1_dice_counter * ((85f+1/_1_dice_counter) / (float)_dice_Texture.height), 1, 85 / (float)_dice_Texture.height));
		GUI.DrawTexture(new Rect(14, (int)_1_dice_yPos + _1_sibebar_yOffset + 78 + Screen.height/2, 84, 84), _dice_BorderTexture);
		
		GUI.DrawTexture(new Rect(15, (int)_1_dice_yPos + _1_sibebar_yOffset + 180 + Screen.height/2, 80, 80), _dice_reload);
		
		for(int i = 0; i < 3 && i < _1_results.Count; i++)
		{
			GUI.DrawTextureWithTexCoords(new Rect(15, (int)_1_dice_yPos + _1_sibebar_yOffset + 280 + Screen.height/2 + i * 100, 80, 80),
			                             _dice_Texture,
			                             new Rect(0, (int)(_1_diceResults[i]) * ((85f+1/(_1_diceResults[i])) / (float)_dice_Texture.height), 1, 85 / (float)_dice_Texture.height));
		}
		
		GUI.DrawTexture(new Rect(14, (int)_1_dice_yPos + _1_sibebar_yOffset + 278 + Screen.height/2, 84, 84), _dice_BorderTexture);
		GUI.DrawTexture(new Rect(14, (int)_1_dice_yPos + _1_sibebar_yOffset + 378 + Screen.height/2, 84, 84), _dice_BorderTexture);
		GUI.DrawTexture(new Rect(14, (int)_1_dice_yPos + _1_sibebar_yOffset + 478 + Screen.height/2, 84, 84), _dice_BorderTexture);

		GUI.DrawTexture(new Rect(15, (int)_1_dice_yPos + _1_sibebar_yOffset + 570 + Screen.height/2, 80, 280), _nextCharacterButtons);
		
		if(_1_dice_numberToFind == -1)
		{
			_1_dice_counter += (Time.realtimeSinceStartup - _elapsed_time)*(_dice_speed);
			_1_dice_counter %=6;
			if(_dice_speed <= 1)
			{
				_1_dice_numberToFind = 6-(int)_1_dice_counter;
				_1_results.Add(_1_dice_numberToFind);
				_1_diceResults.Add(_1_dice_counter);
				//print("Nummer ist: " + _1_dice_numberToFind);
	      	}
	    }
	}
	private void OnGUI2()
	{
		Vector2 pivotPoint = new Vector2(Screen.width/2, Screen.height/2);
		GUIUtility.RotateAroundPivot(180, pivotPoint);

		GUI.DrawTexture(new Rect(0, Screen.height - 130, 96, 130), _sidebar_ArrowUp);
		GUI.DrawTexture(new Rect(0, _2_dice_yPos + Screen.height/2, 120, Screen.height), _dice_backgroundTexture);
		GUI.DrawTextureWithTexCoords(new Rect(15, (int)_2_dice_yPos + _2_sibebar_yOffset + 80 + Screen.height/2, 80, 80), _dice_Texture, new Rect(0, (int)_2_dice_counter * ((85f+1/_2_dice_counter) / (float)_dice_Texture.height), 1, 85 / (float)_dice_Texture.height));
		GUI.DrawTexture(new Rect(14, (int)_2_dice_yPos + _2_sibebar_yOffset + 78 + Screen.height/2, 84, 84), _dice_BorderTexture);
		
		GUI.DrawTexture(new Rect(15, (int)_2_dice_yPos + _2_sibebar_yOffset + 180 + Screen.height/2, 80, 80), _dice_reload);
		
		for(int i = 0; i < 3 && i < _2_results.Count; i++)
		{
			GUI.DrawTextureWithTexCoords(new Rect(15, (int)_2_dice_yPos + _2_sibebar_yOffset + 280 + Screen.height/2 + i * 100, 80, 80),
			                             _dice_Texture,
			                             new Rect(0, (int)(_2_diceResults[i]) * ((85f+1/(_2_diceResults[i])) / (float)_dice_Texture.height), 1, 85 / (float)_dice_Texture.height));
		}
		
		GUI.DrawTexture(new Rect(14, (int)_2_dice_yPos + _2_sibebar_yOffset + 278 + Screen.height/2, 84, 84), _dice_BorderTexture);
		GUI.DrawTexture(new Rect(14, (int)_2_dice_yPos + _2_sibebar_yOffset + 378 + Screen.height/2, 84, 84), _dice_BorderTexture);
		GUI.DrawTexture(new Rect(14, (int)_2_dice_yPos + _2_sibebar_yOffset + 478 + Screen.height/2, 84, 84), _dice_BorderTexture);
			
		GUI.DrawTexture(new Rect(15, (int)_2_dice_yPos + _2_sibebar_yOffset + 570 + Screen.height/2, 80, 280), _nextCharacterButtons);
		
		if(_2_dice_numberToFind == -1)
		{
			_2_dice_counter += (Time.realtimeSinceStartup - _elapsed_time)*(_dice_speed);
			_2_dice_counter %=6;
			if(_dice_speed <= 1)
			{
				_2_dice_numberToFind = 6-(int)_2_dice_counter;
				_2_results.Add(_2_dice_numberToFind);
				_2_diceResults.Add(_2_dice_counter);
				//print("Nummer ist: " + _2_dice_numberToFind);
      		}
   		}
		GUIUtility.RotateAroundPivot(-180, pivotPoint);
  	}
  // Update is called once per frame
	void Update () 
	{
		Update1();
		Update2();
		_dice_speed *= 1f-((Time.realtimeSinceStartup - _elapsed_time)+0.1f);
		_elapsed_time = Time.realtimeSinceStartup;
  	}
	void Update1 () 
	{
		if(_1_diceEnabled)
		{
			for(int i = 0; i < Tuio.Input.touchCount; i++)
			{
				Vector2 pos = Tuio.Input.GetTouch(i).position;
				
				if(pos.x < 150 && pos.y < 150 // op/down arrow
				   && _1_Animation % 2 != 1) // AND there's no animation
				{
					//print ("sldkh" + (Screen.height - ((int)_1_dice_yPos + 250 + Screen.height/2)));
					_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
					_elapsed_time = Time.realtimeSinceStartup;
					_1_Animation++;
				}
				else if(pos.x >= 15 && pos.x <= 95
				        && pos.y >= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 260 + Screen.height/2) && pos.y <= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 180 + Screen.height/2)
				        && _1_dice_numberToFind != -1)
				{
					startDice(1);
				}
				if(pos.x >= 15 && pos.x <= 95
				        && pos.y >= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 690 + Screen.height/2)
				        && pos.y <= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 570 + Screen.height/2)
				        && _1_results.Count == 0)
				{
					if(!_touchOnArrows) GameManager._player1.nextEnemy();
					_touchOnArrows = true;
				}
				else if(pos.x >= 15 && pos.x <= 95
				        && pos.y >= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 850 + Screen.height/2)
				        && pos.y <= Screen.height - ((int)_1_dice_yPos + _1_sibebar_yOffset + 730 + Screen.height/2)
				        && _1_results.Count == 0)
				{
					if(!_touchOnArrows) GameManager._player1.prevEnemy();
					_touchOnArrows = true;
				}
				else
				{
					_touchOnArrows = false;
				}
			}
			if(Tuio.Input.touchCount == 0) _touchOnArrows = false;
		}
		doAnimation1();
	}
	void Update2 () 
	{
		if(_2_diceEnabled)
		{
			for(int i = 0; i < Tuio.Input.touchCount; i++)
			{
				Vector2 pos = Tuio.Input.GetTouch(i).position;

				if(pos.x > Screen.width - 150 && pos.y > Screen.height - 150 // op/down arrow
				   && _2_Animation % 2 != 1) // AND there's no animation
				{
					//print ("sldkh" + (Screen.height - ((int)_2_dice_yPos + 250 + Screen.height/2)));
					_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
					_elapsed_time = Time.realtimeSinceStartup;
					_2_Animation++;
				}
				else if(pos.x >= Screen.width - 95 
				        && pos.x <= Screen.width - 15
				        && pos.y >= -_2_sibebar_yOffset + 40
				        && pos.y <= -_2_sibebar_yOffset + 140
				        && _2_dice_numberToFind != -1)
				{
					startDice(2);
				}
				if(pos.x >= Screen.width - 95
				        && pos.x <= Screen.width - 15
						&& pos.y >= Screen.height - ((int)_2_dice_yPos - _2_sibebar_yOffset + 690)
				        && pos.y <= Screen.height - ((int)_2_dice_yPos - _2_sibebar_yOffset + 570)
				   		&& _2_results.Count == 0)
				{
					if(!_touchOnArrows) GameManager._player2.nextEnemy();
					_touchOnArrows = true;
				}
				else if(pos.x >= Screen.width - 95
				        && pos.x <= Screen.width - 15
				        && pos.y >= Screen.height - ((int)_2_dice_yPos - _2_sibebar_yOffset + 850)
				        && pos.y <= Screen.height - ((int)_2_dice_yPos - _2_sibebar_yOffset + 730)
				        && _2_results.Count == 0)
				{
					if(!_touchOnArrows) GameManager._player2.prevEnemy();
					_touchOnArrows = true;
				}
				else
				{
					_touchOnArrows = false;
				}
				//Debug.Log(pos.y + "\n" + 
				//          (pos.y >= Screen.height - ((int)_2_dice_yPos + _2_sibebar_yOffset + 850)) + "\n" +
				//          (pos.y <= Screen.height - ((int)_2_dice_yPos + _2_sibebar_yOffset + 570)));
			}
			if(Tuio.Input.touchCount == 0) _touchOnArrows = false;
		}
		
		doAnimation2();
  	}
	private void doAnimation1()
	{
		if(_1_Animation % 2 != 0) // only 1 and 3 are movement, 0 and 2 are static states
		{

			float offset = CalcOffset();

			if (_1_Animation == 1) { // DOWN
				if (_1_dice_yPos + offset >= transform.position.y)
				{
					_1_Animation++;
					offset = 0;
					_1_dice_yPos = transform.position.y;
				}
				else
				{
					_1_dice_yPos += offset;
				}
			} 
			else if (_1_Animation == 3) // UP
			{				
				if (_1_dice_yPos - offset <= 0 - transform.position.y)
				{
					_1_Animation=0;
					offset = 0;
					_1_dice_yPos = 0 - transform.position.y;
				}
				else
				{
					_1_dice_yPos -= offset;
				}
			}
		}
	}
	private void doAnimation2()
	{
		if(_2_Animation % 2 != 0) // only 1 and 3 are movement, 0 and 2 are static states
		{
			float offset = CalcOffset();
			
			if (_2_Animation == 1) { // DOWN
				if (_2_dice_yPos + offset >= transform.position.y)
				{
					_2_Animation++;
					offset = 0;
					_2_dice_yPos = transform.position.y;
				}
				else
				{
					_2_dice_yPos += offset;
				}
			} 
			else if (_2_Animation == 3) // UP
			{				
				if (_2_dice_yPos - offset <= 0 - transform.position.y)
				{
					_2_Animation=0;
					offset = 0;
					_2_dice_yPos = 0 - transform.position.y;
		        }
		        else
		        {
					_2_dice_yPos -= offset;
		    	}
			}
		}
	}
	private float CalcOffset()
	{
		float time_diff = Time.realtimeSinceStartup - _elapsed_time;
		float offset = time_diff * _pixelPerSecond;
		//print (time_diff.ToString () + " -- " + offset.ToString ());
		return offset;
	}
	private void startDice(int diceIndex)
	{
		if(diceIndex == 1)_1_dice_numberToFind = -1;
		else _2_dice_numberToFind = -1;
		resetDice ();
	}
	private void resetDice()
	{
		_dice_speed = 250;
		_1_dice_counter = (float)((new System.Random()).NextDouble())*6f;
		_2_dice_counter = (float)((new System.Random()).NextDouble())*6f;
	}
	public List<int> getResults(int playerIndex)
	{
		if(playerIndex == 1) return _1_results;
		else return _2_results;
	}
	public int getDiceStatus(int playerIndex)
	{
		if(playerIndex == 1) return _1_dice_numberToFind;
		else return _2_dice_numberToFind;
	}
    public void resetResults(int diceIndex)
	{
		if(diceIndex == 1)
		{
			_1_results.Clear();
			_1_diceResults.Clear();
    	}
		else 
		{
			_2_results.Clear();
			_2_diceResults.Clear();
    	}
    }
	public void enableSidebar(int number)
	{
		if(number == 1)
		{
			if(_1_Animation % 2 == 0) // retract/extend sidebar
			{
				_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
				_elapsed_time = Time.realtimeSinceStartup;
				_1_Animation = 3;
			}
			if(_2_Animation == 0)
			{
				_2_Animation = 1;
			}
			_1_diceEnabled = true;
			_2_diceEnabled = false;
		}
		else if(number == 2)
		{ 
			if(_2_Animation % 2 == 0) // retract/extend sidebar
			{
				_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
				_elapsed_time = Time.realtimeSinceStartup;
				_2_Animation = 3;
			}
			if(_1_Animation == 0)
			{
				_1_Animation = 1;
			}
			_2_diceEnabled = true;
			_1_diceEnabled = false;
		}
	}
}
