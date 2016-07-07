using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public CameraTarget target;
    public float defaultDistance;
    public Vector3 defaultRotation;
    public Bool3 lockRotation;
    public Bool3 lockPosition;
    public bool invertDistanceScroll = true;
    public bool lookAtTarget = true;
    public Vector3 rotationSpeed = new Vector3(10, 10, 10);
    public KeyCode zKey = KeyCode.LeftControl;

    private float rad;
    private float roll = 0;
    private float theta = 1;
    private float phi = 10;
    private float speedMod = 0.1f;
    private Vector3 origin;

    void Start()
    {
        rad = defaultDistance;
        origin = new Vector3(0, 0, 0);
        Debug.Log("Target is at: " + target.transform.position.ToString());
    }

    void FixedUpdate()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(0))
        {
            if (!lockRotation.z && Input.GetKey(zKey))
            {
                roll += (-(mouseX) * rotationSpeed.z*speedMod);
                
            }
            else
            {
                float x = mouseX * Mathf.Cos(roll) - mouseY * Mathf.Sin(roll);
                float y = mouseX * Mathf.Sin(roll) + mouseY * Mathf.Cos(roll);
                theta += lockRotation.x ? 0 : ((y) * rotationSpeed.x * speedMod);
                phi += lockRotation.y ? 0 : ((x) * rotationSpeed.y * speedMod);
            }
        }

        rad += (invertDistanceScroll ? -1 : 1)
            * Input.mouseScrollDelta.y;

        while (phi > 2 * Mathf.PI)
        {
            phi -= 2 * Mathf.PI;
        }
        while (phi < 0)
        {
            phi += 2 * Mathf.PI;
        }

        float localZ = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        float localX = rad * Mathf.Sin(theta) * Mathf.Sin(phi);
        float localY = rad * Mathf.Cos(theta);

        

        Vector3 newPosition = new Vector3(localX, localY, localZ);
        newPosition += target.transform.position;
        Vector3 difference = newPosition - transform.position;
        origin += difference;
        
        transform.position = newPosition;

        if (lookAtTarget)
        {
            transform.LookAt(target.transform);
            transform.Rotate(0, 0, roll * Mathf.Rad2Deg);
        }
    }
}
