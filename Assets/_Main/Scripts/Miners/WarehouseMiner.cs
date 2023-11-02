using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseMiner : BaseMiner
{
    public Deposit ElevatorDeposit { get; set; }
    public Transform ElevatorDepositLocation { get; set; }
    public Transform WarehouseLocation { get; set; }
    private readonly int _walkingNoGold = Animator.StringToHash("WalkingNoGold");
    private readonly int _walkingWithGold = Animator.StringToHash("WalkingWithGold");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RotateMiner(-1);
            _animator.SetBool(_walkingNoGold, true);
            MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
        }
    }

    protected override void CollectGold()
    {
        if (ElevatorDeposit.CurrentGold <= 0)
        {
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(new Vector2(WarehouseLocation.position.x, transform.position.y));
            return;
        }

        _animator.SetBool(_walkingNoGold, false);


        int currentGold = ElevatorDeposit.CollectGold(this);
        float collectTime = CollectCapacity / CollectPerSecond;

        StartCoroutine(IECollect(currentGold, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        CurrentGold = collectGold;
        ElevatorDeposit.RemoveGold(collectGold);
        _animator.SetBool(_walkingWithGold, true);
        RotateMiner(1);
        ChangeGoal();
        MoveMiner(new Vector2(WarehouseLocation.position.x, transform.position.y));
    }

    protected override void DepositGold()
    {
        if (CurrentGold <= 0)
        {
            RotateMiner(-1);
            ChangeGoal();
            MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
            return;
        }

        _animator.SetBool(_walkingWithGold, false);
        _animator.SetBool(_walkingNoGold, false);
        float depositTime = CurrentGold / CollectPerSecond;

        StartCoroutine(IEDeposit(CurrentGold, depositTime));
    }

    protected override IEnumerator IEDeposit(int goldCollected, float depositTime)
    {
        yield return new WaitForSeconds(depositTime);
        GoldManager.Instance.AddGold(CurrentGold);
        CurrentGold = 0;
        RotateMiner(-1);
        ChangeGoal();
        MoveMiner(new Vector2(ElevatorDepositLocation.position.x, transform.position.y));
    }
}