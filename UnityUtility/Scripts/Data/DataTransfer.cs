using System.Collections.Generic;

public static class DataTransfer
{

    static Dictionary<string, object> data = new Dictionary<string, object>();

    public static void AddData(string key, object data)
    {
        DataTransfer.data.Add(key, data);
    }

    public static void RemoveData(string key)
    {
        data.Remove(key);
    }

    public static void SetData(string existingKey, object data)
    {
        if (DataTransfer.data.ContainsKey(existingKey))
        {
            DataTransfer.data.Remove(existingKey);
            DataTransfer.data.Add(existingKey, data);
        }
    }

    public static object GetData(string key)
    {
        object o;
        data.TryGetValue(key, out o);

        return o;
    }

    public static bool AddIfAbsent(string key, object o)
    {
        if (!data.ContainsKey(key))
        {
            data.Add(key, o);
            return true;
        }

        return false;
    }

    public static bool KeyExists(string key)
    {
        if (data.ContainsKey(key)) return true;

        return false;
    }

}
