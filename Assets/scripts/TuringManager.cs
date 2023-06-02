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
//using LeanTween;

public class TuringManager : MonoBehaviour
{

    private struct deltaT{
        public int ns, rs, ws, mt;
    }

    private struct turingMachine{
        
        public List<string> qTM { get; set; }
        public List<char> sigmaTM { get; set; }
        public List<char> gammaTM { get; set; }
        public List<deltaT>[] deltaTM { get; set; }
        public int q0TM { get; set; }
        public int bTM { get; set; }
        public List<int> fTM { get; set; }
        
    }

    private struct cabecote{
        public int pos;
        public int state;
    }

    public GameObject[] tile = new GameObject[7];
    public GameObject tileAux1, tileAux2;
    //7 and 8 are for aux1 and aux2
    public TextMeshProUGUI[] tileText = new TextMeshProUGUI[9], tilePos = new TextMeshProUGUI[9];
    public TextMeshProUGUI machineState, acceptState, currSpeed;
    public float animationSpeed;
    public Button leftBt, rightBt, mudaC, multiBt, goBack, fastBt, slowBt;
    private bool passed, isMachineExecuting, isMachineMoving;
    
    private turingMachine TM;
    private cabecote C;
    private int[] nonNegPos = new int[1000], negPos = new int[1000];
    void Start()
    {
        animationSpeed = 2f;
        loadFromFile();
        C.pos = 0;
        isMachineExecuting = false;
        isMachineMoving = false;
        C.state = TM.q0TM;
        leftBt.onClick.AddListener(moveLBt);
        rightBt.onClick.AddListener(moveRBt);
        mudaC.onClick.AddListener(nextSymbol);
        multiBt.onClick.AddListener(startSim);
        fastBt.onClick.AddListener(speedUp);
        slowBt.onClick.AddListener(speedDown);

        goBack.onClick.AddListener(LoadMain);
        for(int i = 0; 1000 > i; i++){
            nonNegPos[i] = TM.bTM;
            negPos[i] = TM.bTM;
        }

        machineState.text = "Current state: " + TM.qTM[C.state];
        acceptState.text = "Testing";
        currSpeed.text = "Speed:\n" + animationSpeed.ToString() + "x";
        loadPosition();
        

    }

    // Update is called once per frame
    void Update()
    {
        loadPosition();
        if (Input.GetKey(KeyCode.LeftArrow)){
            moveLBt();
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            moveRBt();
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            nextSymbol();
        }
        else if(Input.anyKeyDown){
            if(!isMachineExecuting && !isMachineMoving){
                foreach(char c in Input.inputString){
                    if(TM.sigmaTM.Contains(c)){
                        if(C.pos >= 0)
                            nonNegPos[C.pos] = TM.gammaTM.IndexOf(c);
                        else
                            negPos[C.pos] = TM.gammaTM.IndexOf(c);
                        moveR();
                    }
                }
            }
        }

    }

    void speedUp(){
        if(animationSpeed < 8)
            animationSpeed *= 2;
        currSpeed.text = "Speed:\n" + animationSpeed.ToString() + "x";
    }

    void speedDown(){
        if(animationSpeed > 1)
            animationSpeed /= 2;
        currSpeed.text = "Speed:\n" + animationSpeed.ToString() + "x";
    }

    void moveRBt(){
        moveR();
    }

    void moveLBt(){
        moveL();
    }
    async System.Threading.Tasks.Task moveR(){
        if(isMachineMoving){
            return;
        }
        isMachineMoving = true;

        for(int i = 0; 7 > i; i++){
            LeanTween.moveLocalX(tile[i], tile[i].transform.localPosition.x - 50f, 1/animationSpeed);
        }
        //assign char to tileAux2

        LeanTween.moveLocalX(tileAux2, tileAux2.transform.localPosition.x - 50f, 1/animationSpeed);

        await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 1000));
        
        loadPosition();

        C.pos++;
        loadPosition();
        isMachineMoving = false;

        if(isMachineExecuting){
            await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 4000));
        }

        return;
    }

    async System.Threading.Tasks.Task moveL(){

        if(isMachineMoving){
            return;
        }
        isMachineMoving = true;

        for(int i = 0; 7 > i; i++){
            LeanTween.moveLocalX(tile[i], tile[i].transform.localPosition.x + 50f, 1/animationSpeed);
        }
        //assign char to tileAux1
        
        LeanTween.moveLocalX(tileAux1, tileAux1.transform.localPosition.x + 50f, 1/animationSpeed);
        
        await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 1000));

        C.pos--;
        loadPosition();

        if(isMachineExecuting){
            await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 4000));
        }

        isMachineMoving = false;

        return;
    }

    void loadFromFile(){
        string filePath = Path.Combine(Application.dataPath, "saves", "save " + PlayerPrefs.GetInt("saveN").ToString());
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            TM = JsonConvert.DeserializeObject<turingMachine>(jsonData);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    void nextSymbol(){
        if(isMachineMoving){
            return;
        }
        //isMachineMoving = true;
        if(C.pos >= 0){
            do
            {

                nonNegPos[C.pos] = (nonNegPos[C.pos] + 1) % TM.gammaTM.Count;
                Debug.Log(nonNegPos[C.pos]);

            }while(!TM.sigmaTM.Contains(TM.gammaTM.ElementAt(nonNegPos[C.pos])) && nonNegPos[C.pos] != TM.bTM);
        }
        else{
            do
            {

                negPos[-C.pos] = (negPos[-C.pos] + 1) % TM.gammaTM.Count;

            }while(!TM.sigmaTM.Contains(TM.gammaTM.ElementAt(negPos[-C.pos])) && negPos[-C.pos] != TM.bTM);
        }
        loadPosition();
    }

    async System.Threading.Tasks.Task nextMove(){
        if(TM.fTM.Contains(C.state)){
            passed = true;
        }
        if(passed)
            return;
        int currState = C.state, curr;
        foreach(var it in TM.deltaTM[C.state]){
            if(C.pos >= 0){
                if(it.rs == nonNegPos[C.pos]){
                    int auxState = C.state, auxVal = nonNegPos[C.pos];
                    nonNegPos[C.pos] = it.ws;
                    C.state = it.ns;
                    if(it.mt % 2 == 0){
                        await moveL();
                        await nextMove();
                        if(passed)
                            return;
                        await moveR();
                    }
                    else{
                        await moveR();
                        await nextMove();
                        if(passed)
                            return;
                        await moveL();
                    }
                    nonNegPos[C.pos] = auxVal;
                    C.state = auxState;
                    await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 4000));
                }
            }
            else{
                if(it.rs == negPos[-C.pos]){
                    int auxState = C.state, auxVal = negPos[-C.pos];
                    negPos[-C.pos] = it.ws;
                    C.state = it.ns;
                    if(it.mt % 2 == 0){
                        await moveL();
                        await nextMove();
                        if(passed)
                            return;
                        await moveR();
                    }
                    else{
                        await moveR();
                        await nextMove();
                        if(passed)
                            return;
                        await moveL();
                    }
                    negPos[C.pos] = auxVal;
                    C.state = auxState;
                    await System.Threading.Tasks.Task.Delay((int)((1/animationSpeed) * 4000));
                }
            }
        }
    }

    void loadPosition(){
        //set position
        int auxPos = -150;
        tileAux1.transform.localPosition = new Vector3(-200, 0, 0);
        for(int i = 0; 7 > i; i++){
            tile[i].transform.localPosition = new Vector3(auxPos, 0, 0);
            auxPos += 50;
        }    
        tileAux2.transform.localPosition = new Vector3(200, 0, 0);
        //set position numbers
        tilePos[7].text = (C.pos - 4).ToString();
        tilePos[8].text = (C.pos + 4).ToString();
        for(int i = 0; 7 > i; i++){
            tilePos[i].text = (C.pos + i - 3).ToString();
        }
        //set tile texts
        if((C.pos - 4)  >= 0)
            tileText[7].text = TM.gammaTM.ElementAt(nonNegPos[C.pos - 4]).ToString();
        else
            tileText[7].text = TM.gammaTM.ElementAt(negPos[0-(C.pos - 4)]).ToString();
        if((C.pos + 4)  >= 0)
            tileText[8].text = TM.gammaTM.ElementAt(nonNegPos[C.pos + 4]).ToString();
        else
            tileText[8].text = TM.gammaTM.ElementAt(negPos[0-(C.pos + 4)]).ToString();
        for(int i = 0; 7 > i; i++){
            if((C.pos + i - 3)  >= 0)
                tileText[i].text = TM.gammaTM.ElementAt(nonNegPos[C.pos + i - 3]).ToString();
            else
                tileText[i].text = TM.gammaTM.ElementAt(negPos[0-(C.pos + i - 3)]).ToString();
        }

    }
    async void startSim(){
        //C.pos = 5;
        passed = false;
        isMachineExecuting = true;
        loadPosition();
        leftBt.onClick.RemoveListener(moveLBt);
        rightBt.onClick.RemoveListener(moveRBt);
        mudaC.onClick.RemoveListener(nextSymbol);

        await nextMove();

        if(passed){
            acceptState.text = "Accepted!";
            acceptState.color = Color.green;
        }
        else{
            acceptState.text = "Rejected!";
            acceptState.color = Color.red;
        }
        leftBt.onClick.AddListener(moveLBt);
        rightBt.onClick.AddListener(moveRBt);
        mudaC.onClick.AddListener(nextSymbol);

    }

    public void LoadMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
