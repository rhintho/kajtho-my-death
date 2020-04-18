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

    /// <summary>
    /// core function, get called by interaction objects in scene, whenever a chat is being opened
    /// you could also use this from an evenhandler script that triggers certain events
    /// </summary>
    public void Interact(VIDE_Assign dialogue) {
        // whenever the user looks at a different chat
        if (dialogue != m_currentDialogue) {
            m_currentDialogue = dialogue;  //store for later
            dialogueState = dialogue.gameObject.GetComponent<CustomDialogueState>();
        }
        //only call this on first load
        if (!VD.isActive && !dialogueState.isPaused && !dialogueState.hasStarted) {
            dialogueState.hasStarted = true;
            PrepareDialogue(m_currentDialogue);
        }
        //whenenver the dialogues has been paused
        else if (!VD.isActive && dialogueState.isPaused) {
            ContinueDialogue(m_currentDialogue);
            dialogueState.isPaused = false;
        }
        //default next load
        else if (VD.isActive) {
            LoadNextNode();
        }
    }

    /// <summary>
    /// only call this on first load
    /// </summary>
    public void PrepareDialogue(VIDE_Assign dialogue) {
        //VD.LoadState("levelState", true);
        //VD.OnActionNode += ActionHandler; // to be implemented
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndConversation; //Required events
        VD.BeginDialogue(dialogue);

    }

    /// <summary>
    /// passes node text to ui handler
    /// also triggers the next node, if the current one is an NPC node
    /// this is so that the player chooses and answer, reads the NPCs sentence and can proceed immediatly
    /// without any interaction
    /// </summary>
    public void UpdateUI(VD.NodeData data) {

        bool isPreloaded = false;
        if (data.extraVars.ContainsKey("Preloaded")) {
            isPreloaded = (bool)data.extraVars["Preloaded"];
        }

        if (!isPreloaded) {
            if (data.isPlayer) {
                currentChatUIController.UpdateButtonText(data.comments, m_currentDialogue);
            }
            else { //not a player
                if (data.comments.Length == 1) {
                    currentChatUIController.PushSpeechbubble(data.comments[0], false, true);
                    if (!data.isEnd)
                        Interact(m_currentDialogue);
                }
            }
        }
        else {
            PushAllNodes();
        }

    }

    /// <summary>
    /// triggered by buttons in scene, passes the players choice
    /// </summary>
    public void LoadNextNode(int choice) {
        //Debug.Log("loadnext");
        if (VD.isActive) {
            var data = VD.nodeData;
            if (choice < data.comments.Length) {
                data.commentIndex = choice;
                //push player answer to UI
                currentChatUIController.PushSpeechbubble(data.comments[choice], true, true);
            }
            else {
                Debug.Log("player choice is higher than node comment index");
                data.commentIndex = 0;
                //push player answer to UI
                currentChatUIController.PushSpeechbubble(data.comments[0], true, true);
            }

            if (!data.isEnd)
                VD.Next();
        }
    }

    void LoadNextNode() {
        VD.Next();
    }

    /// <summary>
    /// unsubscribes all events on end of conversation
    /// buttons will be disabled so the player can see that a conversation has ended
    /// </summary>
    public void EndConversation(VD.NodeData data) {
        Debug.Log("conversation ends");
        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

        currentChatUIController.DisableButtons();
        //VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes
    }

    /// <summary>
    /// this is precaution for when the dialogue manager GO gets disabled for whatever reason
    /// </summary>
    public void OnDisable() {
        Debug.Log("dialogue manager disabled. forcing dialogue end");

        //VD.OnActionNode -= ActionHandler;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= EndConversation;
        VD.EndDialogue();

        VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes

    }

    /// <summary>
    /// this is called whenenver the player leaves the chatapp
    /// called by: home button, back button
    /// will save the current node, this should be done via SaveState and LoadState but atm it seems to be bugged
    /// dialogue needs to be ended and delegates to be unsubscribed
    /// </summary>
    public void PauseConversation() {
        if (VD.isActive && !VD.nodeData.isEnd) {
            Debug.Log("conversation interrupted, saving at node : " + VD.nodeData.nodeID);
            //VD.OnActionNode -= ActionHandler;
            VD.OnNodeChange -= UpdateUI;
            VD.OnEnd -= EndConversation;
            //VD.SaveState("levelState", true); //Saves VIDE stuff related to EVs and override start nodes
            dialogueState.currentNode = VD.nodeData.nodeID;
            VD.EndDialogue();

            dialogueState.isPaused = true;
        }

    }

    /// <summary>
    /// continue dialogue on last active node
    /// subscribe to all events
    /// </summary>
    public void ContinueDialogue(VIDE_Assign dialogue) {
        //Debug.Log("should load at: " + dialogueState.currentNode);
        VD.BeginDialogue(dialogue);
        VD.SetNode(dialogueState.currentNode);
        //VD.OnActionNode += ActionHandler; // to be implemented
        VD.OnNodeChange += UpdateUI;
        VD.OnEnd += EndConversation; //Required events

    }

    /// <summary>
    /// preloads all nodes
    /// not elegant but it works
    /// </summary>
    public void PushAllNodes() {
        //pushing all Nodes
        VD.OnNodeChange -= UpdateUI;

        int nodeCount = VD.GetNodeCount(false);
        string[] empty = new string[] { "", "", "" };
        currentChatUIController.UpdateButtonText(empty, m_currentDialogue);

        for (int i = 0; i < nodeCount; i++) {
            currentChatUIController.PushSpeechbubble(VD.nodeData.comments[0], VD.nodeData.isPlayer, false);
            if (!VD.nodeData.isEnd)
                VD.Next();
        }
    }
}
