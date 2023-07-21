using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

// sebastian lague got me acting unwise https://youtu.be/n_RHttAaRCk
namespace gdg_playground.Assets.scripts
{
    [System.Serializable]
    public class BezierPath
    {
        [HideInInspector]
        public List<Vector2> points;

        /// <summary>
        /// Getter for accessing point at specified position
        /// </summary>
        public Vector2 this[int i]
        {
            get
            {
                return points[i];
            }
        }

        /// <summary>
        /// Returns total number of points
        /// </summary>
        public int point_count
        {
            get
            {
                return points.Count;
            }
        }

        /// <summary>
        /// Returns total number of segments
        /// </summary>
        public int segment_count
        {
            get
            {
                return (points.Count - 4) / 3 + 1;
            }
        }

        /// <summary>
        /// Initializes BezierPath by adding four points making up first cubic curve
        /// </summary>
        public BezierPath(Vector2 center)
        {
            points = new List<Vector2> {
                center + Vector2.left,
                center + (Vector2.left + Vector2.up)*0.5f,
                center + (Vector2.right + Vector2.down)*0.5f,
                center + Vector2.right
            };
        }

        /// <summary>
        /// Adds new segment (i.e. cubic curve) ending in given anchor position
        /// </summary>
        public void Add(Vector2 anchor)
        {
            Vector2 trailing_control = points[points.Count - 1];
            Vector2 trailing_anchor = points[points.Count - 2];

            points.Add(trailing_anchor + (trailing_anchor - trailing_control));
            points.Add((trailing_anchor + anchor) * 0.5f);
            points.Add(anchor);
        }

        public int Get_Segment_Index(int x)
        {
            for (int i = 0; i < point_count; i += 3)
            {
                if (points[i].x < x)
                {
                    return i / 3;
                }
            }

            return point_count - 1;
        }

        /// <summary>
        /// Returns the four points related to specified segment
        /// </summary>
        public Vector2[] Get_Points_For_Segment(int i)
        {
            return new Vector2[] {
                points[i*3],
                points[i*3 + 1],
                points[i*3 + 2],
                points[i*3 + 3]
            };
        }

        /// <summary>
        /// Move specified point to specified position (without moving corresponding anchor points)
        /// </summary>
        public void Move_Point(int i, Vector2 new_pos)
        {
            points[i] = new_pos;
        }

        // good enough for now
        public Vector2 Compute_Segment_At_A(int i, float t)
        {
            Vector2[] segment = Get_Points_For_Segment(i);

            float cx = 3 * (segment[1].x - segment[0].x);
            float cy = 3 * (segment[1].y - segment[0].y);
            float bx = 3 * (segment[2].x - segment[1].x) - cx;
            float by = 3 * (segment[2].y - segment[1].y) - cy;
            float ax = segment[3].x - segment[0].x - cx - bx;
            float ay = segment[3].y - segment[0].y - cy - by;
            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + segment[0].x;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + segment[0].y;

            return new Vector2(resX, resY);
        }

        public Vector2 Compute_Segment_At(int i, float t) {
            Vector2[] segment = Get_Points_For_Segment(i);

            return Cerp(segment[0], segment[1], segment[2], segment[3], t);
        }

        private Vector2 Lerp(Vector2 a, Vector2 b, float t) {
            return a + (b - a) * t;
        }

        private Vector2 Querp(Vector2 a, Vector2 b, Vector2 c, float t) {
            Vector2 p0 = Lerp(a, b, t);
            Vector2 p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }

        private Vector2 Cerp(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t) {
            Vector2 p0 = Querp(a, b, c, t);
            Vector2 p1 = Querp(b, c, d, t);
            return Lerp(p0, p1, t);
        }
    }
}