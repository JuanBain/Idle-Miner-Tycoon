using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private ElevatorMiner miner;
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform depositLocation;
    public ElevatorMiner Miner => miner;
    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform DepositLocation => depositLocation;
}