using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ManagersController : Singleton<ManagersController>
{
    [SerializeField] private ManagerCard managerCardPrefab;
    [SerializeField] private int initialManagerCost = 100;
    [SerializeField] private int managerCostMultiplier = 3;

    [Header("Assign Manager UI")] [SerializeField]
    private Image managerIcon;

    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerName;
    [SerializeField] private TextMeshProUGUI managerLevel;
    [SerializeField] private TextMeshProUGUI boostEffect;
    [SerializeField] private TextMeshProUGUI boostDescription;

    [SerializeField] private TextMeshProUGUI managerPanelTitle;
    [SerializeField] private Transform managersContainer;
    [SerializeField] private GameObject assignedManagerPanel;
    [SerializeField] private GameObject managerPanel;
    [SerializeField] private List<Manager> managerList;

    public BaseManagerLocation CurrentManagerLocation { get; set; }
    public int NewManagerCost { get; set; }
    private List<ManagerCard> _assignedManagerCards = new List<ManagerCard>();
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        NewManagerCost = initialManagerCost;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.transform.GetComponent<MineManager>() != null)
                {
                    CurrentManagerLocation = hitInfo.transform.GetComponent<MineManager>().Location;
                    OpenPanel(true);
                }
            }
        }
    }

    public void UnassignManager()
    {
        RestoreManagerCard(CurrentManagerLocation.Manager);
        CurrentManagerLocation.Manager = null;
        UpdateAssignedManagerInfo(CurrentManagerLocation);
    }

    public void HireManger()
    {
        if (GoldManager.Instance.CurrentGold >= NewManagerCost && managerList.Count > 0)
        {
            ManagerCard card = Instantiate(managerCardPrefab, managersContainer);
            //Get Random Manager
            int randomIndex = Random.Range(0, managerList.Count);
            Manager randomManager = managerList[randomIndex];
            card.SetupManagerCard(randomManager);
            managerList.RemoveAt(randomIndex);
            GoldManager.Instance.RemoveGold(NewManagerCost);
            NewManagerCost *= managerCostMultiplier;
        }
    }

    public void UpdateAssignedManagerInfo(BaseManagerLocation managerLocation)
    {
        if (managerLocation.Manager != null)
        {
            managerIcon.sprite = managerLocation.Manager.managerIcon;
            boostIcon.sprite = managerLocation.Manager.boostIcon;
            managerName.text = managerLocation.Manager.managerName;
            managerLevel.text = managerLocation.Manager.ManagerLevel.ToString();
            boostEffect.text = managerLocation.Manager.boostDuration.ToString();
            boostDescription.text = managerLocation.Manager.boostDescription;
            assignedManagerPanel.SetActive(true);
        }
        else
        {
            managerIcon.sprite = null;
            boostIcon.sprite = null;
            managerName.text = null;
            managerLevel.text = null;
            boostEffect.text = null;
            boostDescription.text = null;
            assignedManagerPanel.SetActive(false);
        }
    }

    public void AddAssignedManagerCard(ManagerCard card)
    {
        _assignedManagerCards.Add(card);
    }


    public void OpenPanel(bool value)
    {
        managerPanel.SetActive(value);
        if (value)
        {
            managerPanelTitle.text = CurrentManagerLocation.LocationTitle;
            UpdateAssignedManagerInfo(CurrentManagerLocation);
        }
    }

    private void RestoreManagerCard(Manager manager)
    {
        ManagerCard managerCard = null;
        foreach (var card in _assignedManagerCards)
        {
            if (card.Manager.managerName == manager.managerName)
            {
                card.gameObject.SetActive(true);
                managerCard = card;
            }
        }

        if (managerCard != null)
        {
            _assignedManagerCards.Remove(managerCard);
        }
    }
}