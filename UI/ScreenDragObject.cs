using UnityEngine;

public class ScreenDragObject : MonoBehaviour
{
    // -------- enums ---------------
    public enum Axis { XY, X, XZ }

    // -------- inspector -----------
	public Camera fromCamera;
    public Axis dragAxis;
    public bool smooth;
    [RangeAttribute(1,10)] public float smoothSpeed = 5;

    // ------- private fields -------
    private Transform objectToMove;

    private Vector3 originalPos;

    void Start()
    {
        this.objectToMove = this.transform;
        this.originalPos = this.objectToMove.position;
        if(fromCamera == null)
            fromCamera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float z = objectToMove.position.z - fromCamera.transform.position.z; // get distance that object is from camera
            Vector3 pos = fromCamera.ScreenToWorldPoint(new Vector3(x, y, z));
            Vector3 targetPos;

            switch (dragAxis)
            {
                case Axis.X:
                    targetPos = new Vector3(pos.x, objectToMove.position.y, objectToMove.position.z);
                    break;
                case Axis.XZ:
                    targetPos = new Vector3(pos.x, objectToMove.position.y, pos.y);
                    break;
                case Axis.XY:
                default:
                    targetPos = new Vector3(pos.x, pos.y, objectToMove.position.z);
                    break;
            }
            
            if(smooth)
                objectToMove.position = Vector3.Lerp(objectToMove.position, targetPos, Time.deltaTime * smoothSpeed);
            else
                objectToMove.position = targetPos;
        }
        else
        {
            objectToMove.position = Vector3.Lerp(objectToMove.position, originalPos, Time.deltaTime * 5.0f);
        }
	}
}
