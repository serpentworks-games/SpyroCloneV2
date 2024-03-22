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
    public static int JumpStartHash = Animator.StringToHash("JumpStart");
    public static int JumpLandHash = Animator.StringToHash("JumpLand");
    public static int GlideHash = Animator.StringToHash("Glide");
}

public class EnemyHashIDs
{
    public static int SpeedHash = Animator.StringToHash("Speed");

    public static int LocomotionHash = Animator.StringToHash("Locomotion");
    public static int Attack1Hash = Animator.StringToHash("Attack1");

}

public class NPCHashIDs
{

}

public class SharedHashIDs
{
    public static int ImpactStateHash = Animator.StringToHash("Impact");
    public static int DeathStateHash = Animator.StringToHash("Death");
}