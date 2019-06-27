using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONHelper : MonoBehaviour
{
    JSONChat chat;
    public void ReadJSON() {
        string path = Application.streamingAssetsPath + "/Chat.json";
        string JSONString = File.ReadAllText(path);
        chat = JsonUtility.FromJson<JSONChat>(JSONString);

        Debug.Log(chat.username);
    }

    public void Awake() {
        ReadJSON();
    }

    public JSONChat GetChat() {
        return chat;
    }
}
