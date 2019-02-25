using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public interface IButtonUser {
    Vector3 GetButtonPosition();
    void OnStayButtons(ButtonEntity[] buttons, int size);
  }
}