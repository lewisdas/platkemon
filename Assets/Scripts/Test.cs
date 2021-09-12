using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Cutscene cutscene;

    // todo: add this preloader in wherever we wind up storing actual Cutscenes
    void Awake()
    {
        foreach(var segment in cutscene.segments)
            if (segment is NPCWalk walk)
                walk.Init();
    }
    
    void Start()
    {
        GameManager.instance.cutsceneController.RunCutscene(cutscene);
    }
}
