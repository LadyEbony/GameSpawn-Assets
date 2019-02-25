using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {
  public class Player : MonoBehaviour {

    public float speed = 1;

    private void Awake() {
      // The GlobalList is not restricted to TriggerEntity.
      // Any class can use it.
      GlobalList<Player>.Add(this);
    }

    private void OnDestroy() {
      GlobalList<Player>.Add(this);
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
      
      // TriggerHandler allows you to use only 1 line.
      var ent = TriggerHandler<AreaTrigger>.GetOverlapEntities(transform.position);

      if (ent.Length > 0){
        foreach(var e in ent)
          e.OnTrigger();
      }

      // TriggerHandler will still require some logic from you.
      // Though it greatly reduces the amount of code required.

      // Assuming buttons do not overlap
      var nTrg = TriggerHandler<ButtonTrigger>.GetOverlapEntity(transform.position);
      if (nTrg != trigger){                     // If new trigger
        if (trigger) trigger.ExitTrigger();     // Visualize last trigger by dimming it,    if it is not null
        if (nTrg) nTrg.EnterTrigger();          // Visualize new trigger by brightening it, if it is not null
      }
      trigger = nTrg;

      if (Input.GetButton("Fire1") && trigger){
        trigger.TurnOn();
      }

    }

  }
}
