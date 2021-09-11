using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Systems")]
    public DialogueSystem dialogueSystem;
    public PlayerInputController playerInputController;

    [HideInInspector] public GameState state = GameState.PlayerRoam;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        dialogueSystem.dialogueFinished += OnDialogueFinished;
    }

    private void OnDialogueFinished()
    {
        state = GameState.PlayerRoam;
    }

    void Update()
    {
        if (state == GameState.PlayerRoam)
            playerInputController.HandleUpdate();
        else if (state == GameState.Dialogue)
            dialogueSystem.HandleUpdate();
    }

    public void ChangeStateToDialogue()
    {
        state = GameState.Dialogue;
    }

    public enum GameState
    {
        PlayerRoam,
        Dialogue
    }
}
