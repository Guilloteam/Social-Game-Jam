using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CvDisplay : MonoBehaviour
{
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
        CvConfig cv_config = QuestlineManager.instance.cv_config;
        if(name != null)
            name.text = cv_config.name;
        if(age != null)
            age.text = cv_config.age;
        if(diplome != null)
            diplome.text = cv_config.diplome;
        if(experience != null)
            experience.text = cv_config.experience;
        if(contact != null)
            contact.text = cv_config.contact;
        if(passion!= null)
            passion.text = cv_config.passion;
        if(origine_country!= null)
            origine_country.text = cv_config.origine_country;
        if(portrait != null)
            portrait.sprite = cv_config.portrait;
    }

}
