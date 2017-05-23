using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkVNRunner : MonoBehaviour {

	[Header("Required")]
	public Animator talkPanelAnimator;
	public Animator continueMarkerAnimator;
	public Text textArea;
	public TextAsset inkJSONAsset;

	[Header("Optional")]
	public AudioSource audioSource;

	[Header("Settings")]
	public string autoStartAtKnot;

	[Header("Tag Defines")]
	public InkVNAnimator[] namedAnimators;
	public InkVNAudioClip[] namedBgms;
	public InkVNAudioClip[] namedSfxs;
	public InkVNCustomTag[] customTags;

	private Story story;

	void Awake()
	{
		if(autoStartAtKnot != "")
		{
			StartVN(autoStartAtKnot);
		}
	}

    //Must call this manually, or use the auto start string.
	public virtual void StartVN(string atKnot)
	{
		story = new Story(inkJSONAsset.text);
		story.ChoosePathString(atKnot);
		StartCoroutine(StoryRoutine());
	}

	private IEnumerator StoryRoutine()
	{
		while (story.canContinue) {
			string text = story.Continue().Trim();
			Debug.Log(text);
		}
		yield return null;
	}

    //This is the default behaviour. You can override it.
	//Only if you didn't override that you have to connect the audio source
	protected virtual void PlayBGM(string name)
	{

	}

    //This one uses PlayOneShot, so you don't need to connect audio source
	protected virtual void PlaySFX(string name)
	{

	}

}
