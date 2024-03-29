using UnityEngine;
/// <summary>
///  Converts string refs for the animator to hashids
/// </summary>
namespace ScalePact.Utils
{
    public static class PlayerHashIDs
    {
        //Variables
        //Movement
        public static int BaseVelocityHash = Animator.StringToHash("BaseVelocity");
        public static int TargetForwardVelocityHash = Animator.StringToHash("TargetForwardVelocity");
        public static int TargetRightVelocityHash = Animator.StringToHash("TargetRightVelocity");

        //Combat
        public static int AttackTriggerHash = Animator.StringToHash("AttackTrigger");
        public static int AttackIndexHash = Animator.StringToHash("AttackIndex");
        public static int ImpactTriggerHash = Animator.StringToHash("ImpactTrigger");

        //Jumping
        public static int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        public static int LandTriggerHash = Animator.StringToHash("LandTrigger");

        //Death
        public static int DeathTriggerHash = Animator.StringToHash("DeathTrigger");

        //States
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
        //Variables
        public static int SpeedHash = Animator.StringToHash("Speed");

        //States
        public static int LocomotionHash = Animator.StringToHash("Locomotion");
        public static int Attack1Hash = Animator.StringToHash("Attack1");

    }

    public class NPCHashIDs
    {

    }

    public class SharedHashIDs
    {
        //States
        public static int ImpactStateHash = Animator.StringToHash("Impact");
        public static int DeathStateHash = Animator.StringToHash("Death");
    }
}