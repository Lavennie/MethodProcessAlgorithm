using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Parameter
{
    private object value;

    protected Parameter(object value)
    {
        this.value = value;
    }

    public object GetValue() { return value; }
}
[Serializable]
public abstract class Parameter<T> : Parameter
{
    protected Parameter(T value) : base(value)
    {
    }

    new public T GetValue() { return (T)base.GetValue(); }
}
[Serializable]
public sealed class ParamInteger : Parameter<int>
{
    public ParamInteger(int value) : base(value) { }
}
[Serializable]
public sealed class ParamFloat : Parameter<float>
{
    public ParamFloat(float value) : base(value) { }
}
[Serializable]
public sealed class ParamBool : Parameter<bool>
{
    public ParamBool(bool value) : base(value) { }
}
[Serializable]
public sealed class ParamVector2 : Parameter<float[]>
{
    public ParamVector2(Vector2 v) : base(new float[2] { v.x, v.y }) { }
    public ParamVector2(float x, float y) : base(new float[2] { x, y }) { }
}
[Serializable]
public sealed class ParamVector3 : Parameter<float[]>
{
    public ParamVector3(Vector3 v) : base(new float[3] { v.x, v.y, v.z }) { }
    public ParamVector3(float x, float y, float z) : base(new float[3] { x, y, 3 }) { }
}
[Serializable]
public sealed class ParamPickup : Parameter<GameObject>
{
    public ParamPickup(GameObject value) : base(value) { }
}
[Serializable]
public sealed class ParamVoid : Parameter<object>
{
    public ParamVoid() : base(null) { }
}