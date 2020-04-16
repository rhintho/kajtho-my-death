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
public class SmsUIManager : MonoBehaviour {
    //VIDE node members
    public TextMeshProUGUI playerAnswerOne;
    public TextMeshProUGUI playerAnswerTwo;
    public TextMeshProUGUI playerAnswerThree;
    //public TextMeshProUGUI currentNPCLabel;
    VIDE_Data.VD2 data;
    //A list that will contain instances of VD2, which will handle dialogue data
    public List<VD2> dialogueDataInstances = new List<VD2>();

    bool m_isLoaded = false;
    int m_currentDialogueIndex;
    //ui handler
    public GameObject smsApp;
    public ChatUIController currentChatController;

    void Start() {
        //prepare dialogue 
        //TODO: get active dialogue tree
        /*
        gameObject.AddComponent<VD>();
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
        data = VD.nodeData;
        */
        //Let's just add 4 instances of VD2 for our dialogues
        for (int i = 0; i < 2; i++)
            dialogueDataInstances.Add(new VD2());

    }

    void OnDisable() {
        VD.EndDialogue();
    }
    public void OnEnable() {

    }

    public void PrepareDialogue(int dialogueIndex) {
        m_currentDialogueIndex = dialogueIndex;
        data = dialogueDataInstances[m_currentDialogueIndex];

        if (!data.isActive) {
            data.OnEnd += EndConversation; //Required events
            data.BeginDialogue(GetComponent<VIDE_Assign>());
            //This will begin the dialogue for that instance
        }
        if (!m_isLoaded) {
            m_isLoaded = true;
            UpdateNPCText(); //Start Conversation
        }

    }

    public void UpdatePlayerText() {
        if (data.nodeData.isPlayer) {
            playerAnswerOne.text = data.nodeData.comments[0];
            playerAnswerTwo.text = data.nodeData.comments[1];
            playerAnswerThree.text = data.nodeData.comments[2];
        }
    }

    public void UpdateNPCText() {
        currentChatController.PushSpeechbubble(data.nodeData.comments[0], false);
        LoadNextNode();
    }

    //triggered by buttons in scene
    public void LoadNextNode(int choice) {
        if (data.isActive) {
            data.nodeData.commentIndex = choice;

            currentChatController.PushSpeechbubble(data.nodeData.comments[choice], true);
            data.Next();
            data = dialogueDataInstances[m_currentDialogueIndex];
            UpdateNPCText();
        }
        else
            data.OnEnd += EndConversation;

    }

    void LoadNextNode() {
        if (data.isActive) {
            data.Next();
            data = dialogueDataInstances[m_currentDialogueIndex];

            UpdatePlayerText();
        }
        else
            data.OnEnd += EndConversation;
    }

    public void EndConversation(VD2 data) {
        Debug.Log("conversation ends");
        data.OnEnd -= EndConversation;
        data.EndDialogue();
    }

}
