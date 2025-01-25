using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    private GameMaster gm;
    public GameObject SomeDropPerfab;
    public int PlaceNum;
    
    void Start(){
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }
    void Update() {
        if(PlaceNum > 0 && Input.GetKeyDown(KeyCode.Q) && GetComponent<ControllerBirb>().isGrounded)
        {
            var go = Instantiate(SomeDropPerfab,new Vector2(transform.position.x, (transform.position.y + 14)), Quaternion.identity);
            PlaceNum--;
        }
    }
}
