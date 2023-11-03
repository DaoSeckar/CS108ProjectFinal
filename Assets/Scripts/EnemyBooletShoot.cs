using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBooletShoot : MonoBehaviour
{
    public GameObject EnemyBullet;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("FireEnemyBullet", 1f);

    }

    // Update is called once per frame
    void Update()
    {

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
