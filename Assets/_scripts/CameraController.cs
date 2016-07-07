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
                roll += lockRotation.z ? 0
                    : ((-(mouseX) * rotationSpeed.z * speedMod));

            }
            else
            {
                /*The next two statements convert out regular XY mouse 
                 * cordinates to a a rotated axis that will allow proper 
                 * rotation.*/
                float x = mouseX * Mathf.Cos(roll) - mouseY * Mathf.Sin(roll);
                float y = mouseX * Mathf.Sin(roll) + mouseY * Mathf.Cos(roll);
                theta += lockRotation.x ? 0 
                    : ((y) * rotationSpeed.x * speedMod);
                phi += lockRotation.y ? 0 
                    : ((x) * rotationSpeed.y * speedMod);
            }
        }
        /*zoom the camera outward.*/
        rad += (invertDistanceScroll ? -1 : 1)
            * Input.mouseScrollDelta.y;
        /*Keep phi from getting out of control with large numbers.*/
        while (phi > 2 * Mathf.PI)
        {
            phi -= 2 * Mathf.PI;
        }
        while (phi < 0)
        {
            phi += 2 * Mathf.PI;
        }
        /*Calculate the x y and z spherical cordinates so that there is a 
         * smooth rotation*/
        float localZ = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        float localX = rad * Mathf.Sin(theta) * Mathf.Sin(phi);
        float localY = rad * Mathf.Cos(theta);
        /*Set the camera to have to correct position.*/
        transform.position = new Vector3(localX, localY, localZ);
        transform.position += target.transform.position;

        if (lookAtTarget)
        {
            transform.LookAt(target.transform);
            /*Do a barrel roll.*/
            transform.Rotate(0, 0, roll * Mathf.Rad2Deg);
        }
    }
}
