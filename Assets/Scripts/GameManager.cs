using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Systems")] 
    public CutsceneController cutsceneController;
    public PlayerInputController playerInputController;

    [HideInInspector] public GameState state = GameState.PlayerRoam;

    // -------- Unity Events ---------------------------------------------------
    
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Update()
    {
        if (state == GameState.PlayerRoam)
            playerInputController.HandleUpdate();
        else if (state == GameState.Cutscene) 
            cutsceneController.HandleUpdate();
    }
    
    // -------- Public Methods -------------------------------------------------

    public void RunCutscene(Cutscene cutscene)
    {
        cutsceneController.RunCutscene(cutscene);
    }
    

    public enum GameState
    {
        PlayerRoam,
        Cutscene
    }
}
