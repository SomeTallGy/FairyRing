using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotate : MonoBehaviour {

	Quaternion quat;

	void Update () {
		if(Input.gyro.enabled)
			GyroModifyRotation();
	}

	void GyroModifyRotation()
	{
		quat = GyroToUnity(Input.gyro.attitude);
		this.transform.rotation = quat;
	}

	void OnGUI()
	{
		GUI.skin.label.fontSize = Screen.width/40;
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
		if(quat != null)
			GUILayout.Label("quat: "+quat.ToString());
	}

	private static Quaternion GyroToUnity(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}
}
