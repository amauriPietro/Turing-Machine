using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;

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

    [Serializable]
    private struct turingMachine{
        
        public List<string> qTM { get; set; }
        public HashSet<char> sigmaTM { get; set; }
        public HashSet<char> gammaTM { get; set; }
        public List<deltaT>[] deltaTM { get; set; }
        public int q0TM { get; set; }
        public int bTM { get; set; }
        public List<int> fTM { get; set; }
        
    }

    void backBtnAct(){
        List<string> q = new List<string>();
        for(int i = 1; stateNum >= i; i++){
            q.Add(i.ToString());
        }
        turingMachine TM = new turingMachine { gammaTM = gamma, deltaTM = delta, bTM = b, q0TM = stateInit, qTM = q, sigmaTM = sigma, fTM = f};
        saveToFile(TM);
        if(mainMenu != null){
            mainMenu.SetActive(true);
        }
        if(machineCreation != null){
            machineCreation.SetActive(false);
        }

    }

    void saveToFile(turingMachine TM){
        foreach(var v in TM.gammaTM){
            Debug.Log(v);
        }
        string savesDir = Path.Combine(Application.dataPath, "saves"), fileName;
        if (!Directory.Exists(savesDir))
        {
            Directory.CreateDirectory(savesDir);
        }
        string[] files = Directory.GetFiles(savesDir);
        fileName = Path.Combine(savesDir, ("save " + files.Length));

        string json = JsonConvert.SerializeObject(TM);

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.Write(json);
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
    public TextMeshProUGUI displayGamma, bText;
    public Button addG, rmvG, backG, sigmaBt, lBBt, rBBt;
    private bool adicionarGamma = true;
    private int b;

    void AddClicado(){
        addG.GetComponent<Image>().color = green;

        rmvG.GetComponent<Image>().color = grey;

        adicionarGamma = true;
    }

    void RmvClicado(){
        addG.GetComponent<Image>().color = grey;

        rmvG.GetComponent<Image>().color = red;

        adicionarGamma = false;
    }

    void backBtnGamma(){
        if(gammaPan != null){
            gammaPan.SetActive(false);
        }
        if(machineCreation != null){
            machineCreation.SetActive(true);
        }
    }

    void lBAct(){
        if(b > 0)
            b--;
        if(b < gamma.Count && gamma.Count > 0)
            bText.text = (gamma.ElementAt(b)).ToString();
    }

    void rBAct(){
        b++;
        if(b < gamma.Count && gamma.Count > 0)
            bText.text = (gamma.ElementAt(b)).ToString();
        
    }

    void sigmaBtAct(){
        if(sigmaPan != null){
            sigmaPan.SetActive(true);
        }
        if(gammaPan != null){
            gammaPan.SetActive(false);
        }
    }
    //Sigma
    public HashSet<char> sigma = new HashSet<char>();
    public TextMeshProUGUI displaySigma;
    public Button addS, rmvS, backSigma;
    private bool adicionarSigma = true;

    void backSigmaAct(){
        if(sigmaPan != null){
            sigmaPan.SetActive(false);
        }
        if(gammaPan != null){
            gammaPan.SetActive(true);
        }
    }

    void AddClicadoSigma(){
        addS.GetComponent<Image>().color = green;

        rmvS.GetComponent<Image>().color = grey;

        adicionarSigma = true;
    }

    void RmvClicadoSigma(){
        addS.GetComponent<Image>().color = grey;

        rmvS.GetComponent<Image>().color = red;

        adicionarSigma = false;
    }

    //States
    public Button up1, up2, down1, down2, up3, down3, backState, editState;
    public TextMeshProUGUI count1, count2, count3;
    private int stateNum, stateCur, stateInit;

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
    void inc3(){
        if(stateInit < stateNum)
            stateInit++;
        count3.text = stateInit.ToString();
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

    void dec3(){
        if(stateInit >= 2)
            stateInit--;
        count3.text = stateInit.ToString();
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
            if(f.Contains(stateCur))
                isFinal.isOn = true;
            else
                isFinal.isOn = false;
            //wcurBehaviour = 1;
        }
    }

    //State editor

    private struct deltaT{
        public int ns, rs, ws, mt;
    }
    public TextMeshProUGUI edState, edNextState, edReadSymbol, edWriteSymbol, edMoveTo;
    public Button lNextState, rNextState, lReadSymbol, rReadSymbol, lWriteSymbol, rWriteSymbol, lMoveTo, rMoveTo, edBackBtn, saveBt;
    public Toggle isFinal;
    private int curNextState, curReadSymbol, curWriteSymbol, curMove;
    private List<deltaT>[] delta = new List<deltaT>[99];
    private List<int> f = new List<int>();

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

    void finalAct(bool isFinalOn){
        if(isFinalOn){
            if(!f.Contains(stateCur))
                f.Add(stateCur);
        }
        else{
            if(f.Contains(stateCur))
                f.Remove(stateCur);
        }
    }

    //Panel management
    public GameObject mainMenu,  machineCreation, gammaPan, qPan, editPan, sigmaPan;
    
    
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
        displayGamma.text = "{ " + string.Join(", ", gamma) + " }";
        backG.onClick.AddListener(backBtnGamma);
        lBBt.onClick.AddListener(lBAct);
        rBBt.onClick.AddListener(rBAct);
        sigmaBt.onClick.AddListener(sigmaBtAct);
        b = 0;

        //Sigma menu
        AddClicadoSigma();
        addS.onClick.AddListener(AddClicadoSigma);
        rmvS.onClick.AddListener(RmvClicadoSigma);
        displaySigma.text = "{ " + string.Join(", ", sigma) + " }";
        backSigma.onClick.AddListener(backSigmaAct);

        //state menu
        stateNum = 1;
        stateCur = 1;
        stateInit = 1;
        count1.text = stateNum.ToString();
        count2.text = stateNum.ToString();
        count3.text = stateInit.ToString();
        up1.onClick.AddListener(inc1);
        up2.onClick.AddListener(inc2);
        up3.onClick.AddListener(inc3);
        down1.onClick.AddListener(dec1);
        down2.onClick.AddListener(dec2);
        down3.onClick.AddListener(dec3);
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
        isFinal.onValueChanged.AddListener(finalAct);

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
        if(gamma.Count > 0){
            edReadSymbol.text = (gamma.ElementAt(curReadSymbol)).ToString();
            edWriteSymbol.text = (gamma.ElementAt(curWriteSymbol)).ToString();
        }
        else{
            edReadSymbol.text = "";
            edWriteSymbol.text = "";
        }
        
        if(curMove % 2 == 0)
            edMoveTo.text = "L";
        else
            edMoveTo.text = "R";

        //pannel management
        machineCreation.SetActive(false);
        gammaPan.SetActive(false);
        qPan.SetActive(false);
        editPan.SetActive(false);
        sigmaPan.SetActive(false);
        mainMenu.SetActive(true);

    }

    void Update(){
        if (Input.anyKeyDown)
        {
            if(gammaPan.activeSelf){
                foreach (char c in Input.inputString)
                {
                    if (!gamma.Contains(c) && adicionarGamma){
                        gamma.Add(c);
                    }
                    if(gamma.Contains(c) && !adicionarGamma){
                        gamma.Remove(c);
                        if(sigma.Contains(c))
                            sigma.Remove(c);
                    }
                }
                displayGamma.text = "{ " + string.Join(", ", gamma) + " }";
                if(b >= gamma.Count)
                    b--;
                if(b < gamma.Count && gamma.Count > 0)
                    bText.text = (gamma.ElementAt(b)).ToString();
                else
                    bText.text = "";
                
            }
            if(sigmaPan.activeSelf){
                foreach (char c in Input.inputString)
                {
                    if (!sigma.Contains(c) && adicionarSigma && gamma.Contains(c)){
                        Debug.Log("passeio");
                        sigma.Add(c);
                    }
                    if(sigma.Contains(c) && !adicionarSigma){
                        sigma.Remove(c);
                    }
                }
                Debug.Log(string.Join(", ", sigma));
                displaySigma.text = "{ " + string.Join(", ", sigma) + " }";                
            }

        }
    }

}
