namespace Tuio
{
	using UnityEngine;
	using System.Collections;

	public class TuioInputUpdater : MonoBehaviour
	{
		private static TuioInputUpdater instance;

		void Awake()
		{
			if(instance)
            {
				Destroy(this);
                return;
            }

            instance = this;
				
			Tuio.Input.Init();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}

		void Update()
		{
			Tuio.Input.Update();
		}
		
		void OnApplicationQuit()
		{
			Tuio.Input.Stop();
			DestroyImmediate(gameObject);
		}
		
		public static void EnsureInstance()
		{
			if(instance) return;
			GameObject go = new GameObject("TuioInput Updater");
			instance = go.AddComponent<TuioInputUpdater>();
		}
	}
}