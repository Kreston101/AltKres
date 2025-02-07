using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool damaged = false;

    //check progress obj
    //if associated item is unlocked, then set value
    //dont need to make a thing to save this part of the player data
    public bool unlockedDash = false;
    public bool unlockedExtraJump = false;
}
