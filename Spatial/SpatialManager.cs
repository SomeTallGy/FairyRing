using System.Collections;
using UnityEngine;
using GoShared;

namespace FairyO.Spatial
{
    public class SpatialManager : MonoBehaviour
    {
        // ----- Constants ------

        public const float GPS_RESOLUTION_ORIGIN = 12;
        public const float GPS_RESOLUTION_MOVING = 8;

        // ----- Enums and Delegates ------
        public enum Status { Initializing, Ready, Pause }
        public enum Zone { Origin, Outside }

        // ---------- Inspector -----------
        public LocationManager locationManager;

        [SerializeField] 
        internal Vector3 position;

        // ---------- Properties ----------
        public static Coordinates origin { get; private set; }
        public static Coordinates myCoordinates { get; private set; }
        public static Quaternion myHeading { get; internal set; }
        public static Quaternion myAttitude { get; internal set; }
        public static Status status { get; set; }
        public static Zone zone { get; private set; }
        public static LocationServiceStatus locationStatus { get; private set; }
        public static Vector3 myPosition                                
        {
            get { return _myPosition; }
            internal set
            {
                singleton.position = _myPosition = value;
            }
        }

        public static Vector3 myPosition_h { get; internal set; }       // hypothetical position (set in editor mode)

        // ---------- Fields --------------
        internal static SpatialManager singleton;

        private static Vector3 _myPosition;
        private static Vector3 _myPosition_h;

        void Awake()
        {
            // setup singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(gameObject);
        }

        void Start()
        {
            locationManager.onOriginSet += (coordinates) => {
                origin = coordinates;
                status = Status.Ready;
            };

            if (!Application.isEditor || Application.isMobilePlatform)
            {
                // enable the gyro
                Input.gyro.enabled = true;
                StartCoroutine(AttitudeCheck());

                // enable the compass
                Input.compass.enabled = true;
                StartCoroutine(HeadingCheck());
            }
            else
            {
                // run compass and gyro by key and mouse
                this.gameObject.AddComponent<SpatialMouseLook>();

                // set status to ready
                status = Status.Ready;
            }
        }

        private void OnOriginSet(Coordinates originCoordinates)
        {
            origin = originCoordinates;
        }

        /*
        protected void OnGUI()
        {
            GUI.skin.label.fontSize = Screen.width / 40;

            if (myCoordinates != null)
            {
                GUILayout.Label("coordinates: " + myCoordinates.toLatLongString());
                GUILayout.Label("position: " + myPosition.ToString());
                GUILayout.Label("distance_o: " + Vector3.Distance(Vector3.zero, myPosition));
            }

            if (myHeading != null)
            {
                GUILayout.Label("heading: " + myHeading.eulerAngles.ToString());
            }

            if (myAttitude != null)
            {
                GUILayout.Label("attitude: " + myAttitude.eulerAngles.ToString());
            }

            GUILayout.Label("location status: " + Input.location.status.ToString());
        }
         */

        private IEnumerator HeadingCheck()
        {
            while (true)
            {
                myHeading = Quaternion.Euler(0, Input.compass.trueHeading + 180, 0);
                yield return null;
            }
        }

        private IEnumerator AttitudeCheck()
        {
            while (true)
            {
                //myAttitude = GyroToUnity(Input.gyro.attitude);
                myAttitude = MSP_Input.GyroAccel.GetRotation();
                yield return null;
            }
        }

        private bool GPS_CheckInOrigin(Coordinates c)
        {
            //print(Coordinates.DistanceBetween2D(c, myCoordinates));
            if (Coordinates.DistanceBetween2D(c, myCoordinates) >= GPS_RESOLUTION_ORIGIN) // if GPS movement is beyond GPS resolution
            {
                myCoordinates = c;
                _myPosition = myCoordinates.convertCoordinateToVector(0);
                return true;
            }
            return false;
        }

        private bool GPS_CheckOutside(Coordinates c)
        {
            //print(Coordinates.DistanceBetween2D(c, myCoordinates));
            if (Coordinates.DistanceBetween2D(c, myCoordinates) >= GPS_RESOLUTION_MOVING) // if GPS movement is beyond GPS resolution
            {
                myCoordinates = c;
                _myPosition = c.convertCoordinateToVector(0);
                return true;
            }
            return false;
        }

        private void ZoneCheck()
        {
            if (zone == Zone.Origin && Coordinates.DistanceBetween2D(myCoordinates, origin) >= 12)
            {
                zone = Zone.Outside;
            }
            else if (zone == Zone.Outside && Coordinates.DistanceBetween2D(myCoordinates, origin) < 12)
            {
                zone = Zone.Origin;
            }
        }

        private static Quaternion GyroToUnity(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }
}