using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    [SerializeField] private TextAsset inkJSON;

    public bool playerHere;

    private void Awake()
    {
        playerHere = false;
        visualCue.SetActive(false);
    } 

    private void Update()
    {
        
        if (playerHere && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
            visualCue.SetActive(true);
        } else {
            visualCue.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHere = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHere = false;
        }
    }
}
