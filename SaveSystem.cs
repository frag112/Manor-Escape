using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem 
{
public static void SavePlayer (Player player)
    {
        BinaryFormatter _formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream _stream = new FileStream(path, FileMode.Create);

        PlayerData _data = new PlayerData(player);

        _formatter.Serialize(_stream, _data);
        _stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string _path = Application.persistentDataPath + "/player.fun";
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            PlayerData _data = _formatter.Deserialize(_stream) as PlayerData;
            _stream.Close();

            return _data;
        }
        else
        {
            Debug.LogError("no saves");
            return null;
        }
    }
}
