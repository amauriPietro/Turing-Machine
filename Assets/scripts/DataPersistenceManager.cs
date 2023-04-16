using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private TuringMachine turingMachine;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake(){
        if(instance != null)
            Debug.LogError("Error.");
        instance = this;
    }

    public void NewMachine(){
        this.turingMachine = new TuringMachine();
    }

    public void LoadMachine(){

    }

    public void SaveMachine(){
        
    }
}
