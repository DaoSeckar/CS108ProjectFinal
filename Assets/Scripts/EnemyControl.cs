using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    GameObject scoreUITextGO;
    public GameObject Explosion;
    float speed;
    float armor = 3;


    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        transform.position = position;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Detect collision with player ship and player bullet
        if (col.CompareTag("PlayerTag"))
        {
            Destroy(gameObject);
        }
        else if (col.CompareTag("PlayerBulletTag"))
        {
            armor--;
            if (armor == 0f)
            {
                PlayExplosion();
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
                Destroy(gameObject);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(Explosion);

        explosion.transform.position = transform.position;
    }
}