using System.Collections;
using UnityEngine;

public class AttackSense : MonoBehaviour
{
    public static AttackSense Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    public void HitPause(float second)
    {
        StartCoroutine(Pause(second));
        IEnumerator Pause(float second)
        {
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(second);
            Time.timeScale = 1;
        }
    }
}
