using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour, ISaveManager
{
    [Header("UI")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private float sliderEffectDuration = 0.5f;
    [SerializeField] private AnimationCurve expCurve;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("LvLs")]
    [SerializeField] private int currentLvl;
    [SerializeField] private int currentExp;
    [SerializeField] private int expToNextLvl;

    public static ExpManager Instance;

    private void Awake()
    {
        Instance = Instance == null ? this : DestroyAndReturnNull();
    }

    private ExpManager DestroyAndReturnNull()
    {
        Destroy(gameObject);
        return null;
    }

    private void Start()
    {
        expToNextLvl = (int)expCurve.Evaluate(currentLvl);
        expSlider.maxValue = expToNextLvl;

        CheckLvlUp(currentExp);
        UpdateExpUI();
    }

    private void Update()
    {
        UpdateSlider();

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(50);
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        CheckLvlUp(currentExp);
        UpdateExpUI();
    }

    private void CheckLvlUp(int exp)
    {
        while (exp >= expToNextLvl)
        {
            EvaluteExp();
            UpdateExpUI();
            CheckLvlUp(exp);
        }
    }

    private void EvaluteExp()
    {
        currentLvl++;
        currentExp -= expToNextLvl;
        expToNextLvl = (int)expCurve.Evaluate(currentLvl);
        expSlider.maxValue = expToNextLvl;
    }

    private void UpdateExpUI()
    {
        lvlText.text = currentLvl.ToString();
        expText.text = currentExp.ToString() + " / " + expToNextLvl.ToString();
    }

    private void UpdateSlider()
    {
        if (expSlider.value != currentExp)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, currentExp, 1/sliderEffectDuration);
            if (Mathf.Round(expSlider.value) == currentExp)
            {
                expSlider.value = currentExp;
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.exp = this.currentExp;
        gameData.lvl = this.currentLvl;
    }

    public void LoadData(GameData gameData)
    {
        this.currentExp = gameData.exp;
        this.currentLvl = gameData.lvl;

        expToNextLvl = (int)expCurve.Evaluate(currentLvl);
        expSlider.maxValue = expToNextLvl;
    }
}
