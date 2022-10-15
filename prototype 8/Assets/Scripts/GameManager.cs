using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private const int Coin_Score_Amount = 5;
    public static GameManager Instance { set; get; }


    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;


    private static AudioSource AudioContoller;
    public AudioClip Jump;
    public AudioClip Click;
    public AudioClip CoinCollect;
    public AudioClip Dead;
    public AudioClip Slide;
    public AudioClip BgAudio;


    //user Interface

    public Animator GameCanvas, DefMenu, DefMenu1;
    public Text scoreText, coinText, modifierText, HighScore, coinUpdate;
    private float score, coinScore, modifierScore;
    public GameObject DefaultScreen;
    public GameObject GamePanel;
    public GameObject MGamePanel;
    public GameObject DeathPanel;
    public GameObject PausePanel;

    private int lastScore;

    //Death Menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadCoinText;


    private void Awake()
    {
        
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");
        HighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        coinUpdate.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    private void Start()
    {
        
        DefaultScreen.SetActive(true);
        DeathPanel.SetActive(false);
        GamePanel.SetActive(false);
        AudioContoller = GetComponent<AudioSource>();
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        AudioContoller.Play();
    }

    private void Update()
    {
        if (MoblieInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            DefaultScreen.SetActive(false);
            DeathPanel.SetActive(false);
            GamePanel.SetActive(true);
            GameCanvas.SetTrigger("Show");
            DefMenu.SetTrigger("Hide");
            DefMenu1.SetTrigger("Hide");
            
        }
        if (isGameStarted && !IsDead)
        {
            //Increase Score
            score += (Time.deltaTime * modifierScore);

            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void GetCoin()
    {
        AudioContoller.PlayOneShot(CoinCollect, 1.0f);
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += Coin_Score_Amount;
        scoreText.text = score.ToString("0");
    }
    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;

        modifierText.text = "x" + modifierScore.ToString("0.0");
    }
    public void PauseMeenu()
    {
        Debug.Log("Clicked");
        AudioContoller.PlayOneShot(Click, 10.0f);
        Time.timeScale = 0;
        MGamePanel.SetActive(false);
        PausePanel.SetActive(true);
    }
    public void ResumeButton()
    {
        AudioContoller.PlayOneShot(Click, 10.0f);
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        MGamePanel.SetActive(true);
    }

    public void OnPlayButton()
    {
        AudioContoller.PlayOneShot(Click, 10.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        DeathPanel.SetActive(true);
        deathMenuAnim.SetTrigger("Dead");
        GameCanvas.SetTrigger("Hide");
        DefaultScreen.SetActive(false);
        DeathPanel.SetActive(true);
        GamePanel.SetActive(false);

        //Check for new HghScore
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("HighScore", (int)score);
        }

        if (PlayerPrefs.HasKey("Coin"))
        {
            int oldValue = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", oldValue + (int)coinScore);
        }
        else
        {
            PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", (int)coinScore);
        }

    }
 
}
