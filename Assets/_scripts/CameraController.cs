using UnityEngine;
using System.Collections;

[System.Serializable]
public class Bool3
{
    public bool x, y, z;
}

public class CameraController : MonoBehaviour
{

    public CameraTarget target;
    public InputProcessor inputProc;
    public float defaultDistance;
    public Vector3 defaultRotation;
    public Bool3 lockRotation;
    public Bool3 lockPosition;
    public bool lookAtTarget = true;
    public bool invertDistanceScroll = true;
    public float rotationSpeed = 10;
    public KeyCode zKey = KeyCode.LeftControl;

    private float distanceToTarget;
    private float xRot = 1f;
    private float yRot = 1f;
    private float roll = 0.0f;

    void Start()
    {
        distanceToTarget = defaultDistance;
        transform.position = target.transform.position;
        transform.Translate(0, 0, -10);
        Debug.Log("Target is at: " + target.transform.position.ToString());
    }

    void FixedUpdate()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(zKey))
            {
                roll += lockRotation.z ? 0 
                    : (-(mouseX/10) * rotationSpeed * Time.deltaTime);
            }
            else
            {
                xRot += lockRotation.x ? 1 : ((mouseX / 100) * rotationSpeed
                    * Time.deltaTime);
                yRot += lockRotation.y ? 1 : ((mouseY / 100) * rotationSpeed
                    * Time.deltaTime);
            }
        }

        distanceToTarget += (invertDistanceScroll ? -1 : 1)
            * Input.mouseScrollDelta.y;

        float x = distanceToTarget * Mathf.Sin(xRot) * Mathf.Sin(yRot);
        float z = distanceToTarget * Mathf.Cos(xRot) * Mathf.Sin(yRot);
        float y = distanceToTarget * Mathf.Cos(yRot);
        transform.position = new Vector3(x, y, z);
        transform.position += target.transform.position;

        if (lookAtTarget)
        {
            transform.LookAt(target.transform);
            transform.Rotate(0, 0, roll);
        }
    }
}
