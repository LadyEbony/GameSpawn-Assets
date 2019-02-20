using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class SphereTriggerBounds: TriggerBounds {

    public Vector3 center;
    public float radius;

    private float scaledRadius{
      get {
        var s = transform.lossyScale;
        return radius * Mathf.Max(s.x, s.y, s.z);
      }
    }

    private float scaledSqrRadius {
      get {
        var sRad = scaledRadius;
        return sRad * sRad;
      }
    }

    private Vector3 offsetCenter {
      get {
        return transformPosition + center;
      }
    }

    public override bool Contains(Vector3 pos) {
      return Vector3.SqrMagnitude(pos - offsetCenter) < scaledSqrRadius;
    }

    public override void OnDrawGizmosSelected() {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(offsetCenter, scaledRadius);
    }

  }
}