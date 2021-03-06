﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using YamlDotNet.RepresentationModel.Serialization;
using YamlDotNet.RepresentationModel.Serialization.NamingConventions;
using System;

#region Contrainer classes
[Serializable]
public class Quiz
{
	public List<Questions> questions { get; set; }
}

[Serializable]
public class Questions
{
	[YamlAlias ("question")]
	public string Question { get; set; }

	public string Answer { get; set; }

	public string Incorrect1 { get; set; }

	public string Incorrect2 { get; set; }

	public string Incorrect3 { get; set; }

	public string Incorrect4 { get; set; }

	public string Incorrect5 { get; set; }

	public string Incorrect6 { get; set; }

	public int correctAnswerIndex;

	public List<string> GetAnswers ()
	{
		List<string> answers = new List<string> ();
		answers.Add (Answer);
		answers.Add (Incorrect1);
		if (Incorrect2 != null)
			answers.Add (Incorrect2);
		if (Incorrect3 != null)
			answers.Add (Incorrect3);
		if (Incorrect4 != null)
			answers.Add (Incorrect4);
		if (Incorrect5 != null)
			answers.Add (Incorrect5);
		if (Incorrect6 != null)
			answers.Add (Incorrect6);


		List<string> randomizedList = new List<string> ();
		System.Random rnd = new System.Random ();

		while (answers.Count != 0) {
			int index = rnd.Next (0, answers.Count);
			
			randomizedList.Add (answers [index]);
			answers.RemoveAt (index);
		}

		correctAnswerIndex = randomizedList.IndexOf (Answer);
//		Debug.Log ("Correct " + correctAnswerIndex);

		return randomizedList;
	}



}

#endregion

public class QuestionManager : MonoBehaviour
{
	
	//	public TextAsset yamlTxt;

	public static QuestionManager Instance;
	public bool doneLoading;

	[SerializeField] private GameObject noConnection;

	private string questionsYaml;
	private WWW www;

	//Test
	private Quiz quiz;
	private List<Questions> usedQuestions;

	private float loadTimeLimit = 2f;
	private bool failLoading = true;



	// Use this for initialization
	IEnumerator Start ()
	{
		Instance = this;
		usedQuestions = new  List<Questions> ();

		WWW www = new WWW ("https://digitalairtest.000webhostapp.com/Questions.yaml");

		while (loadTimeLimit > 0) {
			yield return new WaitForSecondsRealtime (0.25f);
			loadTimeLimit -= 0.25f;
			if (www.isDone) {
				failLoading = false;
				break;
			}
		}
	
		if (!string.IsNullOrEmpty (www.error) || failLoading) {
			noConnection.SetActive (true);

			yield break;
		} else
			questionsYaml = www.text;

		doneLoading = true;

		Debug.Log ("questionsYaml " + failLoading);
		ParseYamltoQuestions ();
	}

	void ParseYamltoQuestions ()
	{
		var input = new StringReader (questionsYaml);
            
		var deserializer = new Deserializer (namingConvention: new CamelCaseNamingConvention ());
            
		quiz = deserializer.Deserialize<Quiz> (input);
           
//		foreach (var item in quiz.questions) {
//			print ("Question: " + item.Question + "\t" + item.Answer + "\t" + item.Incorrect1 + "\t" + item.Incorrect2 + "\t" + item.Incorrect3);
//		}
	}

	public Questions GetQuestion ()
	{
		if (usedQuestions.Count < quiz.questions.Count) {
			Questions nextQuestion = new Questions ();
			do {
				int index = UnityEngine.Random.Range (0, quiz.questions.Count);
				nextQuestion = quiz.questions [index];
			} while(usedQuestions.Contains (nextQuestion));

			usedQuestions.Add (nextQuestion);
			return nextQuestion;
		}
		return null;
	}

	public void CleanUsedQuestions ()
	{
		usedQuestions.Clear ();
	}

	public void ReloadGame ()
	{
		SceneManager.LoadScene (0);
	}
}