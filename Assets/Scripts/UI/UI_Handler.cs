using UnityEngine;
using UnityEngine.UI;
public class UI_Handler : MonoBehaviour
{
    [Header("Health UI")]
    public Health health;
    public Slider healthBar;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    bool isPaused = false;
    public Button resumeButton;
    public Button quitButton;
    

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health.healthpercentage();
        Pause();
        ResumeGame();
        QuitGame();
    }
    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
    void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePauseMenu();
    }
    void ResumeGame()
    {
        if(resumeButton != null)
            resumeButton.onClick.AddListener(TogglePauseMenu);
        
    }
    void QuitGame()
    {
        if(quitButton != null)
            quitButton.onClick.AddListener(QuitApplication);
    }
    void QuitApplication()
    {
        Application.Quit();
    }

}
