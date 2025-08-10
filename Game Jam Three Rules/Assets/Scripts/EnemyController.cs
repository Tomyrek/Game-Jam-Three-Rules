using System;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public float moveSpeed;

    public Transform leftPoint, rightPoint;

    private bool movingRight;
    private Rigidbody2D theRB;
    public SpriteRenderer theSR;
    private Animator anim;

    public float moveTime, waitTime;
    private float moveCount, waitCount;

    public GameObject deathEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;

        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCount > 0)
        {
            moveCount -= Time.deltaTime;

            if (movingRight)
            {
                theRB.linearVelocity = new Vector2(moveSpeed, theRB.linearVelocity.y);

                theSR.flipX = true;

                if (transform.position.x > rightPoint.position.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                theRB.linearVelocity = new Vector2(-moveSpeed, theRB.linearVelocity.y);

                theSR.flipX = false;

                if (transform.position.x < leftPoint.position.x)
                {
                    movingRight = true;
                }
            }
            if (moveCount <= 0)
            {
                waitCount = UnityEngine.Random.Range(waitTime * .75f, waitTime * 1.25f);
            }

            anim.SetBool("isMoving", true);
        }
        else if (waitCount > 0)
        {
            waitCount -= Time.deltaTime;
            theRB.linearVelocity = new Vector2(0f, theRB.linearVelocity.y);

            if (waitCount <= 0)
            {
                moveCount = UnityEngine.Random.Range(moveTime * .75f, waitTime * 1.25f);
            }
            anim.SetBool("isMoving", false);
        }

    }


/////Nova fuknckija koja se poziva nakon skoka na neprijatelja
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Proveri da li je igrač skočio odozgo (da li pada prema dolje)
            Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB != null && playerRB.linearVelocity.y < 0)
            {
                // Obavesti RuleManager da je neprijatelj ubijen
                if (RuleManager.instance != null)
                {
                    RuleManager.instance.OnEnemyKilled();
                }

                // Ovo je tvoj postojeći kod za bounce effect
                PlayerController.instance.Bounce();

                // Kreiraj death effect ako postoji, ali ograniči broj instanci
                if (deathEffect != null && !gameObject.name.Contains("Dying"))
                {
                    GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
                    // Uništi effect nakon 2 sekunde da ne zatrpava hierarchy
                    Destroy(effect, 2f);
                }

                // Označi neprijatelja kao umirući da sprečiš multiple triggerovanje
                gameObject.name += "_Dying";

                // Uništi neprijatelja
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
