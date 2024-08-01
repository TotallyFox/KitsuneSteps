using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100;
    public float physDMG = 20;
    public float physATK = 30;
    public float rangeATK = 10;

    public GameObject playerScript;

    public Transform playerLocation;
    public Transform currentPoint;
    public Vector2 startingPos;
    public float speed = 3f;
    private Rigidbody2D rb;
    public Vector2 playerPos;

    public bool isAgro = false;
    public bool canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playerLocation.transform.position;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (Vector2.Distance(currentPoint.position, playerLocation.position) <= 15f && playerScript.GetComponent<LucaController>().isDashing == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }
        if (Vector2.Distance(currentPoint.position, playerLocation.position) >= 15f && playerScript.GetComponent<LucaController>().isDashing == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startingPos.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
        }

        if (Vector2.Distance(currentPoint.position, playerLocation.position) >= 3f && canDash == true)
        {
            StartCoroutine(EnemyDash());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PhysicalAtk")
        {
            health -= 25;
        }
        if (other.tag == "MagicAtk")
        {
            health -= 50;
        }
    }

    private IEnumerator EnemyDash()
    {
        speed = 0f;

        yield return new WaitForSeconds(0.5f);

        speed = 12f;
        canDash = false;

        yield return new WaitForSeconds(0.3f);

        speed = 0f;

        yield return new WaitForSeconds(0.4f);

        speed = 3f;

        yield return new WaitForSeconds(4f);

        canDash = true;
    }
}
