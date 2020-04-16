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
    VD.NodeData data;

    bool isLoaded = false;

    //ui handler
    public GameObject smsApp;
    ChatUIController currentChatController;

    void Start() {
        //prepare dialogue 
        //TODO: get active dialogue tree
        gameObject.AddComponent<VD>();
        VD.BeginDialogue(GetComponent<VIDE_Assign>());
        data = VD.nodeData;
    }

    void OnDisable() {
        VD.EndDialogue();
    }
    public void OnEnable() {

    }

    public void PrepareDialogue() {
        if (!isLoaded) {
            isLoaded = true;
  
            //TODO: get active chat controller
            currentChatController = smsApp.GetComponent<ChatUIController>();  //prepare ui
            UpdateNPCText(); //Start Conversation
           // smsApp.SetActive(false); //TODO: null pointer when GO not active on load, need to improve this
        }

    }

    public void UpdatePlayerText() {
        if (data.isPlayer) {
            playerAnswerOne.text = data.comments[0];
            playerAnswerTwo.text = data.comments[1];
            playerAnswerThree.text = data.comments[2];
        }
    }

    public void UpdateNPCText() {
        //currentNPCLabel.text = data.comments[0];
        currentChatController.PushSpeechbubble(data.comments[0], false);
        LoadNextNode();
    }

    //triggered by buttons in scene
    public void LoadNextNode(int choice) {
        data.commentIndex = choice;
        currentChatController.PushSpeechbubble(data.comments[choice], true);
        if (data.isEnd)
            EndConversation();
        VD.Next();
        data = VD.nodeData;
        UpdateNPCText();
    }

    void LoadNextNode() {
        if (data.isEnd)
            EndConversation();
        VD.Next();
        data = VD.nodeData;
        UpdatePlayerText();
    }

    public void EndConversation() {
        Debug.Log("conversation ends");

    }
}
