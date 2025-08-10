using UnityEngine;

public class ForbiddenZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public bool isStayZone = false;  //da li zona reaguje na ulazak ili na ostanak

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Obavesti RuleManager da je igrac usao u zabranjenu zonu
            if (RuleManager.instance != null)
            {
                RuleManager.instance.OnForbiddenZoneEntered();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isStayZone)
        {
            //za zpme gde ke zabramkemp pstamal -alternativa
            if (RuleManager.instance != null)
            {
                RuleManager.instance.OnForbiddenZoneEntered();
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
