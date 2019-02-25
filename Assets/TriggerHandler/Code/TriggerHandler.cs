using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

namespace GameSpawn {

  public struct TriggerHit {
    public TriggerEntity entity { get; private set; }
    public float sqrDistance { get; private set; }

    public TriggerHit(TriggerEntity entity, float sqrDistance) {
      this.entity = entity;
      this.sqrDistance = sqrDistance;
    }
  }

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
    /// <param name="pos"></param>
    /// <param name="compareList"></param>
    /// <returns></returns>
    public static T GetOverlapEntity(Vector3 pos, IEnumerable<T> compareList) {
      foreach (var entity in compareList) {
        if (entity.Contains(pos))
          return entity;
      }
      return null;
    }

    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity may not be closest entity.
    /// O(n).
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T GetOverlapEntity(Vector3 pos) {
      return GetOverlapEntity(pos, GetList);
    }

    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity will be closest entity. Slower than <see cref="GetOverlapButton(Vector3)"/>.
    /// O(n).
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="compareList"></param>
    /// <returns></returns>
    public static T GetOverlapEntityClosest(Vector3 pos, IEnumerable<T> compareList) {
      float sqrDis = float.MaxValue;
      float temp;
      T item = null;
      foreach (var entity in compareList) {
        if (entity.Contains(pos, out temp)) {
          if (temp < sqrDis) {
            item = entity;
            sqrDis = temp;
          }
        }
      }
      return item;
    }


    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity will be closest entity. Slower than <see cref="GetOverlapButton(Vector3)"/>.
    /// O(n).
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T GetOverlapEntityClosest(Vector3 pos) {
      return GetOverlapEntityClosest(pos, GetList);
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Modifies the provided <paramref name="results"/> array and returns the number of entities found.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <param name="results"></param>
    /// <param name="compareList"></param>
    /// <returns></returns>
    public static int GetOverlapEntities(Vector3 pos, T[] results, IEnumerable<T> compareList) {
      var i = 0;
      foreach(var entity in compareList) {
        if (entity.Contains(pos)) {
          results[i] = entity;
          i++;
        }
      }
      return i;
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Modifies the provided <paramref name="results"/> array and returns the number of entities found.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <param name="results"></param>
    /// <returns></returns>
    public static int GetOverlapEntities(Vector3 pos, T[] results) {
      return GetOverlapEntities(pos, results, GetList);
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Less effecient, but easier to use than <see cref="GetOverlapEntities(Type, Vector3, TriggerEntity[])"/>.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T[] GetOverlapEntities(Vector3 pos, IEnumerable<T> compareList){
      var list = new List<T>();
      foreach(var entity in compareList) {
        if (entity.Contains(pos)){
          list.Add(entity);
        }
      }
      return list.ToArray();
    }

    /// <summary>
    /// Returns all entities that contains <paramref name="pos"/>.
    /// Less effecient, but easier to use than <see cref="GetOverlapEntities(Type, Vector3, TriggerEntity[])"/>.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static T[] GetOverlapEntities(Vector3 pos) {
      return GetOverlapEntities(pos, GetList);
    }

    /// <summary>
    /// Returns entities that are contained <paramref name="pos"/> and not.
    /// Modifies the provided arrays and returns the number of entities found.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <param name="containResults"></param>
    /// <param name="outsideResults"></param>
    /// <param name="compareList"></param>
    /// <param name="containLength"></param>
    /// <param name="outsideLength"></param>
    /// <returns></returns>
    public static void GetOverlapEntities(Vector3 pos, T[] containResults, T[] outsideResults, IEnumerable<T> compareList, out int containLength, out int outsideLength) {
      containLength = 0;
      outsideLength = 0;
      foreach (var entity in compareList) {
        if (entity.Contains(pos)) {
          containResults[containLength] = entity;
          containLength++;
        } else {
          outsideResults[outsideLength] = entity;
          outsideLength++;
        }
      }
      return;
    }

    /// <summary>
    /// Returns entities that are contained <paramref name="pos"/> and not.
    /// Modifies the provided arrays and returns the number of entities found.
    /// O(n).
    /// </summary>
    /// <remarks>
    /// The array is not sorted.
    /// </remarks>
    /// <param name="pos"></param>
    /// <param name="containResults"></param>
    /// <param name="outsideResults"></param>
    /// <param name="containLength"></param>
    /// <param name="outsideLength"></param>
    /// <returns></returns>
    public static void GetOverlapEntities(Vector3 pos, T[] containResults, T[] outsideResults, out int containLength, out int outsideLength) {
      GetOverlapEntities(pos, containResults, outsideResults, GetList, out containLength, out outsideLength);
    }
  }
}