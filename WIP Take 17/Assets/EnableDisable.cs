using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnableDisable : MonoBehaviour
{
    public GameObject panel;

    public bool enable = false;
    public GameObject Button;
    bool done = false;
    
    void Start()
    {
        panel.SetActive(false);
    }

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Time.timeScale = 0;
            enable = true;

        }


        if (enable == true && done == false)
        {
            
            StartCoroutine(SelectFirsChoice()); 
            done = true;
        } 

        panel.SetActive(enable);

    }
    public void Unpause()
    {
        enable = false;
        done = false;
        Time.timeScale = 1;
    }

    private IEnumerator SelectFirsChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(Button.gameObject);
    }
    

}
