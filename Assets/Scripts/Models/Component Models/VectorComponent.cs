using System;
using UnityEngine;

[Serializable]
public class VectorComponent
{
    public float x;
    public float y;
    public float z;

    public VectorComponent(Vector2 vector2)
    {
        FromVector2(vector2);
    }
    public VectorComponent(Vector3 vector3)
    {
        FromVector3(vector3);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public void FromVector2(Vector2 vector2)
    {
        x = vector2.x;
        y = vector2.y;
    }
    public void FromVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }
}
