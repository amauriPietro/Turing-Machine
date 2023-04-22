using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

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
    public Button backM, gammaBt, qBt;

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

    void qBtnAct(){
        if(machineCreation != null){
            machineCreation.SetActive(false);
        }
        if(qPan != null){
            qPan.SetActive(true);
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
    //States
    public Button up1, up2, down1, down2, backState, editState;
    public TextMeshProUGUI count1, count2;
    private int stateNum, stateCur;

    void inc1(){
        if(stateNum <= 99)
            stateNum++;
        count1.text = stateNum.ToString();
    }

    void inc2(){
        if(stateCur < stateNum)
            stateCur++;
        count2.text = stateCur.ToString();
    }

    void dec1(){
        if(stateNum >= 2)
            stateNum--;
        if(stateNum < stateCur)
            stateCur--;
        count1.text = stateNum.ToString();
        count2.text = stateCur.ToString();
    }

    void dec2(){
        if(stateCur >= 2)
            stateCur--;
        count2.text = stateCur.ToString();
    }

    void backBtnState(){
        if(qPan != null){
            qPan.SetActive(false);
        }
        if(machineCreation != null){
            machineCreation.SetActive(true);
        }
    }

    void editBtnState(){
        if(qPan != null){
            qPan.SetActive(false);
        }
        if(editPan != null){
            editPan.SetActive(true);
            edState.text = "Editing State " + stateCur.ToString();
            //wcurBehaviour = 1;
        }
    }

    //State editor

    private struct deltaT{
        public int ns, rs, ws, mt;
    }
    public TextMeshProUGUI edState, edBehaviour, edNextState, edReadSymbol, edWriteSymbol, edMoveTo;
    public Button lNextState, rNextState, lReadSymbol, rReadSymbol, lWriteSymbol, rWriteSymbol, lMoveTo, rMoveTo, edBackBtn, saveBt;
    private int curNextState, curReadSymbol, curWriteSymbol, curMove;
    private List<deltaT>[] delta = new List<deltaT>[99];

    void lNextBtnState(){
        curNextState = (curNextState - 1 + stateNum) % stateNum;
        edNextState.text = (curNextState+1).ToString();
    }
    void rNextBtnState(){
        curNextState = (curNextState + 1 + stateNum) % stateNum;
        edNextState.text = (curNextState+1).ToString();
    }
    void lReadBtnState(){
        curReadSymbol = (curReadSymbol - 1 + gamma.Count) % gamma.Count;
        edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
        curMove = 0;
        curWriteSymbol = 0;
        curNextState = 0;
        if(delta[stateCur] != null){
            foreach(var i in delta[stateCur]){
                if(i.rs == curReadSymbol){
                    curMove = i.mt;
                    curWriteSymbol = i.ws;
                    curNextState = i.ns;
                }
            }
        }
        edNextState.text = (curNextState+1).ToString();
        edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
        edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();
        if(curMove % 2 == 0)
            edMoveTo.text = "L";
        else
            edMoveTo.text = "R";
    }
    void rReadBtnState(){
        curReadSymbol = (curReadSymbol + 1 + gamma.Count) % gamma.Count;
        edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
        curMove = 0;
        curWriteSymbol = 0;
        curNextState = 0;
        if(delta[stateCur] != null){
            foreach(var i in delta[stateCur]){
                if(i.rs == curReadSymbol){
                    curMove = i.mt;
                    curWriteSymbol = i.ws;
                    curNextState = i.ns;
                }
            }
        }
        edNextState.text = (curNextState+1).ToString();
        edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
        edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();
        if(curMove % 2 == 0)
            edMoveTo.text = "L";
        else
            edMoveTo.text = "R";
        
    }
    void lWriteBtnState(){
        curWriteSymbol = (curWriteSymbol - 1 + gamma.Count) % gamma.Count;
        edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();

    }
    void rWriteBtnState(){
        curWriteSymbol = (curWriteSymbol + 1 + gamma.Count) % gamma.Count;
        edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();
        
    }
    void MoveBtnState(){
        curMove = (curMove+1) % 2;
        if(curMove % 2 == 0)
            edMoveTo.text = "L";
        else
            edMoveTo.text = "R";
    }

    void saveState(){
        if(delta[stateCur] == null)
            delta[stateCur] = new List<deltaT>();
        foreach(var i in delta[stateCur]){
            if(i.rs == curReadSymbol){
                curMove = i.mt;
                curWriteSymbol = i.ws;
                curNextState = i.ns;
                return;
            }
        }
        delta[stateCur].Add(new deltaT { ns = curNextState, rs = curReadSymbol, ws = curWriteSymbol, mt = curMove});
    }

    void edBackAct(){
        if(qPan != null){
            qPan.SetActive(true);
        }
        if(editPan != null){
            editPan.SetActive(false);
        }
    }

    //Multiple uses
    public GameObject mainMenu,  machineCreation, gammaPan, qPan, editPan;
    
    
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
        qBt.onClick.AddListener(qBtnAct);

        //Gamma menu
         AddClicado();
        addG.onClick.AddListener(AddClicado);
        rmvG.onClick.AddListener(RmvClicado);
        gamma.Add(' ');
        display.text = "{ " + string.Join(", ", gamma) + " }";
        backG.onClick.AddListener(backBtnGamma);

        //state menu
        stateNum = 1;
        stateCur = 1;
        count1.text = stateNum.ToString();
        count2.text = stateNum.ToString();
        up1.onClick.AddListener(inc1);
        up2.onClick.AddListener(inc2);
        down1.onClick.AddListener(dec1);
        down2.onClick.AddListener(dec2);
        backState.onClick.AddListener(backBtnState);
        editState.onClick.AddListener(editBtnState);

        //edit state menu
        lNextState.onClick.AddListener(lNextBtnState);
        rNextState.onClick.AddListener(rNextBtnState);
        lReadSymbol.onClick.AddListener(lReadBtnState);
        rReadSymbol.onClick.AddListener(rReadBtnState);
        lWriteSymbol.onClick.AddListener(lWriteBtnState);
        rWriteSymbol.onClick.AddListener(rWriteBtnState);
        lMoveTo.onClick.AddListener(MoveBtnState);
        rMoveTo.onClick.AddListener(MoveBtnState);
        edBackBtn.onClick.AddListener(edBackAct);
        saveBt.onClick.AddListener(saveState);

        curMove = 0;
        curWriteSymbol = 0;
        curNextState = 0;
        curReadSymbol = 0;
        if(delta[stateCur] != null){
            foreach(var i in delta[stateCur]){
                if(i.rs == curReadSymbol){
                    curMove = i.mt;
                    curWriteSymbol = i.ws;
                    curNextState = i.ns;
                }
            }
        }
        edNextState.text = (curNextState+1).ToString();
        edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
        edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();
        if(curMove % 2 == 0)
            edMoveTo.text = "L";
        else
            edMoveTo.text = "R";

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
