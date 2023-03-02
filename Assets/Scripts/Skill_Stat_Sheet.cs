using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Skill_Stat_Sheet
{
    public string name;
    public string effect;
    public string effect_specifics;
    public string targets;
    public int cost;
    public string scale = "Level";
    public int power = 1;
    public int cooldown = 0;
    public int cooldown_counter = 0;
    public int chance = 100;
    public string condition = "Always";
    public string condition_specifics = "Always";

    public Skill_Stat_Sheet(string skill_name, string skill_effect, string e_specifics, string skill_targets, int s_cost, string s_scale = "Level", int s_power = 1, int s_cd = 0, int cd_counter = 0, int s_chance = 100, string s_condition = "Always", string c_specifics = "Always")
    {
        name = skill_name;
        effect = skill_effect;
        effect_specifics = e_specifics;
        targets = skill_targets;
        cost = s_cost;
        scale = s_scale;
        power = s_power;
        cooldown = s_cd;
        cooldown_counter = cd_counter;
        chance = s_chance;
        condition = s_condition;
        condition_specifics = c_specifics;
    }

    public int Determine_Power(Stat_Sheet user)
    {
        if (scale == "Level")
        {
            return power * user.level;
        }
        else if (scale == "Attack")
        {
            return power * user.attack;
        }
        else if (scale == "Defense")
        {
            return power * user.defense;
        }
        else if (scale == "Health")
        {
            return power * user.health;
        }
        else
        {
            return power;
        }
    }
}


[System.Serializable]
public class Skill_Sheet_Wrapper
{
    public List<Skill_Stat_Sheet> skills;
}