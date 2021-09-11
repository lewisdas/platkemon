using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : CutsceneSystem
{
    public event Action dialogueFinished;
    public float textWriteDelay = 0.05f;
    
    [Header("References")]
    public GameObject dialogueBox;
    public TextMeshProUGUI textBox;

    private string[] _lines;
    private int _currLine;
    private bool _isWritingDialogue;

    public override void Run(CutsceneSegment segment)
    {
        GameManager.instance.state = GameManager.GameState.Cutscene;
        dialogueBox.SetActive(true);
        var dialogue = (Dialogue) segment;
        _lines = dialogue.lines;
        _currLine = -1;

        WriteNextLine();
    }

    public override void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // finish current line of dialogue
            if (_isWritingDialogue)
            {
                _isWritingDialogue = false;
                textBox.text = _lines[_currLine];
            }
            // start next line of dialogue
            else
            {
                WriteNextLine();
            }
        }
    }

    /// <summary>
    /// Show next line in the dialogue box.
    /// </summary>
    private void WriteNextLine()
    {
        textBox.text = "";
        _currLine++;
        if (_currLine >= _lines.Length)
        {
            dialogueBox.SetActive(false);
            dialogueFinished();
        }
        else
        {
            StartCoroutine(WriteText(_lines[_currLine]));
        }
    }

    /// <summary>
    /// Writing text is prettier than just showing it!
    /// </summary>
    private IEnumerator WriteText(string line)
    {
        _isWritingDialogue = true;
        
        foreach (var letter in line)
        {
            if (!_isWritingDialogue) break; // in case user aborts typing anim.
            textBox.text += letter;
            yield return new WaitForSeconds(textWriteDelay);
        }
        
        _isWritingDialogue = false;
    }
}
