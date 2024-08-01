using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirController : MonoBehaviour
{
    public float health = 100;
    public float physDMG = 20;
    public float physATK = 30;
    public float rangeATK = 10;
    public GameObject gotHit;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PhysicalAtk")
        {
            health -= 25;
            Instantiate(gotHit, rb.transform.position, rb.transform.rotation);
        }
        if (other.tag == "MagicAtk")
        {
            health -= 50;
            Instantiate(gotHit, rb.transform.position, rb.transform.rotation);
        }
    }
}
