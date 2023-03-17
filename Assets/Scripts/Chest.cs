using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinsAmount = 10;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            coinsAmount = Random.Range(coinsAmount, coinsAmount*2);
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.ShowText("+ "+coinsAmount+" coins", 20, Color.yellow, transform.position,Vector3.up*25,1.0f);
            GameManager.instance.coins += coinsAmount;
        }
    }
}