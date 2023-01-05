using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Reflex.Scripts.Attributes;

public class HurtUI : GameBehaviour
{
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private float fadeTime;
    [Inject]
    private Player player;

    public override void GameStart()
    {
        player.OnReceiveDamage += HurtVFX;
    }

    private void HurtVFX()
    {
        group.alpha = 1;
        group.DOFade(0, fadeTime);
    }
}
