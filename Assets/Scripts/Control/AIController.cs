﻿using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 5f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[SerializeField] float waypointDwellTime = 3f;

		GameObject player;
		Health health;
		Fighter fighter;
		Mover mover;
		ActionSchedular actionSchedular;

		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
		int currentWaypointIndex = 0;

		private void Start()
		{
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
			fighter = GetComponent<Fighter>();
			mover = GetComponent<Mover>();
			actionSchedular = GetComponent<ActionSchedular>();

			guardPosition = transform.position;
		}

		private void Update()
		{
			if (health.IsDead()) return;

			if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
			{
				AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer < suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}

			UpdateTimers();
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
		}

		private void SuspicionBehaviour()
		{
			actionSchedular.CancelCurrentAction();
		}

		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;
			if (patrolPath != null)
			{
				if (AtWaypoint())
				{
					CycleWaypoint();
					timeSinceArrivedAtWaypoint = 0;
				}
				nextPosition = GetCurrentWaypoint();
			}
			if (timeSinceArrivedAtWaypoint > waypointDwellTime)
			{
				mover.StartMoveAction(nextPosition);
			}
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint(currentWaypointIndex);
		}

		private void AttackBehaviour()
		{
			timeSinceLastSawPlayer = 0;
			fighter.Attack(player);
		}

		private bool InAttackRangeOfPlayer()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer < chaseDistance;
		}

		//Called by Unity
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}