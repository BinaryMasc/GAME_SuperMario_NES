using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksScript : MonoBehaviour
{

    private Vector2 initial_position;
    private float time_transition = 0;

    //  Flags
    private bool CollisionPlayer_Order;
    private bool destroyObject;

    // Start is called before the first frame update
    void Start()
    {
        initial_position = transform.position;
        CollisionPlayer_Order = false;
        destroyObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CollisionPlayer_Order) TransitionMoveOrDestroy(destroyObject); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);

        if(side == EntityFunctions.Side_Collision.DOWN && collision.gameObject.GetComponent<PlayerController>() != null)
        {
            CollisionPlayer_Order = true;
            if (collision.gameObject.GetComponent<PlayerController>().Mario_State == PlayerController.Mario_States.Small)
                destroyObject = false;

            else destroyObject = true;

        }
    }


    private void TransitionMoveOrDestroy(bool destroy)
    {
        float d_Moving = 0.9f; //  Distance move animation
        float t_transition_1 = 0.15f; //    Time transition animation 1
        float t_transition_2 = 0.3f; //    Time transition animation 2

        time_transition += Time.deltaTime;

        if (time_transition < t_transition_1)
            if (destroy) Destroy(gameObject); 
            else gameObject.transform.position += new Vector3(0, d_Moving * Time.deltaTime);


        else if (time_transition >= t_transition_1 && time_transition <= t_transition_2)
            gameObject.transform.position += new Vector3(0, -d_Moving * Time.deltaTime);


        else
        {
            transform.position = initial_position;
            time_transition = 0;
            CollisionPlayer_Order = false;
        }
    }
}
