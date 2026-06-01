using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy
{
    public CandyData Data { get; }

    public Candy(CandyData data)
    {
        Data = data;
    }

    public CandyType Type => Data.Type;
}