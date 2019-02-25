using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class TriggerEntity : MonoBehaviour {

    private TriggerBounds bound;

    public virtual void Awake(){
      bound = GetComponent<TriggerBounds>();
    }

    public virtual void OnEnable(){
      // While ineffecient, guarantees that you don't forget calling it.
      // Override this method if you don't want this functionality.
      GlobalList.AddReference(this);
    }

    public virtual void OnDisable(){
      GlobalList.RemoveReference(this);
    }

    /// <summary>
    /// Is point contained in the bounds?
    /// Returns false if no TriggerBounds component is included.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool Contains(Vector3 point){
      if (bound == null) return false;
      return bound.Contains(point);
    }

  }
}