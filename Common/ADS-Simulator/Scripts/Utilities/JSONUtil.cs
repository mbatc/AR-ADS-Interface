using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JSONUtil
{
  public class JSONObject
  {
    public enum DataType
    {
      Object,
      Array,
      Value,
      Null
    }

    public void Set(int value)        { this.data = value.ToString(); }
    public void Set(bool value)       { this.data = value.ToString(); }
    public void Set(float value)      { this.data = value.ToString(); }
    public void Set(string value)     { this.data = value; }
    
    public void Set(JSONObject value)
    {
      SetType(DataType.Null);

      if (IsArray())
      {
        foreach (JSONObject element in arrayMembers)
          Add().Set(element);
      }
      else if (IsObject())
      {
        foreach (var element in objectMembers)
          Add(element.Key).Set(element.Value);
      } 
      else if (IsValue())
      {
        SetType(DataType.Value);
        this.data = value.data;
      }
    }

    public int    AsInt()    { return As<int>(0); }
    public bool   AsBool()   { return As<bool>(false); }
    public float  AsFloat()  { return As<float>(0.0f); }
    public string AsString() { return this.data; }

    public bool IsValue()  { return this.type == DataType.Value; }
    public bool IsArray()  { return this.type == DataType.Array; }
    public bool IsObject() { return this.type == DataType.Object; }
    public bool IsNull()   { return this.type == DataType.Null; }

    public JSONObject Get(int index)
    {
      try
      {
        if (IsArray())
          return this.arrayMembers[index];
      }
      catch {}
      return new JSONObject();
    }

    public JSONObject Get(string name)
    {
      try
      {
        if (IsObject())
          return this.objectMembers[name];
      }
      catch { }
      return new JSONObject();
    }

    public JSONObject Add()
    {
      JSONObject newObject = new JSONObject();
      SetType(DataType.Array);
      arrayMembers.Add(newObject);
      return newObject;
    }

    public JSONObject Add(string name)
    {
      JSONObject newObject = new JSONObject();
      SetType(DataType.Object);
      objectMembers[name] = newObject;
      return newObject;
    }

    public void SetType(DataType type)
    {
      if (this.type == type)
        return;

      this.data          = null;
      this.arrayMembers  = null;
      this.objectMembers = null;

      switch (type)
      {
        case DataType.Value:  this.data          = "";                                   break;
        case DataType.Array:  this.arrayMembers  = new List<JSONObject>();               break;
        case DataType.Object: this.objectMembers = new Dictionary<string, JSONObject>(); break;
      }
    }

    public T As<T>(T defaultVal)
    {
      if (data == null)
        return defaultVal;

      MethodInfo m = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
      if (m == null)
        return defaultVal;

      try
      {
        return (T)m.Invoke(null, new object[] { this.data });
      }
      catch
      {
        return defaultVal;
      }
    }

    public JSONObject this[string name]
    {
      get { return Get(name); }
      set { Get(name).Set(value); }
    }

    private DataType type = DataType.Null;
    private string   data = null;

    private List<JSONObject>               arrayMembers  = null;
    private Dictionary<string, JSONObject> objectMembers = null;
  }


  public static class Extensions
  {
    public static T As<T>(this JToken token, T defaultValue)
    {
      try
      {
        return token.ToObject<T>();
      }
      catch
      {
        return defaultValue;
      }
    }
  }
}
