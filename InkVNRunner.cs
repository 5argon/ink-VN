using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using Ink.Runtime;

public class InkVNRunner : MonoBehaviour {

	[Header("Required")]
    
	//Triggers : Show Hide
	public Animator talkPanelAnimator;
    //Triggers : Show Hide Idle
	public Animator continueMarkerAnimator;
    //Triggers : Show Hide Change
	public Animator namePanelAnimator;

	public Text talkPanelText;
	public Text characterNameText;
	public TextAsset inkJSONAsset;

	[Header("Settings")]
	[Tooltip("When the game cannot find any matching character names this will be used.")]
	public Color defaultCharacterColor = Color.white;

	[Tooltip("Useful for debugging. Jumps to a knot and start ink-VN on awake.")]
	public string autoStartAtKnot;

	[Tooltip("A talk panel shows or hide animation plays when there is a pause in conversation. In this time, you cannot advance a story. Match it with your animation or a bit longer.")]
	public float talkPanelShowHideTime;

	[Tooltip("It can prevent players that repeatedly going forward too fast or by accident. This delay time is in effect even when the talk panel is not showing or hiding. Useful when the text is very short, a player might try to fast forward but results in an advance.")]
	public float advanceStoryDelayTime;

	[Tooltip("The speed which text reveals. The unit is character per second.")]
	public float textRevealSpeed;

    [Tooltip("If enabled, an advance while the text is not finished yet will still skip to the next line.")]
	public bool advanceThroughTextReveal;

	[Tooltip("Whether to trigger a \"Change\" trigger or not when the speaker changes.")]
	public bool characterNameChangeAnimation;

	[Header("Tag Defines")]
	public InkVNAnimator[] namedAnimators;
	public InkVNAltText[] namedAltTexts;
	public InkVNCustomTag[] customTags;

	[Header("Other Defines")]
	public InkVNCharacter[] characters;

	private Story story;
    private TextUnit previousTextUnit;
	private TextUnit currentTextUnit;
	private readonly Color transparent = new Color(0,0,0,0);
	private StringBuilder stringBuilder;

	void Awake()
	{
		stringBuilder = new StringBuilder();
		if(autoStartAtKnot != "")
		{
			StartVN(autoStartAtKnot);
		}
	}

    /// <summary>
    //You must call this manually once, or use the auto start string.
	//You can for example use knots to select which part of story to begin.
    /// </summary>
	public virtual void StartVN(string atKnot)
	{
		story = new Story(inkJSONAsset.text);
		story.ChoosePathString(atKnot);
		AdvanceStory();
	}

    /// <summary>
    //You will call this each time player click the screen so that it go to the next dialog.
	//If the text reveal is not finished yet, it will instead finish the reveal.
    /// </summary>
	public void AdvanceStory()
	{
        //You could not do anything while talk panel is in animation
		if(talkPanelDelayRoutine != null)
		{
			return;
		}

		//In the advance delay time, you can still fast forward.
		if(textRevealRoutine != null)
		{
			TextRevealFastForward(currentTextUnit.text);
			return;
		}

        //But you cannot advance.
		if(advanceStoryDelayRoutine != null)
		{
			return;
		}

        if(story.canContinue)
		{
			string text = story.Continue().Trim();
			currentTextUnit = ParseToTextUnit(text);
			//Debug.Log(currentTextUnit.characterName + " | " + currentTextUnit.text);
            talkPanelText.text = currentTextUnit.text;
			characterNameText.text = currentTextUnit.showingCharacterName;
			characterNameText.color = currentTextUnit.characterColor;

            if(textRevealRoutine != null)
			{
				StopCoroutine(textRevealRoutine);
			}
			textRevealRoutine = TextRevealRoutine(currentTextUnit.text);
			StartCoroutine(textRevealRoutine);

			if(advanceStoryDelayRoutine != null)
			{
				StopCoroutine(advanceStoryDelayRoutine);
			}
			advanceStoryDelayRoutine = AdvanceStoryDelayRoutine();
			StartCoroutine(advanceStoryDelayRoutine);
		}
	}

	private IEnumerator talkPanelDelayRoutine;
	IEnumerator TalkPanelDelayRoutine()
	{
		yield return new WaitForSeconds(talkPanelShowHideTime);
		talkPanelDelayRoutine = null;
	}

	private IEnumerator advanceStoryDelayRoutine;
	IEnumerator AdvanceStoryDelayRoutine()
	{
		yield return new WaitForSeconds(advanceStoryDelayTime);
		advanceStoryDelayRoutine = null;
	}

    private IEnumerator textRevealRoutine;

	IEnumerator TextRevealRoutine(string fullText)
	{
		//In revealing, there is a problem of line break if you just add characters to the text box and let it break.
		//The line break might change repeatedly at the end of each line.
		//The solution is to pre-break the line and then render each line separately.

		//We want the information how many lines it takes.
		//We have to wait so the text box renders the text. In that one frame, the color is transparent to hide them.
		talkPanelText.text = fullText;
		Color originalColor = talkPanelText.color;
		talkPanelText.color = transparent;
		yield return null;

		talkPanelText.color = originalColor;

        //TextGenerator is now available! We will inserts line breaks manually now
		TextGenerator textGenerator = talkPanelText.cachedTextGenerator;
		UILineInfo[] lineInfos = textGenerator.GetLinesArray();
        stringBuilder.Length = 0;
		stringBuilder.Append(fullText);
		for(int i = 1; i < lineInfos.Length; i++)
		{
			stringBuilder.Insert(lineInfos[i].startCharIdx,"\n");
		}
		fullText = stringBuilder.ToString();

		int charactersRevealed = 0;
		float revealTime = 0;
		while ( charactersRevealed < fullText.Length)
		{
			revealTime += Time.deltaTime;
			charactersRevealed = Mathf.Min( (int)(revealTime * textRevealSpeed), fullText.Length);
			talkPanelText.text = fullText.Substring(0, charactersRevealed);

			yield return null;
		}

		textRevealRoutine = null;
	}

	private void TextRevealFastForward(string fullText)
	{
		if(textRevealRoutine != null)
		{
			StopCoroutine(textRevealRoutine);
			textRevealRoutine = null;

			talkPanelText.text = fullText;
		}
	}

    private TextUnit ParseToTextUnit(string rawText)
    {
        int lastColon = rawText.LastIndexOf(':');
        string inkCharacterName, showingCharacterName, realCharacterName, text;
        if (lastColon >= 0)
        {
            inkCharacterName = rawText.Substring(0, lastColon).Trim();
            text = rawText.Substring(lastColon + 1).Trim();

			string[] split = inkCharacterName.Split('(',')');
            
            if(split.Length == 3)
			{
				realCharacterName = split[1];
				showingCharacterName = split[0];
			}
			else
			{
				realCharacterName = inkCharacterName;
				showingCharacterName = inkCharacterName;
			}

            InkVNCharacter character = Array.Find(characters, c => c.CharacterName == realCharacterName);
            Color characterColor = Color.white;
            if (character != null)
            {
                characterColor = character.characterColor;
            }
            return new TextUnit(showingCharacterName, realCharacterName, text, characterColor);
        }
        else
		{
			return new TextUnit(rawText);
		}

	}

    /// <summary>
    //This is the default behaviour. You have to override it.
	//On your subclass, you will likely need an AudioSource property.
	//I am not including it in this class just in case you are using other plugins to play the sound
    /// </summary>
	protected virtual void PlayBGM(string name)
	{

	}

    /// <summary>
    //This one uses PlayOneShot, so you don't need audio source in your subclass.
	//Just find some way to turn the string from your ink file into an actual AudioClip.
    /// </summary>
	protected virtual void PlaySFX(string name)
	{

	}

	private struct TextUnit
	{
		public string showingCharacterName;
		public string realCharacterName; //real name which can be in parentheses
		public string text;
		public Color characterColor;

		public TextUnit(string text)
		{
			this.showingCharacterName = "";
			this.realCharacterName = "";
			this.text = text;
			this.characterColor = Color.white;
		}

		public TextUnit(string showingCharacterName, string realCharacterName, string text, Color characterColor)
		{
            this.showingCharacterName = showingCharacterName;
			this.realCharacterName = realCharacterName;
			this.text = text;
			this.characterColor = characterColor;
		}
	}


}
