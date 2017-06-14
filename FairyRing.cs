using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyRing : MonoBehaviour {

	public Crystal crystal;
	public ToadStools toadStools;
	public House house;

	public GameObject ringFX;
	public GameObject castFX;
	
	public void EmergeToadStools()
	{
		toadStools.gameObject.SetActive(true);
		house.gameObject.SetActive(true);
		ringFX.SetActive(false);
		castFX.SetActive(true);
	}
}
