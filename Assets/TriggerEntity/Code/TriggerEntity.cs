using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class TriggerEntity : GlobalMonoBehaviour {

    private TriggerBounds bound;

    public override void Awake(){
      base.Awake();
      bound = GetComponent<TriggerBounds>();
    }

    public bool Contains(Vector3 pos){
      if (bound == null) return false;
      return bound.Contains(pos);
    }

  }
}