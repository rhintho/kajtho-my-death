using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatApp : MonoBehaviour
{
    public GameObject userChat_pf;
    public GameObject content_go;
    public GameObject chatOpen_pf;
    public GameObject allChatsContainer_go;

    GameObject chatOpen_go;

    RectTransform scrollRectTransform;
    float yScrollPosition = 0;
    float offset = 5;
    int numberOfUsers = 10;
    int currentUser = 0;
    public GameObject[] allUserChats;

    JSONChat chat;
    // Start is called before the first frame update
    void Start() {
        allUserChats = new GameObject[numberOfUsers];
        //read json
        chat = GameObject.Find("Utility").GetComponent<JSONHelper>().GetChat();
        //create prefab instances
        scrollRectTransform = content_go.GetComponent<RectTransform>();
        for (int i = 0; i < numberOfUsers; i++) {
            CreateAllUserChats();
        }
      
        // fill everything into content go

        //create individual chats for each users
    }

    private void CreateAllUserChats() {

        GameObject userChat_go = Instantiate(userChat_pf, content_go.transform);

        //place the answer below the last one
        RectTransform rectangleTransform = userChat_go.GetComponent<RectTransform>();
        rectangleTransform.anchoredPosition = new Vector2(0, yScrollPosition);
        //increase the content container
        scrollRectTransform.sizeDelta = new Vector2(scrollRectTransform.sizeDelta.x, scrollRectTransform.sizeDelta.y + rectangleTransform.sizeDelta.y + offset);
        //Scroll position plus offset
        yScrollPosition -= rectangleTransform.sizeDelta.y + offset;

        //create respective chat for each user
        chatOpen_go = Instantiate(chatOpen_pf, allChatsContainer_go.transform);
        Button btnTmp = userChat_go.GetComponent<Button>();
        userChat_go.GetComponent<Button>().onClick.AddListener(() => ActivateChat(btnTmp));
        //pass content to the chats
        chatOpen_go.GetComponent<ChatController>().SetContent(chat);
        chatOpen_go.SetActive(false);
        //store all users in an array
        allUserChats[currentUser] = chatOpen_go;

        currentUser++;
    }

    void ActivateChat(Button currentUser) {
        allUserChats[currentUser.gameObject.transform.GetSiblingIndex()].SetActive(true);
    }

}
