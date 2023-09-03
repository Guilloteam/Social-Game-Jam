using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct SoundParamConfig
{
    public string param_name;
    public float start_value;
    public float end_value;
}

public class SoundParamManager : MonoBehaviour
{
    public static SoundParamManager instance;
    public AudioMixer mixer;
    public SoundParamConfig[] configs;
    public float transition_duration = 1;
    public float transition_ratio = 0;
    public float anim_direction = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    public void SetAnimDirection(float direction)
    {
        anim_direction = direction;
    }

    void Update()
    {
        transition_ratio += Time.deltaTime * anim_direction / transition_duration;
        transition_ratio = Mathf.Clamp01(transition_ratio);
        for(int i=0; i < configs.Length; i++)
        {
            mixer.SetFloat(configs[i].param_name, Mathf.Lerp(configs[i].start_value, configs[i].end_value, transition_ratio));
        }
    }
}
