using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    public float speed = 10f;
    public float rollSpeed = 10f;
    public float jumpCount = 1;
    public float airDashCount = 1;
    public float dodgeRoll = 2;
    public float lightningJump = 1;
    public bool isFloating;
    public float gravity = -19.62f;

    public float health = 200;
    public float mana = 100;

    public float attCount = 0;
    public float attAirCount = 0;

    public bool isAttacking;

    private bool m_FacingRight = true;

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        var velocity = controller.velocity;

        //To-Do when Grounded
        if (controller.isGrounded)
        {
            velocity.y = 0;
            isFloating = false;
            airDashCount = 1;
        }

        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        //Left & Right Movement
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= speed;
        }

        //Jump
        if (Input.GetKey(KeyCode.Space) && controller.isGrounded && jumpCount == 1)
        {
            velocity.y += 10f;
        }

        //Glide
        if (Input.GetKey(KeyCode.Space) && !controller.isGrounded && isFloating == false)
        {
            velocity.y = 0f;
            isFloating = true;
        }

        //Cancel Glide
        if (Input.GetKey(KeyCode.Space) && !controller.isGrounded && isFloating == true)
        {
            velocity.y -= 5f;
            isFloating = false;
        }

        //Airdash Left
        if (Input.GetKey(KeyCode.LeftShift) && !controller.isGrounded && velocity.x <=0 && airDashCount == 1)
        {
            velocity.x -= rollSpeed;
            airDashCount -= 1;
            isFloating = false;
        }

        //Airdash Right
        if (Input.GetKey(KeyCode.LeftShift) && !controller.isGrounded && velocity.x >= 0 && airDashCount == 1)
        {
            velocity.x += rollSpeed;
            airDashCount -= 1;
            isFloating = false;
        }

        //Dodgeroll Left
        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && velocity.x <= 0 && dodgeRoll < 0)
        {
            velocity.x -= rollSpeed;
            dodgeRoll -= 1;
        }

        //Dodgeroll Right
        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && velocity.x >= 0 && dodgeRoll < 0)
        {
            velocity.x += rollSpeed;
            dodgeRoll -= 1;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
