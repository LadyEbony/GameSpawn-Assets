using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntitySpawner : MonoBehaviour
{
  private List<GameObject> entities;
  [SerializeField] private int entityAmount = 50;
  [SerializeField] private float radius = 10;

  [ContextMenu("Spawn")]
  private void Spawn(){
    if (entities == null) entities = new List<GameObject>();

    var parent = transform;
    while(entities.Count < entityAmount){
      entities.Add(new GameObject("Entity", typeof(TriggerEntity)));

      var t = entities[entities.Count - 1].transform;
      t.SetParent(parent);
      t.position = Random.insideUnitSphere * radius;
    }
    while (entities.Count > entityAmount){
      var lastIndex = entities.Count - 1;
      DestroyImmediate(entities[lastIndex]);
      entities.RemoveAt(lastIndex);
    }

  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radius);
  }

}
