using UnityEngine;
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
	private string questionsYaml;
	private WWW www;

	//Test
	public TextAsset yamlTxt;

	public static QuestionManager Instance;

	private Quiz quiz;
	private List<Questions> usedQuestions;


	// Use this for initialization
	//	IEnumerator Start()
	//	{
	//		WWW www = new WWW("https://digitalairtest.000webhostapp.com/Questions.yaml");
	//		yield return www;
	//
	//		questionsYaml = www.text;
	//		Debug.Log(questionsYaml);
	//		ParseYamltoQuestions();
	//	}

	void Start ()
	{
		usedQuestions = new  List<Questions> ();
		ParseYamltoQuestions ();
		Instance = this;
	}

	void ParseYamltoQuestions ()
	{
		var input = new StringReader (yamlTxt.text);
            
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
}