using System;
using System.Collections;
using UnityEngine;

public class PauseSystem : CutsceneSystem
{
    public event Action finishedPausing;
    
    public override void Run(CutsceneSegment segment)
    {
        var pause = (Pause) segment;
        StartCoroutine(ExecutePause(pause.duration));
    }

    public override void HandleUpdate()
    {
        // do nothing
    }

    private IEnumerator ExecutePause(float duration)
    {
        yield return new WaitForSeconds(duration);
        finishedPausing();
    }
}
