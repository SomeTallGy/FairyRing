using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace FairyO.Shell
{
    public class LoadingScreen : MonoBehaviour
    {
		// 0000000 Singleton 00000000
		private static LoadingScreen singleton;
		void Awake()
        {
            // setup singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);
        }

		// ------- inspector --------
		public Image imageBackground;
		public Image imageLogo;
		public GameObject loadingScreenGameObject;

		// ------- fields -----------

		private static Color clear = new Color(1,1,1,0);

		void Start()
		{
			Init();
		}

		private void Init()
		{
			imageBackground.color = clear;
			imageLogo.color = clear;
		}
		public static void Show(Action callback = null)
		{
			if(!singleton.loadingScreenGameObject.activeInHierarchy)
				singleton.loadingScreenGameObject.SetActive(true);

			singleton.imageBackground.DOColor(Color.white, 1.0f)
				.OnComplete(() => singleton.imageLogo.DOColor(Color.white, 1.5f)
				.OnComplete(() => {if (callback != null) callback();}));
		}

		public static void Hide(Action callback = null)
		{
			Debug.Log("Clear Loading Screen");
			singleton.imageLogo.DOColor(clear, 1.5f)
				.OnComplete(()=> singleton.imageBackground.DOColor(clear, 1.0f)
				.OnComplete(() => {
					if (callback != null) callback();
					singleton.loadingScreenGameObject.SetActive(false);
				}));
		}

    }
}