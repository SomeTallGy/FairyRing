using System.Collections.Generic;
using UnityEngine;
using FairyO.Spatial;

namespace FairyO.World
{
    public class WorldCompass : MonoBehaviour
    {

        // ---------- constants ----------
        public const float COMPASS_RADIUS = 10;                     // radius of compass in Unity Units
        private const float NODE_SCALE_MIN = 1.0f;
        private const float NODE_SCALE_MAX = 3.0f;

        // ---------- inspector ----------
        public WorldObjectCollider nodeDetector;                    // node "detector" on player
        public WorldObjectCollider nodeTrigger;                     // node "trigger" on player
        public GameObject cam;                                      // camera viewing compass
        public GameObject nodePrefab;                               // prefab for node
        public GameObject nodesContainer;                           // container for compass nodes

        public GameObject blipPrefab;                               // prefab for blip
        public GameObject blipsContainer;                           // container for compass blips (nodes, monsters etc.)

        public GameObject face;                                     // compass face


        // ------------ fields -----------
        private Dictionary<int, GameObject> blips = new Dictionary<int, GameObject>();

        void Start()
        {
            nodeDetector.onWorldObjectTrigger += onWorldObjectDetect;
        }

        void Update()
        {
            UpdateBlips();
            UpdateHeading();
            UpdateAttitude();
        }

        private void UpdateBlips()
        {
            float p = COMPASS_RADIUS / nodeDetector.range;
            foreach (var b in blips)
            {
                GameObject go = b.Value;

                IWorldObject wo = WorldSpawner.worldObjects[b.Key];

                go.transform.localPosition = PositionOnEdge(wo);
				go.transform.localScale = ScaleOnEdge(wo);
            }
        }

        private Vector3 PositionOnEdge(IWorldObject wo)
        {
            Vector2 vA = new Vector2(SpatialManager.myPosition.x, SpatialManager.myPosition.z);
            Vector2 vB = new Vector2(wo.spatial.position.x, wo.spatial.position.z);
            Vector2 d = vB - vA;
            float s = (vB.y < vA.y) ? -1.0f : 1.0f;
            float a = Vector2.Angle(Vector2.right, d) * s;
			float x = COMPASS_RADIUS * Mathf.Cos(a * Mathf.Deg2Rad);
            float z = COMPASS_RADIUS * Mathf.Sin(a * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }

		private Vector3 PositionOnFace(IWorldObject wo)
		{
			float p = COMPASS_RADIUS / nodeDetector.range;
            return (wo.spatial.position - SpatialManager.myPosition) * p; 
		}

		private Vector3 ScaleOnEdge(IWorldObject wo)
		{
			float d = Vector3.Distance(SpatialManager.myPosition, wo.spatial.position);
			float s = NODE_SCALE_MIN + (1 - (d / nodeDetector.range)) * (NODE_SCALE_MAX - NODE_SCALE_MIN);

			return new Vector3(s, s, s);
		}

        private void UpdateHeading()
        {
            face.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, face.transform.localRotation.eulerAngles.y, 0), Quaternion.Inverse(SpatialManager.myHeading), 0.05f);
        }

        private void UpdateAttitude()
        {
            float a = SpatialManager.myAttitude.eulerAngles.x;

            // clamp angle
            if (a < 10 || a > 180)
                a = 10;
            else if (a > 90)
                a = 90;

            // offset angle
            a -= 90;

            Quaternion q = Quaternion.Euler(a, 0, 0);
            this.transform.rotation = Quaternion.Slerp(Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, 0), Quaternion.Inverse(q), 0.05f);
        }

		private void onWorldObjectDetect(IWorldObject wo, bool enter)
		{
			if(enter == true)
			{
				GameObject blip = Instantiate(blipPrefab, PositionOnEdge(wo), new Quaternion(0, 0, 0, 1));
            	blip.transform.parent = nodesContainer.transform;

            	// add to dict
            	blips[wo.id] = blip;
			}
			else
			{
				 // destroy game object
            	GameObject blip = blips[wo.id];
            	Destroy(blip);

            	// remove from dict
            	blips.Remove(wo.id);
			}
		}

    }
}