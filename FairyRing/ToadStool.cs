using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToadStool : MonoBehaviour {

	// Use this for initialization
	void Awake()
	{
		this.transform.localScale = Vector3.zero;
		//this.transform.localPosition = Vector3.zero;
	}

	public void Pop()
	{
		this.transform.DOScale(Vector3.one, 1.0f);
		//this.transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 1.0f);
	}
}
