using System.Collections.Generic;
using UnityEngine;

// this class potentially can be extended to hold speaker information, if needed. for now we only store a string
[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)] public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
    public bool canInterrupt = false; // can this dialogue interrupt other ongoing dialogues

    // can this dialogue be interrupted by other dialogues, 
    // so if it's dialogue relating to an important cutscene, set this to false and the dialogue will always play to completion
    public bool canBeInterrupted = true; 
}