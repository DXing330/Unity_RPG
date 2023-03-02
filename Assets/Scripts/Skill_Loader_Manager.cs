using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Loader_Manager : MonoBehaviour
{
    public static Skill_Loader_Manager instance;
    public List<Skill_Stat_Sheet> all_skills;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Load_Skill_Data();
    }

    [ContextMenu("Load Skills")]
    private void Load_Skill_Data()
    {
        TextAsset skill_json = Resources.Load<TextAsset>("skill_dict");
        if (skill_json != null)
        {
            Skill_Sheet_Wrapper skills = JsonUtility.FromJson<Skill_Sheet_Wrapper>(skill_json.text);
            all_skills = skills.skills;
        }
        else
        {
            Debug.LogError("Failed to load skill data!");
        }
        foreach (Skill_Stat_Sheet skill in all_skills)
        {
            Debug.Log(skill.name);
        }
    }

    public Skill_Stat_Sheet Load_Skill(string skill_name)
    {
        foreach (Skill_Stat_Sheet skill in all_skills)
        {
            if (skill.name == skill_name)
            {
                return skill;
            }
        }
        return null;
    }
}
