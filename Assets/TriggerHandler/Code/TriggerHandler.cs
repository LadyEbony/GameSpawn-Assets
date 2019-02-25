using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

namespace GameSpawn {

  /// <summary>
  /// Global physics helper methods using <see cref="TriggerEntity"/>.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public static class TriggerHandler<T> where T : TriggerEntity {

    /// <summary>
    /// The unmodifyable list of <see cref="GlobalList{T}"/>.
    /// </summary>
    public static IReadOnlyCollection<T> GetList {
      get {
        return GlobalList<T>.GetList;
      }
    }

    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity may not be closest entity.
    /// O(n).
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T GetOverlapEntity(Vector3 pos) {
      foreach (var entity in GetList){
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
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <param name="entities"></param>
    /// <returns></returns>
    public static int GetOverlapEntities(Vector3 pos, T[] entities) {
      var i = 0;
      foreach(var entity in GetList) {
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
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T[] GetOverlapEntities(Vector3 pos){
      var list = new List<T>();
      foreach(var entity in GetList) {
        if (entity.Contains(pos)){
          list.Add(entity);
        }
      }
      return list.ToArray();
    }
  }
}