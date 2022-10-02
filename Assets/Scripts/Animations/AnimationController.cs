using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Animations
{
    public class AnimationController 
    {
        private Animator animator;
        private const string DEFAULT_STATE  = "idle";

        private string currentState = string.Empty;

        public AnimationController(Animator animator)
        {
            this.animator = animator;
            currentState = DEFAULT_STATE;
        }

        public void Play(string newState)
        {
            if (currentState == newState) return;
            Debug.Log("PLAYING ANIMATION: " + newState);
            animator.Play(newState);
        }
    }
}
