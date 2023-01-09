using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsPanel;
    [SerializeField]
    private Animator blackOutTitleAnimator;
    private void Awake() {
        int currentStage = SceneManager.GetActiveScene().buildIndex;
        // if this is the first time playing, then set the current stage to 1
        if (currentStage == 0) {
            bool doneTutorial = PlayerPrefs.GetInt("Tutorial", 0) == 1;
            if (!doneTutorial) {
                SceneManager.LoadScene(1);
            }
        }
    }
    //retry
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //menu (index 0)
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void startGame() {
        StartCoroutine(startGameTransition());
    }

    public void quitApp() {
        Application.Quit();
    }

    public void toggleCredits() {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }

    private IEnumerator startGameTransition() {
        blackOutTitleAnimator.Play("BlackOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
        StopAllCoroutines();
    }

    public void goToTutorial() {
        SceneManager.LoadScene(1);
    }
}
