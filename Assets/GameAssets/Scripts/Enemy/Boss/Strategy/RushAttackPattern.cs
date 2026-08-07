using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RushAttackPattern : IBossAttackStrategy
{
    public string AttackName => "Rush";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask ExecuteAttack(Boss boss, CancellationToken ct)
    {
        Debug.Log("Rush Attack!");
        // Implement the rush attack logic here
        // For example, you can move the boss towards the player rapidly
        // and deal damage upon collision.
        await UniTask.Yield(); // Ensure this runs on the main thread
    }
}
