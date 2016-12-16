using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerManager : MonoBehaviour
{

	public UIQuestionManager qm;
	public int maxWaitingTime = 5;


	private Text timerText;

	public void StartTimer ()
	{
		timerText = GetComponent<Text> ();
		StartCoroutine (RunTimer ());
	}

	public void StopTimer ()
	{
		StopAllCoroutines ();
	}

	IEnumerator RunTimer ()
	{
		for (int i = maxWaitingTime; i >= 0; i--) {
			timerText.color = Color.Lerp (Color.red, Color.black, (float)i / maxWaitingTime);
			timerText.text = i.ToString ();
			yield return new WaitForSecondsRealtime (1f);
		}
		if (qm.group.AnyTogglesOn ())
			qm.NextQuestion ();
		else {
			ScoreManager.Instance.Fails += 1;
			qm.TimerOutOfTIme ();
		}
	}
}
