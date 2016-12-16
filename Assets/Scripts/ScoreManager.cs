using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	[SerializeField] private int minScoreToWin = 3;
	[SerializeField] private int minFailsToLose = 2;
	[SerializeField] private AudioClip correctSFX, wrongSFX;

	[SerializeField] private Image button;
	[SerializeField] private Text winText, loseText;

	private new AudioSource audio;

	private int currentScore = 0;
	private int currentHighscore = 0;

	public int CurrentScore { 
		get { return currentScore; } 
		set { 
			currentScore = value; 
			audio.PlayOneShot (correctSFX);
			StopAllCoroutines ();
			StartCoroutine (ChangeColor (true));
			if (currentScore == minScoreToWin) {
				winText.text = "Highscore : " + currentHighscore + "\nScore : " + currentScore;
				StartCoroutine (ScorePost ());
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
			StopAllCoroutines ();
			StartCoroutine (ChangeColor (false));
			if (fails == minFailsToLose) {
				loseText.text = "Highscore : " + currentHighscore + "\nScore : " + currentScore;
				StartCoroutine (ScorePost ());
				currentScore = fails = 0;
				QuestionManager.Instance.CleanUsedQuestions ();
				ScreenController.Instance.LoseGame ();
			}
		} 
	}

	IEnumerator ChangeColor (bool correct)
	{
		Color c = correct ? Color.green : Color.red;

		while (button.color != c) {
			button.color = Color.Lerp (c, button.color, Time.deltaTime);
			yield return null;
		}

		c = !correct ? Color.green : Color.red;
		while (button.color != c) {
			button.color = Color.Lerp (Color.white, button.color, Time.deltaTime);
			yield return null;
		}
	}

	void Start ()
	{
		Instance = this;
		audio = GetComponent<AudioSource> ();

		StartCoroutine (ScoreRead ());
	}

	IEnumerator ScorePost ()
	{
		if (currentScore > currentHighscore) {
			currentHighscore = currentScore;
			print ("posting score");
			WWWForm form = new WWWForm ();
			form.AddField ("highscore", currentScore);

			WWW www = new WWW ("https://digitalairtest.000webhostapp.com/postHighScore.php", form);
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				print (www.error);
			} else {
				print ("Finished Uploading High Score");
			}
		} else
			yield break;
	}

	IEnumerator ScoreRead ()
	{

		WWW www = new WWW ("https://digitalairtest.000webhostapp.com/highScores.txt");
		yield return www;
		if (!string.IsNullOrEmpty (www.error))
			print (www.error);
		else
			print ("Finished Reading High Score");

		print ("www " + www.text);
		currentHighscore = int.Parse (www.text);

	}
}