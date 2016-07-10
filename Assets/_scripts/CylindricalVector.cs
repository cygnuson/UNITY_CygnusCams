using UnityEngine;
using System.Collections;

/// <summary>
/// The CylindricalVector is in RADIANS! Create a vector that is managed in
/// cylindrical coordinates and can be converted to cartesian or spherical
/// coordinates. 
/// </summary>
public class CylindricalVector
{
    /// <summary>
    /// the distance away from the origin on the horizontal plane 
    /// (XZ for unity).
    /// </summary>
    float rho { get; set; }
    /// <summary>
    /// The rotation on the horizontal plane (XZ for unity).
    /// </summary>
    float phi { get; set; }
    /// <summary>
    /// The height above the horizontal plane.
    /// </summary>
    float elevation { get; set; }

    /// <summary>
    /// Empty Constructor.
    /// </summary>
    public CylindricalVector() { }

    /// <summary>
    /// Create a Cylindrical vector from cartesian cordinates.
    /// </summary>
    /// <param name="rightAxis"></param>
    /// <param name="upAxis"></param>
    /// <param name="forwardAxis"></param>
    private CylindricalVector(float rightAxis, float upAxis, float forwardAxis)
    {
        /*The forward axis.*/
        float fa2 = Mathf.Pow(forwardAxis, 2);
        /*The right axis.*/
        float ra2 = Mathf.Pow(rightAxis, 2);
        /*the Up axis.*/
        elevation = upAxis;
        rho = Mathf.Sqrt(fa2 + ra2);
        phi = Mathf.Atan2(rightAxis, forwardAxis);
    }
    
    /// <summary>
    /// Create a standard Cylindrical Vector from its components.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="phi"></param>
    /// <param name="elevation"></param>
    /// <returns></returns>
    static public CylindricalVector Create(float radius,
        float phi, float elevation)
    {
        CylindricalVector cv = new CylindricalVector();
        cv.rho = radius;
        cv.phi = phi;
        cv.elevation = elevation;
        return cv;
    }

    /// <summary>
    /// Create a Cylindrical Vector from cartesian coordinates.
    /// </summary>
    /// <param name="rightAxis"></param>
    /// <param name="upAxis"></param>
    /// <param name="forwardAxis"></param>
    /// <returns></returns>
    static public CylindricalVector CreateFromCartesian(
        float rightAxis, float upAxis, float forwardAxis)
    {
        return new CylindricalVector(rightAxis, upAxis, forwardAxis);
    }

    /// <summary>
    /// Convert this to a standard Vector3.
    /// </summary>
    /// <param name="cv"></param>
    /// <returns></returns>
    static public Vector3 ToCartesian(CylindricalVector cv)
    {
        float forwardAxis = cv.rho * Mathf.Cos(cv.phi);
        float rightAxis = cv.rho * Mathf.Sin(cv.phi);
        float upAxis = cv.elevation;
        return new Vector3(rightAxis,upAxis,forwardAxis);
    }

    /// <summary>
    /// Covnert this to a spehrical vector.
    /// </summary>
    /// <param name="cv"></param>
    /// <returns></returns>
    public static SphericalVector ToSpherical(CylindricalVector cv)
    {
        Vector3 cart = ToCartesian(cv);
        return SphericalVector.Create(cart.x, cart.y, cart.z);
    }

    /// <summary>
    /// Create a cylindrical vector from a cartesian vector.
    /// </summary>
    /// <param name="vec">The vector3</param>
    /// <returns></returns>
    static public CylindricalVector CreateFromCartesian(Vector3 vec)
    {
        return CreateFromCartesian(vec.x, vec.y, vec.z);
    }

    /// <summary>
    /// Create a Cylindrical Vector from a spherical vector.
    /// </summary>
    /// <param name="sv">The SphericalVector.</param>
    /// <returns></returns>
    public static CylindricalVector CreateFromSpherical(SphericalVector sv)
    {
        var sph = SphericalVector.ToCartesian(sv);
        return CylindricalVector.CreateFromCartesian(sph);
    }

    /// <summary>
    /// Get a string reprisentation of this vector.
    /// </summary>
    /// <returns>a string reprisentation of this vector.</returns>
    override
    public string ToString()
    {
        return "[r=" + rho + ",phi=" + phi + ",elev=" + elevation + "]";
    }

}
