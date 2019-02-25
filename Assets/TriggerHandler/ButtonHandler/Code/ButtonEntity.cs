using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class ButtonEntity : TriggerEntity {

    #region Inspector and States

    [Range(0, 7)]
    [SerializeField] private int _mask = 0;
    public int Mask {
      get {
        return _mask;
      }
    }

    public enum ButtonState { Off, Enter, On, Exit }
    public ButtonState state = ButtonState.Off;

    public bool Submit = false;

    #endregion

    #region Components

    public SpriteRenderer SpriteRenderer { get; private set; }

    #endregion

    public override void Awake() {
      base.Awake();
      SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnEnable() {
      GlobalMaskList<ButtonEntity>.Add(this, Mask);
    }

    public override void OnDisable() {
      GlobalMaskList<ButtonEntity>.Remove(this, Mask);
    }

    public void OnStay(ButtonResponder responder){
      if (state.Equals(ButtonState.Off) || state.Equals(ButtonState.Exit)) {
        state = ButtonState.Enter;
        responder.EnterTrigger(this);
        responder.StayTrigger(this);
      } else {
        state = ButtonState.On;
        responder.StayTrigger(this);
      }
    }

    public void OnExit(ButtonResponder responder) {
      if (state.Equals(ButtonState.On) || state.Equals(ButtonState.Enter)) {
        state = ButtonState.Exit;
        responder.ExitTrigger(this);
        responder.OffTrigger(this);
      } else {
        state = ButtonState.Off;
        responder.OffTrigger(this);
      }
    }

    public void OnSubmit(ButtonResponder responder, bool forceTrue = false){
      if (forceTrue){
        Submit = true;
        responder.Submit(this);
      } else {
        Submit = !Submit;
        if (Submit) responder.Submit(this);
      }
    }

  }
}