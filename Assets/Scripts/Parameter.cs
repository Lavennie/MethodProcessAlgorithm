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