﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Reflex;
using Reflex.Scripts.Attributes;

public class DanmakuManager : GameBehaviour
{
    public bool usePS;
    private static DanmakuManager _instance = null;
    public static DanmakuManager instance { get { return _instance; } private set { } }

    [SerializeField]
    private GameObject poolPrefab;
    [SerializeField]
    private GameObject poolParticle;

    private Queue<SpriteRenderer> pool = new Queue<SpriteRenderer>();
    private Queue<ParticleSystem> particlePool = new Queue<ParticleSystem>();
    [SerializeField]
    private int poolSize = 2000;

    [SerializeField]
    private int poolCount;

    [SerializeField]
    private Bound _bound;

    [SerializeField]
    private DanmakuTarget _target = null;
    public DanmakuTarget target { get { return _target; } private set { } }

    private List<DanmakuObstacle> obstacles = new List<DanmakuObstacle>();

    private UnityEvent _bulletHitTarget = new UnityEvent();
    public UnityEvent bulletHitTarget { get { return _bulletHitTarget; } private set { } }

    private List<IDanmaku> danmakus = new List<IDanmaku>();

    [Inject]
    private readonly Container container;

    public override void GameAwake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            KillBehaviour(true);
            return;
        }

        _bound.parent = transform;

        for (int i = 0; i < poolSize; i++)
        {
            //聽著，給我聽著
            //不要在Assert裡寫入會影響程式主邏輯運行的東西，Assertion在正式的Build裡會被自動忽略
            //所以除非這段程式碼只會在Editor裡用到，不然絕對不要這麼做，絕！對！不！要！
            //底下兩行被封印的Assertion就是血淋淋的例子，在此保留以做教訓
            //請未來的我或是看到這個註解的人在撒旦的榮光面前發誓別犯這個智障錯誤

            GameObject newObject = this.container.InstantiateGameObject(poolPrefab);
            SpriteRenderer sr = newObject.GetComponent<SpriteRenderer>();
            //Assert.IsTrue(newObject.TryGetComponent(out sr), "There's no sprite renderer on the game object.");
            sr.sortingOrder = i;
            newObject.SetActive(false);
            pool.Enqueue(sr);

            newObject = this.container.InstantiateGameObject(poolParticle);
            ParticleSystem ps = newObject.GetComponent<ParticleSystem>();
            //Assert.IsTrue(newObject.TryGetComponent(out ps), "There's no particle system on the game object.");
            newObject.SetActive(false);
            particlePool.Enqueue(ps);
        }
    }

    public override void GameFixedUpdate()
    {
        poolCount = pool.Count;
        UpdateDanmaku();
    }

    private void UpdateDanmaku()
    {
        foreach (IDanmaku danmaku in danmakus)
            danmaku.Update(Time.fixedDeltaTime);

        for (int i = danmakus.Count - 1; i >= 0; i--)
        {
            if (danmakus[i].spawnBullets == false && danmakus[i].activeCount <= 0)
            {
                danmakus[i].OnDispose();
                danmakus.RemoveAt(i);
            }
        }
    }

    public void RequestBullets(int count, ref List<SpriteRenderer> sprites)
    {
        if (pool.Count < count)
            return;

        for (int i = 0; i < count; i++)
        {
            SpriteRenderer sr = pool.Dequeue();
            sr.gameObject.SetActive(true);
            sprites.Add(sr);
        }
    }

    public void RequestParticlePlay(Vector2 position)
    {
        if (usePS == false)
            return;
        ParticleSystem ps = particlePool.Dequeue();
        ps.gameObject.transform.position = position;
        ps.gameObject.SetActive(true);
        ps.Play();
        particlePool.Enqueue(ps);
    }

    public void RecycleBullet(ref Queue<SpriteRenderer> sprites)
    {
        while (sprites.Count > 0)
            pool.Enqueue(sprites.Dequeue());
    }

    public void SetDanmakuTarget(IDanmakuTarget target)
    {
        _target = new DanmakuTarget(target);
    }

    public DanmakuObstacle AddObstacle(IDanmakuTarget target)
    {
        DanmakuObstacle obstacle = new DanmakuObstacle(target);
        obstacles.Add(obstacle);
        return obstacle;
    }

    public void RemoveObstacle(DanmakuObstacle obstacle)
    {
        obstacles.Remove(obstacle);
    }

    public bool OverBound(Vector2 position)
    {
        if (_bound.OverBound(position))
            return true;
        foreach (DanmakuObstacle obstacle in obstacles)
            if (obstacle.TouchObstacle(position, 0))
                return true;
        return false;
    }

    public bool TouchTarget(Vector2 position, float radius)
    {
        if (_target == null)
            return false;
        bool result = _target.TouchTarget(position, radius);
        if (result)
            bulletHitTarget.Invoke();
        return result;
    }

    public void AddDanmaku(IDanmaku danmaku)
    {
        danmakus.Add(danmaku);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(_bound.xSize, _bound.ySize));
    }
}
