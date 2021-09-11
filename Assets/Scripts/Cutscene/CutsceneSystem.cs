using System;
using UnityEngine;

public abstract class CutsceneSystem : MonoBehaviour
{
    public abstract void Run(CutsceneSegment segment);

    public abstract void HandleUpdate();
}
