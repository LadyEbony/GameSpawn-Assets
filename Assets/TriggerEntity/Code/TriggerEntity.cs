using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class TriggerEntity : MonoBehaviour {

    private TriggerBounds bound;

    private void Awake() {
      TriggerHandler.AddEntity(this);
      bound = GetComponent<TriggerBounds>();
    }

    private void OnDestroy() {
      TriggerHandler.RemoveEntity(this);
    }

    public bool Contains(Vector3 pos){
      if (bound == null) return false;
      return bound.Contains(pos);
    }

  }
}