using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveGame()
    {
        var formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "save.bcs");

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var saveData = new SaveData(GameManager.Instance.HighestScore, true);
            formatter.Serialize(stream, saveData);
        }
    }

    public static SaveData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.bcs");
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
