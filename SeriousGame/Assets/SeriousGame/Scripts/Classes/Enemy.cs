using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public class Enemy : ActorBase
    {
        private void Start()
        {
        }
        protected override void Initialize(float maxHealth, ActorMeta actorInfo)
        {
            base.Initialize(maxHealth, actorInfo);

        }

        public override void OnDied()
        {
            base.OnDied();
            StartCoroutine("DyingRoutine");
        }

        IEnumerator DyingRoutine()
        {
            yield return new WaitForSeconds(4f);
            Destroy(gameObject);
        }
    }
}

