using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutObject : MonoBehaviour
{
    public static readonly int CutLimit = 3;

    public int NowCutLevel = 0;
    
    public bool CanCut()
    {
        return CutLimit >= NowCutLevel;
    }

    public abstract void Cut();
}
