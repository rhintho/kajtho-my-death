using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;
using TMPro;

/*
 * Always: Update NPC Text -> Next Node -> Update Player Text -> wait for input
 * after input: Get Choice and progress to next Node -> Update NPC Text -> Next Node -> Update Player Text -> wait for input
 * 
 */
public class DialogueManager_Sms : MonoBehaviour {
    //button texts. should update this everytime there is an interaction happening
    public TextMeshProUGUI playerAnswerOne;
    public TextMeshProUGUI playerAnswerTwo;
    public TextMeshProUGUI playerAnswerThree;
    //public TextMeshProUGUI currentNPCLabel;
    int m_choice;
    int m_currentDialogueIndex;
    //ui handler
    public ChatUIController currentChatController;
    VIDE_Assign lastDialogue;

    void Awake() {
        // gameObject.AddComponent<VD>();
        VD.LoadDialogues("MyDialogue");
        VD.LoadDialogues("Mom_sms");
        //load state, needs to be created first
        VD.LoadState("levelState", true);
    }
    void Start() {

    }

    public void Interact(VIDE_Assign dialogue) {
        //Sometimes, we might want to check the ExtraVariables and VAs before moving forward
        //We might want to modify the dialogue or perhaps go to another node, or dont start the dialogue at all
        //In such cases, the function will return true
        //for implementation check VIDEUIManager.cs in demoScene1
        //var doNotInteract = PreConditions(dialogue);
        //if (doNotInteract) return;

        lastDialogue = dialogue;  //store for later

        if (!VD.isActive) {
            PrepareDialogue(dialogue);
        }
        else {
            LoadNextNode();
        }
    }

    public void PrepareDialogue(VIDE_Assign dialogue) {

        //VD.OnActionNode += ActionHandler; // to be implemented
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndConversation; //Required events

        VD.BeginDialogue(dialogue);

    }

    public void UpdateUI(VD.NodeData data) {

        if (data.isPlayer) {
            currentChatController.UpdateButtonText(data.comments, lastDialogue);
        }
        else { //not a player
            if (data.comments.Length == 1) {
                currentChatController.PushSpeechbubble(data.comments[0], false);
                if (!data.isEnd)
                    Interact(lastDialogue);
            }
        }
    }


    //triggered by buttons in scene
    public void LoadNextNode(int choice) {
        if (VD.isActive) {
            var data = VD.nodeData;
            if (choice <= data.comments.Length) {
                data.commentIndex = choice;
                //push player answer to UI
                currentChatController.PushSpeechbubble(data.comments[choice], true);
            }
            else
                Debug.Log("player choice is higher than node comment index");
            if (!data.isEnd)
                VD.Next();
        }
    }

    void LoadNextNode() {
        VD.Next();
    }

    public void EndConversation(VD.NodeData data) {
        Debug.Log("conversation ends");

        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

        VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes
    }

    public void OnDisable() {
        Debug.Log("dialogue manager disabled. forcing dialogue end");

        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

    }

    public void PauseConversation() {
        Debug.Log("conversation interrupted, saving");

        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

        VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes

    }
}
