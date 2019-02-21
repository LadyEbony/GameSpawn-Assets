using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

namespace GameSpawn {
  public static class TriggerHandler {


    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity may not be closest entity.
    /// O(n).
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T GetOverlapEntity<T>(Vector3 pos) where T : TriggerEntity{
      foreach (var entity in GlobalList<T>.GetList){
        if (entity.Contains(pos))
          return entity;
      }
      return null;
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Modifies the provided <paramref name="entities"/> array and returns the number of entities found.
    /// O(n).
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static int GetOverlapEntities<T>(Vector3 pos, T[] entities) where T : TriggerEntity{
      var i = 0;
      foreach(var entity in GlobalList<T>.GetList) {
        if (entity.Contains(pos)) {
          entities[i] = entity;
          i++;
        }
      }
      return i;
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Less effecient, but easier to use than <see cref="GetOverlapEntities(Type, Vector3, TriggerEntity[])"/>.
    /// O(n).
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T[] GetOverlapEntities<T>(Vector3 pos) where T :TriggerEntity{
      var list = new List<T>();
      foreach(var entity in GlobalList<T>.GetList) {
        if (entity.Contains(pos)){
          list.Add(entity);
        }
      }
      return list.ToArray();
    }
  }
}