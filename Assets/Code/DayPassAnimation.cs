using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayPassAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private TextMeshProUGUI energyCountText;
    [SerializeField]
    private TextMeshProUGUI energyNeededText;



    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void nextDay(int day) {
        dayText.text = "Day " + day;
        if (animator.enabled) {
            animator.Play("Start");
        }  else {
            animator.enabled = true;
        }
    }

    public void blackOut() {
        animator.Play("BlackOut");
    }

    public void startCounting() {
        animator.Play("StartCalculation");
    }

    public void endCounting() {
        animator.Play("EndCalculation");
    }

    public void updateNumber(int energyCount, int energyNeeded) {
        energyCountText.text = energyCount.ToString();
        energyNeededText.text = energyNeeded.ToString();
    }

    public void endGame() {
        animator.Play("Retry");
    }

    public void victory() {
        animator.Play("Victory");
    }

    public void victoryEnd() {
        animator.Play("VictoryEnd");
    }

    public void updateTime() {

    }
}
