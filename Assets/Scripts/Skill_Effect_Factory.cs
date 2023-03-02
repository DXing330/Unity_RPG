using System.Collections;
using System.Collections.Generic;


public class Skill_Effect_Factory
{
    public void Determine_Skill_Effect(Stat_Sheet user, Skill_Stat_Sheet skill, List<Stat_Sheet> targets)
    {
        if (skill.effect == "Change_Stats")
        {
            int power = skill.Determine_Power(user);
            foreach (Stat_Sheet target in targets)
            {
                Change_Stats(skill.effect_specifics, power, target);
            }
        }
    }

    public void Change_Stats(string specifics, int power, Stat_Sheet target)
    {
        if (specifics == "Health")
        {
            target.health += power;
        }
        else if (specifics == "Temp_Health")
        {
            target.temp_health += power;
        }
    }
}