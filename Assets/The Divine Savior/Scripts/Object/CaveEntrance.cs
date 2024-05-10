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
                CameraManager.cameraManagerInstance.SwitchCameraToSub(this.transform);
                foreach (Destructible rock in destructibleRocks)
                {
                    rock.BreakFracturedObject();
                }
                CameraManager.cameraManagerInstance.SwitchCameraToMain(GameObject.FindGameObjectWithTag("Player").transform);
                isAllEnemiesDead = true;
                
            }
            else
            {
                aliveEnemies = GameObject.FindObjectsOfType<Enemy>();
            }
        }
    }
}
