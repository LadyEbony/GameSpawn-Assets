using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPoint : MonoBehaviour
{

  static OctTreeParent<TriggerEntity> tree;

  [ContextMenu("Test")]
  public void Test(){
    if (tree == null){
      var list = TriggerEntityList.Entities;

      var bounds = new Bounds();
      foreach (var item in list) {
        bounds.Encapsulate(item.position);
      }

      tree = new OctTreeParent<TriggerEntity>(bounds, TriggerEntityList.Entities);
    }
    var pos = transform.position;

    Debug.Log("Testing octree");
    tree.Contains(pos);

    Debug.Log("Testing brute force");
    TriggerEntityList.Contains(pos);

  }

}
