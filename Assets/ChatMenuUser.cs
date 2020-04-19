using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatMenuUser : MonoBehaviour {
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_lastMessage;
    public TextMeshProUGUI m_date;
    Button btn_activateChat;

    public GameObject go_smsAll;
    public GameObject go_smsMenu;
    public GameObject go_chat;

    public ChatUIController uiController;

    public void Awake() {
        btn_activateChat = GetComponent<Button>();
    }
    public void Start() {
        btn_activateChat.onClick.AddListener(activateChat);
    }
    // Start is called before the first frame update
    public void activateChat() {
        go_smsAll.SetActive(true);
        go_smsMenu.SetActive(false);

        uiController.UpdateActiveChat(go_chat);
        DialogueManager_Sms.Instance.Interact(go_chat.GetComponent<VIDE_Assign>());
    }

    public void SetUserItems(string name, string lastMessage, string date) {
        m_name.text = name;
        m_lastMessage.text = lastMessage;
        m_date.text = date;
    }

    public void SetName(string name) {
        m_name.text = name;
    }
    public void SetDate(string date) {
        m_date.text = date;
    }

    public void SetLastMessage(string lastMessage) {
        m_lastMessage.text = lastMessage;
    }
}
