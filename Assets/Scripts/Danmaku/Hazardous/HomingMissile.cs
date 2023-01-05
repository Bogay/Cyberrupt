﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Scripts.Attributes;

public class HomingMissile : Enemy, ITarget, ISpawnDanmaku
{
    public Transform target => this.player.transform;

    private SpawnDanmakuHelper _danmakuHelper;
    public SpawnDanmakuHelper danmakuHelper { get { return _danmakuHelper; } }

    [Inject]
    private Player player;

    [SerializeField]
    private Transform aimer;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float speed;

    private float timer = 0;

    private Vector2 velocityDirection;

    protected override void EnemyAwake()
    {
        _danmakuHelper = GetComponent<SpawnDanmakuHelper>();

        float rand = Random.Range(0, 2 * Mathf.PI);

        velocityDirection = new Vector2(Mathf.Cos(rand), Mathf.Sin(rand));

        float angle = rand * Mathf.Rad2Deg;
        aimer.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void GameFixedUpdate()
    {
        if (timer >= lifeTime)
            OnDeath.Invoke();

        Vector2 targetDirection = (this.target.position - transform.position).normalized;
        velocityDirection = Vector2.Lerp(velocityDirection, targetDirection, 0.035f).normalized;
        transform.position += (Vector3)velocityDirection * speed * Time.fixedDeltaTime;

        timer += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollisionLayer.CollideWithMask(collision.gameObject, CollisionLayer.instance.playerMask))
        {
            if (!player.isImmune)
            {
                player.OnHit();
                Die();
            }
        }

        if (CollisionLayer.CollideWithMask(collision.gameObject, CollisionLayer.instance.screenMask))
            OnDeath.Invoke();

        if (CollisionLayer.CollideWithMask(collision.gameObject, CollisionLayer.instance.bombMask))
            Die();
    }

    public override void OnKilled()
    {
        base.OnKilled();
        DanmakuManager.instance.RequestParticlePlay(transform.position);
    }
}
