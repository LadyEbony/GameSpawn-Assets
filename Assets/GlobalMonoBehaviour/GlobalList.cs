﻿using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using Type = System.Type;

namespace GameSpawn {

  /// <summary>
  /// Generic static list of <typeparamref name="T"/>. Update the contents on your accord.
  /// </summary>
  /// <remarks>
  /// GlobalList<typeparamref name="T"/> provides ease of use when you need to track every to most
  /// references of the same type.
  /// 
  /// You must specify the exact Type. As such, this is not friendly towards inheritance classes.
  /// </remarks>
  /// <typeparam name="T"></typeparam>
  public static class GlobalList<T> {

    private static List<T> list;

    static GlobalList() {
      list = new List<T>();
    }

    public static IReadOnlyCollection<T> GetList {
      get {
        return list.AsReadOnly();
      }
    }

    public static void Add(T item){
      list.Add(item);
    }

    public static bool Remove(T item) {
      return list.Remove(item);
    }
  }

  /// <summary>
  /// Wrapper class that adds and removes items from <see cref="GlobalList{T}"/> 
  /// using <see cref="object.GetType()"/> as the type. 
  /// Less effecient than using <see cref="GlobalList{T}"/> directly.
  /// </summary>
  /// <remarks>
  /// Uses cached reflection to get the relevent <see cref="GlobalList{T}.Add(T)"/> 
  /// and <see cref="GlobalList{T}.Remove(T)"/> methods.
  /// 
  /// As such, this wrapper class is much more friendly to inheritance.
  /// However, GlobalList can cause long pauses at the beginning if you use these methods extensively.
  /// 
  /// </remarks>
  public static class GlobalList{
    
    private struct MethodInfoStruct{
      public MethodInfo Add;
      public MethodInfo Remove;
    }

    private static Dictionary<Type, MethodInfoStruct> methodInfo;

    static GlobalList(){
      methodInfo = new Dictionary<Type, MethodInfoStruct>();
    }

    private static MethodInfoStruct GetMethodInfo(Type type){
      MethodInfoStruct info;

      if (!methodInfo.TryGetValue(type, out info)){
        var genericType = typeof(GlobalList<>).MakeGenericType(type);
        var add = genericType.GetMethod("Add");
        var remove = genericType.GetMethod("Remove");

        info = new MethodInfoStruct() { Add = add, Remove = remove };
      }

      return info;
    }

    public static void AddReference(object item){
      GetMethodInfo(item.GetType()).Add.Invoke(null, new object[] { item });
    }

    public static void RemoveReference(object item) {
      GetMethodInfo(item.GetType()).Remove.Invoke(null, new object[] { item });
    }

  }

}
