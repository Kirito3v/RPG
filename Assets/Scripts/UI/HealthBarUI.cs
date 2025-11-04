using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private EntityStats stats;
    private new RectTransform transform;
    private Slider slider;
    
    private void Start()
    {
        UpdateHealthUI();
    }

    private void OnEnable()
    {
        transform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<EntityStats>();
        slider = GetComponentInChildren<Slider>();

        entity.OnFlipped += FlipUI;
        stats.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        entity.OnFlipped -= FlipUI;
        stats.OnHealthChanged -= UpdateHealthUI;
    }

    void FlipUI() => transform.Rotate(0, 180, 0);

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.GetCurrentHealthValue();
    }
}
