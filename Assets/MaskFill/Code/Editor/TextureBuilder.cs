using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

using Action = System.Action;

public class TextureBuilder : EditorWindow {
  // Add menu named "My Window" to the Window menu
  [MenuItem("Window/Texture Builder")]
  static void Init() {
    // Get existing open window or if none, make a new one:
    TextureBuilder window = (TextureBuilder)EditorWindow.GetWindow(typeof(TextureBuilder));
    window.minSize = new Vector2(320, 384);
    window.maxSize = new Vector2(320, 384);
    window.Show();
  }

  private Texture2D tex;
  private int texSize = 128;
  private Color startingColor = new Color(0, 0, 0, 0);
  private Color endingColor = new Color(0, 0, 0, 1);
  private bool texReverse = false;
  public float texOffset = 0.0f;
  private enum TextureType { Horizontal, Circular, Radial }
  private TextureType texType;

  private void OnGUI() {
    if (!tex) tex = CreateTexture();
    GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
    boldStyle.fontStyle = FontStyle.Bold;


    EditorGUILayout.BeginVertical();
    EditorGUILayout.Separator();
    OnGUICenter(() => {
      GUILayout.Label("Texture", boldStyle);

    }
    );
    OnGUICenter(() => {
      var rect = GUILayoutUtility.GetRect(128, 128);
      GUI.DrawTexture(rect, tex, ScaleMode.StretchToFill);
    }
    );
    EditorGUILayout.Separator();

    var n_texSize = EditorGUILayout.IntField("Texture Size", texSize);
    var n_texType = (TextureType)EditorGUILayout.EnumPopup("Texture Type", texType);
    var n_texReverse = EditorGUILayout.Toggle("Reverse Texture", texReverse);

    float n_texOffset;
    switch (n_texType) {
      case TextureType.Horizontal:
        n_texOffset = EditorGUILayout.Slider("Texture Offset", texOffset, 0.0f, 1.0f);
        break;
      case TextureType.Radial:
        n_texOffset = EditorGUILayout.IntSlider("Texture Offset", (int)texOffset, 0, 360);
        break;
      default:
        n_texOffset = texOffset;
        break;
    }
    var n_sc = EditorGUILayout.ColorField(new GUIContent("Starting Color"), startingColor, false, true, false);
    var n_ec = EditorGUILayout.ColorField(new GUIContent("Ending Color"), endingColor, false, true, false);

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    if (GUILayout.Button("Create PNG")){
      var bytes = tex.EncodeToPNG();

      var path = EditorUtility.SaveFilePanel(
        "Save texture as PNG",
        "",
        string.Format("texture_{0}.png", Random.Range(0,1000)),
        "png"
      );

      if (!string.IsNullOrWhiteSpace(path)) {
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();
      }
    }

    EditorGUILayout.EndVertical();

    if (n_texSize != texSize || n_sc != startingColor || n_ec != endingColor || n_texType != texType ||
        n_texOffset != texOffset || n_texReverse != texReverse) {
      texSize = n_texSize;
      texType = n_texType;
      texReverse = n_texReverse;
      texOffset = n_texOffset;
      startingColor = n_sc;
      endingColor = n_ec;

      tex = CreateTexture();
    }

  }

  private Texture2D CreateTexture() {
    if (texSize <= 0) return new Texture2D(1, 1);

    var tex = new Texture2D(texSize, texSize);

    var pixels = tex.GetPixels();

    System.Func<int, int, Color> GetColor;
    float offset;
    Color sColor = texReverse ? endingColor : startingColor;
    Color eColor = texReverse ? startingColor : endingColor;
    switch(texType){
      case TextureType.Horizontal:
        offset = texOffset;
        GetColor = (x, y) => GetHorizontalColor(sColor, eColor, x, y, offset);
        break;
      case TextureType.Circular:
        GetColor = (x, y) => GetCircularColor(sColor, eColor, x, y, texSize / 2);
        break;
      case TextureType.Radial:
        offset = texOffset * Mathf.Deg2Rad;
        GetColor = (x, y) => GetRadialColor(sColor, eColor, x, y, offset, texSize / 2);
        break;
      default:
        throw new System.Exception(string.Format("{0} not implemented", texType));
    }

    for (var y = 0; y < texSize; y++) {
      for (var x = 0; x < texSize; x++) {
        pixels[y * texSize + x] = GetColor(x, y);
      }
    }

    tex.SetPixels(pixels);
    tex.Apply();

    return tex;
  }

  private void OnGUICenter(Action gui){
    EditorGUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    gui();
    GUILayout.FlexibleSpace();
    EditorGUILayout.EndHorizontal();
  }

  private Color GetHorizontalColor(Color color1, Color color2, int x, int y, float offset) {
    return Color.Lerp(color1, color2, Mathf.Abs(((float)x / texSize) - offset));
  }

  private Color GetCircularColor(Color color1, Color color2, int x, int y, int radius){
    x -= radius;
    y -= radius;
    return Color.Lerp(color1, color2, Mathf.Sqrt(x * x + y * y) / radius);
  }

  private const float PI2 = Mathf.PI * 2;
  private Color GetRadialColor(Color color1, Color color2, int x, int y, float offset, int halfSize){
    return Color.Lerp(color1, color2, ((Mathf.Atan2(y - halfSize, x - halfSize) + PI2 + offset) % PI2) / PI2);
  }

}