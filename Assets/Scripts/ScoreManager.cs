using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	[SerializeField] private int minScoreToWin = 3;
	[SerializeField] private int minFailsToLose = 2;

	private AudioSource audio;
	public AudioClip correctSFX, wrongSFX;

	private int currentScore = 0;

	public int CurrentScore { 
		get { return currentScore; } 
		set { 
			currentScore = value; 
			audio.PlayOneShot (correctSFX);
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
			audio.PlayOneShot (wrongSFX);
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
		audio = GetComponent<AudioSource> ();
	}

}
