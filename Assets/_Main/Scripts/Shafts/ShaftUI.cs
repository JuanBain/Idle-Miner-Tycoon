using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;
    [Header("BUttons")] [SerializeField] private GameObject buyNewShaftButton;
    [Header("Text")] [SerializeField] private TextMeshProUGUI currentGoldTMP;
    [SerializeField] private TextMeshProUGUI currentLevelTMP;

    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;

    void Start()
    {
        _shaft = GetComponent<Shaft>();
        _shaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    private void Update()
    {
       
        currentGoldTMP.text = Currency.DisplauCurrency(_shaft.CurrentDeposit.CurrentGold);
    }

    public void BuyNewShaft()
    {
        if (GoldManager.Instance.CurrentGold >= ShaftManager.Instance.NewShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.NewShaftCost);
            ShaftManager.Instance.AddShaft();
            buyNewShaftButton.SetActive(false);
        }
    }

    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade);
    }

    private void UpgradeShaft(BaseUpgrade upgrade, int currentLevel)
    {
        if (upgrade == _shaftUpgrade)
        {
            currentLevelTMP.text = $"Level\n{currentLevel}";
        }
    }

    private void OnEnable()
    {
        ShaftUpgrade.OnUpgrade += UpgradeShaft;
    }


    private void OnDisable()
    {
        ShaftUpgrade.OnUpgrade -= UpgradeShaft;
    }
}