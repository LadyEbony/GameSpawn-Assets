using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically updates the health bar display.
/// Doesn't require any method calls.
/// </summary>
[UnityEngine.ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
  // Lazy initialization
  // If you want this to be slightly faster, initialize either in
  //    a) Start()
  //    b) the inspector
  private Material _foreMaterial;
  private Material foreMaterial{
    get {
      if (!_foreMaterial){
        var sr = GetComponentInChildren<SpriteRenderer>();

        // Unity requires sharedMaterial during Edit mode.
        // It is disabled for the actual release.
        #if UNITY_EDITOR
        if (Application.isPlaying){
          _foreMaterial = sr.material;
        } else {
          _foreMaterial = sr.sharedMaterial; 
        }
        #else
        _foreMaterial = sr.material;
        #endif
      }
      return _foreMaterial;
    }
  }

  // Test variables
  // Ideally, you should reference the script that contains the health values
  // and take the values from there.
  public int health = 5;
  public int maxHealth = 10;

  [Tooltip("How fast the health bar fills")]
  [SerializeField] private float fillMultiplier = 1.0f;

  /*
  private void Update() {
    if (Input.GetKeyDown(KeyCode.A)){
      health = 0;
    } else if (Input.GetKeyDown(KeyCode.D)){
      health = maxHealth;
    }
  }*/

  private void LateUpdate() {
    float nfill;
    if (maxHealth > 0) {    // Prevents division by zero.
      nfill = (float)health / maxHealth;
    } else {
      nfill = 0.0f;   
    }

    // Unity cannot update frequently in Edit mode.
    // It is disabled for the actual release.
#if UNITY_EDITOR
    if (Application.isPlaying) {
      var cfill = foreMaterial.GetFloat("_Fill");
      nfill = Mathf.MoveTowards(cfill, nfill, Time.deltaTime * fillMultiplier);
    }
    foreMaterial.SetFloat("_Fill", nfill);
#else
    var cfill = foreMaterial.GetFloat("_Fill");
    nfill = Mathf.MoveTowards(cfill, nfill, Time.deltaTime * fillMultiplier);
  
    foreMaterial.SetFloat("_Fill", nfill);
#endif

  }

}
