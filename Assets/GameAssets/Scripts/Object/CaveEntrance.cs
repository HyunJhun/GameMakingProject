using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    private bool isAllEnemiesDead = false;
    [SerializeField] private EnemyGroup enemyGroup;
    private Destructible[] destructibleRocks;

    private void Awake()
    {
        if (enemyGroup == null)
            enemyGroup = GetComponent<EnemyGroup>();

        if (enemyGroup == null)
            enemyGroup = gameObject.AddComponent<EnemyGroup>();

        destructibleRocks = GetComponentsInChildren<Destructible>();
    }

    private void OnEnable()
    {
        if (enemyGroup != null)
            enemyGroup.OnAllEnemiesDied += HandleAllEnemiesDead;
    }

    private void OnDisable()
    {
        if (enemyGroup != null)
            enemyGroup.OnAllEnemiesDied -= HandleAllEnemiesDead;
    }

    private void HandleAllEnemiesDead()
    {
        if (isAllEnemiesDead)
            return;

        isAllEnemiesDead = true;

        SoundManager.soundManagerInstacne.initializeSFX();

        CameraManager.cameraManagerInstance.SwitchCameraToTarget(
            transform,
            CameraManager.cameraManagerInstance.caveCam
        );

        Invoke(nameof(breakEntrance), 4f);
    }
    private void breakEntrance()
    {
        foreach (Destructible rock in destructibleRocks)
        {
            rock.BreakFracturedObject();
        }
        Invoke("cameBackToMain", 2f);
    }

    private void cameBackToMain()
    {
        CameraManager.cameraManagerInstance.SwitchCameraToMain
            (GameObject.FindGameObjectWithTag("Player").transform, 
            CameraManager.cameraManagerInstance.caveCam
            );
    }
}
