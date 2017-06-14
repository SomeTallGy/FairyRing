using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SelfDestruct : MonoBehaviour {

	// -------- events --------
	public event OnDestructHandler onDestruct;
	public delegate void OnDestructHandler(GameObject go);

	// -------- inspector --------
	public float destructTime = 3.0f;

	void Start () {
		StartDestruct();
	}

	public void StartDestruct(float time = 0)
	{
		if(time != 0)
			Invoke("Destruct", time);
		else
			Invoke("Destruct", destructTime);		
	}

	public void Reset(bool wait)
	{
		CancelInvoke("Destruct");
		if(!wait)
			StartDestruct();
	}

	private void Destruct()
	{
		if(onDestruct != null)
			onDestruct(this.gameObject);

		this.transform.DOScale(Vector3.zero, 0.5f).OnComplete(()=>Destroy(this.gameObject));
	}	
	
}
