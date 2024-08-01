using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class PlantCountUI : MonoBehaviour
{
    public GameObject playerScript;
    public TMP_Text moonlitBerry;
    public TMP_Text spiceComb;
    public TMP_Text mint;

    public TMP_Text mbFlask;
    public TMP_Text scFlask;
    public TMP_Text mFlask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moonlitBerry.text = playerScript.GetComponent<LucaController>().moonlitBerryCount.ToString();
        spiceComb.text = playerScript.GetComponent<LucaController>().spiceCombCount.ToString();
        mint.text = playerScript.GetComponent<LucaController>().mintCount.ToString();
        mbFlask.text = playerScript.GetComponent<LucaController>().berryDrink.ToString();
        scFlask.text = playerScript.GetComponent<LucaController>().spiceDrink.ToString();
        mFlask.text = playerScript.GetComponent<LucaController>().mintDrink.ToString();
    }
}
