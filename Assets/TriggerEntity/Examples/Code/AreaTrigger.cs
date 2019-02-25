using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {

  public class AreaTrigger : TriggerEntity {

    private Transform meshTransform;

    public override void Awake() {
      base.Awake();

      meshTransform = GetComponentInChildren<MeshRenderer>().transform;
    }

    public void OnTrigger() {
      // Simulates a shake effect.

      // Only the mesh renderer is moved around and not AreaTrigger.
      // Otherwise, the shaking can cause the trigger to move outside the Player's position.

      meshTransform.localPosition = Random.insideUnitSphere * 0.0625f;
    }

  }
}