using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaSlider : MonoBehaviour
{
    public GameObject playerScript;
    public Slider lucaMana;

    // Start is called before the first frame update
    void Start()
    {
        lucaMana = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        lucaMana.value = playerScript.GetComponent<LucaController>().mana;
    }
}
