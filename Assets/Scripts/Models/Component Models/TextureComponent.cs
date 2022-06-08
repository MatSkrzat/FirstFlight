using System;
using UnityEngine;

[Serializable]
public class TextureComponent
{
    public string FilePath;
    public int InstanceId;

    public TextureComponent(Texture2D texture2D, string texturePath = null)
    {
        if (texturePath != null)
        {
            FromTexture(texture2D, texturePath);
        }
        else
        {
            FromTexture(texture2D);
        }
    }

    public Texture2D ToTexture()
    {
        if (FilePath != null)
        {
            return Resources.Load<Texture2D>(FilePath);
        }
        else
        {
            throw new Exception("No file path provided");
        }
    }
    public void FromTexture(Texture2D texture2D)
    {
        InstanceId = texture2D.GetInstanceID();
    }
    public void FromTexture(Texture2D texture2D, string texturePath)
    {
        InstanceId = texture2D.GetInstanceID();
        FilePath = texturePath;
    }
}
