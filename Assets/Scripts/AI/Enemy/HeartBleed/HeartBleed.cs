using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reflex.Scripts.Attributes;

public class HeartBleed : Enemy, ITarget, IStateMachine, ISpawnDanmaku
{
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
