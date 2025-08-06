using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    //pozivanje static u damage player skripti umesto FindObjectOfType<PlayerHealthController>().DealDamage();
    public static PlayerHealthController instance;

    public int currentHealth, maxHealth;

    public float invincibleLenght;
    private float invincibleCounter;

    private SpriteRenderer theSR;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            if (invincibleCounter < 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    public void DealDamage()
    {
        if (invincibleCounter <= 0)
        {
            
            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                //gameObject.SetActive(false);

                LevelManager.instance.RespawnPlayer();
            }
            else
            {
                invincibleCounter = invincibleLenght;
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .3f);

                PlayerController.instance.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay();  
         }
    }
}
