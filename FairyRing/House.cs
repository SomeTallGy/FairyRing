using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class House : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.DOScale(Vector3.one, 1.0f);
	}
}
