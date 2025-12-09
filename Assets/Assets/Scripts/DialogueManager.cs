using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    private bool isInDialogue = false;

    private Queue<string> sentences = new Queue<string>();
	private Queue<bool> style = new Queue<bool>();
    private Dialogue currentDialogue;

    [Header("Settings")]
    public float textDelay = 0.05f;
    public float baseLineDelay = 0.5f;
    public float readingSpeed = 14f;

    [Header("References")]
    public TextMeshProUGUI subtitleText;

    public static event Action<Dialogue> onDialogueEnd; // the argument is the dialogue that just ended

    // call this from other scripts to start a dialogue
    public void StartDialogue(Dialogue dialogue) {
        // we are still in a dialogue, don't interrupt
        if (isInDialogue && (!dialogue.canInterrupt || !currentDialogue.canBeInterrupted)) {
            return;
        }

        StopAllCoroutines();

        if (isInDialogue) {
            EndDialogue();
        }

        currentDialogue = dialogue;
        sentences = new Queue<string>();
        foreach (DialogueLine line in dialogue.lines) {
            sentences.Enqueue(line.text);
			style.Enqueue(line.isItalic);
        }
        
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue() {
        subtitleText.text = "";
        isInDialogue = true;

        while (sentences.Count > 0) {
            string sentence = sentences.Dequeue();
			bool isItalic = style.Dequeue();
			subtitleText.fontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal;
            subtitleText.text = "";

            foreach(char letter in sentence.ToCharArray())
            {
                subtitleText.text += letter;
                yield return new WaitForSeconds(textDelay);
            }
            
            float lineDelay = baseLineDelay + (sentence.Length / readingSpeed);
            lineDelay = Mathf.Clamp(1f, lineDelay, 7f);
            yield return new WaitForSeconds(lineDelay);
        }

        EndDialogue();
    }

    void EndDialogue() {
        onDialogueEnd?.Invoke(currentDialogue);

        currentDialogue = null;
        subtitleText.text = "";
        isInDialogue = false; 
    }
}
