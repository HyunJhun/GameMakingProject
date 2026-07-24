using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
public class Firebomb : MonoBehaviour,IPoolable<Firebomb>
{
    [SerializeField] private GameObject explodeArea;
    [SerializeField] private TMP_Text timerText;

    private Destructible fractured;
    public float timer { get; set; }
    private bool collisionObstacle;
    private bool isBoom;


    private IObjectPool<Firebomb> _pool;
    public void SetPool(IObjectPool<Firebomb> pool)
    {
        _pool = pool;
    }
    public void ReturnToPool() => _pool.Release(this);
    private async UniTaskVoid ReturnToPoolDelayed(float delayTime, CancellationToken tk = default)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(delayTime));
        ReturnToPool();
    }
    private void Start()
    {
        timer = 7f;
        collisionObstacle = false;
        isBoom = false;
        fractured = GetComponent<Destructible>();
    }

    private void Update()
    {
        timerText.text = ((int)timer).ToString();
        if (collisionObstacle)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.1f) explodeArea.SetActive(true);
            if (timer <= 0f&& !isBoom)
            {
                isBoom = true;
                // 수정됨: 중앙 ParticleManager의 Firebomb 전용 풀을 사용
                ParticleManager.Instance.Play(
                    ParticleType.FirebombExplosion,
                    transform.position,
                    Quaternion.identity);
                fractured.BreakFracturedObject();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.CompareTag("Player")) // 플레이어와 충돌했을 경우
        {
            //SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Bomb, false);
            // 수정됨: 중앙 ParticleManager의 Firebomb 전용 풀을 사용
            ParticleManager.Instance.Play(
                ParticleType.FirebombExplosion,
                transform.position,
                Quaternion.identity);
            ReturnToPoolDelayed(0.1f).Forget();
        }
        else
        {
            collisionObstacle = true;
        }
    }
}
