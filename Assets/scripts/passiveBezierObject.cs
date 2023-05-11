using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gdg_playground.Assets.scripts;

public class passiveBezierObject : MonoBehaviour
{
    public List<BezierPoint> bezierPoints = new List<BezierPoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 Compute_Point_At(float time) {

        if (bezierPoints.Count < 2)
        {
            return new Vector2(0, 0); 
        }

        List<float> x_components = new List<float>();
        List<float> y_components = new List<float>();

        float previous_x = 0;
        float previous_y = 0;

        // AMONG US SUSSY?!?!?
        for (int i = 1; i < bezierPoints.Count; i++) {

            float x = bezierPoints[i].position.x - bezierPoints[i-1].position.x - previous_x;
            float y = bezierPoints[i].position.y - bezierPoints[i-1].position.y - previous_y;

            x_components.Add(x);
            y_components.Add(y);

            previous_x += x;
            previous_y += y;
        }

        float res_x = bezierPoints[0].position.x;
        float res_y = bezierPoints[0].position.y;
        float factor_component = time;

        // STOP TALKING  ABOUT AMONG US!!!1!11!!
        for (int i = 0; i < x_components.Count; i++) {

            res_x += x_components[i] * factor_component;
            res_y += y_components[i] * factor_component;

            factor_component *= time;
        }

        return new Vector2(res_x, res_y);
    }
}