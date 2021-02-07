using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxScript : MonoBehaviour
{

    public Contains contain = Contains.Coin;
    public Sprite emptyBox;
    public GameObject content_Prototype;
    public GameObject content_Prototype_2;
    public GameObject content;

#pragma warning disable
    private Animator animation;
    private SpriteRenderer render;
#pragma warning enable


    private bool is_used;
    private bool open_order;
    private float time_transition; //internal value from TransitionOpen()
    private Vector3 initial_position;


    // Start is called before the first frame update
    void Start()
    {

        animation = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        initial_position = transform.position;
        is_used = false;
        open_order = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (open_order && !is_used) TransitionOpen();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);

        if(!is_used && side == EntityFunctions.Side_Collision.DOWN && collision.gameObject.GetComponent<PlayerController>() != null && !open_order)
        {
            open_order = true;

            if (contain == Contains.Mushroom_or_Flower)
            {
                if (collision.gameObject.GetComponent<PlayerController>().Mario_State == PlayerController.Mario_States.Small)
                {
                    content = Instantiate(content_Prototype, gameObject.transform.position, new Quaternion());
                    contain = Contains.Mushroom;
                    return;
                }

                else
                {
                    content = Instantiate(content_Prototype_2, gameObject.transform.position, new Quaternion());
                    contain = Contains.Flower;
                    return;
                }

            }


            content = Instantiate(content_Prototype, gameObject.transform.position, new Quaternion());
            
        }
    }

    private void TransitionOpen()
    {
        float d_Moving = 0.9f; //  Distance move animation
        float t_transition_1 = 0.15f; //    Time transition animation 1
        float t_transition_2 = 0.3f; //    Time transition animation 2

        if(contain != Contains.Coin)
        {
            d_Moving /= 2;
            //t_transition_1 *= 2;
            //t_transition_2 *= 2;
        }

        time_transition += Time.deltaTime;

        if (time_transition < t_transition_1)
        {
            //if (contain == Contains.Coin) 
                content.transform.position += new Vector3(0, d_Moving * Time.deltaTime * 14);


            gameObject.transform.position += new Vector3(0, d_Moving * Time.deltaTime);
        }

        else if(time_transition >= t_transition_1 && time_transition <= t_transition_2)
            gameObject.transform.position += new Vector3(0, -d_Moving * Time.deltaTime);

        else
        {
            transform.position = initial_position;

            animation.enabled = false;
            render.sprite = emptyBox;

            if (contain == Contains.Coin) Destroy(content);

            else if (contain == Contains.Mushroom) content.GetComponent<MushroomScript>().isMoving = true;

            is_used = true;
        }

    }


    public enum Contains
    {
        Coin = 0,
        Mushroom = 1,
        Mushroom_or_Flower = 2,
        Flower = 3,
        Star = 4
    }

    

}
