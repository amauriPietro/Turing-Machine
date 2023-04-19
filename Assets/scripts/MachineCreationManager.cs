using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MachineCreationManager : MonoBehaviour
{
    public Button back, gammaBt;

    public GameObject mainMenu,  machineCreation, gammaPan;

    void backBtnAct(){
        if(mainMenu != null){
            mainMenu.SetActive(true);
        }
        if(machineCreation != null){
            machineCreation.SetActive(false);
        }
    }
    void gammaBtnAct(){
        if(machineCreation != null){
            machineCreation.SetActive(false);
        }
        if(gammaPan != null){
            gammaPan.SetActive(true);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(backBtnAct);
        gammaBt.onClick.AddListener(gammaBtnAct);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
