using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region Inspector

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI panelTitle;
    [SerializeField] private GameObject[] stats;
    [SerializeField] private Image panelIcon;

    [Header("Button Colors")] [Space] [SerializeField]
    private Color buttonDisabledColor;

    [SerializeField] private Color buttonEnabledColor;

    [Header("Buttons")] [Space] [SerializeField]
    private GameObject[] upgradeButtons;

    [Header("Text")] [Space] [SerializeField]
    private TextMeshProUGUI upgradeCost;

    [SerializeField] private TextMeshProUGUI currentStat1;
    [SerializeField] private TextMeshProUGUI currentStat2;
    [SerializeField] private TextMeshProUGUI currentStat3;
    [SerializeField] private TextMeshProUGUI currentStat4;
    [SerializeField] private TextMeshProUGUI stat1Title;
    [SerializeField] private TextMeshProUGUI stat2Title;
    [SerializeField] private TextMeshProUGUI stat3Title;
    [SerializeField] private TextMeshProUGUI stat4Title;

    [FormerlySerializedAs("statUpgraded1")]
    [FormerlySerializedAs("statUpgrade1")]
    [FormerlySerializedAs("minersCountUpgraded")]
    [Header("Upgraded")]
    [Space]
    [SerializeField]
    private TextMeshProUGUI stat1Upgraded;

    [SerializeField] private TextMeshProUGUI stat2Upgraded;
    [SerializeField] private TextMeshProUGUI stat3Upgraded;
    [SerializeField] private TextMeshProUGUI stat4Upgraded;

    [Header("Images")] [Space] [SerializeField]
    private Image stat1Icon;

    [SerializeField] private Image stat2Icon;
    [SerializeField] private Image stat3Icon;
    [SerializeField] private Image stat4Icon;

    [Header("Shaft Icons")] [Space] [SerializeField]
    private Sprite shaftMinerIcon;

    [SerializeField] private Sprite minerIcon;
    [SerializeField] private Sprite walkingIcon;
    [SerializeField] private Sprite miningIcon;
    [SerializeField] private Sprite workerCapacityIcon;

    [Header("Elevator Icon")] [Space] [SerializeField]
    private Sprite elevatorMinerIcon;

    [SerializeField] private Sprite loadIcon;
    [SerializeField] private Sprite movementIcon;
    [SerializeField] private Sprite loadingIcon;

    #endregion

    public int TimesToUpgrade { get; set; }
    private Shaft _selectedShaft;
    private BaseUpgrade _currentUpgrade;

    private void Start()
    {
        ActivateButton(0);
        TimesToUpgrade = 1;
    }

    public void Upgrade()
    {
        if (GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost)
        {
            _currentUpgrade.Upgrade(TimesToUpgrade);
            if (_currentUpgrade is ShaftUpgrade)
            {
                UpdateUpgradePanel(_currentUpgrade);
            }

            if (_currentUpgrade is ElevatorUpgrade)
            {
                UpdateElevatorPanel(_currentUpgrade);
            }
        }
    }

    public void OpenUpgradePanel(bool status)
    {
        upgradePanel.SetActive(status);
    }

    #region Upgrade Buttons

    public void UpgradeX1()
    {
        ActivateButton(0);
        TimesToUpgrade = 1;
        upgradeCost.text = $"{_currentUpgrade.UpgradeCost}";
    }

    public void UpgradeX10()
    {
        ActivateButton(1);
        TimesToUpgrade = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
        upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).ToString();
    }

    public void UpgradeX50()
    {
        ActivateButton(2);
        TimesToUpgrade = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
        upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).ToString();
    }

    public void UpgradeXMax()
    {
        ActivateButton(3);
        TimesToUpgrade = CalculateUpgradeCount(_currentUpgrade);
        upgradeCost.text = GetUpgradeCost(TimesToUpgrade, _currentUpgrade).ToString();
    }

    #endregion

    #region Help Methods

    public void ActivateButton(int buttonIndex)
    {
        foreach (var upgradeButton in upgradeButtons)
        {
            upgradeButton.GetComponent<Image>().color = buttonDisabledColor;
        }

        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnabledColor;
        upgradeButtons[buttonIndex].transform
            .DOPunchPosition(transform.localPosition + new Vector3(0f, -5f, 0f), 0.5f)
            .Play();
    }

    private int GetUpgradeCost(int amount, BaseUpgrade upgrade)
    {
        int cost = 0;
        int upgradeUpgradeCost = (int)upgrade.UpgradeCost;
        for (int i = 0; i < amount; i++)
        {
            cost += upgradeUpgradeCost;
            upgradeUpgradeCost *= (int)upgrade.UpgradeCostMultiplier;
        }

        return cost;
    }

    public bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        return count > upgradeAmount;
    }

    public int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        int count = 0;
        int currentGold = GoldManager.Instance.CurrentGold;
        int upgradeUpgradeCost = (int)upgrade.UpgradeCost;
        for (int i = currentGold; i >= 0; i -= upgradeUpgradeCost)
        {
            count++;
            upgradeUpgradeCost *= (int)upgrade.UpgradeCostMultiplier;
        }

        return count;
    }

    #endregion

    #region Update Elevator Panel

    public void UpdateElevatorPanel(BaseUpgrade upgrade)
    {
        ElevatorMiner miner = upgrade.Elevator.Miner;
        panelTitle.text = $"Elevator Level {upgrade.CurrentLevel}";

        //Update Icon Stats
        stats[3].SetActive(false);
        panelIcon.sprite = elevatorMinerIcon;
        stat1Icon.sprite = loadIcon;
        stat2Icon.sprite = movementIcon;
        stat3Icon.sprite = loadingIcon;

        //Update Stats Titles
        stat1Title.text = "Load";
        stat2Title.text = "Movement Speed";
        stat3Title.text = "Loading Speed";

        //Update Current Stats
        currentStat1.text = $"{miner.CollectCapacity}";
        currentStat1.text = $"{miner.MoveSpeed}";
        currentStat1.text = $"{miner.CollectPerSecond}";

        //Update load value upgraded
        int currentCollect = miner.CollectCapacity;
        int collectMultiplier = (int)upgrade.CollectCapacityMultiplier;
        int load = Mathf.Abs(currentCollect - (currentCollect * collectMultiplier));
        stat1Upgraded.text = $"+ {load}";

        //Update move speed Upgraded
        float currentMoveSpeed = miner.MoveSpeed;
        float moveSpeedMultiplier = upgrade.MoveSpeedMultiplier;
        float moveSpeedAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * moveSpeedMultiplier));
        stat2Upgraded.text = (upgrade.CurrentLevel + 1) % 10 == 0 ? $"+ {moveSpeedAdded}/s" : $"+ 0/s";

        //Update new loading speed Added
        float loadingSpeed = miner.CollectPerSecond;
        float loadingSpeedMultiplier = upgrade.CollectPerSecondMultiplier;
        float loadingAdded = Mathf.Abs(loadingSpeed - (loadingSpeed * loadingSpeedMultiplier));
        stat3Upgraded.text = $"+ {loadingAdded}/s";
    }

    #endregion

    #region Update Shaft Panel

    private void UpdateUpgradePanel(BaseUpgrade upgrade)
    {
        panelTitle.text = $"Mine Shaft {_selectedShaft.ShaftId + 1} Level {upgrade.CurrentLevel}";

        upgradeCost.text = $"{upgrade.UpgradeCost}";
        currentStat1.text = $"{_selectedShaft.Miners.Count}";
        currentStat2.text = $"{_selectedShaft.Miners[0].MoveSpeed}";
        currentStat3.text = $"{_selectedShaft.Miners[0].CollectPerSecond}";
        currentStat4.text = $"{_selectedShaft.Miners[0].CollectCapacity}";

        //Update Stats Icons
        stats[3].SetActive(true);
        panelIcon.sprite = shaftMinerIcon;
        stat1Icon.sprite = minerIcon;
        stat2Icon.sprite = walkingIcon;
        stat3Icon.sprite = miningIcon;
        stat4Icon.sprite = workerCapacityIcon;

        //Update Stats Titles
        stat1Title.text = "Miners";
        stat2Title.text = "Walking Speed";
        stat3Title.text = "Mining Speed";
        stat4Title.text = "Worker Capacity";

        //Upgrade worker Capacity
        int collectCapacity = _selectedShaft.Miners[0].CollectCapacity;
        float collectCapacityMultiplier = upgrade.CollectCapacityMultiplier;
        int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity * (int)collectCapacityMultiplier));
        stat4Upgraded.text = $"+ {collectCapacityAdded}";

        //Upgrade Load Speed
        float currentLoadSpeed = _selectedShaft.Miners[0].CollectPerSecond;
        float currentLoadSpeedMultiplier = upgrade.CollectPerSecondMultiplier;
        int loadSpeedAdded = (int)Mathf.Abs(currentLoadSpeed - (currentLoadSpeed * currentLoadSpeedMultiplier));
        stat3Upgraded.text = $"+ {loadSpeedAdded}";

        //Upgrade Walking Speed
        float currentWalkingSpeed = _selectedShaft.Miners[0].MoveSpeed;
        float currentWalkingSpeedMultiplier = upgrade.MoveSpeedMultiplier;
        int walkingSpeedAdded =
            (int)Mathf.Abs(currentWalkingSpeed - (currentWalkingSpeed * currentWalkingSpeedMultiplier));
        stat2Upgraded.text = (upgrade.CurrentLevel + 1) % 10 == 0 ? $"+ {walkingSpeedAdded}/s" : $"+ 0/s";

        // Upgrade Miner Count
        stat1Upgraded.text = (upgrade.CurrentLevel + 1) % 10 == 0 ? $"+ 1" : $"+ 0";
    }

    #endregion

    #region Events

    private void ShaftUpgradeRequest(Shaft shaft, ShaftUpgrade shaftUpgrade)
    {
        List<Shaft> shaftList = ShaftManager.Instance.Shafts;
        foreach (var t in shaftList)
        {
            if (shaft.ShaftId == t.ShaftId)
            {
                _selectedShaft = t;
                _currentUpgrade = t.GetComponent<ShaftUpgrade>();
            }
        }

        _currentUpgrade = shaftUpgrade;
        OpenUpgradePanel(true);

        UpdateUpgradePanel(_currentUpgrade);
    }

    private void ElevatorUpgradeRequest(ElevatorUpgrade elevatorUpgrade)
    {
        _currentUpgrade = elevatorUpgrade;
        OpenUpgradePanel(true);
        UpdateElevatorPanel(elevatorUpgrade);
    }

    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
    }


    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
    }

    #endregion
}