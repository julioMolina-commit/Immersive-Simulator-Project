using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InGameMenu : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject inGameMenu;
    [SerializeField] SceneLoad mainMenu;

    bool isInDialogue;

    [Header("Sliders")]
    public float mouseSensitivity;
    public int fpsRate;
    public Slider sliderFPS;
    public Slider sliderMouseSensitivity;
    public Slider sliderOST;
    public Slider sliderEffect;
    public Slider sliderFootsteps;
    public Slider sliderTyping;
    [Header("Displays")]
    [SerializeField] private TextMeshProUGUI mouseSensitivityDisplay;
    [SerializeField] private TextMeshProUGUI FPSDisplay;
    [SerializeField] private TextMeshProUGUI OSTDisplay;
    [SerializeField] private TextMeshProUGUI effectDisplay;
    [SerializeField] private TextMeshProUGUI footstepsDisplay;
    [SerializeField] private TextMeshProUGUI typingDisplay;
    //Serialize a private "Options" menu for sensitivity, shader effect, fps and such
    private void Awake()
    {
        sliderMouseSensitivity.value = 120;
    }
    private void Start()
    {
        inGameMenu.SetActive(false);

        GameEvents.EnterDialogue.AddListener(EnterDialogue);
        GameEvents.ExitDialogue.AddListener(ExitDialogue);
    }
    private void Update()
    {
        PlayerInput();
    }
    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInDialogue)
        {
            if (inGameMenu.activeInHierarchy == true)
            {
                ExitMenu();
            }
            else
            {
                EnterMenu();
            }
        }
    }
    public void ExitMenu()
    {
        inGameMenu.SetActive(false);

        Time.timeScale = 1f;

        GameObject.Find("Player Body").GetComponent<PlayerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void EnterMenu()
    {
        inGameMenu.SetActive(true);

        Time.timeScale = 0f;

        GameObject.Find("Player Body").GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ChangeFPSValue()
    {
        fpsRate = (int)sliderFPS.value;
        FPSDisplay.text = sliderFPS.value.ToString();
    }
    public void ChangeSensibilityValue()
    {
        mouseSensitivity = sliderMouseSensitivity.value;
        mouseSensitivityDisplay.text = sliderMouseSensitivity.value.ToString();
    }
    public void ChangeOstDisplay()
    {
        OSTDisplay.text = sliderOST.value.ToString();
    }
    public void ChangeEffectDisplay()
    {
        effectDisplay.text = sliderEffect.value.ToString();
    }
    public void ChangeFootstepsDisplay()
    {
        footstepsDisplay.text = sliderFootsteps.value.ToString();
    }
    public void ChangeTypingDisplay()
    {
        typingDisplay.text = sliderTyping.value.ToString();
    }
    public void SaveNewSettings()
    {
        GameEvents.SaveNewSettings.Invoke();
    }
    public void ExitToMainMenu()
    {
        mainMenu.LoadSelectedScene();
    }
    public void EnterDialogue()
    {
        isInDialogue = true;
    }
    public void ExitDialogue() 
    {
        isInDialogue = true;
    }
}