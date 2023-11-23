using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerControl : MonoBehaviour
{
	public GameManager GameManagerGO;

	public GameObject PlayerBoolets;
	public GameObject BulletPlayerPos;
	public GameObject Explosion;
    public GameObject poweruoBar;

    public TextMeshProUGUI LivesUIText;
    public TextMeshProUGUI SpecialUIText;

    const int MaxLives = 3;
    const int MaxSpecial = 3;
    int lives;
	int specials;

	public float speed;
	public bool IsPowerup = false;
	float CurrentFireRate = 0f;
	public void Init()
    {
        specials = MaxSpecial;
        lives = MaxLives;

		LivesUIText.text = lives.ToString();
		SetSpecial();


        transform.position = new Vector2(0, -4);
        IsPowerup = false;
        CurrentFireRate = 0f;
        gameObject.SetActive(true);
    }

	void SetSpecial()
	{
		SpecialUIText.text = "x"+ specials.ToString();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown("space") && !IsPowerup)
        {
			Fire();
        }
        else if (Input.GetKeyDown(KeyCode.V) && !IsPowerup)
        {
            Fire(true);
        }
        else if (IsPowerup && CurrentFireRate >= .05f)
		{
			CurrentFireRate = 0;
            Fire();
		}
		else
		{
			CurrentFireRate += Time.deltaTime;
		}

		float x = Input.GetAxisRaw("Horizontal");//the value will be -1, 0 or 1 (for left, no input, and right)
		float y = Input.GetAxisRaw("Vertical");//the value will be -1, 0 or 1 (for down, no input, and up)

		//now based on the input we compute a direction vector, and we normalize it to get a unit vector
		Vector2 direction = new Vector2(x, y).normalized;

		//noe we call the function that computes and sets the player's position
		Move(direction);
	}

	void Fire(bool isSpecial=false)
	{
		if (isSpecial)
		{
			if (specials <= 0) return;
			specials--;
			SetSpecial();
			Invoke("FillSpecial", 3);
		}

        GetComponent<AudioSource>().Play();
        GameObject bullet01 = (GameObject)Instantiate(PlayerBoolets);
        bullet01.transform.position = BulletPlayerPos.transform.position;
		
		if (isSpecial)
		{
            bullet01.transform.localScale *= 3;
            bullet01.name = "SpecialBullet";
        }
		
    }

	void FillSpecial()
	{
        if (FindObjectOfType<GameManager>().GMState == GameManager.GameManagerState.GameOver) return;

		if (specials < 3)
		{
			specials++;
			SetSpecial();
		}


    }

    void Move(Vector2 direction)
	{
		//find the screen limits to the player's movement (left, right, top and bottom edges of the screen)
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); //this is the bottom-left point (corner) of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); //this is the top-right point (corner) of the screen

		max.x = max.x - 0.225f; //subtract the player sprite half width
		min.x = min.x + 0.225f; //add the player sprite half width

		max.y = max.y - 0.285f; //subtract the player sprite half height
		min.y = min.y + 0.285f; //add the player sprite half height

		//Get the player's current position
		Vector2 pos = transform.position;

		//Calculate the new position
		pos += direction * speed * Time.deltaTime;

		//Make sure the new position is outside the screen
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		//Update the player's position
		transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("EnemyTag") || col.CompareTag("EnemyBulletTag"))
		{
			PlayExplosion();

			lives--;
			LivesUIText.text = lives.ToString();

			if (lives == 0)
			{
                poweruoBar.SetActive(false);
                GameManagerGO.SetGameManagerState(GameManager.GameManagerState.GameOver);
				gameObject.SetActive(false);
			}
		}


		if (col.CompareTag("Powerup"))
		{
			PowerUp();
			Destroy(col.gameObject);
		}
	}


	void PowerUp()
	{
		IsPowerup = true;
		poweruoBar.SetActive(true);
		Invoke("PowerUpOff", 7);
	}

	void PowerUpOff()
	{
        poweruoBar.SetActive(false);
        IsPowerup = false;
	}

	void PlayExplosion()
    {
		GameObject explosion = (GameObject)Instantiate(Explosion);

		explosion.transform.position = transform.position;
    }
}
