using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public bool IsBoos = false;
    GameObject scoreUITextGO;
    public GameObject Explosion;
    public GameObject Powerup;
    float speed;
    float armor = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        if (IsBoos)
        {
            armor = 17;
            speed = 3; 
        }

    }

    bool enemyGoingLeft = true; 
    void Update()
    {
        Vector2 position = transform.position;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (IsBoos)
        {
            if (enemyGoingLeft)
            {
                position = new Vector2(position.x - speed * Time.deltaTime, position.y);
                if (transform.position.x < min.x) enemyGoingLeft = false;
            }
            else
            {
                position = new Vector2(position.x + speed * Time.deltaTime, position.y);
                if (transform.position.x > max.x) enemyGoingLeft = true;
            }
          
            transform.position = position;


            return;
        }
        else
        {
            position = new Vector2(position.x, position.y - speed * Time.deltaTime);
            transform.position = position;

        }


       

        

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
            BossDead();
        }
        else if (col.CompareTag("PlayerBulletTag"))
        {
            if (col.name.StartsWith("SpecialBullet")) armor -= 7f;
            else armor--;

            if (armor <= 0f)
            {
                PlayExplosion();
                if(!IsBoos)
                scoreUITextGO.GetComponent<GameScore>().Score += 100;
                else
                    scoreUITextGO.GetComponent<GameScore>().Score += 500;


                Destroy(gameObject);
                BossDead();
                PowerupSet();
            }
        }
    }


    void PowerupSet()
    {
        if (!FindObjectOfType<PlayerControl>().IsPowerup)
        {
            if (Random.Range(1,6) == 3)
            {
                var e = GameObject.FindGameObjectsWithTag("Powerup");
                foreach (var item in e)
                    Destroy(item.gameObject);

                GameObject power = (GameObject)Instantiate(Powerup);
                power.transform.position = transform.position;

            }
        }
        
    }

    
    void BossDead()
    {
        if (IsBoos)
        {
            FindObjectOfType<EnemySpawner>().ScheduleEnemySpawner();
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(Explosion);

        explosion.transform.position = transform.position;
    }

    public void BossShoot()
    {
        transform.GetChild(0).GetComponent<EnemyBooletShoot>().Shoot();
    }
}