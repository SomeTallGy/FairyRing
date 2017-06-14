using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GroundMap : MonoBehaviour {

	public GameObject mapCamera;
	// Use this for initialization
	void Start () {
		this.GetComponent<MeshRenderer>().material.DOColor(Color.white, 3.0f);
	}

	void OnMouseDown()
	{
		if(Input.GetMouseButton(0))
		{
			if(!mapCamera.activeInHierarchy)
			{
				mapCamera.SetActive(true);
			}
			else
			{
				mapCamera.SetActive(false);
			}
		}
	}
}
