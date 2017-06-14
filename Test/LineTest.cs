using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour {

	public LineSeg lineSeg1;
	public LineSeg lineSeg2;

	// Use this for initialization
	void Start () {

		Debug.DrawLine(new Vector3(lineSeg1.a.x, 0, lineSeg1.a.y), new Vector3(lineSeg1.b.x, 0, lineSeg1.b.y), Color.blue, 10000.0f, false);
		Debug.DrawLine(new Vector3(lineSeg2.a.x, 0, lineSeg2.a.y), new Vector3(lineSeg2.b.x, 0, lineSeg2.b.y), Color.cyan, 10000.0f, false);
		
		Debug.Log(LinesIntersect.FasterLineSegmentIntersection(lineSeg1.a, lineSeg1.b, lineSeg2.a, lineSeg2.b));

		//Debug.Log(LinesIntersect.DoLinesIntersect(lineSeg1, lineSeg2));
	}
}
