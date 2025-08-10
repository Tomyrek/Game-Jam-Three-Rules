using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public float waitToRespawn;
    public int gemsCollected;

    public string levelToLoad;

    public float timeInLevel;
    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeInLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());

    }

    private IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);
        AudioManager.instance.PlaySFX(8);

        yield return new WaitForSeconds(waitToRespawn - (1f / UIController.instance.fadeSpeed));
        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + .2f);

        if (RuleManager.instance != null) //novo, restart pravila kada se igrac respawnuje
        { 
            RuleManager.instance.ResetRules();
        }

        UIController.instance.FadeFromBlack();

        PlayerController.instance.gameObject.SetActive(true);

        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        UIController.instance.UpdateHealthDisplay();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }
    public IEnumerator EndLevelCo()
    {

        AudioManager.instance.PlayLevelVictory();

        PlayerController.instance.stopInput = true;

        CameraContoller.instance.stopFollow = true;

        UIController.instance.levelCompleteText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 3f);

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_gems"))
        {
            if (gemsCollected > PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_gems"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_time"))
        {
            if (timeInLevel < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "time"))
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
        }

        SceneManager.LoadScene(levelToLoad);
    }

}
