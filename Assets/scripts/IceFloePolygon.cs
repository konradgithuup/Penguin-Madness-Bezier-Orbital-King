using gdg_playground.Assets.scripts;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    public class IceFloePolygon : MaskableGraphic
    {
        public BezierPath bezierPath;
        public int numSamplePoints = 20;

        private float baseWidth = 35;
        private float baseHeight = 35;

        protected override void Start()
        {
            bezierPath = new BezierPath(Vector2.zero);
        }

        public override Texture mainTexture
        {
            get
            {
                return s_WhiteTexture;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            // Calc width and height:
            float width = baseWidth;
            float height = baseHeight;
            if (bezierPath.points != null && bezierPath.points.Count >= 2)
            {
                width *= bezierPath.points[bezierPath.point_count - 1].x - bezierPath.points[0].x;
                height *= (Mathf.Max(bezierPath.points[0].y, bezierPath.points[bezierPath.point_count - 1].y) + 0.5f);
            }

            // Render bezierPath:
            vh.Clear();
            Vector2 offset = new Vector2(-(float)width / 2, -(float)rectTransform.rect.height / 2 + 5.0f);
            Vector2[] pos = new Vector2[4];
            for (int i = 0; i < numSamplePoints; i++)
            {
                // Calc positions:
                float x0 = (float)i / numSamplePoints;
                float x1 = (float)(i + 1) / numSamplePoints;
                pos[0] = new Vector2(x0 * width, 0.0f);
                pos[1] = new Vector2(x0 * width, (bezierPath.Compute_Segment_At(0, x0).y + 0.5f) * height);
                pos[2] = new Vector2(x1 * width, (bezierPath.Compute_Segment_At(0, x1).y + 0.5f) * height);
                pos[3] = new Vector2(x1 * width, 0.0f);

                // Add vertices to mesh:
                UIVertex[] verts = new UIVertex[4];
                for (int j = 0; j < verts.Length; j++)
                {
                    UIVertex vert = UIVertex.simpleVert;
                    vert.position = pos[j] + offset;
                    verts[j] = vert;
                }
                vh.AddUIVertexQuad(verts);
            }
        }

        public void Redraw()
        {
            SetVerticesDirty();
        }
    }
}
