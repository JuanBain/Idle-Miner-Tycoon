using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineManager : MonoBehaviour
{
    [SerializeField] private BaseManagerLocation location;
    [SerializeField] private Image boostImage;
    public BaseManagerLocation Location => location;
    public ShaftManagerLocation ShaftManagerLocation { get; set; }
    public Image BoostImage { get; set; }

    public void SetupManager(BaseManagerLocation baseManagerLocation)
    {
        BoostImage = boostImage;
        location = baseManagerLocation;
        ShaftManagerLocation = baseManagerLocation as ShaftManagerLocation;
    }

    public void RunBoost()
    {
        ShaftManagerLocation.RunBoost();
    }
}