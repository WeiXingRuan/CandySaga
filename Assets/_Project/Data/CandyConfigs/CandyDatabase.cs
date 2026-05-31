using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CandyConfigs/Candy Database")]
public class CandyDatabase : ScriptableObject
{
    public List<CandyData> Candies;
    public CandyData GetCandyData(CandyType type)
    {
        return Candies.Find(c => c.Type == type);
    }
}
