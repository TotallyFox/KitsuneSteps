using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlaskCharge : MonoBehaviour
{
    private Slider slider;

    public GameObject playerScript;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerScript.GetComponent<LucaController>().flaskCharge;
    }
}
