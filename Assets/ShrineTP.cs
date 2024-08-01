using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineTP : MonoBehaviour
{
    public Transform shrinePosEnter;
    public Transform shrinePosExit;

    public GameObject shrineEnter;
    public GameObject shrineExit;

    // Start is called before the first frame update
    void Start()
    {
        shrinePosEnter = shrineEnter.transform;
        shrinePosExit = shrineExit.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
