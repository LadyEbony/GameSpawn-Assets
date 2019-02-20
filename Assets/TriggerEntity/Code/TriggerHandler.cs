using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

namespace GameSpawn {
  public static class TriggerHandler {

    private static Dictionary<Type, List<TriggerEntity>> entities;

    static TriggerHandler(){
      entities = new Dictionary<Type, List<TriggerEntity>>();
    }

    /// <summary>
    /// Returns all entities of the provided <paramref name="type"/>.
    /// Returns an empty list if no entities exist.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<TriggerEntity> GetEntities(Type type){
      if (!type.IsSubclassOf(typeof(TriggerEntity))){
        throw new System.Exception(string.Format("Type '{0}' is not a subclass of TriggerEntity", type.Name));
      }

      List<TriggerEntity> list;
      if (!entities.TryGetValue(type, out list)){
        list = new List<TriggerEntity>();
        entities.Add(type, list);
      }
      return list;
    }

    public static void AddEntity(TriggerEntity entity){
      GetEntities(entity.GetType()).Add(entity);
    }

    public static bool RemoveEntity(TriggerEntity entity){
      return GetEntities(entity.GetType()).Remove(entity);
    }

    /// <summary>
    /// Returns the first entity that contains <paramref name="pos"/>.
    /// The first entity may not be closest entity.
    /// O(n).
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static TriggerEntity GetOverlapEntity(Type type, Vector3 pos){
      foreach(var entity in GetEntities(type)){
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
    public static int GetOverlapEntities(Type type, Vector3 pos, TriggerEntity[] entities){
      var i = 0;
      foreach(var entity in GetEntities(type)){
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
    public static TriggerEntity[] GetOverlapEntities(Type type, Vector3 pos){
      var list = new List<TriggerEntity>();
      foreach(var entity in GetEntities(type)){
        if (entity.Contains(pos)){
          list.Add(entity);
        }
      }
      return list.ToArray();
    }
  }
}