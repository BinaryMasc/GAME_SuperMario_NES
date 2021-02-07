using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{

    [SerializeField] private MushroomType type;

    [SerializeField] private float velocity = 1.21f; // Velocity in x

    private float time_live;

    public bool isMoving = false;

    GameObject levelController;


    // Start is called before the first frame update
    void Start()
    {
        levelController = GameObject.Find("LevelController");
        time_live = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) Move();
        if (time_live < 1) time_live += Time.deltaTime;
    }

    private void Move()
    {
        transform.position += new Vector3(velocity * Time.deltaTime, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EntityFunctions.Side_Collision side = EntityFunctions.DetectSideCollision(collision, gameObject);

        bool collision_withPlayer = collision.gameObject.GetComponent<PlayerController>() != null ? true : false;

        if (collision_withPlayer && time_live >= 1)
        {
            

            collision.gameObject.GetComponent<PlayerController>().PowerUp(PlayerController.PowerUpType.Mushroom);

            Destroy(gameObject);

            levelController.GetComponent<SceneController>().PauseTemp(1f);
        }

        else if ((side == EntityFunctions.Side_Collision.LEFT || side == EntityFunctions.Side_Collision.RIGHT) && !collision.gameObject.CompareTag("Floor") && !collision_withPlayer)
            velocity *= -1;
    }

    public enum MushroomType
    {
        grow = 0,
        oneUp = 1
    }
}
