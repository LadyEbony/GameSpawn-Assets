using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {
  public class SimpleButtonResponder : ButtonResponder {
    public override void StayTrigger(ButtonEntity entity) {
      entity.SpriteRenderer.color = new Color(1, 1, 1, 1f) * (entity.Submit ? Color.red : Color.white);
    }

    public override void OffTrigger(ButtonEntity entity) {
      entity.SpriteRenderer.color = new Color(1, 1, 1, 0.5f) * (entity.Submit ? Color.red : Color.white);
    }

  }
}