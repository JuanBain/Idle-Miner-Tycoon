using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    public int CurrentGold { get; set; }


    public void DepositGold(int amount)
    {
        CurrentGold += amount;
    }

    public void RemoveGold(int amount)
    {
        CurrentGold -= amount;
    }

    public int CollectGold(BaseMiner miner)
    {
        int collectCapacity = miner.CollectCapacity;
        int currentGold = miner.CurrentGold;
        int minerCapacity = collectCapacity - currentGold;
        return EvaluateAmountToCollect(minerCapacity);
    }

    private int EvaluateAmountToCollect(int minerCollectCapacity)
    {
        if (minerCollectCapacity <= CurrentGold)
        {
            return minerCollectCapacity;
        }
        else
        {
            return CurrentGold;
        }
    }

    public bool CanCollectGold()
    {
        return CurrentGold > 0;
    }
}