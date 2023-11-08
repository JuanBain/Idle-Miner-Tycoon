using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [Header("Prefab")] [SerializeField] private GameObject warehouseMinerPrefab;

    [Header("Extras")] [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorLocation;
    [SerializeField] private Transform warehouseDepositLocation;

    [SerializeField] private List<WarehouseMiner> _miners;
    public List<WarehouseMiner> Miners => _miners;

    private void Start()
    {
        _miners = new List<WarehouseMiner>();
        AddMiner();
    }

    public void AddMiner()
    {
        GameObject newMiner = Instantiate(warehouseMinerPrefab, warehouseDepositLocation.position, quaternion.identity);
        WarehouseMiner miner = newMiner.GetComponent<WarehouseMiner>();
        miner.ElevatorDeposit = elevatorDeposit;
        miner.ElevatorDepositLocation = elevatorLocation;
        miner.WarehouseLocation = warehouseDepositLocation;
        _miners.Add(miner);
    }


    private void WarehouseMinerBoost(WarehouseManagerLocation warehouseManagerLocation)
    {
        switch (warehouseManagerLocation.Manager.BoostType)
        {
            case BoostType.Movement:
                foreach (WarehouseMiner miner in Miners)
                {
                    ManagersController.Instance.RunMovementBoost(miner, warehouseManagerLocation.Manager.boostDuration,
                        warehouseManagerLocation.Manager.boostValue);
                }

                break;
            case BoostType.Loading:
                foreach (WarehouseMiner miner in Miners)
                {
                    ManagersController.Instance.RunLoadingBoost(miner, warehouseManagerLocation.Manager.boostDuration,
                        warehouseManagerLocation.Manager.boostValue);
                }

                break;
        }
    }

    private void OnEnable()
    {
        WarehouseManagerLocation.OnBoost += WarehouseMinerBoost;
    }

    private void OnDisable()
    {
        WarehouseManagerLocation.OnBoost -= WarehouseMinerBoost;
    }
}