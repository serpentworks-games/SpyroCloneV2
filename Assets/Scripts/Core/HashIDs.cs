using UnityEngine;
/// <summary>
///  Converts string refs for the animator to hashids
/// </summary>
public static class PlayerHashIDs
{
    //Animator Variables
    public static int BaseVelocityHash = Animator.StringToHash("BaseVelocity");
    public static int TargetForwardVelocityHash = Animator.StringToHash("TargetForwardVelocity");
    public static int TargetRightVelocityHash = Animator.StringToHash("TargetRightVelocity");

    //Animator States
    public static int FreeLookMoveHash = Animator.StringToHash("Locomotion");
    public static int TargettingMoveHash = Animator.StringToHash("TargettingLocomotion");
    public static int LightAttack1Hash = Animator.StringToHash("LightAttack1");
    public static int LightAttack2Hash = Animator.StringToHash("LightAttack2");
    public static int LightAttack3Hash = Animator.StringToHash("LightAttack3");
}

public class EnemyHashIDs
{

}

public class NPCHashIDs
{

}

public class MiscHashIDs
{

}