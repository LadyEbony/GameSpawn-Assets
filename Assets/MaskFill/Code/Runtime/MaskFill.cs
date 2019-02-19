using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * You can create more alpha masks using Window/Texture Builder.
 * Create the alpha mask you require.
 * If it is missing a functionality, you can create it yourself (I hope the code isn't too confusing!)
 * Or you can ask Jose (LadyEbony on GameSpawn discord).
 */


namespace GameSpawn {
  /// <summary>
  /// Helper class to the MaskFill shader.
  /// </summary>
  public class MaskFill {

    private Material material;
    private int materialFillID;

    public MaskFill(Material material){
      this.material = material;
      materialFillID = Shader.PropertyToID("_Fill");  // Using property identifies is more effecient than passing strings
    }

    /// <summary>
    /// Returns current _Fill parameter value.
    /// </summary>
    /// <returns></returns>
    public float Get(){
      return material.GetFloat(materialFillID);
    }

    /// <summary>
    /// Updates the _Fill parameter to <paramref name="newValue"/>.
    /// </summary>
    /// <param name="newValue"></param>
    public void Set(float newValue){
      material.SetFloat(materialFillID, newValue);
    }

  }
}