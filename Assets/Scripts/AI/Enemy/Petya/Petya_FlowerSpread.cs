﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UltEvents;

public class Petya_FlowerSpread : AITimerState
{
    [SerializeField]
    public UltEvent OnTweenOver = new UltEvent();
    [SerializeField]
    private float tweenTime;

    protected override void OnStateEnter()
    {
        base.OnStateEnter();
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        transform.DOMove(Vector3.zero, tweenTime);
        yield return new WaitForSeconds(tweenTime);
        OnTweenOver.Invoke();
    }
}
