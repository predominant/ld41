using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LD41.Scripts.Extensions
{
    public static class Vector2Extensions
    {
        public static float Slope(this Vector2 a, Vector2 b)
        {
            var c = (b - a).normalized;
            return Mathf.Abs(c.y / c.x);
        }
    }
}