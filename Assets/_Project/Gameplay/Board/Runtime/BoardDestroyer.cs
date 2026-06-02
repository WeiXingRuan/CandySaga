using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDestroyer 
{
    public void DestroyMatches(List<MatchGroup> matches)
    {
        foreach (MatchGroup group in matches)
        {
            foreach (Cell cell in group.Cells)
            {
                cell.Candy = null;
            }
        }
    }

}
