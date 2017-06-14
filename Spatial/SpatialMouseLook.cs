using UnityEngine;

namespace FairyO.Spatial
{
    public class SpatialMouseLook : MonoBehaviour
    {
		// -------- Inspector -------------
        [SerializeField] private float rotationX = 0F;
        [SerializeField] private float rotationY = 0F;

        // ---------- Fields --------------
        float sensitivityX = 1.0F;
        float sensitivityY = 1.0F;
        float minimumX = -360F;
        float maximumX = 360F;
        float minimumY = -60F;
        float maximumY = 60F;
        Quaternion originalRotation = new Quaternion(0, 0, 0, 1);

        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                SpatialManager.myHeading = originalRotation * xQuaternion;
                SpatialManager.myAttitude = originalRotation * xQuaternion * yQuaternion;
				//SpatialManager.myAttitude = originalRotation * yQuaternion;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}