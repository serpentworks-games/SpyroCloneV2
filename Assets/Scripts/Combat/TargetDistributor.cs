using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Combat
{
    public class TargetDistributor : MonoBehaviour
    {
        [System.Serializable]
        public class TargetFollower
        {
            public bool requireAttackSlot;
            public int assignedAttackSlot;
            public Vector3 requiredPosition;

            public TargetDistributor distributor;

            public TargetFollower(TargetDistributor owner)
            {
                distributor = owner;
                requiredPosition = Vector3.zero;
                requireAttackSlot = false;
                assignedAttackSlot = -1;
            }
        }

        [SerializeField] int arcsCount;

        protected Vector3[] worldDirection;
        protected bool[] freeArcs;
        protected float arcDegree;

        protected List<TargetFollower> followers;

        private void OnEnable() {
            worldDirection = new Vector3[arcsCount];
            freeArcs = new bool[arcsCount];

            followers = new();

            arcDegree = 360.0f / arcsCount;
            Quaternion rot = Quaternion.Euler(0, -arcDegree, 0);
            Vector3 currentDir = Vector3.forward;

            for (int i = 0; i < arcsCount; i++)
            {
                freeArcs[i] = true;
                worldDirection[i] = currentDir;
                currentDir = rot * currentDir;
            }
        }

        public TargetFollower RegisterNewFollower()
        {
            TargetFollower follower = new TargetFollower(this);
            followers.Add(follower);
            return follower;
        }

        public void UnregisterFollower(TargetFollower follower)
        {
            if(follower.assignedAttackSlot != -1)
            {
                freeArcs[follower.assignedAttackSlot] = true;
            }

            followers.Remove(follower);
        }

        private void LateUpdate() {
            for (int i = 0; i < followers.Count; i++)
            {
                var follower = followers[i];

                if(follower.assignedAttackSlot != -1)
                {
                    freeArcs[follower.assignedAttackSlot] = true;
                }

                if(follower.requireAttackSlot)
                {
                    follower.assignedAttackSlot = GetFreeArcIndex(follower);
                }
            }
        }

        public Vector3 GetWorldDirection(int index)
        {
            return worldDirection[index];
        }

        public int GetFreeArcIndex(TargetFollower follower)
        {
            bool found = false;

            Vector3 wantedPos = follower.requiredPosition - transform.position;
            Vector3 raycastPos = transform.position + Vector3.up * 0.4f;

            wantedPos.y = 0;
            float wantedDist = wantedPos.magnitude;

            wantedPos.Normalize();

            float angle = Vector3.SignedAngle(wantedPos, Vector3.forward, Vector3.up);

            if(angle < 0)
            {
                angle = 360 + angle;
            }

            int wantedIndex = Mathf.RoundToInt(angle / arcDegree);

            if(wantedIndex >= worldDirection.Length)
            {
                wantedIndex -= worldDirection.Length;
            }

            int chosenIndex = wantedIndex;

            RaycastHit hit;
            if(!Physics.Raycast(raycastPos, GetWorldDirection(chosenIndex), out hit, wantedDist))
            {
                found = freeArcs[chosenIndex];
            }

            if(!found)
            {
                int offset = 1;
                int halfCount = arcsCount / 2;

                while (offset <= halfCount)
                {
                    int leftIndex = wantedIndex - offset;
                    int rightIndex = wantedIndex + offset;

                    if(leftIndex < 0)
                    {
                        leftIndex += arcsCount;
                    }
                    
                    if(rightIndex >= arcsCount)
                    {
                        rightIndex -= arcsCount;
                    }

                    if(!Physics.Raycast(raycastPos, GetWorldDirection(leftIndex), wantedDist) && freeArcs[leftIndex])
                    {
                        chosenIndex = leftIndex;
                        found = true;
                        break;
                    }

                    if (!Physics.Raycast(raycastPos, GetWorldDirection(rightIndex), wantedDist) && freeArcs[rightIndex])
                    {
                        chosenIndex = rightIndex;
                        found = true;
                        break;
                    }

                    offset += 1;
                }
            }

            if(!found)
            {
                return -1;
            }

            freeArcs[chosenIndex] = false;
            return chosenIndex;
        }

        public void FreeIndex(int index)
        {
            freeArcs[index] = true;
        }
    }
}