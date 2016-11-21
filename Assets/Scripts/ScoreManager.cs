using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	[SerializeField] private int minScoreToWin = 3;
	[SerializeField] private int minFailsToLose = 2;

	private int currentScore = 0;

	public static ScoreManager Instance;

	public int CurrentScore { 
		get { return currentScore; } 
		set { 
			currentScore = value; 
			if (currentScore == minScoreToWin) {
				currentScore = fails = 0;
				QuestionManager.Instance.CleanUsedQuestions ();
				ScreenController.Instance.WinGame ();
			}
		} 
	}

	private int fails = 0;

	public int Fails { 
		get { return fails; } 
		set { 
			fails = value; 
			if (fails == minFailsToLose) {
				currentScore = fails = 0;
				QuestionManager.Instance.CleanUsedQuestions ();
				ScreenController.Instance.LoseGame ();
			}
		} 
	}

	void Start ()
	{
		Instance = this;
	}

}
