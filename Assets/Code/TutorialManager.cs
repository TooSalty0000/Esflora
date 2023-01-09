using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private MenuManager menuManager;
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    [SerializeField]
    private DayPassAnimation dayPassAnimation;
    [TextArea(3, 10)]
    public List<string> tutorialTexts;

    public int currentTextIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for every click, show the next text
        if (Input.GetMouseButtonDown(0)) {
            // if first character is a @, then ignore it
            if (tutorialTexts[currentTextIndex][0] != '@') {
                if (tutorialTexts[currentTextIndex] == "END") {
                    StartCoroutine(endTutorial());
                    return;
                }
                tutorialText.text = tutorialTexts[currentTextIndex];
                currentTextIndex++;
            }
        }
    }

    public void getAction(string actionName) {
        if (actionName == tutorialTexts[currentTextIndex]) {
            currentTextIndex++;
            tutorialText.text = tutorialTexts[currentTextIndex];
            currentTextIndex++;
        }

    }

    public IEnumerator endTutorial() {
        dayPassAnimation.blackOut();
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("Tutorial", 1);
        menuManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MenuManager>();
        menuManager.Menu();
        StopAllCoroutines();
    }
}
