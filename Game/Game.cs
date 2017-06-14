using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;

namespace FairyO.Game
{
    public class Game : MonoBehaviour
    {

        // ------- Enums ------------
        public enum State { Idle, Map }

		// ------- inspector ---------
		public LocationManager locationManager;
		public GOMap goMap;

        // ------- fields ------------
        [SerializeField]
        private State _state;

        // ------- properties --------
        public State state
        {
            get
            {
                return _state;
            }
            set
            {
                switch (value)
                {
                    case State.Idle:
						Dais.state = Dais.State.Idle;
						FairyO.Music.state = FairyO.Music.State.Idle;
                        break;
                    case State.Map:
						Dais.state = Dais.State.Map;
						FairyO.Music.state = FairyO.Music.State.Map;
                        break;
                }
				_state = value;
            }
        }

		void Start()
		{
			locationManager.onOriginSet += onOriginSet;
			this.state = State.Idle;
			FairyO.Shell.Shell.state = FairyO.Shell.Shell.State.Other;
		}

		private void onOriginSet(Coordinates coordinates)
		{
			FairyO.Shell.LoadingScreen.Hide(onGameReady);
		}

		private void onGameReady()
		{
			this.state = State.Map;
		}

    }
}
