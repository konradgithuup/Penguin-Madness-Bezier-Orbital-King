using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

namespace gdg_playground.Assets.scripts
{
    [System.Serializable]
   public class BezierPoint
    {
        public Vector2 position = new Vector2(0, 0);
        public double weight = 1.0;

        public BezierPoint() {
            // pass
        }
    }
}