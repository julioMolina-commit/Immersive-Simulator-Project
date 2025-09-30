using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManagement : MonoBehaviour
{
    [SerializeField] private GameObject dialogueParent;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;
    [SerializeField] private Button option4Button;

    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float turnSpeed = 2f;

    [SerializeField] private AudioSource typeSource;
    [SerializeField] private AudioClip[] typeSound;

    private List<dialogueString> dialogueList;

    [Header("Player")]
    [SerializeField] private PlayerMovement firstPersonController;
    private Transform playerCamera;

    private int currentDialogueIndex = 0;

    private void Start()
    {
        dialogueParent.SetActive(false);
        playerCamera = Camera.main.transform;
    }
    public void DialogueStart(List<dialogueString> textToPrint, Transform NPC)
    {
        GameEvents.EnterDialogue.Invoke();

        dialogueParent.SetActive(true);
        firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(TurnCameraTowardsNPC(NPC));

        dialogueList = textToPrint;
        currentDialogueIndex = 0;

        DisableButtons();

        StartCoroutine(PrintDialogue());
    }

    private void DisableButtons()
    {
        option1Button.interactable = false;
        option2Button.interactable = false;
        option3Button.interactable = false;
        option4Button.interactable = false;

        option1Button.GetComponentInChildren<TMP_Text>().text = "";
        option2Button.GetComponentInChildren<TMP_Text>().text = "";
        option3Button.GetComponentInChildren<TMP_Text>().text = "";
        option4Button.GetComponentInChildren<TMP_Text>().text = "";
    }

    private IEnumerator TurnCameraTowardsNPC(Transform NPC)
    {
        Quaternion startRotation = playerCamera.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(NPC.position - playerCamera.position);

        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            playerCamera.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * turnSpeed;
            yield return null;
        }

        playerCamera.rotation = targetRotation;
    }

    private bool optionSelected = false;
    private IEnumerator PrintDialogue()
    {
        while (currentDialogueIndex < dialogueList.Count)
        {
            dialogueString line = dialogueList[currentDialogueIndex];

            line.startDialogueEvent?.Invoke();

            if (line.isQuestion)
            {
                yield return StartCoroutine(TypeText(line.text));

                if (!(line.answerOption1 == ""))
                {
                    option1Button.interactable = true;
                }
                if (!(line.answerOption2 == ""))
                {
                    option2Button.interactable = true;
                }
                if (!(line.answerOption3 == ""))
                {
                    option3Button.interactable = true;
                }
                if (!(line.answerOption4 == ""))
                {
                    option4Button.interactable = true;
                }

                option1Button.GetComponentInChildren<TMP_Text>().text = line.answerOption1;
                option2Button.GetComponentInChildren<TMP_Text>().text = line.answerOption2;
                option3Button.GetComponentInChildren<TMP_Text>().text = line.answerOption3;
                option4Button.GetComponentInChildren<TMP_Text>().text = line.answerOption4;

                option1Button.onClick.AddListener(() => HandleOptionSelected(line.option1IndexJump));
                option2Button.onClick.AddListener(() => HandleOptionSelected(line.option2IndexJump));
                option3Button.onClick.AddListener(() => HandleOptionSelected(line.option3IndexJump));
                option4Button.onClick.AddListener(() => HandleOptionSelected(line.option4IndexJump));

                yield return new WaitUntil(() => optionSelected);
            }
            else
            {
                yield return StartCoroutine(TypeText(line.text));
            }

            line.endDialogueEvent?.Invoke();

            optionSelected = false;
        }
        DialogueStop();
    }

    private void HandleOptionSelected(int indexJump)
    {
        optionSelected = true;
        DisableButtons();

        currentDialogueIndex = indexJump;
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach(char letter in text.ToCharArray())
        {
            dialogueText.text += letter;

            if (letter.ToString() == "o" || letter.ToString() == "u")
            {
                typeSource.PlayOneShot(typeSound[0]);
            }
            else if (letter.ToString() == "e" || letter.ToString() == "i")
            {
                typeSource.PlayOneShot(typeSound[1]);
            }
            else if (letter.ToString() == "a")
            {
                typeSource.PlayOneShot(typeSound[2]);
            }

            //int i = Random.Range(0, 5);
            //if (i == 1)
                //GetComponent<AudioSource>().PlayOneShot(typeSound[Random.Range(0, typeSound.Length)]);

            yield return new WaitForSeconds(typingSpeed);
        }

        if (!dialogueList[currentDialogueIndex].isQuestion)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        if (dialogueList[currentDialogueIndex].isEnd)
            DialogueStop();

        currentDialogueIndex++;
    }

    private void DialogueStop()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueParent.SetActive(false);

        firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameEvents.ExitDialogue.Invoke();
    }
}
