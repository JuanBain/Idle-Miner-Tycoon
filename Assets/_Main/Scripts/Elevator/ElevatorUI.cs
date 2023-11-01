using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI elevatorDepositGold;
    private Elevator _elevator;

    private void Start()
    {
        _elevator = GetComponent<Elevator>();
    }

    private void Update()
    {
        elevatorDepositGold.text = _elevator.ElevatorDeposit.CurrentGold.ToString();
    }
}