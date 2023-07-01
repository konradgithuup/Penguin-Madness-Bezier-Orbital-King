using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float CalcAngle()
    {
        Vector2 m = new Vector2(this.dx, this.dy);
        Vector2 ground = new Vector2(this.dx, 0);
        float cosa = Vector2.Dot(m, ground)/(m.magnitude * ground.magnitude);
        return Mathf.Acos(cosa);
    }
}
