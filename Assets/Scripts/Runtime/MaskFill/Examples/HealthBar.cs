using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpawn.MaskFillExamples {

  public class HealthBar : MonoBehaviour {

    private MaskFill Fill;

    private void Awake() {
      var sr = GetComponentInChildren<SpriteRenderer>();
      Fill = new MaskFill(sr.material);
    }

    // Ideally, you should reference the script that contains the health values
    // and take the values from there.
    public int health = 5;
    public int maxHealth = 10;

    #region Inspector

    [Tooltip("How fast the health bar fills")]
    [SerializeField] private float fillMultiplier = 1.0f;

    #endregion

    private void Update() {
      if (Input.GetKeyDown(KeyCode.A)){
        health--;
      }  
      if (Input.GetKeyDown(KeyCode.D)) {
        health++;
      }
    }

    // LateUpdate() is guaranteed after all Update().
    // So if all health changes occur then, then you can expect the health bar to update correctly.
    private void LateUpdate() {
      float nfill;
      if (maxHealth > 0) {    // Prevents division by zero.
        nfill = (float)health / maxHealth;
      } else {
        nfill = 0.0f;
      }

      var cfill = Fill.Get();
      nfill = Mathf.MoveTowards(cfill, nfill, Time.deltaTime * fillMultiplier);
      Fill.Set(nfill);

    }

  }
}