using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBooletShoot : MonoBehaviour
{
    public GameObject EnemyBullet;
    public bool IsEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        if (IsEnemy)
        {
            //InvokeRepeating("FireEnemyBullet", 1f,1f);
        }
        else
        Invoke("FireEnemyBullet", 1f);

    }

    public void Shoot()
    {
        FireEnemyBullet();
    }

    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find("carlosalbert_0");

        if (playerShip != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBullet);

            bullet.transform.position = transform.position;

            Vector2 direction = playerShip.transform.position - bullet.transform.position;

            bullet.GetComponent<EnemyBoolet>().SetDirection(direction);
        }
    }
}
