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
        
        dialogue.Add("TutorialNPC1", new string[]
        {
            "Did you know that you can use your tongue to collect items or butterflies by pressing Ctrl Left or the middle mouse button?",
            "Butterflies allow you to change the color.",
            "Blue chameleons can perform a double jump!"
        });
        
        dialogue.Add("TutorialNPC2", new string[]
        {
            "Green chameleons can climb certain walls.",
            "Just jump against a wall or fire your tongue against a wall when you're in the air!",
            "While climbing you can move up and down with W and S and perform a walljump with the spacebar!"
        });
        
        dialogue.Add("TutorialNPC3", new string[]
        {
            "As a red Chameleon, you can perform a Dash, which helps you jump further or destroy unstable walls.",
            "Use the left shift key to execute the Dash!",
            "Make sure you have enough run-up!"
        });
        
        dialogue.Add("TutorialNPC4", new string[]
        {
            "This color balloon was your first checkpoint. Should you make a mistake, you can continue from there.",
            "By the way, you can change your color with E and Q or with the mouse buttons, if you should be in possession of several colors.",
            "Each color-specific move consumes color from your supply. If you waste it, you won't be able to complete your tasks.",
            "In this case, you can use the pause menu to get to the last checkpoint."
        });
        
        dialogue.Add("TutorialNPC5", new string[]
        {
            "Congratulations, you have successfully completed the tutorial!",
            "Unfortunately, there are no more levels available yet.",
            "I would be very happy to receive feedback!"
        });
    }
}
