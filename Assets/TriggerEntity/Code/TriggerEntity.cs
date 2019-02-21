using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class TriggerEntity : MonoBehaviour {

    private TriggerBounds bound;

    public virtual void Awake(){
      GlobalList.AddReference(this);
      bound = GetComponent<TriggerBounds>();
    }

    public virtual void OnDestroy(){
      GlobalList.RemoveReference(this);
    }

    public bool Contains(Vector3 pos){
      if (bound == null) return false;
      return bound.Contains(pos);
    }

  }
}