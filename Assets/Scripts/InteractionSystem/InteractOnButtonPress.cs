using ScalePact.Core.Input;
using UnityEngine;
using UnityEngine.Events;

namespace ScalePact.InteractionSystem
{
    public class InteractOnButtonPress : InteractOnTrigger
    {
        [SerializeField] UnityEvent OnButtonPress;

        bool canExecuteButtonPress = false;

        InputManager inputs;

        private void Awake() {
            inputs = GameObject.FindWithTag("Player").GetComponent<InputManager>();
        }
        
        private void OnEnable() {
            inputs.InteractEvent += OnButtonUsed;
        }

        private void OnDisable() {
            inputs.InteractEvent -= OnButtonUsed;
        }

        protected override void ExecuteOnEnter(Collider other)
        {
            canExecuteButtonPress = true;
        }

        protected override void ExecuteOnExit(Collider other)
        {
            canExecuteButtonPress = false;
        }

        void OnButtonUsed()
        {
            if(canExecuteButtonPress)
            {
                OnButtonPress?.Invoke();
            }
        }
    }
}