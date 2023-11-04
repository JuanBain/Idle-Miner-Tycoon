using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
    public static Action<ElevatorUpgrade> OnUpgradeRequest;
    [SerializeField] private TextMeshProUGUI elevatorDepositGold;
    private Elevator _elevator;
    private ElevatorUpgrade _elevatorUpgrade;

    private void Start()
    {
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();
        _elevator = GetComponent<Elevator>();
    }

    private void Update()
    {
        elevatorDepositGold.text = _elevator.ElevatorDeposit.CurrentGold.ToString();
    }

    public void RequestUpgrade()
    {
        OnUpgradeRequest?.Invoke(_elevatorUpgrade);
    }
}