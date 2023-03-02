using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Up_Manager : MonoBehaviour
{
    public static Level_Up_Manager instance;

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
    }

    public void Initialize_Hero_Stats(Stat_Sheet hero)
    {
        if (hero.name == "Summoner")
        {
            hero.base_health = 20;
            hero.base_attack = 11;
            hero.base_defense = 7;
            hero.base_energy = 11;
            hero.Refresh_Stats();
        }
        else if (hero.name == "Angel")
        {
            hero.base_health = 1;
            hero.base_attack = 1;
            hero.base_defense = 1;
            hero.base_energy = 1;
            hero.Refresh_Stats();
        }
        else if (hero.name == "Warrior")
        {
            hero.base_health = 20;
            hero.base_attack = 17;
            hero.base_defense = 9;
            hero.base_energy = 7;
            hero.Refresh_Stats();
        }
    }

    public void Level_Up_Hero(Stat_Sheet hero)
    {
        if (hero.name == "Summoner")
        {
            hero.base_health += 8;
            hero.base_attack += 2;
            hero.base_defense += 1;
            hero.base_energy += 2;
            hero.Refresh_Stats();
        }
        else if (hero.name == "Angel")
        {
            hero.base_health += 1;
            hero.base_attack += 1;
            hero.base_defense += 1;
            hero.base_energy += 1;
            hero.Refresh_Stats();
        }
        else if (hero.name == "Warrior")
        {
            hero.base_health += 9;
            hero.base_attack += 2;
            hero.base_defense += 2;
            hero.base_energy += 1;
            hero.Refresh_Stats();
        }
    }

    private void Hero_Level_Up_Learn_Skill(Stat_Sheet hero)
    {
        if (hero.name == "Angel")
        {
            switch (hero.level)
            {
                case 2:
                    Skill_Stat_Sheet new_skill = Skill_Loader_Manager.instance.Load_Skill("Heal Ally");
                    break;
                default:
                    break;
            }
        }
    }
}
