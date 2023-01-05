﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Reflex.Scripts.Attributes;

public class BossUI : GameBehaviour
{
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI bossName;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private UIOverlayChecker checker;

    private Enemy boss = null;

    [Inject]
    private Player player;

    public override void GameStart()
    {
        checker.update = false;
        EnemyManager.instance.OnBossSpawn.AddListener(ShowBossUI);
        player.OnDied.AddListener(DisableUI);
    }

    public override void GameUpdate()
    {
        if (boss != null)
            healthBar.value = boss.hp / boss.maxHP;
    }


    private void ShowBossUI(Enemy inputBoss)
    {
        boss = inputBoss;
        bossName.text = inputBoss.Name;
        boss.OnDeath += HideBossUI;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 0.5f);
        healthBar.value = 1;
        checker.update = true;
    }

    private void HideBossUI()
    {
        checker.update = false;
        boss = null;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 0.5f);
    }

    public void DisableUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
