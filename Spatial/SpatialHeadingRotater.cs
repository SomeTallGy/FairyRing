using UnityEngine;
using FairyO.Spatial;

public class SpatialHeadingRotater : MonoBehaviour {

	public float offsetX = 0;
	public float offsetY = 0;
	public float offsetZ = 0;
	public float easingSpeed = 0.05f;

	void Update () {
		this.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(offsetX, this.transform.localRotation.eulerAngles.y, offsetZ), Quaternion.Euler(offsetX, SpatialManager.myHeading.eulerAngles.y + offsetY, offsetZ), 0.05f);
	}
}
