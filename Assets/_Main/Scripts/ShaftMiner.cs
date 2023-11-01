using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShaftMiner : BaseMiner
{
    public Shaft CurrentShaft { get; set; }
    private static readonly int Mining = Animator.StringToHash("Mining");
    private static readonly int Walking = Animator.StringToHash("Walking");


    public override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(Walking);
    }

    protected override void CollectGold()
    {
        float collectTime = CollectCapacity / CollectPerSecond;
        _animator.SetTrigger(Mining);
        StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);
        CurrentGold = collectGold;
        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(CurrentShaft.DepositLocation.position);
    }

    protected override void DepositGold()
    {
        CurrentShaft.CurrentDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(CurrentShaft.MiningLocation.position);
    }
}