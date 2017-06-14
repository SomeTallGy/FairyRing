using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crystal : MonoBehaviour {

	private bool clicked = false;

	void Awake ()
	{
		this.transform.localScale = Vector3.zero;
	}
	// Use this for initialization
	void Start () {
		this.transform.DOScale(new Vector3(2,2,2), 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(0,1,0) * Time.deltaTime * 100, Space.World);
	}

	void OnMouseDown() {
		if(Input.GetMouseButton(0) && !clicked)
		{
			clicked = true;
			this.transform.parent.GetComponent<FairyRing>().EmergeToadStools();
			Dispose();
		}
	}

	void Dispose()
	{
		this.transform.DOScale(Vector3.zero, 1.0f).OnComplete( ()=> {
			Destroy(this.gameObject);
		});
	}
}
