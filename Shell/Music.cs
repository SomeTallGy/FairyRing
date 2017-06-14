using UnityEngine;
using DG.Tweening;

namespace FairyO
{
    public class Music : MonoBehaviour
    {
		// 0000000 Singleton 00000000
		private static Music singleton;
		void Awake()
        {
            // setup singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);
        }

		// ------- Enums ------------
		public enum State { Idle, Menu, Map }

		// ------- inspector --------
		public AudioSource source;

		public AudioClip menuClip;
		public AudioClip mapClip;

		// ------- fields ------------
		[SerializeField] private static State _state;


		private static Tween tween;

		// ------- properties --------
		public static State state{
			get{
				return _state;
			}
			set{
				switch(value)
				{
					case State.Idle:
						Fade();
						break;
					case State.Menu:
						Play(singleton.menuClip);
						break;
					case State.Map:
						Play(singleton.mapClip);
						break;
				}
				_state = value;
			}
		}

		private static void Fade()
		{
			tween = singleton.source.DOFade(0f, 3.0f).OnComplete(()=>{
				singleton.source.Stop();
			});
		}

		private static void Play(AudioClip clip)
		{
			if(tween != null)
				tween.Complete();

			singleton.source.volume = 1;
			singleton.source.PlayOneShot(clip);

		}
        
    }
}