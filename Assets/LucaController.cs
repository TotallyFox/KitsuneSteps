using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Animations;

public class LucaController : MonoBehaviour
{
    [Header("Character Movement")]
    private float horizontal;
    private float speed = 8f;
    private float jumpHeight = 22f;
    private bool isFacingRight = true;
    public float jumpCount = 1;
    public float lightningJump = 1;
    public bool isCharged = false;
    public bool isFloating = false;
    public bool canTeleport = false;
    public Transform teleportPos;
    public bool hitInv = false;
    public Animator anim;

    [Header("Character Stats")]
    public float health = 200;
    public float maxHealth = 200;
    public float mana = 100;
    public float maxMana = 100;
    public float minMana = 0;
    public float flaskCharge = 200;
    public float maxFlaskCharge = 200;

    [Header("Potion Stats")]
    public float moonlitBerryCount = 0;
    public float flaskGen = 15;
    public float spiceCombCount = 0;
    public float healthGen = 20;
    public float mintCount = 0;
    public float manaGen = 10;

    [Header("Attack Stats")]
    public float atkCount = 0;
    public float atkAirCount = 0;
    public float atkDelay = 0.7f;
    public GameObject physHurtBox;
    public Transform physBoxSpawn;
    public GameObject fireBall;
    public GameObject fireBallLeft;
    public Transform fireBallSpawn;
    public GameObject lightningAtk;
    public bool isAttacking = false;

    [Header("Dash Stats")]
    private bool canDash = true;
    public bool isDashing;
    private float dashingPower = 34f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Header("Potion Drinking")]
    public bool isDrinking = false;
    public float berryDrink = 0;
    public float spiceDrink = 0;
    public float mintDrink = 0;

    [Header("Misc")]
    public GameManager gameManager;
    private bool isDead;
    public GameObject onHit;
    public GameObject player;
    public Renderer rend;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FlaskDegen", 1f, 1f);
        InvokeRepeating("ManaGen", 1f, 1f);
        anim = rb.GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            isFloating = false;
            rb.gravityScale = 4f;
            lightningJump = 1;
            atkAirCount = 0;
            anim.SetBool("isGroundedAnim", true);
            anim.SetBool("isFloatingAnim", false);
            anim.SetBool("isRunning", false);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (mana > maxMana)
        {
            mana = maxMana;
        }
        if (mana < minMana)
        {
            mana = minMana;
        }
        if (flaskCharge > maxFlaskCharge)
        {
            flaskCharge = maxFlaskCharge;
        }

        if (health <= 0 && isDead == false || flaskCharge <= 0 && isDead == false)
        {
            isDead = true;
            gameManager.gameOver();
            Instantiate(onHit, rb.transform.position, rb.transform.rotation);
            rend.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        //Misc. Animation Controls
        if (Input.GetButton("Horizontal") && IsGrounded())
        {
            anim.SetBool("isRunning", true);
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            anim.SetBool("isRunning", false);
        }

        if (!IsGrounded())
        {
            anim.SetBool("isGroundedAnim", false);
        }

        if (Input.GetButtonDown("Atk1") && atkCount == 0 && IsGrounded())
        {
            anim.SetTrigger("atkGroundOne");
        }
        if (Input.GetButtonDown("Atk1") && atkCount == 1 && IsGrounded())

        {
            anim.SetTrigger("atkGroundTwo");
        }
        if (Input.GetButtonDown("Atk1") && atkCount == 2 && IsGrounded())
        {
            anim.SetTrigger("atkGroundThree");
        }

        if (Input.GetButtonDown("Atk1") && atkAirCount == 0 && !IsGrounded())
        {
            anim.SetTrigger("atkAirOne");
        }
        if (Input.GetButtonDown("Atk1") && atkAirCount == 1 && !IsGrounded())
        {
            anim.SetTrigger("atkAirTwo");
        }
        if (Input.GetButtonDown("Atk1") && atkAirCount == 2 && !IsGrounded())
        {
            anim.SetTrigger("atkAirThree");
        }

        //Jump & Hover
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        if (Input.GetButtonDown("Jump") && IsGrounded() && isCharged == true && mana >= 10)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight * 1.5f);
            isCharged = false;
            mana -= 10;
            StartCoroutine(JumpTrail());
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Jump") && !IsGrounded() && isFloating == false)
        {
            isFloating = true;
            anim.SetBool("isFloatingAnim", true);
            rb.velocity = new Vector2(transform.localScale.x, 0f);
            rb.gravityScale = 0.2f;
        }
        if (Input.GetButtonUp("Jump") && !IsGrounded() && isFloating == true)
        {
            isFloating = false;
            anim.SetBool("isFloatingAnim", false);
            rb.gravityScale = 4f;
        }
        while (isFloating == true && rb.velocity.y > 3)
        {
            rb.velocity = new Vector2(rb.velocity.x, 3f);
        }

        //Charge
        if (Input.GetButtonDown("Charge") && isCharged == false)
        {
            isCharged = true;
        }

        //Dodge
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        //Ground Attack
        if (Input.GetButtonDown("Atk1") && IsGrounded() && atkCount < 2)
        {
            Instantiate(physHurtBox, physBoxSpawn.position, physBoxSpawn.rotation);
            rb.velocity = new Vector2(transform.localScale.x * 0.4f, 0f);
            isAttacking = true;
            atkCount += 1;
        }
        else if (Input.GetButtonDown("Atk1") && IsGrounded() && atkCount == 2)
        {
            atkCount += 1;
            Instantiate(physHurtBox, physBoxSpawn.position, physBoxSpawn.rotation);
            rb.velocity = new Vector2(transform.localScale.x * 0.4f, 0f);
            isAttacking = true;
            StartCoroutine(atkReset());
        }

        //Aerial Attack
        if (Input.GetButtonDown("Atk1") && !IsGrounded() && atkAirCount < 2)
        {
            Instantiate(physHurtBox, physBoxSpawn.position, physBoxSpawn.rotation);
            rb.velocity = new Vector2(0f, 8f);
            isAttacking = true;
            atkAirCount += 1;
        }
        else if (Input.GetButtonDown("Atk1") && !IsGrounded() && atkAirCount == 2)
        {
            atkAirCount += 1;
            Instantiate(physHurtBox, physBoxSpawn.position, physBoxSpawn.rotation);
            rb.velocity = new Vector2(0f, 8f);
            isAttacking = true;
            StartCoroutine(atkAirReset());
        }

        //Fireball Attack
        if (Input.GetButtonDown("Atk2") && isFacingRight && mana >= 25 && !IsGrounded())
        {
            Instantiate(fireBall, fireBallSpawn.position, fireBallSpawn.rotation);
            rb.velocity = new Vector2(0f, 8f);
            mana -= 25;
            anim.SetTrigger("fireAnim");
        }
        else if (Input.GetButtonDown("Atk2") && !isFacingRight && mana >= 25 && !IsGrounded())
        {
            Instantiate(fireBallLeft, fireBallSpawn.position, fireBallSpawn.rotation);
            rb.velocity = new Vector2(0f, 8f);
            mana -= 25;
            anim.SetTrigger("fireAnim");
        }
        if (Input.GetButtonDown("Atk2") && isFacingRight && mana >= 25 && IsGrounded())
        {
            Instantiate(fireBall, fireBallSpawn.position, fireBallSpawn.rotation);
            mana -= 25;
            anim.SetTrigger("fireAnim");
        }
        else if (Input.GetButtonDown("Atk2") && !isFacingRight && mana >= 25 && IsGrounded())
        {
            Instantiate(fireBallLeft, fireBallSpawn.position, fireBallSpawn.rotation);
            mana -= 25;
            anim.SetTrigger("fireAnim");
        }

        /*else if (Input.GetButtonDown("Atk2") && isCharged == true)
        {
            StartCoroutine(LightningAttack());
            isCharged = false;
        }*/

        //Potion Use
        if (Input.GetButtonDown("PotionUse") && IsGrounded() && isDrinking == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            rb.Sleep();
            rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y * 0);
            StartCoroutine(Drinking());
        }
        else if (Input.GetButtonDown("PotionUse") && IsGrounded() && isDrinking == false && berryDrink == 0 && spiceDrink == 0 && mintDrink == 0)
        {
            return;
        }

        if (Input.GetButtonDown("AddBerry") && moonlitBerryCount > 0)
        {
            moonlitBerryCount -= 1;
            berryDrink += 1;
        }
        if (Input.GetButtonDown("AddSpice") && spiceCombCount > 0)
        {
            spiceCombCount -= 1;
            spiceDrink += 1;
        }
        if (Input.GetButtonDown("AddMint") && mintCount > 0)
        {
            mintCount -= 1;
            mintDrink += 1;
        }

        //Shrine Teleport
        if (Input.GetButtonDown("Teleport") && canTeleport == true)
        {
            transform.position = teleportPos.transform.position;
        }

        //On-Hit I-Frames
        while (hitInv == true)
        {
            return;
        }

        Flip();
    }

    private IEnumerator Drinking()
    {
        isDrinking = true;
        anim.SetTrigger("isDrinkingAnim");

        flaskCharge = flaskCharge + flaskGen * berryDrink;
        mana = mana + manaGen * mintDrink;
        health = health + healthGen * spiceDrink;

        berryDrink = 0;
        mintDrink = 0;
        spiceDrink = 0;

        yield return new WaitForSeconds(0.5f);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDrinking = false;
        rb.WakeUp();
    }

    private IEnumerator LightningAttack()
    {
        rb.gravityScale = 0.2f;
        rb.velocity = new Vector2(0f, 4f);
        mana -= 35;

        yield return new WaitForSeconds(0.4f);

        Instantiate(lightningAtk, physBoxSpawn.position, physBoxSpawn.rotation);

        yield return new WaitForSeconds(0.1f);

        rb.gravityScale = 4f;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashAnim", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashAnim", false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator JumpTrail()
    {
        tr.emitting = true;
        yield return new WaitForSeconds(1.2f);
        tr.emitting = false;
    }

    private IEnumerator atkReset()
    {
        yield return new WaitForSeconds(atkDelay);
        atkCount = 0;
        isAttacking = false;
    }
    private IEnumerator atkAirReset()
    {
        yield return new WaitForSeconds(atkDelay);
        isAttacking = false;
    }

    private IEnumerator EnableBox(float waitTime)
    {
        hitInv = true;
        yield return new WaitForSeconds(waitTime);
        hitInv = false;
        Physics.IgnoreLayerCollision(8, 11, false);
    }

    private void FlaskDegen()
    {
        flaskCharge -= 1;
    }

    private void ManaGen()
    {
        mana += 1;
    }

    private void FlaskFull()
    {
        flaskCharge += 100;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MoonlitBerry")
        {
            moonlitBerryCount += 1;
        }
        if (other.tag == "SpiceComb")
        {
            spiceCombCount += 1;
        }
        if (other.tag == "Mint")
        {
            mintCount += 1;
        }
        
        if (other.tag == "EnemyPhys" && !isDashing && hitInv == false)
        {
            health -= 20;
            rb.velocity = new Vector2(0f, 15f);
            Physics.IgnoreLayerCollision(8, 11, true);
            Instantiate(onHit, rb.transform.position, rb.transform.rotation);
            StartCoroutine(EnableBox(1.0f));
        }
        if (other.tag == "EnemyAtk" && !isDashing && hitInv == false)
        {
            health -= 8;
            rb.velocity = new Vector2(0f, 15f);
            Physics.IgnoreLayerCollision(8, 11, true);
            Instantiate(onHit, rb.transform.position, rb.transform.rotation);
            StartCoroutine(EnableBox(1.0f));
        }
        if (other.tag == "EnemyRange" && !isDashing && hitInv == false)
        {
            health -= 5;
            rb.velocity = new Vector2(0f, 15f);
            Physics.IgnoreLayerCollision(8, 11, true);
            Instantiate(onHit, rb.transform.position, rb.transform.rotation);
            StartCoroutine(EnableBox(1.0f));
        }

        if (other.tag == "Shrine")
        {
            teleportPos = other.GetComponent<ShrineTP>().shrinePosExit;
            canTeleport = true;
            //transform.position = teleportPos.transform.position;
        }

        if (other.tag == "Hazard" && isDashing == false)
        {
            health -= 20;
            rb.velocity = new Vector2(0f, 15f);
            Physics.IgnoreLayerCollision(8, 11, true);
            Instantiate(onHit, rb.transform.position, rb.transform.rotation);
            StartCoroutine(EnableBox(1.0f));
        }

        if (other.tag == "End")
        {
            gameManager.endGame();
            InvokeRepeating("FlaskFull", 1f, 1f);
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Shrine")
        {
            teleportPos = null;
            canTeleport = false;
        }
    }
}
