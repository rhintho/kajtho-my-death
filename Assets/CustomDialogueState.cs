using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDialogueState : MonoBehaviour
{
    int m_currentNode = 0;
    [SerializeField]
    public int currentNode {
        get { return m_currentNode; }
        set { m_currentNode = value; }
    }

    /// <summary>
    /// flags if the dialogue is paused or not
    /// gets paused by DialogueManager
    /// </summary>
    bool m_isPaused = false;
    [SerializeField]
    public bool isPaused {
        get { return m_isPaused; }
        set { m_isPaused = value; }
    }

    /// <summary>
    /// check is the dialogue has been loaded once
    /// is started by DialogueManager
    /// </summary>
    bool m_hasStarted = false;
    [SerializeField]
    public bool hasStarted {
        get { return m_hasStarted; }
        set { m_hasStarted = value; }
    }

    /// <summary>
    /// if dialogue is preloaded, all nodes will be loaded on start
    /// this helps to generate old conversations for the player to read
    /// </summary>
    bool m_isPreloaded = false;
    [SerializeField]
    public bool isPreloaded {
        get { return m_isPreloaded; }
        set { m_isPreloaded = value; }
    }

}
