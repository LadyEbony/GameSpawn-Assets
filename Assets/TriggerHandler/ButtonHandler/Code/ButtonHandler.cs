using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Type = System.Type;

namespace GameSpawn {

  public struct ButtonHit {
    public ButtonEntity entity { get; private set; }
    public float sqrDistance { get; private set; }

    public ButtonHit(ButtonEntity entity, float sqrDistance) {
      this.entity = entity;
      this.sqrDistance = sqrDistance;
    }
  }

  /// <summary>
  /// Global physics helper methods using <see cref="ButtonHandler"/>.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public static class ButtonHandler {

    private static List<IButtonUser> users;
    private static List<ButtonDirectory> directories;

    static ButtonHandler(){
      users = new List<IButtonUser>();
      directories = new List<ButtonDirectory>();
    }

    public static void Subscribe(IButtonUser user){
      users.Add(user);
    }

    public static void Subscribe(ButtonDirectory directory) {
      directories.Add(directory);
    }

    public static bool Unsubscribe(IButtonUser user){
      return users.Remove(user);
    }

    public static bool Unsubscribe(ButtonDirectory directory) {
      return directories.Remove(directory);
    }

    /// <summary>
    /// The unmodifyable list of registered users.
    /// </summary>
    public static IReadOnlyCollection<IButtonUser> Users{
      get {
        return users;
      }
    }

    /// <summary>
    /// The unmodifyable main list of <see cref="GlobalMaskList{ButtonEntity}"/>.
    /// </summary>
    public static IReadOnlyCollection<ButtonEntity> GetMainList() {
      return GlobalList<ButtonEntity>.GetList;
    }

    /// <summary>
    /// The unmodifyable masked list of <see cref="GlobalMaskList{ButtonEntity} at <paramref name="index"/>"/>.
    /// </summary>
    public static IReadOnlyCollection<ButtonEntity> GetMaskedList(int index) {
      return GlobalMaskList<ButtonEntity>.GetMaskedList(index);
    }

    public static void Submit(IButtonUser user){
      foreach(var direct in directories){
        direct.Submit(user);
      }
    }

  }
}