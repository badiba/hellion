using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public static SaveData Default => new SaveData(0, true);

    public int HighestScore { get; private set; }
    public bool IsSoundOn { get; private set; }

    public SaveData(int highestScore, bool isSoundOn)
    {
        HighestScore = highestScore;
        IsSoundOn = isSoundOn;
    }
}
