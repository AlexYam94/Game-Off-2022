using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour
{
    public bool canDoubleJump  = true;
    public bool canDash  = true;
    public bool canBecomeBall  = false;
    public bool canDropBomb  = false;

    public void UnlockAbility(AbilityEnum abilitiyToUnlock)
    {
        switch (abilitiyToUnlock)
        {
            case AbilityEnum.ball:
                canBecomeBall = true;
                break;
            case AbilityEnum.dash:
                canDash = true;
                break;
            case AbilityEnum.bomb:
                canDropBomb = true;
                break;
            case AbilityEnum.doubleJump:
                canDoubleJump = true;
                break;
        }
    }

    public void LockAbility(AbilityEnum abilitiyToLock)
    {
        switch (abilitiyToLock)
        {
            case AbilityEnum.ball:
                canBecomeBall = false;
                break;
            case AbilityEnum.dash:
                canDash = false;
                break;
            case AbilityEnum.bomb:
                canDropBomb = false;
                break;
            case AbilityEnum.doubleJump:
                canDoubleJump = false;
                break;

        }
    }
}
