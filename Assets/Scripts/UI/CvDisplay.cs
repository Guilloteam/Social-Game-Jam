using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CvDisplay : MonoBehaviour
{
    public CvConfig cv_config;
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI age;
    public TMPro.TextMeshProUGUI diplome;
    public TMPro.TextMeshProUGUI experience;
    public TMPro.TextMeshProUGUI contact;
    public TMPro.TextMeshProUGUI passion;
    public TMPro.TextMeshProUGUI origine_country;
    public Image portrait;

    public void Start()
    {
        name.text = cv_config.name;
        age.text = cv_config.age;
        diplome.text = cv_config.diplome;
        experience.text = cv_config.experience;
        contact.text = cv_config.contact;
        passion.text = cv_config.passion;
        origine_country.text = cv_config.origine_country;
        portrait.sprite = cv_config.portrait;
    }

}
