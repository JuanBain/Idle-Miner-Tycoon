using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
    public static Action<Warehouse, WarehouseUpgrade> OnUpgradeRequest;
    [SerializeField] private TextMeshProUGUI globalGoldTMP;
    [SerializeField] private TextMeshProUGUI currentLevelTMP;
    private Warehouse _warehouse;
    private WarehouseUpgrade _warehouseUpgrade;

    private void Start()
    {
        _warehouse = GetComponent<Warehouse>();
        _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
    }

    private void Update()
    {
        globalGoldTMP.text = Currency.DisplauCurrency(GoldManager.Instance.CurrentGold);
    }

    private void UpdateWarehouse(BaseUpgrade upgrade, int currentLevel)
    {
        if (upgrade == _warehouseUpgrade)
        {
            currentLevelTMP.text = $"Level\n{currentLevel}";
        }
    }


    public void UpgradeRequest()
    {
        OnUpgradeRequest?.Invoke(_warehouse, _warehouseUpgrade);
    }

    private void OnEnable()
    {
        WarehouseUpgrade.OnUpgrade += UpdateWarehouse;
    }
}