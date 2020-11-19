using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Parameter
{
    private object value;

    public object GetValue() { return value; }
}
public abstract class Parameter<T> : Parameter
{
    new public T GetValue() { return (T)base.GetValue(); }
}
public sealed class ParamLinked : Parameter<uint> { }
public sealed class ParamInteger : Parameter<int> { }
public sealed class ParamFloat : Parameter<float> { }
public sealed class ParamBool : Parameter<bool> { }