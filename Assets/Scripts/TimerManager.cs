using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerManager : MonoBehaviour
{

	public UIQuestionManager qm;
	public int maxWaitingTime = 5;


	private Text timerText;

	void Awake ()
	{
	}

	//	public void OnEnable ()
	//	{
	//		StartTimer ();
	//	}

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
			timerText.color = Color.Lerp (Color.red, Color.white, (float)i / maxWaitingTime);
			timerText.text = i + " s";
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
