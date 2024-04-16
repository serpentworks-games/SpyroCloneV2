using System;
using UnityEngine;

namespace ScalePact.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if(currentAction == action) return;
            if(currentAction != null)
            {
                currentAction.CancelAction();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}