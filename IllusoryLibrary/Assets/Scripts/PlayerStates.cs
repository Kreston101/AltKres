using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public bool jumping = false;
    public bool dashing = false;
    public bool damaged = false;

    //attatch to items later
    public bool unlockedDash = false;
    public bool unlockedExtraJump = false;
}
