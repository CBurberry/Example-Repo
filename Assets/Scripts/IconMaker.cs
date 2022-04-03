using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMaker : MonoBehaviour
{
    public Image icon;
    public Camera cam;

    public Sprite getIcon()
    {
        // get dimensions
        int resX = Screen.width;
        int resY = Screen.height;

        //variables for clipping to a square
        int clipX = 0;
        int clipY = 0;

        if (resX > resY)
            clipX = resX - resY;
        else if (resX < resY)
            clipY = resY - resX;
        //else it's already square


        //initialise parts
        Texture2D tex = new Texture2D(resX - clipX, resY - clipY, TextureFormat.RGBA32, false);
        RenderTexture rt = new RenderTexture(resX, resY, 24);
        cam.targetTexture = rt;
        RenderTexture.active = rt; 

        // grab icon and stick it in the texture
        cam.Render();
        tex.ReadPixels(new Rect(clipX/2, clipY/2, resX - clipX, resY - clipY), 0, 0);
        tex.Apply();

        // Clean up
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);


        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0,0)); 
    }



    // Start is called before the first frame update
    void Start()
    {
        icon.sprite = getIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
