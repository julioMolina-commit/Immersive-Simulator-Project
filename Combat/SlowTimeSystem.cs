using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeSystem : MonoBehaviour
{
    bool isSlowed;
    [SerializeField] float slowTimeCooldown;
    private void Start()
    {
        GameEvents.SlowTime.AddListener(OnSlowTime);
    }
    public void OnSlowTime()
    {
        if (!isSlowed)
        {
            isSlowed = true;

            Time.timeScale = 0.1f;

            Invoke(nameof(SlowTimeCooldown), slowTimeCooldown);
        }
    }
    private void SlowTimeCooldown()
    {
        isSlowed = false;

        Time.timeScale = 1f;
    }
}
