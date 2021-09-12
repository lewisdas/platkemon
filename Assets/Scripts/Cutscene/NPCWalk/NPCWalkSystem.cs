using System;
using UnityEngine;

public class NPCWalkSystem : CutsceneSystem
{
    public event Action FinishedNPCWalk;
    
    public override void Run(CutsceneSegment segment)
    {
        // read data from segment
        var npcWalk = (NPCWalk) segment;
        var npc = npcWalk.npc;
        var startPos = npcWalk.startPos;
        var endPos = npcWalk.endPos;
        var endingDirection = npcWalk.endingDirection;

        // build path from NPC location to intended destination
        var path = BuildPath(startPos, endPos);
        
        npc.Move(path);
        
        // finish walking
        npc.FinishedWalking += () =>
        {
            npcWalk.npc.SetDirection(endingDirection);
            FinishedNPCWalk();
        };
    }

    // todo: proper implementation
    private NPCController.Direction[] BuildPath(Vector2 startPos, Vector2 endPos)
    {
        var steps = new []
        {
            NPCController.Direction.Right,
            NPCController.Direction.Right,
            NPCController.Direction.Down,
            NPCController.Direction.Left,
            NPCController.Direction.Up
        };
        return steps;
    }

    public override void HandleUpdate() { } // do nothing
}
