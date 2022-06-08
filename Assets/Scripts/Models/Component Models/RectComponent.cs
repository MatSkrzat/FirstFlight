using System;
using UnityEngine;

[Serializable]
public class RectComponent
{
    public float x;
    public float y;
    public float width;
    public float height;

    public RectComponent(Rect rect)
    {
        FromRect(rect);
    }

    public Rect ToRect()
    {
        return new Rect(x, y, width, height);
    }
    public void FromRect(Rect rect)
    {
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;
    }
}
