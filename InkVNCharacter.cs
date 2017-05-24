using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InkVNCharacter {

    //This means it shows up in the inspector but when getting it you must use the accessor
    [SerializeField] private string characterName;
    //You could make a translation in this accessor
    public string CharacterName{
        get{ return characterName;}
    }

	public Color characterColor;
}
