using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Finis {
    public static class Utils {
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) {
            return new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
        }

        public static float EaseOutCubic(float x) {
            if(x > 1) {
                return 1;
            }
            return 1 - Mathf.Pow(1 - x, 3);
        }

        public static float EaseInBack(float x) {
            if(x > 1) {
                return 1;
            }
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;
            return c3 * x * x * x - c1 * x * x;
        }
    }
}
