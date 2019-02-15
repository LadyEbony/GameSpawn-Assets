using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Action = System.Action;

namespace GameSpawn.MaskFillExamples {

  public class Timer : MonoBehaviour {
    private MaskFill Fill;

    private void Awake() {
      var sr = GetComponentInChildren<SpriteRenderer>();
      Fill = new MaskFill(sr.material);

      countdownEvent = GetComponentInChildren<ParticleSystem>().Play;
    }

    public float currentTime = 3.0f;
    public float countdownTime = 3.0f;
    public Action countdownEvent;

    private void Update() {
      if(Input.GetKeyDown(KeyCode.Space)){
        currentTime = countdownTime;
      }

      if (currentTime <= 0.0f) return;

      currentTime -= Time.deltaTime;

      if (currentTime <= 0.0f){
        currentTime = 0.0f;
        countdownEvent?.Invoke();
      }

      Fill.Set(currentTime / countdownTime);
    }

  }
}
