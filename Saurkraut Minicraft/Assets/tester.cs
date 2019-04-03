using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    public void LoadAddOnClick(int level)
    {
        Application.LoadLevelAdditive(level);
    }
}
