using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture _startButton_Texture;
	public GameObject _panel;

	// Use this for initialization
	void Start () {
		_panel.transform.position = new Vector3 (-100, 100, 0); //make the panel "invisible"
		try
		{
			Application.LoadLevelAdditive("scene");
		}
		catch(UnityException e)
		{
			Debug.Log(e.Message);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < Tuio.Input.touchCount; i++)
		{
			Vector2 pos = Tuio.Input.GetTouch(i).position;
			
			if(pos.x >= Screen.width/2-50 && pos.x <= Screen.width/2+50
			   && pos.y >= Screen.height/2-50 && pos.y <= Screen.height/2+50)
			{
				GetComponent<Sidebar>().enabled = true;
				GetComponent<Sidebar>().enableSidebar(1);
				//GetComponent<Sidebar>().setSidebar(true, 1);
				GetComponent<TouchManager>().enabled = true;
				//GetComponent<TouchManager>().enable(false); // is in canvas activated but does not do anything
				GetComponent<GameManager>().enabled = true;

				GetComponent<MainMenu>().enabled = false; // disable this one
			}
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width/2-50, Screen.height/2-50, 100, 100), _startButton_Texture);
	}
}
