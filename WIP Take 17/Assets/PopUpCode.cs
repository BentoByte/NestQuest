using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpCode : MonoBehaviour
{
    // Create Popup
    public static PopUpCode CreateNew(Vector3 position, int damage) {
        GameObject myPrefab;
        myPrefab = GameObject.FindWithTag("PopUpper");
        Transform PopUpTransform = Instantiate(myPrefab.transform, new Vector3(position.x, position.y, 0), Quaternion.identity);
        
        PopUpCode popupcode = PopUpTransform.GetComponent<PopUpCode>();
        popupcode.dontdestroyme = false;
        popupcode.SetUp(damage);
        return popupcode;
    }
    public bool dontdestroyme;
    Color textColor;
    TextMeshPro textMesh;
    float num;
    public float disapearTimer;
    void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
        num = Random.Range(-10.0f, 10.0f);
        transform.position += new Vector3(num,num,0);
        textColor = textMesh.color;
    }
    public void SetUp(int Damage) {
        textMesh.SetText(Damage.ToString());
        textColor = textMesh.color;
        disapearTimer = 0.5f;
    }
    void Update() {
        float moveYspeed = 20f;
        transform.position += new Vector3(num, moveYspeed) * Time.deltaTime;

        disapearTimer -= Time.deltaTime;
        if (disapearTimer < 0 && !dontdestroyme) {
            float disaspeed = 1.7f;
            textColor.a -= disaspeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                if(!dontdestroyme)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
