using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Text fields.
    public Text level_text;
    public Text health_text;
    public Text coin_text;
    public Text mana_crystal_text;
    public Text weapon_level_text;
    public Text upgrade_cost;
    public Text exp_text;
    public Text familiar_level_text;
    public Text familiar_upgrade_cost;

    // Logic.
    public Image weapon_sprite;
    public RectTransform exp_bar;
    private bool showing;

    // Components.
    private Animator animator;

    // Get the animator.
    public void Start()
    {
        animator = GetComponent<Animator>();
        showing = false;
    }

    public void Showing(bool show)
    {
        showing = show;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (showing)
            {
                showing = false;
                animator.SetTrigger("Hide");
            }
            else
            {
                showing = true;
                UpdateMenu();
                animator.SetTrigger("Show");
            }
        }
    }

    // Weapon Upgrade
    public void OnWeaponUpgrade()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    // Familiar Upgrade
    public void OnFamiliarUpgrade()
    {
        if (GameManager.instance.TryUpgradeFamiliar())
        {
            UpdateMenu();
        }
    }

    // Character info.
    public void UpdateMenu()
    {
        // Use the game manager to get the information.
        // Weapon.
        int weapon_level = GameManager.instance.weapon.weaponLevel;
        weapon_level_text.text = weapon_level.ToString();
        if (GameManager.instance.weapon.weaponLevel >= GameManager.instance.weaponSprites.Count)
        {
            weapon_sprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weaponSprites.Count];
        }
        else
        {
            weapon_sprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        }
        upgrade_cost.text = GameManager.instance.DeterminePrice("weapon").ToString();
        // Familiar.
        // Print the upgrade cost for the familiar.
        int current_familiar_level = GameManager.instance.familiar.familiar_level;
        familiar_level_text.text = current_familiar_level.ToString();
        familiar_upgrade_cost.text = GameManager.instance.DeterminePrice("familiar").ToString();
        // Meta.
        health_text.text = GameManager.instance.player.health.ToString()+" / "+GameManager.instance.player.max_health.ToString();
        coin_text.text = GameManager.instance.coins.ToString();
        mana_crystal_text.text = GameManager.instance.mana_crystals.ToString();
        int current_level = GameManager.instance.player.playerLevel;
        level_text.text = current_level.ToString();
        // EXP Bar.
        int currect_exp = GameManager.instance.experience;
        int exp_to_level = GameManager.instance.GetExptoLevel();

        float exp_ratio = (float)currect_exp / (float)exp_to_level;
        exp_bar.localScale = new Vector3(exp_ratio, 1, 1);
        exp_text.text = currect_exp.ToString() + " / " + exp_to_level.ToString();
    }
}
