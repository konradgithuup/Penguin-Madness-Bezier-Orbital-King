using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the platform prefab. It can be used in the following way.
/// I. Accessing the platform
///     1. Access the active platforms inside the PlatformManager-Script
///     2. Iterate over the platforms and check, if it intersects with the current trajectory.
///         - The trajectory is implemented in TrajectoryCalculator#CalcY
///     3. Once the penguin lands on a platform, store it as the most recent platform
/// II. Movement
///     1. When the penguin takes off, the entire flight path is known.
///         - get the x-Component off it's velocity (simply use its last two positions for that divided by Time.deltaTime)
///         - on every update, use #CalcY with x being this.transform.position.x + (v_x * Time.deltaTime), with v_x being the value calculated in the previous step
///         - move penguin to the calculated position
///     2. Once it lands on the next plate (intersection described in step I), use the bezier calculations implemented in the BezierPlatform script (you get the platform from step I)
///         - calculating the y position for a specific x on a bezier curve is hard. using t is not. The bezier curves used for the platforms are so tame that t approx. x, so just make your life easy and use t (the function is already implemented)
///         - while on the platform increase penguin velocity at a constant rate
///         - to update the position on the platform I wouldn't be too precise with it either. The update time-steps are so small that you can pretend the surface is linear for that segment. Again, use t instead of x.
///         - I imagine, this will still be a bit tricky. Since you don't know about curvature of the platform ahead, you don't know the x and y components of the distance you cover. I wouldn't mind if we just eyeball it and say: cover the current velocity in t direction, then look up the y and go to there. It'll probably still look natural ;)
///         - when leaving the platform, it's back to II/1
/// </summary>
public class TrajectoryCalculator : MonoBehaviour
{
    private float dx;
    private float dy;
    public float v0 = 100f;
    public float g = 9.81f;

    public BezierPlatform platform = null;
    
    // Start is called before the first frame update
    void Start()
    {
        this.platform = this.transform.gameObject.GetComponent<BezierPlatform>();
        Vector2 p0 = this.platform.GetBezierPath(0.9f);
        Vector2 p1 = this.platform.GetBezierPath(1f);

        this.dx = p1.x - p0.x;
        this.dy = p1.y - p0.y;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !Debugger.Render_Gizmos())
        {
            return;
        }
        Gizmos.color = new Color(0, 1, 0, 1);
        Vector2 p0 = platform.GetBezierPath(1.0f);
        Vector3 pos;
        float x = 0f;
        do {
            float y = this.CalcY(x);
            pos = transform.position + new Vector3(p0.x, p0.y, 1) + new Vector3(x, y, 1);
            Gizmos.DrawSphere(pos, 0.03f);
            x += 0.1f;
        } while (pos.y >= -10 && pos.y < 10);
    }

    private float CalcY(float x)
    {
        // TODO winkel nicht jedes mal bestimmen
        float a = CalcAngle();
        float cos2a = 2 * Mathf.Sin(a) * Mathf.Cos(a);
        float y = Mathf.Tan(a) * x - (g / (2*this.v0*cos2a)) * (x * x);
        return y;
    }

    private float calcVx()
    {
        float a = CalcAngle();
        return this.v0 * Mathf.Cos(a);
    }

    private float CalcAngle()
    {
        Vector2 m = new Vector2(this.dx, this.dy);
        Vector2 ground = new Vector2(this.dx, 0);
        float cosa = Vector2.Dot(m, ground)/(m.magnitude * ground.magnitude);
        return Mathf.Acos(cosa);
    }
    
}
