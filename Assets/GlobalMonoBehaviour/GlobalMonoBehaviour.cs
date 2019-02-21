using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

namespace GameSpawn {
  public class GlobalMonoBehaviour : MonoBehaviour {

    public virtual void Awake() {
      GlobalMBList.AddEntity(this);
    }

    public virtual void OnDestroy() {
      GlobalMBList.RemoveEntity(this);
    }

    
  }
}