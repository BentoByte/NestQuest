using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject FirstButton;
    private static GameMaster instance;
    public Vector3 lastCheckPointPos;
    
    void Awake()
    {
        StartCoroutine(SelectFirstChoice());
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        this.transform.parent = null;
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
        

    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(FirstButton.gameObject);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public virtual void SwitchScene(string scenename)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scenename);
    }

}
