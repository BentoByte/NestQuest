using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIscript : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject Button;
    private bool done;
    void Awake()
    {
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        if(FindObjectOfType<ControllerBirb>().isDead == true && done == false)
        {
            StartCoroutine(SelectFirsChoice());
            done = true;
        }
    }
    private void OnEnable()
    {
        Health.onPlayerDeath += EnableGameOverMenu;
        
    }
    private void OnDisable()
    {
        Health.onPlayerDeath -= EnableGameOverMenu;
    }
    public void EnableGameOverMenu()
    {
        Debug.Log("Ouch");
        gameOverMenu.SetActive(true);
    }

    private IEnumerator SelectFirsChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(Button.gameObject);
    }
}
