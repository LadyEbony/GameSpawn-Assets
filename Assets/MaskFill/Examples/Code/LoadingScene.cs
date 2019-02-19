using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSpawn.MaskFillExamples {
  public class LoadingScene : MonoBehaviour {

    private MaskFill Fill;

    private void Awake() {
      var sr = GetComponentInChildren<SpriteRenderer>();
      Fill = new MaskFill(sr.material);
    }

    /// <summary>
    /// Load scene in async. Displays a progress bar.
    /// </summary>
    /// <remarks>
    /// Due to a missing function in Unity, you cannot pass a string for async.
    /// You can get around it by creating some Dictionary.
    /// </remarks>
    /// <param name="sceneBuildIndex"></param>
    public void SwitchScene(int sceneBuildIndex){
      StartCoroutine(AsyncSceneLoad(sceneBuildIndex));
    }

    IEnumerator AsyncSceneLoad(int scene) {
      // Use LoadSceneMode.Single to switch to only one scene.
      // Use LoadSceneMode.Additive add an additional scene to the current one.
      var asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
      asyncLoad.allowSceneActivation = false;   // Deactivates automatical loading

      // AsyncLoad stops at 0.9f and waits for an allowSceneActivation
      while (asyncLoad.progress < 0.9f) {
        yield return null;
        Fill.Set(asyncLoad.progress);
      }

      asyncLoad.allowSceneActivation = true;

    }
  }
}