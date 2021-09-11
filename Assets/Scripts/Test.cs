using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Cutscene cutscene;

    void Start()
    {
        GameManager.instance.cutsceneController.RunCutscene(cutscene);
    }
}
