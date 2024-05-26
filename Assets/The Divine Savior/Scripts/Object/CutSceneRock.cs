using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneRock : MonoBehaviour
{
    private Destructible[] rocks;
    // Start is called before the first frame update
    void Start()
    {
        rocks = this.GetComponentsInChildren<Destructible>();

        foreach (Destructible rock in rocks)
        {
            rock.BreakFracturedObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
