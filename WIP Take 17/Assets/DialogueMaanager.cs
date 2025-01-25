using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]  private float typingSpeed = 0.04f;
    private static DialogueManager instance;
    [SerializeField]  GameObject dialoguePanel;
    [SerializeField]  TextMeshProUGUI dialogueText;
    [SerializeField]  TextMeshProUGUI SecretText;
    [SerializeField]  TextMeshProUGUI Name;
    [SerializeField] GameObject[] choices;
    [SerializeField] Animator portraitsAnime;
    [SerializeField] private TextAsset globalFile;
    TextMeshProUGUI[] choicesText;
    const string Speaker_Tag = "speaker";
    const string Portrait_Tag = "portrait";
    const string Layout_Tag = "layout";
    const string Action_Tag = "action";
    [SerializeField] private GameObject ContinueIcon;
    Story currentStory;
    Animator layoutAnime;
    private VarChanger varchange;
    private bool canContinueNextLine = false;
    private Coroutine displayLineCoroutine;
    public bool dialogueIsPlaying; 
    bool canSkip;
    bool AddingRichTextTag = false;
    public string ByteText1;
    public string ByteText2;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than ONE!");
        }
        instance = this;

        varchange = new VarChanger(globalFile);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnime = dialoguePanel.GetComponentInChildren<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void Update()
    {


        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.J) && canContinueNextLine)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        varchange.StartListen(currentStory);
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        varchange.StopListen(currentStory);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));


            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private IEnumerator DisplayLine(string line)
    {
        StartCoroutine(CanSkip());
        dialogueText.text = "";
        ContinueIcon.SetActive(false);
        HideChoices();
        canContinueNextLine = false;
        foreach (char letter in line.ToCharArray())
        {
            if (canSkip == true && Input.GetKeyDown(KeyCode.J))
            {
                dialogueText.text = line;
                break;
            }
            if (letter == '<' || AddingRichTextTag) 
            {
                AddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {
                    AddingRichTextTag = false;
                }
            }
            else    
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);                
            }


        }
        ContinueIcon.SetActive(true);
        DisplayChoices();
        canContinueNextLine = true;
    }
    private void HideChoices()
    {
        foreach(GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }
    private IEnumerator CanSkip()
    {
        canSkip = false; 
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case Speaker_Tag:
                    Name.text = tagValue;
                    break;
                case Portrait_Tag:
                    portraitsAnime.Play(tagValue);
                    Debug.Log(tagValue);
                    break;
                case Layout_Tag:
                    layoutAnime.Play(tagValue);
                    break;
                case Action_Tag:
                    if (tagValue == "BYTETEXT1")
                    {
                        SecretText.text = ByteText1;
                    } else if (tagValue == "BYTETEXT2")
                    {
                        SecretText.text = ByteText2;
                    } else if (tagValue == "TURN")
                    {
                        Debug.Log("GOGOGO");
                    }   else if (tagValue == "SLOWTEXT")
                    {
                        typingSpeed = 0.1f;
                    } else if (tagValue == "TEXTSPEED")
                    {
                        typingSpeed = 0.04f;
                    }
                    break;
                default:
                    Debug.LogWarning("Oopsie Poopsie,Tag came but not handled" + tag);
                    break;

            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Oops, too many choices");
        }
        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;

            index++;
        }   

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        
        StartCoroutine(SelectFirstChoice());
    }
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    public void MakeChoice(int choiceIndex)
    {
        if (canContinueNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
        }

        
    }
}
