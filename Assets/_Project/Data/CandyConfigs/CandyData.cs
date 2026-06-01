using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CandyConfigs/Candy Data")]
public class CandyData : ScriptableObject
{
    public CandyType Type;
    public Sprite Sprite;
    public bool CanSpawn = true;
}
