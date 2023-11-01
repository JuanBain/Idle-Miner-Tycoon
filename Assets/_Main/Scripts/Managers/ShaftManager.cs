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
    [SerializeField] private int newShaftCost = 500;

    [Header("Shaft")] [SerializeField] private List<Shaft> _shafts;
    public List<Shaft> Shafts => _shafts;
    public int NewShaftCost => newShaftCost;


    public void AddShaft()
    {
        Transform lastShaft = _shafts.Last().transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, quaternion.identity);
        newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition,
            lastShaft.position.z);
        _shafts.Add(newShaft);
    }
}