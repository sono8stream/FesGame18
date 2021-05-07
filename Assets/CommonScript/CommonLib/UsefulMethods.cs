using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TextureDivider
{
    public static Sprite TrimSp(this Sprite s, int x, int y, int width, int height,
        int posX, int posY)
    {
        Texture2D tex = new Texture2D(width, height);
        tex.SetPixels(posX, posY, width, height,
            s.texture.GetPixels(x, y, width, height));
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
    }

    public static Texture2D TrimTx(this Texture2D tex, Texture2D texture,
        int width, int height, int posX1, int posY1, int posX2, int posY2)
    {
        tex.SetPixels(posX2, posY2, width, height,
            texture.GetPixels(posX1, posY1, width, height));
        tex.Apply();
        return tex;
    }

    public static Sprite SetTexture(this Sprite s, Texture2D tex,float unitPixels)
    {
        return Sprite.Create(tex,
            new Rect(0, 0, tex.width, tex.height), Vector2.zero, unitPixels);
    }
}

public static class VectorAdvance
{
    public static Vector2 Round(Vector2 vec, int digit = 0)
    {
        vec = new Vector2((float)Math.Round(vec.x, digit),
            (float)Math.Round(vec.y, digit));
        return vec;
    }
}
