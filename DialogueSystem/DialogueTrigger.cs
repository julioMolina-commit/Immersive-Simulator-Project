using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<dialogueString> dialogueStrings = new List<dialogueString>();
    [SerializeField] private Transform NPCTransform;

    private bool hasSpoken = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hasSpoken)
        {
            other.gameObject.GetComponent<DialogueManagement>().DialogueStart(dialogueStrings, NPCTransform);
            hasSpoken = true;

            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
        }
    }

    public void InteractionStart()
    {
        if (!hasSpoken)
        {
            GameObject.Find("Player Body").GetComponent<DialogueManagement>().DialogueStart(dialogueStrings, NPCTransform);
            hasSpoken = true;
        }
    }
}
[System.Serializable]
public class dialogueString
{
    [TextArea(15,20)]
    public string text;
    public bool isEnd;

    [Header("Branch")]
    public bool isQuestion;
    public string answerOption1;
    public string answerOption2;
    public string answerOption3;
    public string answerOption4;
    public int option1IndexJump;
    public int option2IndexJump;
    public int option3IndexJump;
    public int option4IndexJump;
    [Header("TriggeredEvents")]
    public UnityEvent startDialogueEvent;
    public UnityEvent endDialogueEvent;
}
