using UnityEngine;

namespace FairyO
{
    public class Dais : MonoBehaviour
    {
		// 0000000 Singleton 00000000
		public static Dais singleton;
		void Awake()
        {
            // setup singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);

			GameObject.DontDestroyOnLoad(this);
        }

		// ------- Enums ------------
		public enum State { Menu, Idle, Map }

		// ------- inspector ---------
		public GameObject background_menu;
		public GameObject background_game;
		public GameObject starLight;
		public GameObject sunBeams;
		public GameObject fog;
		public GameObject map;

		// ------- fields ------------
		[SerializeField] private static State _state;

		// ------- properties --------
		internal static State state{
			get {
				return _state;
			}
			set{
				switch(value)
				{
					case State.Menu :
						singleton.ModeMenu();
						break;
					case State.Idle :
						singleton.ModeIdle();
						break;
					case State.Map :
						singleton.ModeMap();
						break;
				}
				_state = value;
			}
		}

		private void ModeMenu()
		{
			background_menu.SetActive(true);
			background_game.SetActive(false);
			starLight.SetActive(false);
			sunBeams.SetActive(false);
			fog.SetActive(true);

			map.SetActive(false);
		}

		private void ModeIdle()
		{
			background_menu.SetActive(false);
			background_game.SetActive(false);
			starLight.SetActive(false);
			sunBeams.SetActive(false);
			fog.SetActive(false);

			map.SetActive(false);
		}

		private void ModeMap()
		{
			background_menu.SetActive(false);
			background_game.SetActive(true);
			starLight.SetActive(true);
			sunBeams.SetActive(true);
			fog.SetActive(false);

			map.SetActive(true);
		}


    }
}