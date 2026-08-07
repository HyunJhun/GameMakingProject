using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
public class BreathAttackPattern : IBossAttackStrategy
{
    public string AttackName => "Breath";
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
        Debug.Log("Breath Attack!");
        // Implement the breath attack logic here
        // For example, you can make the boss perform a breath attack animation
        // and deal damage to players in front of it.
        await UniTask.Yield(); // Ensure this runs on the main thread
    }
}
