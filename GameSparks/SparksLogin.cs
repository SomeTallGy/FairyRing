using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Core;
using UnityEngine;

public class SparksLogin : MonoBehaviour {

	public delegate void GSLoginHandler(EventState state, string msg = null, GSData data = null);
	public static event GSLoginHandler onGSLogin;

	public enum EventState
        {
            Init,                   // Initializing Loading
            Success,                // Successful at Loading Asset
            Error                   // ERROR while loading Asset
        };
	
	public GameSparksUnity gameSparksUnity;         // reference to the GameSparksUnity object

	public static bool loggedIn { get; private set; }

	// Use this for initialization
	IEnumerator Start()
        {
            gameSparksUnity.enabled = true;             // enable gameSparks
            yield return StartCoroutine(Login());       // start login process
        }

        ///<summary> Login to gameSparks (after initialization) </summary>
        public IEnumerator Login()
        {
			if(onGSLogin != null) onGSLogin(EventState.Init, "CONNECTING SERVICES");

            if (!GS.Available)
            {
                // turn on gamesparks
                while (!GS.Available)
                    yield return null;
            }

            new DeviceAuthenticationRequest().Send((response) =>
            {
                if (response.HasErrors)
                {
					if(onGSLogin != null) onGSLogin(EventState.Error, "COULD NOT CONNECT :(");
                }
                else
                {
                    Debug.Log("GameSparks DeviceLogin Successful");
					if(onGSLogin != null) onGSLogin(EventState.Success, "CONNECTED!");
                    loggedIn = true;
                }
            });
        }
}
