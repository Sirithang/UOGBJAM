using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FourColorColoring : MonoBehaviour
{
    public Material mat;

    void OnEnable()
    { 
        //calling the instance to force it to load
        GameData data = GameData.Instance;
    } 

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
#if UNITY_EDITOR
        if (mat == null)
            return;
#endif

        Graphics.Blit(src, dest, mat);
    }
}
