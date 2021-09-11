using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        InitializeDialogue();
    }
    
    public void InitializeDialogue()
    {
        GameManager.instance.dialogueSystem.Initialize(dialogue);
    }
}
