using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackSense : MonoBehaviour
{
    public static AttackSense Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void HitPause(float second)
    {
        StartCoroutine(Pause(second));
    }

    IEnumerator Pause(float second)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(second);
        Time.timeScale = 1;
    }
}
