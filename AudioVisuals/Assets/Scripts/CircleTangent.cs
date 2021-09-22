using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTangent : MonoBehaviour
{
    /**/

    /*Calculate the tangent of outer circle by an angle*/
    protected Vector3 GetRotatedTangent(float degree, float outRadius)
    {
        //Consider double for precision and System.Math.Sin
        float angle = degree * Mathf.Deg2Rad;
        float newX = outRadius * Mathf.Sin(angle);
        float newZ = outRadius * Mathf.Cos(angle);

        return new Vector3(newX, 0, newZ);
    }

    /*Calculate tangent circle position and radius*/
    protected Vector4 GetTangentCircle(Vector4 outer, Vector4 inner, float degree)
    {
        // tangent point of outer circle
        Vector3 tangentPoint = GetRotatedTangent(degree, outer.w);

        // calculate distances between pints
        float outerInner = Mathf.Max(Vector3.Distance(new Vector3(outer.x, outer.y, outer.z), new Vector3(inner.x, inner.y, inner.z)), 0.1f); // prevent division by 0
        float outerTan = Vector3.Distance(new Vector3(outer.x, outer.y, outer.z), tangentPoint);
        float innerTan = Vector3.Distance(new Vector3(inner.x, inner.y, inner.z), tangentPoint);

        //calculate angle & radius
        float angleCAB = ((outerInner*outerInner) + (outerTan * outerTan) - (innerTan*innerTan)) / ( 2 * outerInner * outerTan);
        float tanRadius = ((outer.w*outer.w) - (inner.w*inner.w) + (outerInner*outerInner) - (2*outer.w*outerInner*angleCAB)) / (2 * (outer.w + inner.w - outerInner*angleCAB));

        tangentPoint = GetRotatedTangent(degree, outer.w - tanRadius); 

        return new Vector4(tangentPoint.x, tangentPoint.y, tangentPoint.z, tanRadius);
    }
}
