using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        UnityUtility.Init();
        UnityUtility.SlowTime(5, 5);
    }
}
