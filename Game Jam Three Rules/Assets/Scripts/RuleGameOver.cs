using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RuleGameOver : MonoBehaviour
{
    public static RuleGameOver instance;
    
    [Header("Game Over Settings")]
    public string levelSelectSceneName = "LevelSelect";
    public bool returnToLevelSelect = false;
    
    private void Awake()
    {
        instance = this;
    }
    
    public void TriggerRuleGameOver()
    {
        StartCoroutine(GameOverSequence());
    }
    
    private IEnumerator GameOverSequence()
    {
        // Zaustavi sve ODMAH
        if (PlayerController.instance != null)
        {
            PlayerController.instance.stopInput = true;
            PlayerController.instance.theRB.linearVelocity = Vector2.zero;
        }
        
        // INSTANT RESTART 
        if (returnToLevelSelect)
        {
            SceneManager.LoadScene(levelSelectSceneName);
        }
        else
        {
            // Restartuj trenutni level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        yield return null; // Samo da zadovolji IEnumerator
    }
}