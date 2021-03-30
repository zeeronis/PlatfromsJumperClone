using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataState
{
    public static int BestPlatform { get; set; }
    public static int TotalCoins { get; set; }


    public static void LoadData()
    {
        DataHolder dataHolder = null;
        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            using (FileStream stream = new FileStream(Application.persistentDataPath + "/state.s", FileMode.OpenOrCreate))
            {
                if (stream.Length != 0)
                    dataHolder = (DataHolder)formatter.Deserialize(stream);

                if (dataHolder == null)
                    dataHolder = new DataHolder().SetDefaultData();

                dataHolder.ApplyToDataState();
            }
        }
        catch (System.Exception)
        {
            dataHolder = new DataHolder().SetDefaultData();
            dataHolder.ApplyToDataState();
        }
    }

    public static void SaveData()
    {
        DataHolder state = new DataHolder();
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(Application.persistentDataPath + "/state.s", FileMode.Create))
        {
            formatter.Serialize(stream, state);
        }
    }
}
