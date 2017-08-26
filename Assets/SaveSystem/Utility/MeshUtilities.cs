using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public static class MeshUtilities {

    public static Bounds GetRealBounds(this Transform tr) {
        //get max bounds
        Bounds real = new Bounds();

        MeshRenderer[] renders = tr.GetComponentsInChildren<MeshRenderer>();

        float offset = 100000f;
        Vector3 pos = tr.position;

        real.min = new Vector3(pos.x + offset, pos.y + offset, pos.z + offset);
                                                   
        real.max = new Vector3(pos.x - offset, pos.y - offset, pos.z - offset);




        foreach (var mr in renders) {

            Vector3 max = mr.bounds.max;
            Vector3 min = mr.bounds.min;

            float minx = Mathf.Min(real.min.x, min.x);
            float miny = Mathf.Min(real.min.y, min.y);
            float minz = Mathf.Min(real.min.z, min.z);

            real.min = new Vector3(minx, miny, minz);

            float maxx = Mathf.Max(real.max.x, max.x);
            float maxy = Mathf.Max(real.max.y, max.y);
            float maxz = Mathf.Max(real.max.z, max.z);

            real.max = new Vector3(maxx, maxy, maxz);
        }

        return real;
    }

    /// <summary>
    /// Returns maximum size of the bounds on biggest axis
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static float BoundsSizeMax(this Bounds bounds) {
        return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
    }

}
