using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   public static void SavePlayer(PlayerInfo player, MyQuestSaver mqs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bbsse";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, mqs);
        bf.Serialize(stream, data);
        stream.Close();
   }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bbsse";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = bf.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
