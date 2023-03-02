using System.Collections;
using System.Collections.Generic;


public class Skill_Activation
{
    public void Determine_Cost_Cooldown(Stat_Sheet user, Skill_Stat_Sheet skill)
    {
        user.energy -= skill.cost;
        skill.cooldown += skill.cooldown_counter;
    }

    public bool Determine_Activation(Stat_Sheet user, Skill_Stat_Sheet skill, List<Stat_Sheet> targets)
    {
        if (skill.condition == "Always")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
