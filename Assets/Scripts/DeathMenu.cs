using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public float transitionTime = 2f;
    public Animator transition;
    // Start is called before the first frame update
    void Start()
    {
        PauseGame();
        transition = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; 
    }

    public void PauseGame()
    {
        Debug.Log("GAME PAUSED");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; 
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application Exited");
    }

    public void RestartScene()
    {
        ResumeGame();
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        transition.SetTrigger("restart");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        this.gameObject.SetActive(false);
    }
}
