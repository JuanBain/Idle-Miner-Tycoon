using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{
    [SerializeField] private Shaft shaftPrefab;
    [SerializeField] private float newShaftYPosition;
    public int NewShaftCost => newShaftCost;
    private int _currentShaftIndex;
    [SerializeField] private int newShaftCost = 500;

    [Header("Shaft")] [SerializeField] private List<Shaft> _shafts;
    public List<Shaft> Shafts => _shafts;

    private void Start()
    {
        _shafts[0].ShaftId = 0;
    }

    public void AddShaft()
    {
        Transform lastShaft = _shafts.Last().transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, quaternion.identity);
        newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition,
            lastShaft.position.z);
        
        _currentShaftIndex++;
        
        newShaft.ShaftId = _currentShaftIndex;
        _shafts.Add(newShaft);
    }
}