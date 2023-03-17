using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        OnHealthChange();
    }

    // Resources.
    public List<Sprite> weaponSprites;
    public int weaponPrice;
    public int familiarPrice;
    public int expLevelUp;

    // References
    public Player player;
    public Weapon weapon;
    public Familiar familiar;
    public FloatingTextManager floatingTextManager;
    public RectTransform healthBar;
    public Text healthText;

    // Logic.
    public int coins;
    public int mana_crystals;
    public int experience;
    public int stat_points;
    public int familiar_stat_points;

    // Floating text.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Determine prices.
    public int DeterminePrice(string thing)
    {
        int level = 1;
        int price = 0;
        int cost = 0;
        if (thing == "weapon")
        {
            level = weapon.weaponLevel + 1;
            price = weaponPrice;
        }
        else if (thing == "familiar")
        {
            level = familiar.familiar_level + 1;
            price = familiarPrice;
        }
        else
        {
            // pass.
        }
        cost = price * level * level;
        return cost;
    }

    // Upgrade Weapon.
    public bool TryUpgradeWeapon()
    {
        int cost = DeterminePrice("weapon");
        if (coins >= cost)
        {
            coins -= cost;
            weapon.UpgradeWeapon();
            return true;
        }

        else
        {
            return false;
        }
    }

    // Upgrade Familiar.
    public bool TryUpgradeFamiliar()
    {
        int cost = DeterminePrice("familiar");
        if (mana_crystals >= cost)
        {
            mana_crystals -= cost;
            familiar.SetLevel(familiar.familiar_level+1);
            familiar_stat_points += familiar.familiar_level;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UpgradePlayerStats(string upgraded_stat)
    {
        if (stat_points < 0)
        {
            if (upgraded_stat == "bonus_health")
            {
                player.bonus_health++;
            }
            else if (upgraded_stat == "damage_multiplier")
            {
                player.damage_multiplier++;
            }
            else if (upgraded_stat == "damage_reduction")
            {
                player.damage_reduction++;
            }
            else if (upgraded_stat == "mana")
            {
                player.mana++;
            }
            else if (upgraded_stat == "luck")
            {
                player.luck++;
            }
            else
            {
                stat_points++;
            }
            stat_points--;
            return true;
        }
        return false;
    }

    public bool UpgradeFamiliarStats(string upgraded_stat)
    {
        if (familiar_stat_points < 0)
        {
            if (upgraded_stat == "rotate_speed_increase")
            {
                familiar.rotate_speed_increase++;
            }
            else if (upgraded_stat == "heal_threshold_increase")
            {
                familiar.heal_threshold_increase++;
            }
            else
            {
                familiar_stat_points++;
            }
            familiar_stat_points--;
            return true;
        }
        return false;
    }

    public int GetExptoLevel()
    {
        int level = player.playerLevel + 1;
        int exp = expLevelUp * level * level;

        return exp;
    }
    public void GrantExp(int exp)
    {
        experience += exp;
        if(experience >= GetExptoLevel())
        {
            experience -= GetExptoLevel();
            PlayerLevelUp();
        }
    }
    public void PlayerLevelUp()
    {
        player.LevelUp();
        stat_points += player.playerLevel;
    }

    public void PlayerDefeated()
    {
        coins -= player.max_health;
        player.health = player.max_health;
        OnHealthChange();
        if (coins < 0)
        {
            coins = 0;
        }
        experience -= player.playerLevel;
        if (experience < 0)
        {
            experience = 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        SaveState();
        ShowText("You were defeated but I brought you back.", 15, Color.red, familiar.transform.position, Vector3.zero, 2.0f);
    }

    public void OnHealthChange()
    {
        float ratio  = (float)player.health / (float)player.max_health;
        healthBar.localScale = new Vector3(ratio, 1, 1);
        healthText.text = player.health.ToString()+" / "+player.max_health.ToString();
    }

    // Saving and loading.
    public void SaveState()
    {
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        SaveDataWrapper save_data = new SaveDataWrapper();
        save_data.UpdateData();
        string save_json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText("Assets/Saves/save_data.json", save_json);
        PlayerStatsWrapper player_stats = new PlayerStatsWrapper();
        player_stats.UpdateData();
        string player_stats_json = JsonUtility.ToJson(player_stats, true);
        File.WriteAllText("Assets/Saves/player_stats.json", player_stats_json);
        FamiliarStatsWrapper familiar_stats = new FamiliarStatsWrapper();
        familiar_stats.UpdateData();
        string familiar_stats_json = JsonUtility.ToJson(familiar_stats, true);
        File.WriteAllText("Assets/Saves/familiar_stats.json", familiar_stats_json);
        Debug.Log("Saved");
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            player.SetLevel(loaded_data.player_level);
            familiar.SetLevel(loaded_data.familiar_level);
            weapon.SetLevel(loaded_data.weapon_level);
            coins = loaded_data.coins;
            mana_crystals = loaded_data.mana_crystals;
            experience = loaded_data.experience;
            string player_stats = File.ReadAllText("Assets/Saves/player_stats.json");
            PlayerStatsWrapper loaded_player_stats = JsonUtility.FromJson<PlayerStatsWrapper>(player_stats);
            player.SetStats(loaded_player_stats);
            string familiar_stats = File.ReadAllText("Assets/Saves/familiar_stats.json");
            FamiliarStatsWrapper loaded_familiar_stats = JsonUtility.FromJson<FamiliarStatsWrapper>(familiar_stats);
            familiar.SetStats(loaded_familiar_stats);
        }
        else
        {
            Debug.Log("Load failed");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
