using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{

    [SerializeField] private float velocity = -1f; // Velocity in x
    [SerializeField] private Sprite sprite_dead;
    

#pragma warning disable
    private bool deadOrder;
    private float time_DeadOrder;

    private Animator move;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody;

#pragma warning enable

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        deadOrder = false;

        time_DeadOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (deadOrder) Dead();

        else Move();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool collision_withPlayer = collision.gameObject.GetComponent<PlayerController>() != null ? true : false;

        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);
        

        if(side == EntityFunctions.Side_Collision.UP && collision_withPlayer || 
            (collision_withPlayer && collision.gameObject.GetComponent<PlayerController>().Have_Star()))
        {
            //Dead

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f));

            deadOrder = true;
        }

        else if ((side == EntityFunctions.Side_Collision.LEFT || side == EntityFunctions.Side_Collision.RIGHT) && !collision.gameObject.CompareTag("Floor") && !collision_withPlayer)
            velocity *= -1;


    }

    private void Dead()
    {
        if(time_DeadOrder == 0)
        {
            move.enabled = false;
            velocity = 0;
            spriteRenderer.sprite = sprite_dead;
            boxCollider2D.enabled = false;
            rigidbody.gravityScale = 0;
            gameObject.transform.position += new Vector3(0, -0.25f);
        }

        time_DeadOrder += Time.deltaTime;

        if(time_DeadOrder >= 1) Destroy(gameObject);
    }

    private void Move()
    {
        transform.position += new Vector3(velocity * Time.deltaTime, 0f);
    }

}
