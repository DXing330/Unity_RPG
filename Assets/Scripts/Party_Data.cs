using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Data : ScriptableObject
{
    // Party Members.
    public List<Stat_Sheet> heroes = new List<Stat_Sheet>();
    public List<Stat_Sheet> spirits = new List<Stat_Sheet>();

    // Items.
    //public int coins;
    //public int mana_crystals;

    // Other stuff; like quests, etc.
    // TODO.
}