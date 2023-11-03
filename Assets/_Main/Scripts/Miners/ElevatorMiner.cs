using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
    [SerializeField] private Elevator _elevator;
    private int _currentShaftIndex = -1;
    private Deposit _currentDeposit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToNextLocation();
        }
    }

    public void MoveToNextLocation()
    {
        _currentShaftIndex++;
        Shaft currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
        Vector2 nextPosition = currentShaft.DepositLocation.position;
        Vector2 fixedPosition = new Vector2(transform.position.x, nextPosition.y);
        _currentDeposit = currentShaft.CurrentDeposit;
        MoveMiner(fixedPosition);
    }

    protected override void CollectGold()
    {
        if (!_currentDeposit.CanCollectGold() && _currentDeposit != null &&
            _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1)
        {
            _currentShaftIndex = -1;
            ChangeGoal();
            Vector3 elevatorDepositPosition = new Vector3(transform.position.x, _elevator.DepositLocation.position.y);
            MoveMiner(elevatorDepositPosition);
            return;
        }

        int amountToCollect = _currentDeposit.CollectGold(this);
        float collectTime = amountToCollect / CollectPerSecond;
        OnLoading?.Invoke(this, collectTime);

        StartCoroutine(IECollect(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        if (CurrentGold > 0 && CurrentGold < CollectCapacity)
        {
            CurrentGold += collectGold;
        }
        else
        {
            CurrentGold = collectGold;
        }

        _currentDeposit.RemoveGold(collectGold);
        yield return new WaitForSeconds(0.5f);

        if (CurrentGold == CollectCapacity || _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1)
        {
            _currentShaftIndex = -1;
            ChangeGoal();
            Vector3 elevatorDepositPosition = new Vector3(transform.position.x, _elevator.DepositLocation.position.y);
            MoveMiner(elevatorDepositPosition);
        }
        else
        {
            MoveToNextLocation();
        }
    }

    protected override void DepositGold()
    {
        if (CurrentGold <= 0f)
        {
            _currentShaftIndex = -1;
            ChangeGoal();
            MoveToNextLocation();
            return;
        }

        float depositTime = CurrentGold / CollectPerSecond;
        OnLoading?.Invoke(this, depositTime);
        StartCoroutine(IEDeposit(CurrentGold, depositTime));
    }

    protected override IEnumerator IEDeposit(int goldCollected, float depositTime)
    {
        yield return new WaitForSeconds(depositTime);
        _elevator.ElevatorDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;
        _currentShaftIndex = -1;
        //Update goal and move to next location
        ChangeGoal();
        MoveToNextLocation();
    }
}