using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehavior : MonoBehaviour
{
    public GameObject moonLitBerryDrop;
    public GameObject spiceCombDrop;
    public GameObject mintDrop;

    public Transform plantLocation;
    public GameObject currentPlant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PhysicalAtk" || other.tag == "MagicAtk")
        {
            if (currentPlant.tag == "MoonlitBerryPlant")
            Instantiate(moonLitBerryDrop, plantLocation.position, plantLocation.rotation);

            if (currentPlant.tag == "SpiceCombPlant")
            Instantiate(spiceCombDrop, plantLocation.position, plantLocation.rotation);

            if (currentPlant.tag == "MintPlant")
            Instantiate(mintDrop, plantLocation.position, plantLocation.rotation);

            Destroy(gameObject);
        }
    }
}
