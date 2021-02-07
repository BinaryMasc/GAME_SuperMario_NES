using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityFunctions 
{

    public static Side_Collision DetectSideCollision(Collision2D other, GameObject thisGameObject)
    {

        Vector2 relativePosition = thisGameObject.transform.InverseTransformPoint(other.transform.position);

        float abs_distance = Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.y) ? 
            Mathf.Abs(relativePosition.x) : Mathf.Abs(relativePosition.y);

        if(Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.y))
        {

            if (relativePosition.x < 0) return Side_Collision.LEFT;
            else return Side_Collision.RIGHT;
        }
        else
        {
            if (relativePosition.y < 0) return Side_Collision.DOWN;
            else return Side_Collision.UP;
        }

    }




    public enum Side_Collision
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    };
}
