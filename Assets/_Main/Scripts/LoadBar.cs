using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{
    [SerializeField] private GameObject loadingBarPrefab;

    [SerializeField] private Transform loadingBarPosition;
    public Transform BarContainer { get; set; }
    private Image _fillImage;
    private BaseMiner _miner;
    private GameObject _barCanvas;

    private void Start()
    {
        _miner = GetComponent<BaseMiner>();
        CreateLoadBar();
        BarContainer = _barCanvas.transform;
    }

    private void CreateLoadBar()
    {
        _barCanvas = Instantiate(loadingBarPrefab, loadingBarPosition.position, quaternion.identity);
        _barCanvas.transform.SetParent(transform);
        _fillImage = _barCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
    }

    private void LoadingBar(BaseMiner minerSender, float duration)
    {
        if (_miner == minerSender)
        {
            _barCanvas.gameObject.SetActive(true);
            _fillImage.fillAmount = 0;
            _fillImage.DOFillAmount(1f, duration).OnComplete(() => _barCanvas.SetActive(false));
        }
    }

    private void OnEnable()
    {
        BaseMiner.OnLoading += LoadingBar;
    }


    private void OnDisable()
    {
        BaseMiner.OnLoading -= LoadingBar;
    }
}