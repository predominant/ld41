using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LD41.Scripts.Extensions
{
    public static class CameraExtensions
    {
        public static bool IsVisibleFrom(this Camera cam, Renderer r)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(cam);
            return GeometryUtility.TestPlanesAABB(planes, r.bounds);
        }
    }
}