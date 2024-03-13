using ScalePact.Core.Input;
using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.StateMachines
{
    public class PlayerStateMachine : StateMachine
    {
        //Variables
        [field: SerializeField] public float BaseMoveSpeed { get; private set; } = 5f;
        //Properties
        public InputManager InputManager { get; private set; }
        public CharacterController CharacterController { get; private set; }


        private void Awake() {
            InputManager = GetComponent<InputManager>();
            CharacterController = GetComponent<CharacterController>();
        }

        void Start()
        {
            SwitchState(new PlayerMoveState(this));
        }
    }
}