using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager
{
    public SaveData game_data;

    public void SaveGameData()
    {
        /*
        string str = "";
        str += player.playerLevel.ToString() + "|";
        str += coins.ToString() + "|";
        str += experience.ToString() + "|";
        str += weapon.weaponLevel.ToString() + "|";
        str += familiar.familiar_level.ToString() + "|";
        str += mana_crystals.ToString();

        PlayerPrefs.SetString("SaveState", str);
        */
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        SaveDataWrapper save_data = new SaveDataWrapper();
        save_data.UpdateData();
        string save_json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText("Assets/Saves/save_data.json", save_json);
    }

    public void LoadGameData()
    {
        /*
        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        Debug.Log("Loaded");
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        
        player.SetLevel(int.Parse(data[0]));
        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        if (experience < 0)
        {
            experience = 0;
        }
        weapon.UpdateWeaponLevel(int.Parse(data[3]));
        familiar.SetLevel(int.Parse(data[4]));
        mana_crystals = int.Parse(data[5]);
        */
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            GameManager.instance.player.SetLevel(loaded_data.player_level);
            GameManager.instance.familiar.SetLevel(loaded_data.familiar_level);
            GameManager.instance.weapon.SetLevel(loaded_data.weapon_level);
            GameManager.instance.coins = loaded_data.coins;
            GameManager.instance.mana_crystals = loaded_data.mana_crystals;
            GameManager.instance.experience = loaded_data.experience;
        }
        else
        {
            Debug.LogWarning("Data file not found!");
        }
    }
}

public class SaveDataWrapper
{
    public int player_level;
    public int familiar_level;
    public int weapon_level;
    public int coins;
    public int mana_crystals;
    public int experience;
    public int stat_points;
    public int familiar_stat_points;
    public string random_stuff;

    public void UpdateData()
    {
        player_level = GameManager.instance.player.playerLevel;
        familiar_level = GameManager.instance.familiar.familiar_level;
        weapon_level = GameManager.instance.weapon.weaponLevel;
        coins = GameManager.instance.coins;
        mana_crystals = GameManager.instance.mana_crystals;
        experience = GameManager.instance.experience;
        stat_points = GameManager.instance.stat_points;
        familiar_stat_points = GameManager.instance.familiar_stat_points;
    }
}

public class PlayerStatsWrapper
{
    public int bonus_health;
    public int damage_multiplier;
    public int damage_reduction;
    public int mana;
    public int luck;

    public void UpdateData()
    {
        bonus_health = GameManager.instance.player.bonus_health;
        damage_multiplier = GameManager.instance.player.damage_multiplier;
        damage_reduction = GameManager.instance.player.damage_reduction;
        mana = GameManager.instance.player.mana;
        luck = GameManager.instance.player.luck;
    }
}

public class FamiliarStatsWrapper
{
    public int rotate_speed_increase;
    public int heal_threshold_increase;

    public void UpdateData()
    {
        rotate_speed_increase = GameManager.instance.familiar.rotate_speed_increase;
        heal_threshold_increase = GameManager.instance.familiar.heal_threshold_increase;
    }
}