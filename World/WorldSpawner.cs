using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GoMap;

namespace FairyO.World
{
    public abstract class WorldSpawner : MonoBehaviour
    {
		// -------- inspector ---------
		public LocationManager locationManager;
		public GOMap goMap;
		
		// -------- fields ---------
		public static Dictionary<int, IWorldObject> worldObjects = new Dictionary<int, IWorldObject>();

		void Start()
		{
			//SpawnNodes();	// create nodes
			//SpawnNodeAt(new Vector3(0, 0, 50));
		}


		protected float AreaOfCirc(float radius)
		{
			return Mathf.PI * (radius * radius);
		}

    }
}