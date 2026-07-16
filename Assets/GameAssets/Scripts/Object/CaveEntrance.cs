using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    private bool isAllEnemiesDead = false;
    private Enemy[] aliveEnemies;
    private Destructible[] destructibleRocks;
    // Start is called before the first frame update
    void Start()
    {
        aliveEnemies = GameObject.FindObjectsOfType<Enemy>();
        destructibleRocks = this.GetComponentsInChildren<Destructible>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAllEnemiesDead)
        {
            if (aliveEnemies.Length == 0)
            {
                isAllEnemiesDead = true;
                SoundManager.soundManagerInstacne.initializeSFX();
                CameraManager.cameraManagerInstance.SwitchCameraToTarget(
                    transform,CameraManager.cameraManagerInstance.caveCam
                    );
                Invoke("breakEntrance", 4f);         
            }
            else
            {
                aliveEnemies = GameObject.FindObjectsOfType<Enemy>();
            }
        }
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
