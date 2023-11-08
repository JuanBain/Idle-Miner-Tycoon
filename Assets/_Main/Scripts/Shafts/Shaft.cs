using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Shaft : MonoBehaviour
{
    [Header("Prefab")] [SerializeField] private ShaftMiner minerPrefab;
    [SerializeField] private Deposit depositPrefab;

    [Header("Locations")] [SerializeField] private Transform miningLocation;
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Transform shaftDepositPosition;

    [Header("Manager")] [SerializeField] private Transform managerPosition;
    [SerializeField] private GameObject managerPrefab;
    public Transform MiningLocation => miningLocation;
    public Transform DepositLocation => depositLocation;
    public List<ShaftMiner> Miners => _miners;
    public Deposit CurrentDeposit { get; set; }

    public int ShaftId { get; set; }

    private GameObject _minersContainer;
    private List<ShaftMiner> _miners;
    private ShaftManagerLocation _shaftManagerLocation;

    private void Start()
    {
        _shaftManagerLocation = GetComponent<ShaftManagerLocation>();
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

    public void CreateManager()
    {
        GameObject shaftManager = Instantiate(managerPrefab, managerPosition.position, quaternion.identity);
        MineManager mineManager = shaftManager.GetComponent<MineManager>();
        mineManager.SetupManager(_shaftManagerLocation);
        _shaftManagerLocation.MineManager = mineManager;
    }

    private void CreateDeposit()
    {
        Deposit newDeposit = Instantiate(depositPrefab, shaftDepositPosition.position, quaternion.identity);
        CurrentDeposit = newDeposit;
        CurrentDeposit.transform.SetParent(transform);
        newDeposit.name = depositPrefab.name;
    }

    private void ShaftBoost(Shaft shaft, ShaftManagerLocation shaftManagerLocation)
    {
        if (shaft == this)
        {
            switch (shaftManagerLocation.Manager.BoostType)
            {
                case BoostType.Movement:
                    foreach (ShaftMiner shaftMiner in _miners)
                    {
                        ManagersController.Instance.RunMovementBoost(shaftMiner,
                            shaftManagerLocation.Manager.boostDuration, shaftManagerLocation.Manager.boostValue);
                    }

                    break;
                case BoostType.Loading:
                    foreach (ShaftMiner shaftMiner in _miners)
                    {
                        ManagersController.Instance.RunLoadingBoost(shaftMiner,
                            shaftManagerLocation.Manager.boostDuration, shaftManagerLocation.Manager.boostValue);
                    }

                    break;
            }
        }
    }

    private void OnEnable()
    {
        ShaftManagerLocation.OnBoost += ShaftBoost;
    }


    private void OnDisable()
    {
        ShaftManagerLocation.OnBoost += ShaftBoost;
    }
}