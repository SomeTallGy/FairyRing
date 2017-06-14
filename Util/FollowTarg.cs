using UnityEngine;

public class FollowTarg : MonoBehaviour {

	public GameObject target;

	public enum FollowAxis{ XZ, XY, XYZ };
	public FollowAxis followAxis;

	private Vector3 original;

	// Use this for initialization
	void Start () {
		original = this.transform.localPosition;
		Follow();
	}
	
	// Update is called once per frame
	void Update () {
		Follow();
	}

	private void Follow()
	{
		if(target != null)
		{	
			switch(followAxis)
			{
				case FollowAxis.XZ :
					this.transform.localPosition = new Vector3(target.transform.position.x, original.y, target.transform.position.z);
					break;
				case FollowAxis.XY :
					this.transform.localPosition = new Vector3(target.transform.position.x, target.transform.position.y, original.z);
					break;
				case FollowAxis.XYZ :
					this.transform.localPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
					break;
				
			}
			
		}
			
	}
}
