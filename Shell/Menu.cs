using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace FairyO
{
    public class Menu : MonoBehaviour
    {
		// ---------- constants -------------
        private const int PLAY_GAME = 0;
        private const int PLAY_AR = 1;

		// ---------- inspector -------------
		public Image logo;
		public List<Button> buttons = new List<Button>();

		public void Awake()
		{
			HideMenu();
		}

		public void Start()
		{
			FadeInLogo(ShowButtons);
			FairyO.Music.state = FairyO.Music.State.Menu;
			FairyO.Shell.Shell.state = FairyO.Shell.Shell.State.Menu;
			Dais.state = Dais.State.Menu;
			FairyO.Shell.LoadingScreen.Hide();
		}

		public void Select(int itemNum)
        {
			Action callback;
            switch (itemNum)
            {
                case PLAY_GAME:
					callback = LoadGame;
                    break;
                case PLAY_AR:
					callback = LoadAR;
                    break;
				default:
					callback = LoadGame;
					break;
            }

			print("Menu.Select "+callback);

			HideButtons();
			FairyO.Shell.LoadingScreen.Show(callback);
        }

		private void HideMenu()
		{
			Color logoColor = logo.color;
			logoColor.a = 0;
			logo.color = logoColor;

			foreach(var button in buttons)
			{
				button.gameObject.SetActive(false);
			}
		}

		private void FadeInLogo(Action next)
		{
			Color logoColor = logo.color;
			logoColor.a = 1;
			logo.DOColor(logoColor, 2.0f).OnComplete(()=>next());
		}

		private void ShowButtons()
		{
			foreach(var button in buttons)
			{
				button.gameObject.SetActive(true);
			}
		}

		private void HideButtons()
		{
			foreach(var button in buttons)
			{
				button.gameObject.SetActive(false);
			}
		}

		private void LoadGame()
		{
			SceneManager.LoadScene("scn_Game");
		}

		private void LoadAR()
		{
			SceneManager.LoadScene("scn_FairyRing_AR");
		}
    }
}