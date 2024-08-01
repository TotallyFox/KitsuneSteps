using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliders : MonoBehaviour
{
    public GameObject playerScript;

    public Slider lucaHealth;

    // Start is called before the first frame update
    void Start()
    {
        lucaHealth = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        lucaHealth.value = playerScript.GetComponent<LucaController>().health;
    }
}
