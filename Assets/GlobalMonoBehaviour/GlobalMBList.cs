using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

using Type = System.Type;

namespace GameSpawn {
  public static class GlobalMBList {
    private static Dictionary<Type, object> entities;

    private struct MethodInfoStruct{
      public MethodInfo Add;
      public MethodInfo Remove;
    }

    private static Dictionary<Type, MethodInfoStruct> entityMethodInfo;

    static GlobalMBList() {
      entities = new Dictionary<Type, object>();
      entityMethodInfo = new Dictionary<Type, MethodInfoStruct>();
    }

    /// <summary>
    /// Returns all items of the provided type <typeparamref name="G"/>.
    /// Returns an empty list if no entities exist.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static List<G> GetEntities<G>() where G : GlobalMonoBehaviour {
      object list;
      Type type = typeof(G);

      if (!entities.TryGetValue(type, out list)) {
        list = new List<G>();
        entities.Add(type, list);
      }
      return (List<G>)list;
    }

    private static MethodInfoStruct GetEntityMethodInfo(Type type){
      MethodInfoStruct info;

      if (!entityMethodInfo.TryGetValue(type, out info)) {
        var globalType = typeof(GlobalMBList);
        var types = new Type[] { type };

        var add = globalType.GetMethod("AddEntityGeneric").MakeGenericMethod(types);
        var remove = globalType.GetMethod("RemoveEntityGeneric").MakeGenericMethod(types);

        info = new MethodInfoStruct(){ Add = add, Remove = remove };
        entityMethodInfo.Add(type, info);
      }
      return info;
    }

    /// <summary>
    /// Add <paramref name="entity"/> to its corresponding list according to <typeparamref name="G"/>.
    /// </summary>
    /// <typeparam name="G"></typeparam>
    /// <param name="entity"></param>
    public static void AddEntityGeneric<G>(G entity) where G: GlobalMonoBehaviour{
      GetEntities<G>().Add(entity);
    }

    /// <summary>
    /// Add <paramref name="entity"/> to its corresponding list according to <see cref="object.GetType()"/>.
    /// Works for derived classes. Use <see cref="AddEntity{G}(G)"/> for increased effeciency.
    /// </summary>
    /// <param name="entity"></param>
    public static void AddEntity(GlobalMonoBehaviour entity) {
      GetEntityMethodInfo(entity.GetType()).Add.Invoke(null, new object[] { entity });
    }


    public static bool RemoveEntityGeneric<G>(G entity) where G : GlobalMonoBehaviour {
      return GetEntities<G>().Remove(entity);
    }

    public static bool RemoveEntity(GlobalMonoBehaviour entity) {
      return (bool)GetEntityMethodInfo(entity.GetType()).Remove.Invoke(null, new object[] { entity }); ;
    }

  }
}
