using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script stores every dialogue conversation in a public Dictionary.*/

public class Dialogue : MonoBehaviour
{

    public Dictionary<string, string[]> dialogue = new Dictionary<string, string[]>();

    void Start()
    {
        //NPC
        dialogue.Add("CharacterA", new string[] {
            "Hey man, how are you doing today?",
            "I'm an NPC!",
            "If you wanna hang out let me know!",
            "You are my best friend!"
        });

        dialogue.Add("CharacterAChoice1", new string[] {
            "",
            "",
            "Stop being annoying pls."
        });

        dialogue.Add("CharacterAChoice2", new string[] {
            "",
            "",
            "Yeah i'm in!"
        });
    }
}
