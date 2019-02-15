using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTreeParent<T> : Octree<T> where T: TriggerEntity{

  public OctTreeParent(Bounds bound, List<TriggerEntity> objList) : base(bound, objList){
    BuildTree();
  }

  Queue<Octree<T>> m_pendingInsertion = new Queue<Octree<T>>();
}

public class Octree<T> where T: TriggerEntity{

  protected Bounds m_bounds;
  protected List<TriggerEntity> m_list;

  public Octree(Bounds bound, List<TriggerEntity> objList){
    m_bounds = bound;
    m_list = objList;
    m_curLife = -1;
  }

  Octree<T> _parent;
  protected Octree<T>[] m_childNode = new Octree<T>[8];
  protected byte m_activeNodes = 0;

  protected const int MIN_SIZE = 1;

  protected int m_maxLifespan = 8;
  protected int m_curLife = -1;

  protected virtual void BuildTree(){
    if (m_list.Count <= 1) return;

    Vector3 dimension = m_bounds.size;

    if (dimension.x <= MIN_SIZE && dimension.y <= MIN_SIZE && dimension.z <= MIN_SIZE)
      return;

    Vector3 half = m_bounds.extents;
    Vector3 center = m_bounds.min + half / 2.0f;

    Bounds[] octant = new Bounds[8];
    octant[0] = new Bounds(center, half);
    octant[1] = new Bounds(center + new Vector3(half.x, 0.0f, 0.0f), half);
    octant[2] = new Bounds(center + new Vector3(0.0f, half.y, 0.0f), half);
    octant[3] = new Bounds(center + new Vector3(0.0f, 0.0f, half.z), half);
    octant[4] = new Bounds(center + new Vector3(half.x, half.y, 0.0f), half);
    octant[5] = new Bounds(center + new Vector3(half.x, 0.0f, half.z), half);
    octant[6] = new Bounds(center + new Vector3(0.0f, half.y, half.z), half);
    octant[7] = new Bounds(center + half, half);

    List<TriggerEntity>[] octList = new List<TriggerEntity>[8];
    for(var i = 0; i < 8; i++)
      octList[i] = new List<TriggerEntity>();

    Vector3 position;
    foreach(var obj in m_list){
      for(var i = 0; i < 8; i++){
        position = obj.position;
        if (octant[i].Contains(position)){
          octList[i].Add(obj);
          break;
        }
      }
    }

    for(var i = 0; i < 8; i++){
      if (octList[i].Count != 0){
        m_childNode[i] = new Octree<T>(octant[i], octList[i]);
        m_childNode[i]._parent = this;
        m_activeNodes |= (byte)(1 << i);
        m_childNode[i].BuildTree();
      }
    }
  }

  public void Render() {
    for (var i = 0; i < 8; i++){
      if ((m_activeNodes & (1 << i)) > 0){
        m_childNode[i].Render();
      }
    }
    Gizmos.color = Color.blue;
    Gizmos.DrawWireCube(m_bounds.center, m_bounds.size);
  }

  public bool IsLeaf{
    get {
      return m_activeNodes == 0;
    }
  }

  public virtual bool Contains(Vector3 position){
    if (IsLeaf) {
      foreach (var item in m_list)
        if (item.Contains(position)) return true;
      return false;
    } 

    for (var i = 0; i < 8; i++) {
      if ((m_activeNodes & (1 << i)) > 0) {
        var child = m_childNode[i];
        if (child.m_bounds.Contains(position) && child.Contains(position))
          return true;
      }
    }
    return false;
  }

}
