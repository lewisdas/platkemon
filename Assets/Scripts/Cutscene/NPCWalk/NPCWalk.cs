using UnityEngine;

[System.Serializable]
public class NPCWalk: CutsceneSegment
{
    public NPCController npcPrefab;
    public Vector2 startPos;
    public Vector2 endPos;
    public NPCController.Direction endingDirection;
    [HideInInspector] public NPCController npc;

    private Vector3 _gridCorrection = new Vector3(.5f, .5f, 0);

    /// <summary>
    /// Create NPC at specified startPos.
    /// </summary>
    public void Init()
    {
        npc = Instantiate(npcPrefab, startPos, Quaternion.identity);
        npc.transform.position += _gridCorrection;
    }
}