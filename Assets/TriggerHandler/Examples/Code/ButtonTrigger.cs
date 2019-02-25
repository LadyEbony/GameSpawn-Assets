using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.TriggerExamples {
  public class ButtonTrigger : TriggerEntity {

    private SpriteRenderer _renderer;
    private SpriteRenderer spriteRenderer {
      get {
        return _renderer ?? (_renderer = GetComponentInChildren<SpriteRenderer>());
      }
    }

    private bool inTrigger = false;
    private bool on = false;

    private void OnEnable() {
      UpdateColor();
    }

    public void EnterTrigger() {
      inTrigger = true;
      UpdateColor();
    }

    public void ExitTrigger(){
      inTrigger = false;
      UpdateColor();
    }

    public void TurnOn(){
      // TriggerHandler does not restrict you from accessing every trigger entity if need be.

      // Keep in mind that you cannot modify the entity directly.
      // Though you can still set variables and call methods.
      foreach(var ent in TriggerHandler<ButtonTrigger>.GetList){
        ent.on = false;
        ent.UpdateColor();
      }
      on = true;
      UpdateColor();
    }

    private void UpdateColor(){
      spriteRenderer.color = (inTrigger ? Color.white : new Color(1f, 1f, 1f, 0.5f))
                            * (on ? Color.red : Color.white);
    }

  }
}