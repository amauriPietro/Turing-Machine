using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button create, execute, quit;

    public GameObject mainMenu,  machineCreation;

    public string scene2 = "MachineExecution";
    void createBt(){
        if(mainMenu != null){
            mainMenu.SetActive(false);
        }
        if(machineCreation != null){
            machineCreation.SetActive(true);
        }
    }
    //kk
    void executeBt(){
        //UnityEngine.SceneManagement.SceneManager.LoadScene(scene2, LoadSceneMode.Single);
    }
    void quitBt(){

    }

    void Start(){
        create.onClick.AddListener(createBt);
        execute.onClick.AddListener(executeBt);
        quit.onClick.AddListener(quitBt);
        

    }

}
