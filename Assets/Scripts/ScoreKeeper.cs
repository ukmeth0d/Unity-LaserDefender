using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

    public static int currentPoints = 0;
    private Text myText;

    void Start() {
        myText = GetComponent<Text>();
        Reset();
        myText.text = "Score: " + currentPoints.ToString();
    }

	public void Score(int points) {
        currentPoints += points;
        myText.text = "Score: " + currentPoints.ToString();
    }

    public static void Reset() {
        currentPoints = 0;
    }
}
