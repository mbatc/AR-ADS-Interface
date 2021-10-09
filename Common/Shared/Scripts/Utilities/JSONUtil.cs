using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Util
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

    public enum ValueType
    {
      None,
      Int,
      Bool,
      Float,
      String
    }


    public static readonly JSONObject Null = new JSONObject();

    public JSONObject()
    {
      contentType = DataType.Null;
    }

    public JSONObject(string json)
      : this(JObject.Parse(json))
    {}

    public JSONObject(object content)
      : this(JObject.FromObject(content))
    {}

    public JSONObject(JToken token)
    {
      Set(token);
    }

    public void Set(int value)    { SetValue(ValueType.Int, value); }
    public void Set(bool value)   { SetValue(ValueType.Bool, value); }
    public void Set(float value)  { SetValue(ValueType.Float, value); }
    public void Set(string value) { SetValue(ValueType.String, value); }

    void SetValue(ValueType type, object value)
    {
      SetType(DataType.Value);
      this.valueType = type;
      this.data      = value.ToString();
    }

    public void Set(JSONObject value)
    {
      SetType(DataType.Null);

      if (value.IsArray())
      {
        foreach (JSONObject element in value.arrayMembers)
          Add().Set(element);
      }
      else if (value.IsObject())
      {
        foreach (var element in value.objectMembers)
          Add(element.Key).Set(element.Value);
      } 
      else if (value.IsValue())
      {
        SetValue(value.valueType, value.data);
      }
    }

    public void Set(JToken token)
    {
      SetType(DataType.Null);

      switch (token.Type)
      {
        case JTokenType.Object:
          foreach (JProperty member in token.Children<JProperty>())
            Add(member.Name).Set(member.Value);
          break;
        case JTokenType.Array:
          foreach (JToken element in token.Children())
            Add().Set(element);
          break;
        case JTokenType.Boolean:
          Set((bool)token);
          break;
        case JTokenType.Integer:
          Set((int)token);
          break;
        case JTokenType.Float:
          Set((float)token);
          break;
        case JTokenType.String:
          Set((string)token);
          break;
        case JTokenType.Null:
          break;
        case JTokenType.None:
          break;
      }
    }

    public int    AsInt()    { return As<int>(0); }
    public bool   AsBool()   { return As<bool>(false); }
    public float  AsFloat()  { return As<float>(0.0f); }
    public string AsString() { return this.data; }

    public bool IsValue()  { return this.contentType == DataType.Value; }
    public bool IsArray()  { return this.contentType == DataType.Array; }
    public bool IsObject() { return this.contentType == DataType.Object; }
    public bool IsNull()   { return this.contentType == DataType.Null; }

    public JSONObject Get(int index)
    {
      try
      {
        if (IsArray())
          return this.arrayMembers[index];
      }
      catch {}
      return JSONObject.Null;
    }

    public JSONObject Get(string name)
    {
      try
      {
        if (IsObject())
          return this.objectMembers[name];
      }
      catch { }
      return JSONObject.Null;
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
      if (this.contentType == type)
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
      this.contentType = type;
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

    public string ToJSON()
    {
      JToken jtoken = ToJToken();
      return jtoken == null ? "null" : jtoken.ToString();
    }

    public JToken ToJToken()
    {
      switch (contentType)
      {
        case DataType.Object:
          {
            JObject content = new JObject();
            foreach (var member in objectMembers)
              content.Add(member.Key, member.Value.ToJToken());
            return content;
          }
          
        case DataType.Array:
          {
            JArray content = new JArray();
            foreach (JSONObject member in arrayMembers)
              content.Add(member.ToJToken());
            return content;
          }
          
        case DataType.Value:
          {
            return new JValue(this.data);
          }

        case DataType.Null:
          {
            return null;
          }
      }

      return null;
    }

    public JSONObject this[string name]
    {
      get { return Get(name); }
      set { Get(name).Set(value); }
    }

    public JSONObject this[int index]
    {
      get { return Get(index); }
      set { Get(index).Set(value); }
    }

    private ValueType valueType   = ValueType.None;
    private DataType  contentType = DataType.Null;
    private string    data        = null;

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
