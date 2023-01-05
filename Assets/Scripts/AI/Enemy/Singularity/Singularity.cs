using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Scripts.Attributes;

public class Singularity : Enemy, ITarget, IStateMachine, ISpawnDanmaku
{
    public Transform target => this.player.transform;

    private AIStateMachine _stateMachine;
    public AIStateMachine stateMachine { get { return _stateMachine; } }

    private SpawnDanmakuHelper _danmakuHelper;
    public SpawnDanmakuHelper danmakuHelper { get { return _danmakuHelper; } }

    //====================

    [SerializeField]
    private Transform parent;
    [SerializeField]
    private float speed;
    [Inject]
    private Player player;

    protected override void EnemyAwake()
    {
        _stateMachine = GetComponent<AIStateMachine>();
        _danmakuHelper = GetComponent<SpawnDanmakuHelper>();

        _stateMachine.OnUpdateTransform.AddListener(UpdateTransform);
    }

    private void UpdateTransform()
    {
        Vector2 direction = (target.position - parent.position).normalized;
        parent.position += (Vector3)direction * speed * Time.fixedDeltaTime;
    }

    public override void OnKilled()
    {
        if (parent != null)
            DestroySafe(parent.gameObject);
    }
}
