using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CvConfig : ScriptableObject
{
    public new string name;
    public new string age;
    public new string french_level;
    public new string diplome;
    public new string experience;
    [TextAreaAttribute(3, 5)]
    public new string contact;
    [TextAreaAttribute(3, 5)]
    public new string passion;
    [TextAreaAttribute(3, 5)]
    public new string origine_country;
    public Sprite portrait;
}
