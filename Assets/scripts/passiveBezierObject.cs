using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gdg_playground.Assets.scripts;

/*
public class passiveBezierObject : MonoBehaviour
{
    public List<BezierPoint> controlPoints = new List<BezierPoint>
    {new BezierPoint(), new BezierPoint(), new BezierPoint(), new BezierPoint()};

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("What da hell");
    }

    // debug
    private void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(0, 1, 0, 1);
        for (float t = 0; t <= 1.0f; t += 0.05f)
        {
            Vector2 v2 = test(t, controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position);
            Gizmos.DrawSphere(transform.position + new Vector3(v2.x, v2.y, 1), 0.01f);
        }
        

        Gizmos.color = new Color(1, 0, 0, 1);
        for (float t = 0; t <= 1.0f; t += 0.05f)
        {
            Vector2 v2 = Compute_Point_At(t);
            Gizmos.DrawSphere(transform.position + new Vector3(v2.x, v2.y, 1), 0.01f);
        }

        Gizmos.color = new Color(0, 0, 1, 1);
        for (int i = 1; i < controlPoints.Count; i++)
        {
            Vector2 v2_1 = controlPoints[i - 1].position;
            Vector2 v2_2 = controlPoints[i].position;
            Gizmos.DrawLine(transform.position + new Vector3(v2_1.x, v2_1.y, 1), transform.position + new Vector3(v2_2.x, v2_2.y, 1));
        }
        for (int i = 0; i < controlPoints.Count; i++)
        {
            Vector2 v2 = controlPoints[i].position;
            Gizmos.DrawSphere(transform.position + new Vector3(v2.x, v2.y, 1), 0.02f);
        }
    }

    
    * TODO: Das Berechnen der x und y-components kann wahrscheinlich gecached werden, bzw. in unserem Anwendungsfall (Eisscholle) sollte das nur zur instanziierung laufen,
    * die Kontrollpunkte ändern sich ja nicht mehr. Nur der 2. Teil ist tatsächlich von time abhängig. Also irgendwie entkoppeln...
    * 
    * TODO: Gewichte der Kontrollpunkte müssen berücksichtigt werden.
    * 
    * TODO: Vllt. kriegen wir das auf n>2 Kontrollpunkte verallgemeinert.
    
    Vector2 Compute_Point_At(float time)
    {

        // only accept cubic curves
        if (controlPoints.Count < 4)
        {
            return new Vector2(0, 0);
        }

        // handle oob time
        if (time <= 0)
        {
            return controlPoints[0].position;
        }
        else if (time >= 1)
        {
            return controlPoints[controlPoints.Count - 1].position;
        }

        List<float> x_components = new List<float>();
        List<float> y_components = new List<float>();

        float previous_x = 0;
        float previous_y = 0;

        int segment_count = controlPoints.Count - 1;

        // AMONG US SUSSY?!?!?
        for (int i = 1; i < controlPoints.Count; i++)
        {
            int j = (i == segment_count)? 0 : i - 1;
            float x = (controlPoints[i].position.x - controlPoints[j].position.x);
            float y = (controlPoints[i].position.y - controlPoints[j].position.y);

            if (i < segment_count)
            {
                x *= segment_count;
                y *= segment_count;
            }

            x -= previous_x;
            y -= previous_y;

            x_components.Add(x);
            y_components.Add(y);

            previous_x += x;
            previous_y += y;
        }

        Debug.Log("Actual:");
        Debug.Log(x_components[0]);
        Debug.Log(y_components[0]);

        float res_x = controlPoints[0].position.x;
        float res_y = controlPoints[0].position.y;
        float factor_component = time;

        // STOP TALKING  ABOUT AMONG US!!!1!11!!
        for (int i = 0; i < x_components.Count; i++)
        {

            res_x += x_components[i] * factor_component;
            res_y += y_components[i] * factor_component;

            factor_component *= time;
        }
        Debug.Log("=============");

        return new Vector2(res_x, res_y);
    }

    private Vector2 test(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float cx = 3 * (p1.x - p0.x);
        float cy = 3 * (p1.y - p0.y);
        float bx = 3 * (p2.x - p1.x) - cx;
        float by = 3 * (p2.y - p1.y) - cy;
        float ax = p3.x - p0.x - cx - bx;
        float ay = p3.y - p0.y - cy - by;
        float Cube = t * t * t;
        float Square = t * t;

        Debug.Log("Expected:");
        Debug.Log(ax);
        Debug.Log(ay);

        float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.x;
        float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.y;

        return new Vector2(resX, resY);
    }
}
*/