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
    public List<ShaftMiner> Miners => _miners;
    public Deposit CurrentDeposit { get; set; }
    private GameObject _minersContainer;
    private List<ShaftMiner> _miners;

    private void Start()
    {
        _miners = new List<ShaftMiner>();
        _minersContainer = new GameObject("Miners");
        CreateMiner();
        CreateDeposit();
    }

    public void CreateMiner()
    {
        ShaftMiner newMiner = Instantiate(minerPrefab, depositLocation.position, quaternion.identity);
        newMiner.CurrentShaft = this;
        newMiner.MoveMiner(miningLocation.position);
        newMiner.transform.SetParent(_minersContainer.transform);
        newMiner.name = minerPrefab.name;

        if (_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(newMiner);
    }

    private void CreateDeposit()
    {
        Deposit newDeposit = Instantiate(depositPrefab, shaftDepositPosition.position, quaternion.identity);
        CurrentDeposit = newDeposit;
        CurrentDeposit.transform.SetParent(transform);
        newDeposit.name = depositPrefab.name;
    }
}