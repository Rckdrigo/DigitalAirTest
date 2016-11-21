using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerToggle : MonoBehaviour
{
	[SerializeField] private Text answerText;
	private Toggle answerToggle;

	void Awake ()
	{
		answerToggle = GetComponent<Toggle> ();
	}

	public void SetValue (string text)
	{
		answerToggle.group = transform.GetComponentInParent<ToggleGroup> ();
		answerText.text = text;
	}

	void OnDisable ()
	{
		answerToggle.isOn = false;
	}
}
