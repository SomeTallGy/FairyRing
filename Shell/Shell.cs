using UnityEngine;
using UnityEngine.SceneManagement;

namespace FairyO.Shell
{
    public class Shell : MonoBehaviour
    {
		// 0000000 Singleton 00000000
		private static Shell singleton;
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
		public enum State { Menu, Other }

		// ------- inspector ---------
        public GameObject button_backToMainMenu;

        // ------- fields ------------
		[SerializeField] private static State _state;

        // ------- properties --------
		public static State state{
			get {
				return _state;
			}
			set{
				switch(value)
				{
					case State.Menu :
						singleton.ModeMenu();
						break;
					case State.Other:
						singleton.ModeOther();
						break;
				}
				_state = value;
			}
		}

        public void ReturnToMenu()
        {
            FairyO.Music.state = FairyO.Music.State.Idle;
            FairyO.Shell.LoadingScreen.Show(LoadMenu);
            FairyO.Shell.Shell.state = FairyO.Shell.Shell.State.Menu;
        }

        private void LoadMenu()
        {
            SceneManager.LoadScene("scn_Menu");
        }

        private void ModeMenu()
        {
            button_backToMainMenu.SetActive(false);
        }

        private void ModeOther()
        {
            button_backToMainMenu.SetActive(true);
        }
		
    }
}