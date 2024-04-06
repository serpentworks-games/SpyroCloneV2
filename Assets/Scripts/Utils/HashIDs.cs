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

        //Jumping/Gliding
        public static int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
        public static int GlideTriggerHash = Animator.StringToHash("GlideTrigger");
        public static int LandTriggerHash = Animator.StringToHash("LandTrigger");

        //Death
        public static int DeathTriggerHash = Animator.StringToHash("DeathTrigger");
    }

    public class EnemyHashIDs
    {
        //Variables
        public static int SpeedHash = Animator.StringToHash("Speed");
        public static int AttackTriggerHash = Animator.StringToHash("BasicAttackTrigger");
        public static int ImpactTriggerHash = Animator.StringToHash("ImpactTrigger");
        public static int DeathTriggerHash = Animator.StringToHash("DeathTrigger");

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