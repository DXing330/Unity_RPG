using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Stat_Sheet 
{
    public string name;
    public int level;
    public int max_level;
    public int experience;
    public int base_health = 0;
    public int health = 0;
    public int base_attack = 0;
    public int attack = 0;
    public int base_defense = 0;
    public int defense = 0;
    public int base_energy = 0;
    public int energy = 0;
    public int temp_health = 0;
    public int accuracy = 100;
    public int evasion = 0;
    public int damage_dealt = 100;
    public int damage_taken = 100;
    public List<Skill_Stat_Sheet> skill_list = new List<Skill_Stat_Sheet>();
    public List<Skill_Stat_Sheet> passive_list = new List<Skill_Stat_Sheet>();

    public Stat_Sheet(string class_name, int current_level, int end_level, int exp, int b_health = 0, int b_attack = 0, int b_defense = 0, int b_energy = 0) 
    {
        name = class_name;
        level = current_level;
        max_level = end_level;
        experience = exp;
        base_health = b_health;
        base_attack = b_attack;
        base_defense = b_defense;
        base_energy = b_energy;
    }

    public void Refresh_Stats()
    {
        health = base_health;
        attack = base_attack;
        defense = base_defense;
        energy = base_energy;
    }

    public void Level_Up()
    {
        if (experience > level * level && level < max_level)
        {
            level++;
            experience = 0;
            Level_Up_Stats();
        }
    }

    public void Level_Up_Stats()
    {
        base_health += 10;
        base_attack += 3;
        base_defense += 2;
        base_energy += 1;
        Refresh_Stats();
    }

    public void Learn_Skill(Skill_Stat_Sheet new_skill)
    {
        bool learned = false;
        foreach (Skill_Stat_Sheet old_skill in skill_list)
        {
            if (old_skill.name == new_skill.name)
            {
                learned = true;
            }
        }
        if (!learned)
        {
            skill_list.Add(new_skill);
        }
    }

    public void Learn_Passive(Skill_Stat_Sheet new_skill)
    {
        bool learned = false;
        foreach (Skill_Stat_Sheet old_skill in passive_list)
        {
            if (old_skill.name == new_skill.name)
            {
                learned = true;
            }
        }
        if (!learned)
        {
            passive_list.Add(new_skill);
        }
    }
}

[System.Serializable]
public class Stat_Sheet_Wrapper
{
    public List<Stat_Sheet> characters;
}