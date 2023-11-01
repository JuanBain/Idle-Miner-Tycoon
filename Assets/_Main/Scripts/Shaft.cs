using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Prefab")] [SerializeField] private ShaftMiner minerPrefab;
    [SerializeField] private Deposit depositPrefab;

    [Header("Locations")] [SerializeField] private Transform miningLocation;
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Transform shaftDepositPosition;
    public Transform MiningLocation => miningLocation;
    public Transform DepositLocation => depositLocation;
    public Deposit CurrentDeposit { get; set; }
    private GameObject _minersContainer;

    private void Start()
    {
        _minersContainer = new GameObject("Miners");
        CreateMiner();
        CreateDeposit();
    }

    private void CreateMiner()
    {
        ShaftMiner newMiner = Instantiate(minerPrefab, depositLocation.position, quaternion.identity);
        newMiner.CurrentShaft = this;
        newMiner.MoveMiner(miningLocation.position);
        newMiner.transform.SetParent(_minersContainer.transform);
        newMiner.name = minerPrefab.name;
    }

    private void CreateDeposit()
    {
        Deposit newDeposit = Instantiate(depositPrefab, shaftDepositPosition.position, quaternion.identity);
        CurrentDeposit = newDeposit;
        CurrentDeposit.transform.SetParent(transform);
        newDeposit.name = depositPrefab.name;
    }
}