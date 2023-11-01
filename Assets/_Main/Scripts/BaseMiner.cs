using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BaseMiner : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int initialCollectCapacity = 200;
    [SerializeField] private float goldCollectPerSecond = 50f;
    public float MoveSpeed { get; set; }
    public int CurrentGold { get; set; }
    public int CollectCapacity { get; set; }
    public float CollectPerSecond { get; set; }
    public bool IsTimeToCollect { get; set; }
    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        IsTimeToCollect = true;
        MoveSpeed = moveSpeed;
        CurrentGold = 0;
        CollectCapacity = initialCollectCapacity;
        CollectPerSecond = goldCollectPerSecond;
    }

    public virtual void MoveMiner(Vector3 newPosition)
    {
        transform.DOMove(newPosition, 10f / MoveSpeed).OnComplete(() =>
        {
            if (IsTimeToCollect)
            {
                CollectGold();
            }
            else
            {
                DepositGold();
            }
        }).Play();
    }

    protected virtual void CollectGold()
    {
    }

    protected virtual IEnumerator IECollect(int collectGold, float collectTime)
    {
        yield return null;
    }

    protected virtual void DepositGold()
    {
    }

    public void RotateMiner(int direction)
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void ChangeGoal()
    {
        IsTimeToCollect = !IsTimeToCollect;
    }
}