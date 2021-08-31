using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Systems")]
    public DialogueSystem DialogueSystem;
    public PlayerInputController PlayerInputController;

    private GameState _state = GameState.PlayerRoam;

    void Awake()
    {
        if (this != null)
            Destroy(gameObject);
        instance = this;
    }

    void Update()
    {
        if (_state == GameState.PlayerRoam)
            PlayerInputController.HandleUpdate();
        else if (_state == GameState.Dialogue)
            DialogueSystem.HandleUpdate();
    }

    enum GameState
    {
        PlayerRoam,
        Dialogue
    }
}
