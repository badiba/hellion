using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveGame()
    {
        // TODO
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/save.bcs";
        var stream = new FileStream(path, FileMode.Create);
        var saveData = SaveData.Default;
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        // TODO
        string path = Application.persistentDataPath + "/save.bcs";
        var saveData = SaveData.Default;

        if (File.Exists(path))
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                saveData = formatter.Deserialize(stream) as SaveData;
            }
        }

        return saveData;
    }
}
