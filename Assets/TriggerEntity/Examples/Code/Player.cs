using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {
  public class Player : MonoBehaviour {

    private Material mat;

    public float speed = 1;

    private void Awake() {
      mat = GetComponent<MeshRenderer>().material;
    }

    void Update() {
      HandleInput();
      HandleButtons();
    }

    void HandleInput(){
      var h = Input.GetAxisRaw("Horizontal");
      var v = Input.GetAxisRaw("Vertical");

      var delta = new Vector3(h, v, 0).normalized * speed * Time.deltaTime;

      transform.position += delta;
    }

    void HandleButtons(){
      var ent = TriggerHandler.GetOverlapEntity<ButtonTrigger>(transform.position);

      mat.SetColor("_Color", ent ? Color.green : Color.white);
    }

  }
}
