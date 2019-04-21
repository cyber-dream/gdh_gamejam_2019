using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanTaskUI : MonoBehaviour {

    public TaskHuman Task;
    public TextMeshProUGUI Text;
    
    public int Done;


    private Image background;
    // Start is called before the first frame update
    void Start() {

        background = GetComponent<Image>();
        background.color = GameManager.Instance.GetHumanColor(Task.Type);

    }


    // Update is called once per frame
    void Update() {
        
        Text.text = Done + "/" + Task.Count;
    }
}