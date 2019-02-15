using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TriggerEntity : MonoBehaviour
{
  [SerializeField] internal float radius = 1.0f;

  public Vector3 position {
     get {
      return transform.position;
     }
  }

  public virtual void Awake() {
    TriggerEntityList.AddEntity(this);
  }

  public virtual void OnDestroy(){
    TriggerEntityList.RemoveEntity(this);
  }

  public TriggerEntity GetFirstTrigger(Vector3 position){
    return TriggerEntityList.FirstSearch(position);
  }

  public virtual void OnDrawGizmos(){
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, radius);
  }

  public bool Contains(Vector3 position){
    return Vector3.SqrMagnitude(this.position - position) < radius * radius;
  }

}

internal static class TriggerEntityList {

  private static List<TriggerEntity> _entities;
  public static List<TriggerEntity> Entities {
    get {
      return _entities;
    }
  }

  static TriggerEntityList(){
    _entities = new List<TriggerEntity>();
  }

  internal static void AddEntity(TriggerEntity entity) {
    _entities.Add(entity);
  }

  internal static void RemoveEntity(TriggerEntity entity) {
    if (!_entities.Remove(entity)){
      Debug.LogWarningFormat("{0} is being removed, but it doesn't exist.", entity.name);
    }
  }

  internal static bool Contains(Vector3 position){
    foreach(var entity in _entities){
      if (entity.Contains(position)) return true;
    }
    return false;
  }

  internal static TriggerEntity FirstSearch(Vector3 position){
    foreach (var entity in _entities) {
      var radius = entity.radius;
      if (Vector3.SqrMagnitude(position - entity.position) < radius * radius) {
        return entity;
      }
    }
    return null;
  }

  internal static TriggerEntity ClosestSearch(Vector3 position) {
    var r_sqrD = float.MaxValue;
    TriggerEntity r_entity = null;
    foreach (var entity in _entities) {
      var radius = entity.radius;
      var sqrD = Vector3.SqrMagnitude(position - entity.position);
      if (sqrD < radius * radius && sqrD < r_sqrD) {
        r_sqrD = sqrD;
        r_entity = entity;
      }
    }
    return r_entity;
  }

}
