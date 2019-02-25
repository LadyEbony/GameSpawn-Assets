using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn {
  public class ButtonDirectory : MonoBehaviour {

    private ButtonResponder responder;

    [Range(0, 7)]
    public int Mask;

    public bool AllowMultipleSubmits = false;

    private void Awake() {
      responder = GetComponent<ButtonResponder>();
      
      if (responder == null){
        Debug.LogWarningFormat("Button Handler[{0}] requires a Button Responder. Deleting script.", gameObject);
      }
    }

    private void OnEnable() {
      ButtonHandler.Subscribe(this);
    }

    private void OnDisable() {
      ButtonHandler.Unsubscribe(this);
    }

    private ButtonEntity[] contains = new ButtonEntity[32];
    private int containLength = 0;
    private ButtonEntity[] outsides = new ButtonEntity[32];
    private int outsideLength = 0;

    private void Update() {
      foreach (var user in ButtonHandler.Users) {

        TriggerHandler<ButtonEntity>.GetOverlapEntities(user.GetButtonPosition(),
                                                        contains, outsides,
                                                        ButtonHandler.GetMaskedList(Mask),
                                                        out containLength, out outsideLength);

        var resp = responder; // cache
        // Buttons contained 
        if (containLength > 0) {
          for (var i = 0; i < containLength; i++) {
            contains[i].OnStay(resp);
          }
          user.OnStayButtons(contains, containLength);
        }

        // Buttons outside
        if (outsideLength > 0) {
          for (var i = 0; i < outsideLength; i++) {
            outsides[i].OnExit(resp);
          }
        }
      }
    }

    public void Submit(IButtonUser user){
      var resp = responder;
      if (AllowMultipleSubmits) {
        for(var i = 0; i < containLength; i++){
          contains[i].OnSubmit(resp);
        }
      } else {
        if (containLength == 0) return;

        foreach(var item in ButtonHandler.GetMaskedList(Mask)){
          item.Submit = false;
        }

        contains[0].OnSubmit(resp, true);

      }
      
    }

  }
}