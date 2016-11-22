using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIQuestionManager : MonoBehaviour
{
	[HideInInspector]public ToggleGroup group;
	public TimerManager timer;

	[SerializeField] private Text questionText;
	[SerializeField] private Transform answerParent;
	private Questions question;

	void Start ()
	{
		group = answerParent.GetComponent<ToggleGroup> ();
	}

	void OnEnable ()
	{
		SetQuestion ();
	}

	void SetQuestion ()
	{
		question = QuestionManager.Instance.GetQuestion ();

		if (question == null)
			Debug.LogError ("OutOfQUestions");
		else {
			List<string> answers = question.GetAnswers ();

			questionText.text = question.Question;
			for (int i = 0; i < answers.Count; i++) {
				GameObject temp = ObjectPool.instance.GetGameObjectOfType ("Toggle");
				temp.transform.SetParent (answerParent);

				temp.GetComponent<AnswerToggle> ().SetValue (answers [i]);
			} 
		}

		timer.StartTimer ();
	}

	public void NextQuestion ()
	{
		
		if (!group.AnyTogglesOn ())
			return;
		timer.StopTimer ();
		bool correct = false;

		for (int i = 0; i < answerParent.childCount; i++) {
			if (answerParent.GetChild (i).GetComponent<Toggle> ().isOn && i == question.correctAnswerIndex) {
				correct = true;
				break;
			}
		}

		if (correct)
			ScoreManager.Instance.CurrentScore += 1;
		else
			ScoreManager.Instance.Fails += 1;

		Reset ();
		SetQuestion ();
	}

	public void TimerOutOfTIme ()
	{
		Reset ();
		SetQuestion ();
	}

	void Reset ()
	{
		questionText.text = string.Empty;
		while (answerParent.childCount > 0)
			ObjectPool.instance.PoolGameObject (answerParent.GetChild (0).gameObject);	
	}

	void OnDisable ()
	{
		Reset ();
	}
}
