using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    // subsystems
    private DialogueSystem _dialogueSystem;
    
    // trackers
    private CutsceneSegment[] _segments;
    private int _currentSegment;
    private CutsceneSystem _currentSystem;

    // -------- Unity Events ---------------------------------------------------
    
    /// <summary>
    /// Get references to cutscene segment controllers and add listeners.
    /// </summary>
    private void Awake()
    {
        // grab references
        _dialogueSystem = GetComponent<DialogueSystem>();

        // add listeners
        _dialogueSystem.dialogueFinished += OnSegmentFinished;
    }
    
    // -------- Public Methods -------------------------------------------------

    public void RunCutscene(Cutscene cutscene)
    {
        GameManager.instance.state = GameManager.GameState.Cutscene;
        _currentSegment = 0;
        _segments = cutscene.segments;
        
        RunNextCutsceneSegment();
    }

    public void HandleUpdate()
    {
        _dialogueSystem.HandleUpdate();
    }
    
    // -------- Private Methods ------------------------------------------------

    private void RunNextCutsceneSegment()
    {
        var segment = _segments[_currentSegment];
        if (segment is Dialogue dialogue)
        {
            _currentSystem = _dialogueSystem;
            _dialogueSystem.Run(dialogue);
        }
    }

    /// <summary>
    /// Run next cutscene segment or finish cutscene.
    /// </summary>
    private void OnSegmentFinished()
    {
        _currentSegment++;
        if (_currentSegment == _segments.Length)
            FinishCutscene();
        else
            RunNextCutsceneSegment();
    }

    /// <summary>
    /// Restore control to player.
    /// </summary>
    private void FinishCutscene()
    {
        GameManager.instance.state = GameManager.GameState.PlayerRoam;
    }
}
