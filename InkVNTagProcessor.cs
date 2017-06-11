using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using Ink.Runtime;

public partial class InkVNRunner : MonoBehaviour{

/* 
	private InkVNCustomTag[] builtInCustomTags = {
		new InkVNCustomTag("image-trigger", "trigger"),
		new InkVNCustomTag("animation-trigger", "trigger")
	};
*/

	private IEnumerator EvaluateTag(string tag)
	{
		string[] split = tag.Split(' ');
		if(split.Length <= 0)
		{
			yield break;
		}

        //Search for built-in tags
		switch(split[0])
		{
			case "wait":
			{
				yield return new WaitForSeconds(ParseFloatOnArgument(1,split));
				yield break;
			}
			case "trigger" :
			{
                if(ArgumentCountIs(3,split))
				{
					InkVNAnimator findAnimator = Array.Find<InkVNAnimator>(namedAnimators, a => a.name == split[1]);
					if(findAnimator != null)
					{
						findAnimator.animator.SetTrigger(split[2]);
					}
				}
				yield break;
			}
		}

        //In this case, search for custom tags
        InkVNCustomTag customTag = Array.Find<InkVNCustomTag>(customTags, t => t.customTag == split[0]);
        if (customTag != null)
        {
			string[] resolvedTags = customTag.evaluatesTo.Split('\n');
            foreach(string oneLine in resolvedTags)
			{
                //Recursive call
				yield return StartCoroutine(EvaluateTag(oneLine));
			}
        }
		yield break;
	}

	private float ParseFloatOnArgument(int argumentIndex, string[] split)
	{
        float result;
        if (ArgumentCountIs(argumentIndex+1, split))
        {
            if (float.TryParse(split[argumentIndex], out result))
            {
                return result;
            }
            else
            {
                throw new Exception(split[argumentIndex] + " - float is not in a correct format.");
            }
        }
		else
		{
			return 0;
		}
    }

    private Exception TagError(string tag, string message)
	{
		return new Exception("#" + tag + " - " + message);
	} 

	private bool ArgumentCountIs(int count, string[] split)
	{
		if(split.Length > count)
		{
			return true;
		}
		else
		{
			throw TagError(String.Join(" ", split), "Argument count incorrect.");
		}
	}

}
