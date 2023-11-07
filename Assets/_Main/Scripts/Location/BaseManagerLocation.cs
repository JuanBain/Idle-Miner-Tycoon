using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerLocation : MonoBehaviour
{
    [SerializeField] private string locationTitle;
    public string LocationTitle => locationTitle;
    public Manager Manager { get; set; }

    public virtual void RunBoost()
    {
    }
}