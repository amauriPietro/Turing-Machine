using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Main menu
    public Button create, execute, quit;

    void createBt(){
        if(mainMenu != null){
            mainMenu.SetActive(false);
        }
        if(machineCreation != null){
            machineCreation.SetActive(true);
        }
    }

    //Machine creation menu
    public Button backM, gammaBt;

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

    //Gamma
    public Color red = new Color(0.56f, 0.23f, 0.21f),
    green = new Color(0.39f, 0.41f, 0.19f),
    grey = new Color(0.33f, 0.33f, 0.33f);
    public HashSet<char> gamma = new HashSet<char>();
    public TextMeshProUGUI display;
    public Button addG, rmvG, backG;
    private bool adicionar = true;

    void AddClicado(){
        addG.GetComponent<Image>().color = green;

        rmvG.GetComponent<Image>().color = grey;

        adicionar = true;
    }

    void RmvClicado(){
        addG.GetComponent<Image>().color = grey;

        rmvG.GetComponent<Image>().color = red;

        adicionar = false;
    }

    void backBtnGamma(){
        if(gammaPan != null){
            gammaPan.SetActive(false);
        }
        if(machineCreation != null){
            machineCreation.SetActive(true);
        }
    }

    //Multiple uses
    public GameObject mainMenu,  machineCreation, gammaPan;
    
    
    //Go to second scene
    void executeBt(){
        //UnityEngine.SceneManagement.SceneManager.LoadScene(scene2, LoadSceneMode.Single);
    }
    void quitBt(){
        Application.Quit();
    }

    void Start(){

        //Main menu
        create.onClick.AddListener(createBt);
        execute.onClick.AddListener(executeBt);
        quit.onClick.AddListener(quitBt);

        //Machine creation menu
        backM.onClick.AddListener(backBtnAct);
        gammaBt.onClick.AddListener(gammaBtnAct);

        //Gamma menu
         AddClicado();
        addG.onClick.AddListener(AddClicado);
        rmvG.onClick.AddListener(RmvClicado);
        gamma.Add(' ');
        display.text = "{ " + string.Join(", ", gamma) + " }";
        backG.onClick.AddListener(backBtnGamma);

        //Q menu

    }

    void Update(){
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if(c == ' '){
                    continue;
                }
                if (!gamma.Contains(c) && adicionar){
                    gamma.Add(c);
                }
                if(gamma.Contains(c) && !adicionar){
                    gamma.Remove(c);
                }
            }
            display.text = "{ " + string.Join(", ", gamma) + " }";
        }
    }

}
