using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="gamedata", menuName ="game data")]
public class GameData : ScriptableObject
{
    static protected GameData s_instance = null;
    static public GameData Instance
    {
        get
        { 
            if (s_instance ==null)
                Load();

            return s_instance;
        }
    }

    public PaletteResource[] palettes;
    public int usedPalette = 0;

    static void Load() 
    {
        s_instance = Resources.Load<GameData>("gamedata");
        s_instance.SetupPalette(); 
    }


    void SetupPalette()
    {
        Vector4[] palette = new Vector4[4]
        {
            palettes[usedPalette].color0,
            palettes[usedPalette].color1,
            palettes[usedPalette].color2,
            palettes[usedPalette].color3
        };

        Shader.SetGlobalVectorArray("_PaletteColor", palette);

        Resources.FindObjectsOfTypeAll<PixelCamera2D>()[0].camera.backgroundColor = palette[0];
    }
}
