using UnityEngine;

namespace FairyO.Spatial
{
    public class SpatialWASD : MonoBehaviour
    {
        // ---------- Constants -----------
        internal const float WASD_WALK_SPEED = 1.4f;  // meters per second (avg human walk speed)
        internal const float WASD_RUN_SPEED = 4.2f;  // meters per second (avg human run speed)

        // Update is called once per frame
        void Update()
        {
            // position variables
            Vector3 current_v = SpatialManager.myPosition_h;
            Vector3 v = SpatialManager.myPosition_h;
            Quaternion q = SpatialManager.myAttitude;

			float d;
			
			if (Input.GetKey(KeyCode.LeftShift))
			{
				d = WASD_RUN_SPEED * Time.deltaTime;
			}
			else
			{
				d = WASD_WALK_SPEED * Time.deltaTime;
			}

            if (Input.GetKey(KeyCode.W))
            {	// move forward
                v += new Vector3(d * Mathf.Sin(q.eulerAngles.y * Mathf.Deg2Rad), 0, d * Mathf.Cos(q.eulerAngles.y * Mathf.Deg2Rad));
            }
            if (Input.GetKey(KeyCode.S))
            {	// move back
                v -= new Vector3(d * Mathf.Sin(q.eulerAngles.y * Mathf.Deg2Rad), 0, d * Mathf.Cos(q.eulerAngles.y * Mathf.Deg2Rad));
            }

            if (v != current_v)
            {	// update values
				SpatialManager.singleton.position = SpatialManager.myPosition_h = v;
            }
        }
    }
}