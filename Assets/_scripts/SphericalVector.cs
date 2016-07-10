using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;




/// <summary>
/// The SphericalVector is in RADIANS!
/// 
/// Create a spherical, cartesian, or cylindrical vector for use in the Unity 
/// coordinate system.  The class assumes that the z-axis points forward and 
/// backward, the rightAxis-axis  points left and right, and the y-axis points 
/// upward and downward.  The  Spherical Vector can be explicitly casted to a 
/// Vector3 with the coordinates set properly to be used directly in unity. 
/// See wikipediafor more info on spherical coordinates.
/// </summary>
[System.Serializable]
public class SphericalVector
{

    /// <summary>
    /// The radius away from the origin.
    /// </summary>
    public Component radius = new Component();
    /// <summary>
    /// The azithmal angle. The angle that rotates around the right axis in 
    /// RADIANS!
    /// </summary>
    public Component theta = new Component();
    /// <summary>
    /// The polar angle. The angle that rotates about the up axis in RADIANS!
    /// </summary>
    public Component phi = new Component();

    /// <summary>
    /// Empty Constructor
    /// </summary>
    public SphericalVector()
    {
        
    }


    /// <summary>
    /// Create a spherical vector based on cartesian coordinates.
    /// </summary>
    /// <param name="rightAxis"></param>
    /// <param name="upAxis"></param>
    /// <param name="forwardAxis"></param>
    private SphericalVector(float rightAxis, float upAxis, float forwardAxis)
    {
        float ra2 = Mathf.Pow(rightAxis, 2);
        float ua2 = Mathf.Pow(upAxis, 2);
        float fa2 = Mathf.Pow(forwardAxis, 2);
        radius.ForceSet(Mathf.Sqrt(ra2 + ua2 + fa2));
        theta.ForceSet(Mathf.Acos(upAxis / radius.value));
        phi.ForceSet(Mathf.Atan2(rightAxis, forwardAxis));
    }

    /// <summary>
    /// Create a spherical vector based on cartesian coordinates.
    /// </summary>
    /// <param name="rightAxis"></param>
    /// <param name="upAxis"></param>
    /// <param name="forwardAxis"></param>
    static public SphericalVector CreateFromCartesian(
       float rightAxis, float upAxis, float forwardAxis)
    {
        return new SphericalVector(rightAxis, upAxis, forwardAxis);
    }

    /// <summary>
    /// Create a spherical vector from a cartesian vector.
    /// </summary>
    /// <param name="vec">The vector3</param>
    /// <returns></returns>
    static public SphericalVector CreateFromCartesian(Vector3 vec)
    {
        return CreateFromCartesian(vec.x, vec.y, vec.z);
    }

    /// <summary>
    /// Create a Sperical component from spherical coordinates.
    /// </summary>
    /// <param name="radius">
    /// The distance away from the center point.
    /// </param>
    /// <param name="theta">
    /// The zenith angle (theta) of the value.  The angle that comes down off
    /// upward axis (Y in unity) and touches the point.  The angle must be 
    /// satisfy the following: 0 less than theta less than pi. If the angle 
    /// reaches pi or zero, distortion of the camera view ill occure.
    /// </param>
    /// <param name="phi">
    /// The azimuth angle (phi) of the value. The angle of the horizontal 
    /// plane.  In unity, its the ZX plane.  The values range from 0 -> 2pi and
    /// repeat.
    /// </param>
    /// <returns>
    /// A Spherical Vector.
    /// </returns>
    static public SphericalVector Create(
         float radius, float phi, float theta)
    {
        SphericalVector sv = new SphericalVector();
        sv.radius.ForceSet(radius);
        sv.phi.ForceSet(phi);
        sv.theta.ForceSet(theta);
        return sv;
    }

    /// <summary>
    /// Convert a spherical vector to a vector3. Its rightAxis,y,z coordinates will be
    /// properly calculated so that they can be used with the cartesian system.
    /// </summary>
    /// <param name="sv">The spherical vector to convert.</param>
    /// <returns>A Vector3 with properly calculated values.</returns>
    static public Vector3 ToCartesian(SphericalVector sv)
    {
        /*It may look weird, but they coordinates are moved around to work 
         with unitys axis of y(up) rightAxis(right) z(forward)*/
        /*The forward axis*/
        float forwardAxis = sv.radius.value * Mathf.Sin(sv.theta.value)
           * Mathf.Cos(sv.phi.value);
        /*The right axis*/
        float rightAxis = sv.radius.value * Mathf.Sin(sv.theta.value)
            * Mathf.Sin(sv.phi.value);
        /*The up axis.*/
        float upAxis = sv.radius.value * Mathf.Cos(sv.theta.value);
        return new Vector3(rightAxis, upAxis, forwardAxis);
    }

    /// <summary>
    /// Convert this to cylindrical cordinates.
    /// </summary>
    /// <param name="sv"></param>
    /// <returns></returns>
    static public CylindricalVector ToCylindrical(SphericalVector sv)
    {
        Vector3 cart = SphericalVector.ToCartesian(sv);
        return CylindricalVector.CreateFromCartesian(cart.x, cart.y, cart.z);
    }

    /// <summary>
    /// Create a spherical vector rom a cylindrical one.
    /// </summary>
    /// <param name="cv"></param>
    /// <returns></returns>
    static public SphericalVector CreateFromCylindrical(CylindricalVector cv)
    {
        var cyl = CylindricalVector.ToCartesian(cv);
        return SphericalVector.CreateFromCartesian(cyl);
    }

    /// <summary>
    /// Get a string reprisentation of this vector.
    /// </summary>
    /// <returns>a string reprisentation of this vector.</returns>
    override
    public string ToString()
    {
        return "[r=" + radius + ",phi=" + phi + ",theta=" + theta + "]";
    }

}
