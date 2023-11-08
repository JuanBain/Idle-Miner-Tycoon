using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorUI : MonoBehaviour
{
    public static Action<ElevatorUpgrade> OnUpgradeRequest;
    [SerializeField] private TextMeshProUGUI elevatorDepositGold;
    [SerializeField] private TextMeshProUGUI currentLevelTMP;
    private Elevator _elevator;
    private ElevatorUpgrade _elevatorUpgrade;

    private void Start()
    {
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();
        _elevator = GetComponent<Elevator>();
    }

    private void Update()
    {
        elevatorDepositGold.text = Currency.DisplauCurrency(_elevator.ElevatorDeposit.CurrentGold);
    }

    public void RequestUpgrade()
    {
        OnUpgradeRequest?.Invoke(_elevatorUpgrade);
    }

    private void UpgradeElevator(BaseUpgrade upgrade, int currentLevel)
    {
        if (upgrade == _elevatorUpgrade)
        {
            currentLevelTMP.text = $"Level\n{currentLevel}";
        }
    }

    private void OnEnable()
    {
        ElevatorUpgrade.OnUpgrade += UpgradeElevator;
    }


    private void OnDisable()
    {
        ElevatorUpgrade.OnUpgrade -= UpgradeElevator;
    }
}