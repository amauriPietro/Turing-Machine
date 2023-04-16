using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class TuringMachine
{
    HashSet<string> gamma = new HashSet<string>();
    HashSet<string> b = new HashSet<string>();
    HashSet<string> sigma = new HashSet<string>();
    HashSet<string> Q = new HashSet<string>();
    HashSet<string> q0 = new HashSet<string>();
    HashSet<string> F = new HashSet<string>();
    //curr state, next state, read symbol, write symbol, move(true = r, false = l) 
    HashSet<(string, string, char, char, bool)> delta = new HashSet<(string, string, char, char, bool)>();
    

}
