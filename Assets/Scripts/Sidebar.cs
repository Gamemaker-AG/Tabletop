using UnityEngine;
using System.Collections;

public class Sidebar : MonoBehaviour
{
	int _Animation = 0,
		// 0 = no animation
		// 1 = plane is moving up
		// 2 = no animation
		// 3 = plane is moving down
		_dice_numberToFind = -2; // -1 -> Do dice animation, everything else: No animation, number has been found
	float _elapsed_time, 
		_elapsed_time_dice,
		_pixelPerSecond,
		_dice_counter, 
		_dice_speed = 150, 
		_dice_yPos;
	public Texture _dice_backgroundTexture,
		_dice_Texture,
		_dice_BorderTexture,
		_sidebar_ArrowUp,
		_dice_reload;
	public bool _isLeftSidebar; // TODO: Für die andere Seite machen

	// Use this for initialization
	void Start () 
	{
		_dice_yPos = Screen.height / 2 + 10;
		resetDice ();
		
		_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
		_elapsed_time = Time.realtimeSinceStartup;
		Vector3 canvasPos = transform.position;
		
		print (transform.position.x);
		float relativePixel_X = 1/(transform.position.x*2);
	}
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, Screen.height - 130, 96, 130), _sidebar_ArrowUp);
		GUI.DrawTexture(new Rect(0, _dice_yPos + Screen.height/2, 120, Screen.height), _dice_backgroundTexture);
		GUI.DrawTextureWithTexCoords(new Rect(15, (int)_dice_yPos + 150 + Screen.height/2, 80, 80), _dice_Texture, new Rect(0, (int)_dice_counter * ((85f+1/_dice_counter) / (float)_dice_Texture.height), 1, 85 / (float)_dice_Texture.height));
		GUI.DrawTexture(new Rect(14, (int)_dice_yPos + 148 + Screen.height/2, 84, 84), _dice_BorderTexture);

		GUI.DrawTexture(new Rect(15, (int)_dice_yPos + 250 + Screen.height/2, 80, 80), _dice_reload);
		GUI.DrawTexture(new Rect(14, (int)_dice_yPos + 148 + Screen.height/2, 84, 84), _dice_BorderTexture);


		if(_dice_numberToFind == -1)
		{
			_dice_counter += (Time.realtimeSinceStartup - _elapsed_time)*(_dice_speed);
			_dice_counter %=6;
			if(_dice_speed <= 1)
			{
				_dice_numberToFind = 6-(int)_dice_counter;
				print("Nummer ist: " + _dice_numberToFind);
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < Tuio.Input.touchCount; i++)
		{
			Vector2 pos = Tuio.Input.GetTouch(i).position;
			
			if(pos.x < 150 && pos.y < 150 // op/down arrow
			   && _Animation % 2 != 1) // AND there's no animation
			{
				print ("sldkh" + (Screen.height - ((int)_dice_yPos + 250 + Screen.height/2)));
				_pixelPerSecond = Screen.height/1080f * 2500f; // same speed for different screen height
				_elapsed_time = Time.realtimeSinceStartup;
				_Animation++;
			}
			else if(pos.x >= 15 && pos.x <= 95
			   && pos.y >= Screen.height - ((int)_dice_yPos + 330 + Screen.height/2) && pos.y <= Screen.height - ((int)_dice_yPos + 250 + Screen.height/2)
			   && _dice_numberToFind != -1)
			{
				startDice();
      		}
    	}

		doAnimation();
		_dice_speed *= 1f-((Time.realtimeSinceStartup - _elapsed_time));
		_elapsed_time = Time.realtimeSinceStartup;
  	}
	private void doAnimation()
	{
		if(_Animation % 2 != 0) // only 1 and 3 are movement, 0 and 2 are static states
		{

			float offset = CalcOffset();

			if (_Animation == 1) { // DOWN
				if (_dice_yPos + offset >= transform.position.y)
				{
					_Animation++;
					offset = 0;
					_dice_yPos = transform.position.y;
				}
				else
				{
					_dice_yPos += offset;
				}
			} 
			else if (_Animation == 3) // UP
			{				
				if (_dice_yPos - offset <= 0 - transform.position.y)
				{
					_Animation=0;
					offset = 0;
					_dice_yPos = 0 - transform.position.y;
				}
				else
				{
					_dice_yPos -= offset;
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
	private void startDice()
	{
		_dice_numberToFind = -1;
		resetDice ();
	}
	private void resetDice()
	{
		_dice_speed = 250;
		_dice_counter = (float)((new System.Random()).NextDouble())*6f;
	}
}
