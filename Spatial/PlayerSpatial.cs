using UnityEngine;

namespace FairyO.Spatial
{
    public class PlayerSpatial : WorldSpatial
    {
		public Transform camera_transform;

        // Update is called once per frame
        void Update()
        {
			if(SpatialManager.status == SpatialManager.Status.Ready)
			{
				//transform.position = Vector3.Slerp(transform.position, SpatialManager.myPosition, 0.05f);
				heading = SpatialManager.myHeading;
				attitude = SpatialManager.myAttitude;
			}

			Vector3 forward = camera_transform.TransformDirection(Vector3.forward) * 10;
        	Debug.DrawRay(camera_transform.position, forward, Color.white);
        }

		public override Quaternion heading{get; set;}

		protected Quaternion attitude{
			get{ 
				return Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, this.transform.rotation.eulerAngles.z); 
			}
			set{
				this.transform.localRotation = Quaternion.Euler(0, value.eulerAngles.y, 0);
				camera_transform.localRotation = Quaternion.Euler(value.eulerAngles.x, 0, value.eulerAngles.z);
			}
		}
    }
}