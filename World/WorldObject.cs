using UnityEngine;
using FairyO.Spatial;

namespace FairyO.World
{
    public abstract class WorldObject : MonoBehaviour
    {
		private static int w_id = 0;

		public int id {get; private set;}

		void Awake()
		{
			id = w_id++;
		}

        public WorldSpatial spatial
        {
            get { return transform.parent.GetComponent<WorldSpatial>(); }
        }

		void OnDestroy()
		{
			WorldSpawner.worldObjects.Remove(id);
		}
    }
}