using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class JSONHelper : MonoBehaviour
{
    IEnumerator loadJSONCoroutine;
    JSONChat chat;
    IEnumerator ReadJSON() {
        string path = "";
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
            path = Application.streamingAssetsPath + "/Chat.json";

            if (File.Exists(path)) {
                string JSONString = File.ReadAllText(path);
                chat = JsonUtility.FromJson<JSONChat>(JSONString);
            }
            else
                Debug.Log("doesnt find JSON");
        }
         

        else if (Application.platform == RuntimePlatform.Android) {
            path = Path.Combine(Application.streamingAssetsPath + "/", "Chat.json");

            UnityWebRequest www = UnityWebRequest.Get(path);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
                print("json file: " + www.downloadHandler.text);

            chat = JsonUtility.FromJson<JSONChat>(www.downloadHandler.text);

        }
 
        Debug.Log("username: " + chat.username);
    }

    public void Awake() {
        /*
        loadJSONCoroutine = ReadJSON();
        StartCoroutine(loadJSONCoroutine);
        */
    }

    public JSONChat GetChat() {
        return chat;
    }
}
