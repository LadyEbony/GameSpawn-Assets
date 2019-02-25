using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {
  public class Player : MonoBehaviour, IButtonUser {

    public float speed = 1;

    private void OnEnable() {
      // The GlobalList is not restricted to TriggerEntity.
      // Any class can use it.
      GlobalList<Player>.Add(this);
      ButtonHandler.Subscribe(this);
    }

    private void OnDisable() {
      GlobalList<Player>.Remove(this);
      ButtonHandler.Unsubscribe(this);
      //ButtonHandler.Instance.
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

    private ButtonTrigger trigger;

    void HandleButtons(){
      if (Input.GetButtonDown("Fire1")){
        ButtonHandler.Submit(this);
      }
    }

    public Vector3 GetButtonPosition() {
      return transform.position;
    }

    public void OnStayButtons(ButtonEntity[] buttons, int size) {
      
    }
  }
}
