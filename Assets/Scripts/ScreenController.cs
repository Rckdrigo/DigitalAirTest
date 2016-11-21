using UnityEngine;
using System.Collections;

public class ScreenController : MonoBehaviour
{
	public static ScreenController Instance;
	private Animator anim;

	void Start ()
	{
		Instance = this;
		anim = GetComponent<Animator> ();
	}

	public void StartGame ()
	{
		anim.SetTrigger ("Start");
	}

	public void WinGame ()
	{
		anim.SetTrigger ("End");
		anim.SetBool ("Win", true);
	}

	public void LoseGame ()
	{
		anim.SetTrigger ("End");
		anim.SetBool ("Win", false);
	}
}
