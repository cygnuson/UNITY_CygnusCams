using System;
using UnityEngine;


class SphericalCamera : AbstractCamera
{

    /// <summary>
    /// The coordinates on spherical components of the camera.
    /// </summary>
    public SphericalVector sphericalVector;

    /// <summary>
    /// Zoom in or zoom out with a negative amount.  The radius filter will 
    /// apply automatically if its set.
    /// </summary>
    /// <param name="amt">The amount to zoom. amt tess than 0 for zoom out.
    /// </param>
    override
    public void Zoom(float amt)
    {
        sphericalVector.radius += amt;
    }

    /// <summary>
    /// Spin the camera around. If its on a track that it will stay on its
    /// track properly.
    /// </summary>
    /// <param name="aboutUpAxis_phi">The amount to spin about the Up axis. 
    /// Also known as YAW</param>
    /// <param name="aboutRightAxis_theta">The amount to spin about the right
    /// axis. Also known as PITCH</param>
    /// <param name="aboutForwardAxis_roll">The amount to spin about the 
    /// forward axis. Also known as ROLL</param>
    override
    public void Spin(float aboutUpAxis_phi, float aboutRightAxis_theta, 
        float aboutForwardAxis_roll)
    {
        /*Phi and theta will be filtered automaticaly if they have it set.*/
        if(aboutUpAxis_phi != 0)
            sphericalVector.phi += aboutUpAxis_phi;
        if (aboutRightAxis_theta != 0)
            sphericalVector.theta += aboutRightAxis_theta;
        if (aboutForwardAxis_roll != 0)
            roll += aboutForwardAxis_roll;
    }


    /// <summary>
    /// Create the class.
    /// </summary>
    /// <param name="distance">The distance that the camera should be about 
    /// the target (How far behind the target does the camera start).</param>
    /// <param name="rollAmount">the Roll in radians of the forward axis tilting.
    /// zero is straight up and down.</param>
    /// <param name="moveSpeed">The speed that the camera will move.</param>
    /// <param name="phi">The default value of the polar angle (how rotated to
    /// the sides does the camera start - Zero is directly behind).</param>
    /// <param name="theta">The default value of the azithmal angle. (How high
    /// above the player does the camera start - Zero is behind).</param>
    public SphericalCamera(float distance, float rollAmount, float moveSpeed, 
        float rollSpeed, float theta, float phi) : base(rollAmount)
    {
        sphericalVector = SphericalVector.Create(distance, phi, theta);
    }

    /// <summary>
    /// Get the Vector3 from this camera trackign object.
    /// </summary>
    /// <returns>A vector in cartesian coordinates.</returns>
    override
    public Vector3 GetLocation()
    {
        return SphericalVector.ToCartesian(sphericalVector);
    }

}
