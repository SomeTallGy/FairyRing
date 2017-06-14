using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyO.Spatial;

namespace FairyO.World
{
    public class WorldNode : WorldObject, IWorldObject
    {

		// -------- properties --------
		public Node node {get; internal set;}
		
        void OnMouseDown()
		{
			//Destroy(this.transform.parent.gameObject);
		}

		public void Activate()
		{
			
		}
    }
}