using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class ButtonEntity : TriggerEntity {
    public override void OnEnable() {
      GlobalList<ButtonEntity>.Add(this);
    }

    public override void OnDisable() {
      GlobalList<ButtonEntity>.Remove(this);
    }
  }
}