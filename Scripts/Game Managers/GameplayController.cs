using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameplayController : MonoBehaviour {
    private Text scoreText, HealthText, levelText;
    private float score, health, level;

    private GameObject pausePanel;
    public static GameplayController instance;
    [SerializeField]
    private AudioSource audioSource;

    [HideInInspector]
    public bool canCountScore;

    private BGScroller bgScroller;
	// Use this for initialization
	void Awake () {
        MakeInstance();
        scoreText = GameObject.Find(Tags.SCORE_TEXT_OBJ).GetComponent<Text>();
        HealthText = GameObject.Find(Tags.HEALTH_TEXT_OBJ).GetComponent<Text>();
        levelText = GameObject.Find(Tags.LEVEL_TEXT_OBJ).GetComponent<Text>();
        bgScroller = GameObject.Find(Tags.BACKGROUND_GAME_OBJ).GetComponent<BGScroller>();
        pausePanel = GameObject.Find(Tags.PAUSE_PANEL_OBJ);
        pausePanel.SetActive(false);

    }
    void Start()
    {
        if (GameManager.instance.canPlayMusic)
            audioSource.Play();
    }

    void Update()
    {
        IncrementScore(1);

    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
        instance = null;
    }

    //delegate function
    void OnSceneWasLoaded(Scene scene , LoadSceneMode mode)
    {
        if (scene.name == Tags.GAMEPLAY_SCENE)
        {
            if (GameManager.instance.gameStartedFromMainMenu)
            {
                GameManager.instance.gameStartedFromMainMenu = false;
                score = 0f;
                health = 3f;
                level = 0f;

            }else if(GameManager.instance.gameRestartedPlayerDied)
            {

                //Restart Game when die
                GameManager.instance.gameRestartedPlayerDied = false;
                score = GameManager.instance.score;
                health = GameManager.instance.health;
                level = GameManager.instance.level;
               
            }
            scoreText.text = score.ToString();
            HealthText.text = health.ToString();
            levelText.text = level.ToString();
        }
    }//scene

    public void IncrementHealth()
    {
        health++;
        HealthText.text = health.ToString();
    }

    public void TakeDamage()
    {
        health--;
        if (health >= 0)
        {
            //restart
            StartCoroutine(PlayerDied(Tags.GAMEPLAY_SCENE));

            HealthText.text = health.ToString();
        }else
        {
            StartCoroutine(PlayerDied(Tags.MAIN_MENU_SCENE));
        }
        
    }

    public void IncrementScore(float scoreValue)
    {
        if (canCountScore)
            score += scoreValue;
        scoreText.text = score.ToString();
    }
	
    IEnumerator PlayerDied(string sceneName)
    {
        canCountScore = false;
        //stop BG scroll
        bgScroller.canScroll = false;
        GameManager.instance.gameRestartedPlayerDied = true;
        GameManager.instance.score=score;
        GameManager.instance.health = health;
        GameManager.instance.level = level;
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        canCountScore = false;
        Time.timeScale = 0f;
        


    }

    public void IncrementLevel()
    {
        level++;
        levelText.text = level.ToString();
    }
    public void ResumeGame()
    {
        canCountScore = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

    }

    public void HomeBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tags.MAIN_MENU_SCENE);
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Tags.GAMEPLAY_SCENE);
    }


}//class








