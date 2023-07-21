using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gdg_playground.Assets.scripts;

public class BezierPlatform : SecondOrderDynamics
{
    public BezierPath surface = null;

    private GameObject platform = null;

    private SpriteRenderer platform_renderer = null;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        this.platform = this.transform.gameObject;
        this.platform_renderer = platform.GetComponent<SpriteRenderer>();
        this.surface = new BezierPath(platform.transform.position); 

        this.Remesh();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public Vector2 GetBezierPath(float t)
    {
        return this.surface.Compute_Segment_At(0, t);
    }

    public void Remesh() {
        
        // regenerate platform bounds
        
        platform.transform.localScale = Vector3.one;
        Vector3 globalScale = new Vector3(3 + Random.Range(-1f, 1f), 1 + Random.Range(0f, 0.2f), 1);
        platform.transform.localScale = new Vector3 (globalScale.x/platform.transform.lossyScale.x, globalScale.y/platform.transform.lossyScale.y, globalScale.z/platform.transform.lossyScale.z);
        
        // Debug.Log(platform.transform.localScale);

        // regenerate surface
        Vector3 upper_left = platform.transform.TransformPoint(-0.5f,0.5f,0) - transform.position;
        Vector3 upper_right = platform.transform.TransformPoint(0.5f,0.5f,0) - transform.position;

        for (int i = 0; i < this.surface.point_count; i++) {
            bool is_anchor = (i == 0 || i == this.surface.point_count - 1);
            Vector2 interpolated_point = this.Randomized_Lerp(upper_left, upper_right, ((float)i)/(this.surface.point_count -1), is_anchor);
            this.surface.Move_Point(i, interpolated_point);    
        }

        // update shader
        float ratio = this.platform.transform.lossyScale.x/this.platform.transform.lossyScale.y;
        Vector2[] segment = this.surface.Get_Points_For_Segment(0);
        MaterialPropertyBlock props = new MaterialPropertyBlock();

        props.SetFloat("_Anchor1", segment[0].y/this.platform.transform.lossyScale.y + 0.5f);
        props.SetFloat("_Control1", segment[1].y/this.platform.transform.lossyScale.y + 0.5f);
        props.SetFloat("_Control2", segment[2].y/this.platform.transform.lossyScale.y + 0.5f);
        props.SetFloat("_Anchor2", segment[3].y/this.platform.transform.lossyScale.y + 0.5f);

        Material mat = this.platform_renderer.material;
        this.platform_renderer.SetPropertyBlock(props);
    }

    // debug
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !Debugger.Render_Gizmos())
        {
            return;
        }
        Gizmos.color = new Color(1, 0, 0, 1);
        for (float t = 0; t <= 1.0f; t += 0.05f)
        {
            Vector2 v2 = surface.Compute_Segment_At(0, t);
            Gizmos.DrawSphere(transform.position + new Vector3(v2.x, v2.y, 1), 0.01f);
        }

        Gizmos.color = new Color(0, 0, 1, 1);
        Vector2[] segment = surface.Get_Points_For_Segment(0);

        for (int i = 1; i < segment.Length; i++)
        {
            Vector2 v2_1 = segment[i - 1];
            Vector2 v2_2 = segment[i];
            Gizmos.DrawLine(transform.position + new Vector3(v2_1.x, v2_1.y, 1), transform.position + new Vector3(v2_2.x, v2_2.y, 1));
        }
        for (int i = 0; i < segment.Length; i++)
        {
            Vector2 v2 = segment[i];
            Gizmos.DrawSphere(transform.position + new Vector3(v2.x, v2.y, 1), 0.02f);
        }
    }

    private Vector2 Randomized_Lerp(Vector3 start, Vector3 end, float t, bool is_anchor) {
        return (is_anchor)? Randomized_Lerp(start, end, t, 0, 0.3f) : Randomized_Lerp(start, end, t, 0.4f, 1.0f);
    }

    private Vector2 Randomized_Lerp(Vector3 start, Vector3 end, float t, float upper_bound, float lower_bound) {
        Vector3 res = (end-start)*t + start;

        float randomized_y = res.y - res.y * 2*Random.Range(lower_bound, upper_bound);

        return new Vector2(res.x, randomized_y);
    }
}
