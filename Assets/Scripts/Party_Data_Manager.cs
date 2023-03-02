using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Will hold the party's data, character info, inventory, etc.
public class Party_Data_Manager : MonoBehaviour
{
    public static Party_Data_Manager instance;
    public static Skill_Loader_Manager skill_loader;
    public static Level_Up_Manager level_up_manager;
    public static Party_Data party_data;

    // Start is called before the first frame update
    private void Start()
    {
        if (Party_Data_Manager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        party_data = ScriptableObject.CreateInstance<Party_Data>();
        DontDestroyOnLoad(gameObject);
        Load_Data();
    }

    [ContextMenu("New Data")]
    private void New_Data()
    {
        party_data = ScriptableObject.CreateInstance<Party_Data>();
        skill_loader = GameObject.FindObjectOfType<Skill_Loader_Manager>();
        level_up_manager = GameObject.FindObjectOfType<Level_Up_Manager>();
        Stat_Sheet new_hero = new Stat_Sheet("Summoner", 1, 10, 0);
        level_up_manager.Initialize_Hero_Stats(new_hero);
        party_data.heroes.Add(new_hero);
        Stat_Sheet new_hero_2 = new Stat_Sheet("Warrior", 1, 10, 0);
        level_up_manager.Initialize_Hero_Stats(new_hero_2);
        party_data.heroes.Add(new_hero_2);
        Stat_Sheet new_spirit = new Stat_Sheet("Angel", 1, 10, 0);
        level_up_manager.Initialize_Hero_Stats(new_spirit);
        Skill_Stat_Sheet new_skill = skill_loader.Load_Skill("Shield Ally");
        new_spirit.Learn_Skill(new_skill);
        party_data.spirits.Add(new_spirit);
        Save_Data();
    }

    public void Save_Data()
    {
        // Check if the directory exists, create it if it doesn't
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        Stat_Sheet_Wrapper hero_wrapper = new Stat_Sheet_Wrapper();
        hero_wrapper.characters = party_data.heroes;
        string hjson = JsonUtility.ToJson(hero_wrapper, true);
        Debug.Log(hjson);
        File.WriteAllText("Assets/Saves/heroes.json", hjson);
        Stat_Sheet_Wrapper spirit_wrapper = new Stat_Sheet_Wrapper();
        spirit_wrapper.characters = party_data.spirits;
        string sjson = JsonUtility.ToJson(spirit_wrapper, true);
        File.WriteAllText("Assets/Saves/spirits.json", sjson);
        Debug.Log(sjson);
    }

    [ContextMenu("Load Data")]
    public void Load_Data()
    {
        party_data = ScriptableObject.CreateInstance<Party_Data>();
        if (File.Exists("Assets/Saves/heroes.json"))
        {
            string hero_data = File.ReadAllText("Assets/Saves/heroes.json");
            Stat_Sheet_Wrapper hero_wrapper = JsonUtility.FromJson<Stat_Sheet_Wrapper>(hero_data);
            party_data.heroes = hero_wrapper.characters;
        }
        else
        {
            Debug.LogWarning("Hero data file not found!");
        }
        if (File.Exists("Assets/Saves/spirits.json"))
        {
            string spirit_data = File.ReadAllText("Assets/Saves/spirits.json");
            Stat_Sheet_Wrapper spirit_wrapper = JsonUtility.FromJson<Stat_Sheet_Wrapper>(spirit_data);
            party_data.spirits = spirit_wrapper.characters;
        }
        else
        {
            Debug.LogWarning("Spirit data file not found!");
        }
        foreach (Stat_Sheet ally in party_data.spirits)
        {
            foreach (Skill_Stat_Sheet skill in ally.skill_list)
            {
                Debug.Log(skill.name);
            }
        }
    }
}