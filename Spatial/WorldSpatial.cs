using UnityEngine;
using GoShared;

namespace FairyO.Spatial
{
    public class WorldSpatial : MonoBehaviour
    {

		public virtual Coordinates coordinates{
			get{ 
				Vector3 c = GPSEncoder.USCToGPS(this.transform.localPosition);
				return new Coordinates(c.x, c.y, c.z);
			}
			set{
				this.transform.localPosition = value.convertCoordinateToVector();
			}
		}

		public virtual Quaternion heading{
			get{ return Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0); }
			set{
				this.transform.localRotation = value;										// add to current localRotation
			}
		}

		public virtual Vector3 position{
			get{
				return this.transform.localPosition;
			}
		}

    }
}