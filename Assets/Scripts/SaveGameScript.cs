using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameScript : MonoBehaviour
{
    public void SaveGame()
    {
        GameManager.instance.SaveState();
    }
}
