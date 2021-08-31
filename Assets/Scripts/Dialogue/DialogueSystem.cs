using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public float textWriteDelay = 0.25f;
    
    [Header("References")]
    public GameObject dialogueBox;
    public TextMeshProUGUI textBox;
    
    public void Initialize(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);

        StartCoroutine(WriteText(dialogue.lines[0]));
        
        
        foreach (var line in dialogue.lines)
            Debug.Log(line);
    }

    public void HandleUpdate()
    {
        // progress to next dialogue line, if there is one
    }

    /// <summary>
    /// Writing text is prettier than just showing it!
    /// </summary>
    private IEnumerator WriteText(string line)
    {
        foreach (var letter in line)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(textWriteDelay);
        }
    }
}
