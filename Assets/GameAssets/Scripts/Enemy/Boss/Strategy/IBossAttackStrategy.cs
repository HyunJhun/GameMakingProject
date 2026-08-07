using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public interface IBossAttackStrategy
{
    string AttackName { get; }
    public UniTask ExecuteAttack(Boss boss,CancellationToken ct);
}
