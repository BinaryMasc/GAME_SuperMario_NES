using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

#pragma warning disable

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float jump_Power = 960f;
    [SerializeField] private float speed = 2f;
    //[SerializeField] private float runSpeed = 2f;


    //  Sprites Mario Base
    [SerializeField] private Sprite sprite_jump_Right_base;
    [SerializeField] private Sprite sprite_jump_Left_base;

    [SerializeField] private Sprite sprite_stay_Right_base;
    [SerializeField] private Sprite sprite_stay_Left_base;
    //
    //  Sprites Mario Big
    [SerializeField] private Sprite sprite_jump_Right_Big;
    [SerializeField] private Sprite sprite_jump_Left_Big;

    [SerializeField] private Sprite sprite_CrouchedD_Right_Big;
    [SerializeField] private Sprite sprite_CrouchedD_Left_Big;

    [SerializeField] private Sprite sprite_stay_Right_Big;
    [SerializeField] private Sprite sprite_stay_Left_Big;
    //


    private Sprite sprite_Jump_Left;
    private Sprite sprite_Jump_Right;

    private Sprite sprite_Stay_Left;
    private Sprite sprite_Stay_Right;


    GameObject levelController;


#pragma warning disable

    private Camera camera;
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRender;
    private Animator animator;
    private BoxCollider2D boxCollider;
    
#pragma warning enable

    private bool is_Grounded;
    private bool is_running;
    private bool is_move;
    private bool view_right;

    private bool Key_Jump;
    private bool Key_Move_Left;
    private bool Key_Move_Right;

    private bool have_star;

    private float actual_speed;

    private int coins;
    private int lives;

    [HideInInspector] public Mario_States Mario_State;



    // Start is called before the first frame update
    void Start()
    {

        sprite_Jump_Left = sprite_jump_Left_base;
        sprite_Jump_Right = sprite_jump_Right_base;

        sprite_Stay_Left = sprite_stay_Left_base;
        sprite_Stay_Right = sprite_stay_Right_base;

        camera = GetComponent<Camera>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        levelController = GameObject.Find("levelController");

        is_Grounded = false;
        is_move = false;
        is_running = false;
        have_star = false;
        view_right = true;

        //  TODO
        //lives = 3;

        Mario_State = Mario_States.Small;
    }

    // Update is called once per frame
    void Update()
    {

        //  TODO: MULTIPLATFORM EARRING (CrossPlatformInputManager)
        if (Input.GetKeyDown(KeyCode.Space)) Key_Jump = true;

        if (Input.GetKey(KeyCode.LeftShift)) is_running = true;
        else
        {
            is_running = false;
            animator.speed = 1;
        }



        actual_speed = CrossPlatformInputManager.GetAxis("Horizontal") * (is_running ? 2 : 1);

        if(actual_speed != 0)
        {
            transform.position += new Vector3(actual_speed * Time.deltaTime * speed, 0);
            is_move = true;

            view_right = actual_speed > 0 ? true : false;

            animator.SetBool("is_view_right", view_right);

            if (!animator.enabled)
            {
                animator.enabled = true;
                //if(view_right) animator.Play("Mario_Walk_base_right");
                //else animator.Play("Mario_Walk_base_left");

                if (is_running) animator.speed = 2;
            }


        }
        else is_move = false;



        if (!is_Grounded)
        {
            animator.enabled = false;

            if (view_right) spriteRender.sprite = sprite_Jump_Right;
            else spriteRender.sprite = sprite_Jump_Left;
        }
        else if (!is_move)
        {
            animator.enabled = false;
            spriteRender.sprite = view_right ? sprite_Stay_Right : sprite_Stay_Left;
            //if (view_right) animator.Play("Mario_Walk_base_right");
            //else animator.Play("Mario_Walk_base_left");
        }


        //Debug.Log(Mario_State);
    }

    private void FixedUpdate()
    {
        if (Key_Jump && is_Grounded)
        {
            rigidbody.AddForce(Vector2.up * jump_Power);
            is_Grounded = false;

        }


        ResetKeys();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);

        if (collision.gameObject.CompareTag("Floor"))
        {
            is_Grounded = true;
            return;
        }

        else if(side == EntityFunctions.Side_Collision.DOWN) is_Grounded = true;

        bool is_enemy = collision.gameObject.CompareTag("Enemy") ? true : false;

        

        if (is_enemy)
        {
            //  Kill enemies
            if (side == EntityFunctions.Side_Collision.DOWN)
            {

            }
            //  Oops
            else
            {
                Damage();
            }
        }

        

        //---




    }

    


    private void OnCollisionExit2D(Collision2D collision)
    {

        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);

        if (side == EntityFunctions.Side_Collision.DOWN) 
        { 
            is_Grounded = false; 
            return; 
        }

    }


    private void AddPoints(int points)
    {
        //  CODE HERE...
    }

    //---
    private void SetMarioBase()
    {
        Mario_State = Mario_States.Small;
        animator.SetInteger("Mario_State", 0);

        boxCollider.size = new Vector2(0.1f, 0.1f);

        //  Set sprites
        sprite_Jump_Left = sprite_jump_Left_base;
        sprite_Jump_Right = sprite_jump_Right_base;
        //
        sprite_Stay_Left = sprite_stay_Left_base;
        sprite_Stay_Right = sprite_stay_Right_base;
    }

    private void SetMarioBig()
    {
        Mario_State = Mario_States.Big;
        animator.SetInteger("Mario_State", 1);
        spriteRender.size = new Vector2(0.1f,1.5f);

        boxCollider.size = new Vector2(0.15f, 0.32f);

        //  Set sprites
        sprite_Jump_Left = sprite_jump_Left_Big;
        sprite_Jump_Right = sprite_jump_Right_Big;
        //
        sprite_Stay_Left = sprite_stay_Left_Big;
        sprite_Stay_Right = sprite_stay_Right_Big;
    }

    //---
    private void Damage()
    {
        if (Mario_State == Mario_States.Big) SetMarioBase();
        if (Mario_State == Mario_States.Flower)
        {
            //  CODE HERE...
        }
        if (Mario_State == Mario_States.Small)
        {
            // CODE HERE... (dead)
        }
    }

    public void PowerUp(PowerUpType powerUp)
    {

        if (powerUp == PowerUpType.Mushroom) SetMarioBig();

        if (powerUp == PowerUpType.Flower)
        {
            if (Mario_State == Mario_States.Flower) AddPoints(1000);
            else Mario_State = Mario_States.Flower;
            animator.SetInteger("Mario_State", 2);
        }
        
    }

    private void ResetKeys()
    {
        Key_Jump = false;
        Key_Move_Left = false;
        Key_Move_Right = false;
    }


    public bool Have_Star() { return have_star; }


    public enum PowerUpType
    {
        Mushroom = 0,
        Flower = 1,
        Star = 2
    }


    public enum Mario_States
    {
        Small = 0,
        Big = 1,
        Flower = 2,
    }

}
