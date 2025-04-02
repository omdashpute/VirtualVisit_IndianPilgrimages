using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScreenTimeout : MonoBehaviour
{
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
