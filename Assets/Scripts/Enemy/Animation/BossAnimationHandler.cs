using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationHandler : MonoBehaviour
{
    private Animator bossAnimator;

    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
    }

    public void animationUpdate()
    {
        return;
    }
}
