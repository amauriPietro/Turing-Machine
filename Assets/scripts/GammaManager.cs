using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GammaManager : MonoBehaviour
{
    public Color red = new Color(0.56f, 0.23f, 0.21f);
    public Color green = new Color(0.39f, 0.41f, 0.19f);
    public Color grey = new Color(0.33f, 0.33f, 0.33f);

    public HashSet<char> gamma = new HashSet<char>();
    
    public TextMeshProUGUI display;

    public Button add, rmv;

    private bool adicionar = true;

    void Start(){
        AddClicado();
        add.onClick.AddListener(AddClicado);
        rmv.onClick.AddListener(RmvClicado);
    }

    void AddClicado(){
        Debug.Log("add!!");
        add.GetComponent<Image>().color = green;

        rmv.GetComponent<Image>().color = grey;

        adicionar = true;
    }

    void RmvClicado(){
        Debug.Log("rmv!!");
        add.GetComponent<Image>().color = grey;

        rmv.GetComponent<Image>().color = red;

        adicionar = false;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
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