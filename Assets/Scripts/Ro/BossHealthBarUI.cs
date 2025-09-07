using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{

    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private GameObject sliderObject;
    [SerializeField] private float turnOffDelay;
    private Coroutine turningOffHealthbar = null;

    public static BossHealthBarUI instance;

    void Awake()
    {
        sliderObject.SetActive(false);
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        Enemy_Spawner.Instance.BossSpawned.AddListener(TurnOnBossHealthBar);
    }

    private void TurnOnBossHealthBar()
    {
        sliderObject.SetActive(true);
        bossHealthSlider.value = 1f;
    }

    public void UpdateBossHealthBar(float currentHealth, float maxHealth)
    {
        if (!bossHealthSlider) return;
        if (currentHealth == 0)
        {
            TurnOffHealthBar();
        }
        bossHealthSlider.value = currentHealth / maxHealth;
    }

    private void TurnOffHealthBar()
    {
        StartCoroutine(TurnOff(turnOffDelay));
    }

    private IEnumerator TurnOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        bossHealthSlider.value = 0;
        sliderObject.SetActive(false);
    }

}