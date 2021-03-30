using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class DataHolder
{
    public int bestPlatform;
    public int totalCoins;


    public DataHolder()
    {
        bestPlatform = DataState.BestPlatform;
        totalCoins = DataState.TotalCoins;
    }

    public void ApplyToDataState()
    {
        DataState.BestPlatform = bestPlatform;
        DataState.TotalCoins = totalCoins;
    }

    internal DataHolder SetDefaultData()
    {
        bestPlatform = 0;
        totalCoins = 0;

        return this;
    }
}
