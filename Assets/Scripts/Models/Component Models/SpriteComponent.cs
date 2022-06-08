using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteComponent
{
    public TextureComponent Texture;
    public float PixelsPerUnit;
    public VectorComponent Pivot;
    public RectComponent Rect;

    public SpriteComponent(Sprite sprite, string texturePath = null)
    {
        FromSprite(sprite, texturePath);
    }

    public SpriteComponent() { }

    public Sprite ToSprite()
    {
        return Sprite.Create(Texture.ToTexture(), Rect.ToRect(), Pivot.ToVector2(), PixelsPerUnit);
    }
    public void FromSprite(Sprite sprite, string texturePath = null) {
        if(texturePath != null)
        {
            Texture = new TextureComponent(sprite.texture, texturePath);
        }
        else
        {
            Texture = new TextureComponent(sprite.texture);
        }
        PixelsPerUnit = sprite.pixelsPerUnit;
        Pivot = new VectorComponent(
            new Vector2(
                sprite.pivot.x / sprite.rect.width, 
                sprite.pivot.y / sprite.rect.height
                )
            );
        Rect = new RectComponent(sprite.rect);
    }
}
