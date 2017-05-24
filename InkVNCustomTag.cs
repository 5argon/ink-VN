using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InkVNCustomTag {

	public string customTag;
	[TextArea] public string evaluatesTo;

	public InkVNCustomTag(string customTag, string evaluatesTo)
	{
		this.customTag = customTag;
		this.evaluatesTo = evaluatesTo;
	}

}
