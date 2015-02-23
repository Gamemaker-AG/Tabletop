using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour 
{
	public GameObject _panel;
	public static System.Collections.Generic.List<TouchField> _touchList = new System.Collections.Generic.List<TouchField>();
	public static System.Collections.Generic.List<TouchField> _bufferTouchList = new System.Collections.Generic.List<TouchField>(); // for touches 
	private System.Collections.Generic.List<TouchField> _lastRemovedCharacters = new System.Collections.Generic.List<TouchField>();
	private float _xRelative, _yRelative;
	public Texture tex;
	//private bool _enabled = true; // true: detects all the stuff; false: does nothing

	// Use this for initialization
	void Start () {
		_xRelative = 1 / (float)Screen.width;
		_yRelative = 1 / (float)Screen.height;
		_panel.transform.localScale = new Vector3 (100 * _xRelative, 100 * _yRelative);
	}

	// Update is called once per frame
	void Update () {
		//if(!_enabled)return;
		//Check if there's a field without a touch
		for(int k = 0; k < _touchList.Count; k++)
		{
			bool _fieldHasTouch = false;

			for (int i = 0; !_fieldHasTouch && i < Tuio.Input.touchCount; i++) 
			{
				_fieldHasTouch = _fieldHasTouch | _touchList[k].TouchInField(Tuio.Input.GetTouch(i).position);
			}
			if(!_fieldHasTouch) // Field without any touch
			{
				if(_touchList[k].isCharacter())
				{
					// put character in temporary list
					_lastRemovedCharacters.Add (_touchList[k]);
					_touchList[k].getPlayer().removeCharacter(_touchList[k].getCharacter()); 
				}
				_touchList[k].removePanel();
				_touchList.RemoveAt(k);
			}
		}

		//Check if theres a touch without a Field
		for (int i = 0; i < Tuio.Input.touchCount; i++) 
		{
			bool _touchHasField = false;
			for(int k = 0; !_touchHasField && k < _touchList.Count; k++)
			{
				_touchHasField = _touchHasField | _touchList[k].TouchInField(Tuio.Input.GetTouch(i).position);
			}
			for(int k = 0; !_touchHasField && k < _bufferTouchList.Count; k++)
			{
				_touchHasField = _touchHasField | _bufferTouchList[k].TouchInField(Tuio.Input.GetTouch(i).position);
			}
			if(!_touchHasField) // Touch without field -> create new field for this touch
			{
				GameObject o = Instantiate (_panel) as GameObject; // create a "copy" of the panel
				o.transform.SetParent(transform, false);
				TouchField f = new TouchField(Tuio.Input.GetTouch(i).position, o, Tuio.Input.GetTouch(i)); // create a touchField for this touch
				//print (o.transform.localPosition);
				_bufferTouchList.Add(f);
			}
		}

		
		foreach(TouchField tf in _bufferTouchList)
		{
			if(Time.realtimeSinceStartup - tf.getTime() >= 1)
			{
				Debug.Log ("Add to real list");
				tf.setPanelPosition();

				_touchList.Add(tf);
				foreach(TouchField c in _lastRemovedCharacters)
				{
					Debug.Log ("Re-Add player");
					c.getPlayer().addCharacter(c.getCharacter()); // re-add character ; responsibility if this re-add is correct lies by the player class
				}
				//_bufferTouchList.Remove(tf);
			}
		}
		// delete fields wich are older than 1 sec and therefore in the _touchList
		foreach(TouchField tl in _touchList)
		{
			if(_bufferTouchList.Contains(tl))
			{
				Debug.Log ("Remove ");
				_bufferTouchList.Remove(tl);
			}
		}
	}
}
public class TouchField
{
	private Vector2 _position;
	private Vector2 _size;
	private GameObject _panel;
	private Tuio.Touch _touch;
	private Character _character = null;
	private float _creationTime;

	public TouchField(Vector2 position, GameObject panel, Tuio.Touch touch)
	{
		_position = position;
		_size = new Vector2 (100, 100);
		_panel = panel;
		_touch = touch;
		//_panel.transform.position = new Vector3 (_position.x, _position.y, 0);
		_creationTime = Time.realtimeSinceStartup;
	}
	public TouchField(Vector2 position, Vector2 size, GameObject panel, Tuio.Touch touch)
	{
		_position = position;
		_size = size;
		_panel = panel;
		_touch = touch;
		_panel.transform.position = new Vector3 (_position.x, _position.y, 0);
		_creationTime = Time.realtimeSinceStartup;
	}
	public bool TouchInField(Vector2 touchPosition)
	{
		return (touchPosition.x >= _position.x - _size.x/2f && 
		        touchPosition.x <= _position.x + _size.x/2f &&
				touchPosition.y >= _position.y - _size.y/2f && 
		        touchPosition.y <= _position.y + _size.y/2f);
	}
	public void removePanel()
	{
		Object.Destroy(_panel);
	}
	public void setTouch(Tuio.Touch newTouch)
	{
		_touch = newTouch;
		_position = _touch.position;
		_panel.transform.position = new Vector3 (_position.x, _position.y, 0);
	}
	public void setCharacter(Character character)
	{
		_character = character;
	}
	public Character getCharacter()
	{
		return _character;
	}
	public bool isCharacter()
	{
		return _character != null;
	}
	public Vector2 getPosition()
	{
		return _position;
	}
	public Rect getFieldRect()
	{
		return new Rect(_position.x - _size.x/2, Screen.height - _position.y + _size.y/2, _size.x, _size.y);
	}
	public Player getPlayer()
	{
		return _character.getPlayer();
	}
	public float getTime()
	{
		return _creationTime;
	}
	public GameObject getPanel()
	{
		return _panel;
	}
	public void setPanelPosition()
	{
		_panel.transform.position = new Vector3 (_position.x, _position.y, 0);
	}
}