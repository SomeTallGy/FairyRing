using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadStools : MonoBehaviour {

	List<GameObject> toadStools = new List<GameObject>();

	// Use this for initialization
	void Start () {
		// get all children and add them to the toadstools

		for(int c = 0; c < this.transform.childCount; c++)
		{
			toadStools.Add(this.transform.GetChild(c).gameObject);
		}

		StartCoroutine( PopStools() );
	}

	IEnumerator PopStools()
	{
		for(int c = 0; c < toadStools.Count; c++)
		{
			yield return new WaitForSeconds(0.2f);
			toadStools[c].GetComponent<ToadStool>().Pop();
		}

		yield return this;
	}
}
