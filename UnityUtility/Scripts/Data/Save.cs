using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Save
{
    
    public static bool Write(string name, object data)
    {
        string path = Application.persistentDataPath + "/Saves/" + name + ".bin";

        BinaryFormatter formatter = new BinaryFormatter();
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/Saves")) System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        FileStream stream = new FileStream(path, System.IO.File.Exists(path) ? FileMode.Open : FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        return true;
    }

    public static T Read<T>(string objectName)
    {
        string path = Application.persistentDataPath + "/Saves/" + objectName + ".bin";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        T o = (T)formatter.Deserialize(stream);
        stream.Close();

        return o;
    }

}

public enum SaveType
{
    PlayerPrefs,
    File,
}