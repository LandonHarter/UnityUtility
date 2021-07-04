using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtility : MonoBehaviour
{

    private static UnityUtility Instance;
    public static bool Initialized = false;

    public static void Init()
    {
        GameObject unityUtility = new GameObject("Unity Utility");
        unityUtility.AddComponent<UnityUtility>();
        DontDestroyOnLoad(unityUtility);
        Initialized = true;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public static void SlowTime(float slownessMultiplier, float slownessTime)
    {
        if (!Initialized)
        {
            Debug.LogError("Please initialize Unity Utility before using it functions.");
            return;
        }

        Instance.StartCoroutine(SlowTimeDown(slownessMultiplier, slownessTime));
    }

    private static IEnumerator SlowTimeDown(float slownessMultiplier, float slownessTime)
    {
        float timeScale = 1 / slownessMultiplier;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = timeScale * 0.02f;

        while (Time.timeScale <= 0.99)
        {
            Time.timeScale += (1 / slownessTime) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
            yield return new WaitForSeconds(Time.unscaledDeltaTime);
        }

        Time.timeScale = 1;
    }
}
