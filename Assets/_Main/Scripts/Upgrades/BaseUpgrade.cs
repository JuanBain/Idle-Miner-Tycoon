using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade, int> OnUpgrade;
    [Header("Upgrades")] [SerializeField] protected float collectCapacityMultiplier = 2f;
    [SerializeField] protected float collectPerSecondMultiplier = 2f;
    [SerializeField] protected float moveSpeedMultiplier = 1.25f;

    [Header("Cost")] [SerializeField] private float initialUpgradeCost = 200f;
    [SerializeField] private float upgradeCostMultiplier = 2f;

    public int CurrentLevel { get; set; }
    public float UpgradeCost { get; set; }

    public float CollectCapacityMultiplier => collectCapacityMultiplier;
    public float CollectPerSecondMultiplier => collectPerSecondMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float UpgradeCostMultiplier => upgradeCostMultiplier;
    public Elevator Elevator => _elevator;
    
    protected Shaft _shaft;
    protected Elevator _elevator;

    private void Start()
    {
        _elevator = GetComponent<Elevator>();
        _shaft = GetComponent<Shaft>();
        CurrentLevel = 1;
        UpgradeCost = initialUpgradeCost;
    }

    public virtual void Upgrade(int upgradeAmount)
    {
        if (upgradeAmount > 0)
        {
            for (int i = 0; i < upgradeAmount; i++)
            {
                UpgradeSuccess();
                UpdateUpgradeValues();
                RunUpgrade();
            }
        }
    }

    protected virtual void UpgradeSuccess()
    {
        GoldManager.Instance.RemoveGold((int)UpgradeCost);
        CurrentLevel++;
        OnUpgrade?.Invoke(this, CurrentLevel);
    }

    protected virtual void UpdateUpgradeValues()
    {
        // Update Upgrade
        UpgradeCost *= upgradeCostMultiplier;
    }

    protected virtual void RunUpgrade()
    {
        //Upgrade logic
    }
}