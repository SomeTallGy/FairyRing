using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// start the location service
		Input.location.Start();
		// enable the compass
		Input.compass.enabled = true;
		// enable the gyro
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		// read the magnetometer / compass value and generate a quaternion
		Quaternion trueHeading = Quaternion.Euler (0, Input.compass.trueHeading, 0);
		// interpolate between old and new position
		transform.localRotation = Quaternion.Slerp (transform.localRotation, trueHeading, 0.05f);	
	}
}
