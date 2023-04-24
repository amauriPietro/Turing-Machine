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
        public HashSet<char> sigmaTM { get; set; }
        public HashSet<char> gammaTM { get; set; }
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
    public TextMeshProUGUI currState;
    public float animationSpeed;
    public Button leftBt, rightBt, mudaC, startBt;
    private bool failed;
    
    private turingMachine TM;
    private cabecote C;
    private int[] nonNegPos = new int[1000], negPos = new int[1000];
    void Start()
    {
        animationSpeed = 0.5f;
        loadFromFile();
        C.pos = 0;
        C.state = TM.q0TM;
        leftBt.onClick.AddListener(moveL);
        rightBt.onClick.AddListener(moveR);
        mudaC.onClick.AddListener(nextSymbol);
        startBt.onClick.AddListener(startSim);
        for(int i = 0; 1000 > i; i++){
            nonNegPos[i] = TM.bTM;
            negPos[i] = TM.bTM;
        }

        currState.text = "Current state: " + TM.qTM[C.state];
        loadPosition();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void moveR(){
        
        for(int i = 0; 7 > i; i++){
            LeanTween.moveLocalX(tile[i], tile[i].transform.localPosition.x - 50f, animationSpeed);
        }
        //assign char to tileAux2

        LeanTween.moveLocalX(tileAux2, tileAux2.transform.localPosition.x - 50f, animationSpeed);
        StartCoroutine(afterR());
    }

    void moveL(){
        for(int i = 0; 7 > i; i++){
            LeanTween.moveLocalX(tile[i], tile[i].transform.localPosition.x + 50f, animationSpeed);
        }
        //assign char to tileAux1
        
        LeanTween.moveLocalX(tileAux1, tileAux1.transform.localPosition.x + 50f, animationSpeed);
        StartCoroutine(afterL());
    }
    void loadFromFile(){
        string filePath = Path.Combine(Application.dataPath, "saves", "save 0");
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

    void nextMove(){
        failed = true;
        if(TM.fTM.Contains(C.state)){
            currState.text = "Accepted!";
            currState.color = Color.green;
            return;
        }
        foreach(var it in TM.deltaTM[C.state]){
            if(C.pos >= 0){
                if(it.rs == nonNegPos[C.pos]){
                    nonNegPos[C.pos] = it.ws;
                    C.state = it.ns;
                    if(it.mt % 2 == 0)
                        moveL();
                    else
                        moveR();
                    failed = false;
                    break;
                }
            }
            else{
                if(it.rs == negPos[-C.pos]){
                    negPos[-C.pos] = it.ws;
                    C.state = it.ns;
                    if(it.mt % 2 == 0)
                        moveL();
                    else
                        moveR();
                    failed = false;
                    break;
                }
            }
        }
        if(failed){
            currState.text = "Rejected!";
            currState.color = Color.red;
            return;
        }
        currState.text = "Current state: " + TM.qTM[C.state];
        StartCoroutine(afterNextMove());
    }

    IEnumerator afterR()
    {

        yield return new WaitForSeconds(animationSpeed);

        C.pos++;
        loadPosition();
        //tile[7].transform.position = tileAux2.transform.position;

    }

    IEnumerator afterNextMove()
    {

        yield return new WaitForSeconds(animationSpeed*4);

        nextMove();

    }

    IEnumerator afterL()
    {

        yield return new WaitForSeconds(animationSpeed);

        C.pos--;
        loadPosition();

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
    void startSim(){
        C.pos = 0;
        loadPosition();
        leftBt.onClick.RemoveListener(moveR);
        rightBt.onClick.RemoveListener(moveL);
        mudaC.onClick.RemoveListener(nextSymbol);
        startBt.onClick.RemoveListener(startSim);
        nextMove();

    }
}
