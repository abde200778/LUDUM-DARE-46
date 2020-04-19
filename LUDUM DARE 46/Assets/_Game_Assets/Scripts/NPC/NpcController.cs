using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

namespace Huntrox.Games.LD46
{


	public class NpcController : MonoBehaviour
	{
		private NavMeshAgent agent;
		private Animator _animator;
	//	private Rigidbody rb;
		[SerializeField] private NpcStete npcStete;
		[SerializeField] private float movespeed;
		[SerializeField] private Vector3[] movePathPositins;
		[SerializeField] private Transform[] positions;
		private int currentPosition_indx = 0;
		[SerializeField] private float moveTimer = 15f;
		[SerializeField] private int curve = 10;


		// Start is called before the first frame update
		void Start()
		{
			_animator = GetComponent<Animator>();
		//	rb = GetComponent<Rigidbody>();
			movePathPositins = new Vector3[positions.Length];
			for (int i = 0; i < movePathPositins.Length; i++)
			{
				movePathPositins[i] = positions[i].position;
			}


			if(positions.Length != 0)
			agent = GetComponent<NavMeshAgent>();
			NextWayPoint();

		}

		// Update is called once per frame
		void Update()
		{
			_animator.SetInteger("State", (int)npcStete);

			if (positions.Length != 0)
			{



				if (!agent.pathPending && agent.remainingDistance < 0.5f)
				{
					NextWayPoint();

				}



			}



		}






		private void NextWayPoint()
		{
			if (positions.Length == 0)
				return;

			agent.SetDestination(movePathPositins[currentPosition_indx]);

			currentPosition_indx = (currentPosition_indx + 1) % positions.Length;

		}
	}


	public enum NpcStete {idle, walk, walk_carry, walk_wagon, /*sit_casting,*/ sit_fishing_idle, sit_fishing, castingfishing_rod, fishing_idle, fishing, Hold_fish, rowboat_idle, rowboat_pedaling, Sailboat, hammer }
}