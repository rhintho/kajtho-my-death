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
    int m_currentNodeID;
    //ui handler
    public ChatUIController currentChatUIController;
    VIDE_Assign m_currentDialogue;
    public CustomDialogueState dialogueState;

    void Awake() {
        // gameObject.AddComponent<VD>();
        VD.LoadDialogues("MyDialogue");
        VD.LoadDialogues("Mom_sms");
        //load state, needs to be created first
        //VD.LoadState("levelState", true);
    }

    public void Interact(VIDE_Assign dialogue) {
        //Sometimes, we might want to check the ExtraVariables and VAs before moving forward
        //We might want to modify the dialogue or perhaps go to another node, or dont start the dialogue at all
        //In such cases, the function will return true
        //for implementation check VIDEUIManager.cs in demoScene1
        //var doNotInteract = PreConditions(dialogue);
        //if (doNotInteract) return;
        if(dialogue != m_currentDialogue) {
            m_currentDialogue = dialogue;  //store for later
            dialogueState = dialogue.gameObject.GetComponent<CustomDialogueState>();
        }
        //Debug.Log("vd is active: " + VD.isActive + " is paused: " + dialogueState.isPaused + " has Started " + dialogueState.hasStarted);

        if (!VD.isActive && !dialogueState.isPaused && !dialogueState.hasStarted) {
            dialogueState.hasStarted = true;
            PrepareDialogue(m_currentDialogue);        
        }
        else if (!VD.isActive && dialogueState.isPaused) {
            ContinueDialogue(m_currentDialogue);
            dialogueState.isPaused = false;
        }
        else if (VD.isActive) {
            LoadNextNode();           
        }
    }

    public void PrepareDialogue(VIDE_Assign dialogue) {
        //Debug.Log("prepare");
        //load state, needs to be created first
        //VD.LoadState("levelState", true);
        //VD.OnActionNode += ActionHandler; // to be implemented
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndConversation; //Required events
        VD.BeginDialogue(dialogue);
    }

    public void UpdateUI(VD.NodeData data) {

        if (data.isPlayer) {
            currentChatUIController.UpdateButtonText(data.comments, m_currentDialogue);
        }
        else { //not a player
            if (data.comments.Length == 1) {
                currentChatUIController.PushSpeechbubble(data.comments[0], false);
                if (!data.isEnd)
                    Interact(m_currentDialogue);
            }
        }
    }


    //triggered by buttons in scene
    public void LoadNextNode(int choice) {
        //Debug.Log("loadnext");
        if (VD.isActive) {
            var data = VD.nodeData;

            if (choice <= data.comments.Length) {
                data.commentIndex = choice;
                //push player answer to UI
                currentChatUIController.PushSpeechbubble(data.comments[choice], true);
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

        currentChatUIController.DisableButtons();
        //VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes
    }

    public void OnDisable() {
        Debug.Log("dialogue manager disabled. forcing dialogue end");

        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

        VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes

    }

    public void PauseConversation() {

        if (VD.isActive && !VD.nodeData.isEnd) {
            Debug.Log("conversation interrupted, saving at node : " + VD.nodeData.nodeID);
            //VD.OnActionNode -= ActionHandler;
            VD.OnNodeChange -= UpdateUI;
            VD.OnEnd -= EndConversation;
            //VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes
            // m_currentNodeID = VD.nodeData.nodeID;
            dialogueState.currentNode = VD.nodeData.nodeID;
            VD.EndDialogue();

        }
        dialogueState.isPaused = true;
    }

    public void ContinueDialogue(VIDE_Assign dialogue) {
        //Debug.Log("continuing");
        //load state, needs to be created first
        Debug.Log("should load at: " + dialogueState.currentNode);
        VD.BeginDialogue(dialogue);
        VD.SetNode(dialogueState.currentNode);
        //VD.Next();
        //VD.OnActionNode += ActionHandler; // to be implemented
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndConversation; //Required events

    }
}
