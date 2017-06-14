using UnityEngine;

namespace FairyO.World
{
	public class WorldObjectCollider : MonoBehaviour {
		
		// ---------- events -------------
		public delegate void OnWorldObjectHandler(IWorldObject o, bool enter);
		public event OnWorldObjectHandler onWorldObjectTrigger;
		
		// ---------- fields ----------
		[SerializeField] private float _range = 50;			// range of detection	

		public float range{
			get{ return _range; }
			set{ 
				this.GetComponent<SphereCollider>().radius = _range = value;
			}
		}

		void Start()
		{
			range = _range;
		}

		void OnTriggerEnter(Collider other) {
			// get IWorldObject
			Component[] components = other.GetComponents(typeof(IWorldObject));
			IWorldObject wo = components[0] as IWorldObject;
			
			// send off event
			if(onWorldObjectTrigger != null)
				onWorldObjectTrigger(wo, true);
    	}

		void OnTriggerExit(Collider other) {
			// remove from list
			Component[] components = other.GetComponents(typeof(IWorldObject));
			IWorldObject wo = components[0] as IWorldObject;

			// send off event
			if(onWorldObjectTrigger != null)
				onWorldObjectTrigger(wo, false);
		}

	}
}

