﻿using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		[SerializeField] float weaponRange = 2f;
		[SerializeField] float weaponDamage = 5f;
		[SerializeField] float timeBetweenAttacks = 1f;

		Health target;
		float timeSinceLastAttack = 0;

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;

			if (target == null) return;
			if (target.IsDead()) return;

			if (!GetIsInRange())
			{
				GetComponent<Mover>().MoveTo(target.transform.position);
			}
			else
			{
				GetComponent<Mover>().Cancel();
				AttackBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform);
			if (timeSinceLastAttack > timeBetweenAttacks)
			{
				//This will trigger the Hit() method
				TriggerAttack();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttack()
		{
			GetComponent<Animator>().ResetTrigger("stopAttack");
			GetComponent<Animator>().SetTrigger("attack");
		}

		//Animation Event
		void Hit()
		{
			if (target == null) return;
			target.TakeDamage(weaponDamage);
		}

		public bool CanAttack(CombatTarget combatTarget)
		{
			if (combatTarget == null) return false;
			Health targetToTest = combatTarget.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead();
		}

		private bool GetIsInRange()
		{
			return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
		}

		public void Attack(CombatTarget combatTarget)
		{
			GetComponent<ActionSchedular>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}

		public void Cancel()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
			target = null;
		}
	}
}