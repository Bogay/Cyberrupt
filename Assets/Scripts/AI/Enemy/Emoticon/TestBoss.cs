using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Scripts.Attributes;

public class TestBoss : Enemy, ITarget, IStateMachine, ISpawnDanmaku
{
    /*
     Twemoji
      - Copyright 2020 Twitter, Inc and other contributors
      - Graphics licensed under CC-BY 4.0: 
        https://creativecommons.org/licenses/by/4.0/
    */

    public Transform target => this.player.transform;

    private AIStateMachine _stateMachine;
    public AIStateMachine stateMachine { get { return _stateMachine; } }

    private SpawnDanmakuHelper _danmakuHelper;
    public SpawnDanmakuHelper danmakuHelper { get { return _danmakuHelper; } }

    //====================

    [Inject]
    private Player player;

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject clearScreen;

    protected override void EnemyAwake()
    {
        _stateMachine = GetComponent<AIStateMachine>();
        _danmakuHelper = GetComponent<SpawnDanmakuHelper>();

        _stateMachine.OnUpdateTransform.AddListener(UpdateTransform);
        Death.AddAction(() => Instantiate(clearScreen));
    }

    private void UpdateTransform()
    {
        Vector2 direction = this.target.position - transform.position;
        transform.position += (Vector3)direction.normalized * speed * Time.fixedDeltaTime;
    }
}
