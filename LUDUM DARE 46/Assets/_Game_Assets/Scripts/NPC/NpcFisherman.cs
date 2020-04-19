using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

namespace Huntrox.Games.LD46
{

	[System.Serializable]
	public class npcBoats
	{

		public string name;
		public float BoatSpeed;
		public int PlayerFeePerc;
		public int feshesCapacity;
		public bool Paddles;

	}
	public class NpcFisherman : MonoBehaviour
	{

		[SerializeField] npcBoats[] _boats;
		[SerializeField] private GameObject[] BoatsObjects;
		[SerializeField] private GameObject[] body;
		[SerializeField] private GameObject[] Cloth;
		[SerializeField] private GameObject[] hats;
		[SerializeField] private NpcStete npcStete;
		[SerializeField] private bool fishSpotDetected = false;
		[SerializeField] private bool isFishing = false;
		[SerializeField] private bool isreturnTrader = false;
		[SerializeField] private List<Fish> Inventory = new List<Fish>();
		[SerializeField] private int feshesCapacity = 3;
		[SerializeField] private int npcCurrentMoney = 3;
		[SerializeField] private int PlayerFeePerc = 3;


		private NavMeshAgent agent;
		private Animator _animator;
		private GameObject fishingRod;
		private Transform fishSpotTarget;
		[SerializeField] private Transform traderPosition;
		[SerializeField] private float FishingTimer = 2f;
	//	[SerializeField] private Vector2 moveDirRang = new Vector2(-7.5f, 7.5f);
		[SerializeField] private Vector2 TimerRange = new Vector2(5f, 10f);
		[SerializeField] private Vector3 moveDir;
		[SerializeField] private float stopingdistance = .5f;
		[SerializeField] private int CurrentBoatindex;
		private void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
			_animator = GetComponent<Animator>();
		
		}

		public void Init(int boatIndex)
		{
			body = new GameObject[]
			{
			   transform.Find("Character1").gameObject,
			   transform.Find("Character2").gameObject,
			   transform.Find("Character3").gameObject
			}; Cloth = new GameObject[]
			{
			   transform.Find("Cloth1").gameObject,
			   transform.Find("Cloth2").gameObject,
			   transform.Find("Cloth3").gameObject,
			   transform.Find("Cloth4").gameObject
			}; hats = new GameObject[]
			{
			   transform.Find("Hat1").gameObject,
			   transform.Find("Hat2").gameObject,
			   transform.Find("Hat3").gameObject
			};
			BoatsObjects = new GameObject[]
			{
			   transform.Find("RowBoat").gameObject,
			   transform.Find("Boat2").gameObject
			};

			foreach (GameObject b in body)
			{
				b.SetActive(false);
			}
			foreach (GameObject c in Cloth)
			{
				c.SetActive(false);
			}
		    foreach (GameObject h in hats)
			{
				h.SetActive(false);
			}


			traderPosition = GameObject.Find("TraderTrigger").transform;
			fishingRod = transform.Find("FishingRod").gameObject;
			SetUint(boatIndex);// just test


		}
		 void SetUint(int boatIndex)
		{

			body[Random.Range(0, body.Length)].SetActive(true);
			Cloth[Random.Range(0, Cloth.Length)].SetActive(true);
			hats[Random.Range(0, hats.Length)].SetActive(true);



			for (int i = 0; i < _boats.Length; i++)
			{

				if (i == boatIndex)
				{
					BoatsObjects[i].SetActive(true);



				}
				else
				{
					BoatsObjects[i].SetActive(false);

				}
			}

			

			PlayerFeePerc = _boats[boatIndex].PlayerFeePerc;
			feshesCapacity = _boats[boatIndex].feshesCapacity;
			CurrentBoatindex = boatIndex;
			agent.speed = _boats[boatIndex].BoatSpeed;
			agent.avoidancePriority = Random.Range(50, 101);
			moveDir = SpawnHandler.inctanse.GetRandomPosition();
			agent.SetDestination(moveDir);
		}

		private void Update()
		{

			_animator.SetInteger("State", (int)npcStete);

			StanderMovment();



		}

		void StanderMovment()
		{

			if(isreturnTrader)
			{
				fishSpotDetected = false;
				fishSpotTarget = null;
				fishingRod.SetActive(false);
				agent.stoppingDistance = 0;
				agent.SetDestination(traderPosition.position);
				if (agent.remainingDistance > agent.stoppingDistance)
				{

					if (_boats[CurrentBoatindex].Paddles)
						npcStete = NpcStete.rowboat_pedaling;
					else
						npcStete = NpcStete.Sailboat;

				}


				GetComponent<SphereCollider>().radius = 2;
			}
			else
			{
				GetComponent<SphereCollider>().radius = 12;
			}



			if (!fishSpotDetected&& !isreturnTrader)
			{
				agent.stoppingDistance = 0;
				
				fishingRod.SetActive(false);
				isFishing = false;
				




				if (agent.remainingDistance > agent.stoppingDistance)
				{

					if (_boats[CurrentBoatindex].Paddles)
						npcStete = NpcStete.rowboat_pedaling;
					else
						npcStete = NpcStete.Sailboat;

				}
				else
				{
					moveDir = SpawnHandler.inctanse.GetRandomPosition();
					agent.SetDestination(moveDir);

					if (_boats[CurrentBoatindex].Paddles)
						npcStete = NpcStete.rowboat_idle;
					else
						npcStete = NpcStete.Sailboat;

				}







			}
			else if(fishSpotDetected && !isreturnTrader)
			{
				agent.SetDestination(fishSpotTarget.position);
				agent.stoppingDistance = stopingdistance;

				if (agent.remainingDistance > agent.stoppingDistance)
				{
					if (_boats[CurrentBoatindex].Paddles)
						npcStete = NpcStete.rowboat_idle;
					else
						npcStete = NpcStete.Sailboat;

					fishingRod.SetActive(false);
					isFishing = false;
				}
				else
				{
					npcStete = NpcStete.fishing;
				    fishingRod.SetActive(true);
					if (!isFishing)
					{
						FishingTimer = Random.Range(TimerRange.x, TimerRange.y);
						isFishing = true;
					}

					

				}

			}

			if (isFishing)
			{

				if(Inventory.Count >= feshesCapacity)
				{
					isFishing = false;
					isreturnTrader = true;
				
					return;
				}

				FishingTimer -= Time.deltaTime;
				if (FishingTimer < 0f)
				{
					FishSize RandomfishSize = (FishSize)Random.Range(0, 3);
					Fish t_fish = new Fish();
					t_fish = t_fish.CreateFish(RandomfishSize);
					bool addfish;
					addfish = AddFish(t_fish);

					if (!addfish)
					{
						isFishing = false;
						isreturnTrader = true;
					}
					else
					{

						fishSpotTarget.GetComponent<FishingSpot>().removeFish(3);
					}




				FishingTimer = Random.Range(TimerRange.x, TimerRange.y);

				}



			}




		}
		public bool AddFish(Fish p_fish)
		{



			if (Inventory.Count >= feshesCapacity)
			{
			
				return false;
			}


			Inventory.Add(p_fish);
			

			return true;
		}
		void SellFishes()
		{
			if (Inventory.Count == 0)
				return;
			int totallPrice = Trader_Sell.inctanse.Trader_Sell_ToNPC(Inventory);
			Debug.Log("npc  fishing trip result :" + totallPrice);
			int PlayerFee =( totallPrice * PlayerFeePerc )/ 100;
			Debug.Log("PLayer return money result :" + PlayerFee);
			FishingSystem.instance.updateNpcFee(PlayerFee);

			Inventory.Clear();

		}



		private void OnTriggerEnter(Collider other)
		{

			if (other.CompareTag("FishingSpot"))
			{
				if (!fishSpotDetected&& !isreturnTrader)
				{
					fishSpotTarget = other.transform;
					fishSpotDetected = true;
				}


			}

			if (other.CompareTag("TraderTrigger"))
			{
				SellFishes();
				isreturnTrader = false;
				agent.isStopped= false;

			}



		}

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("FishingSpot"))
			{
				if (other.transform == fishSpotTarget)
				{
					fishSpotDetected = false;
					fishSpotTarget = null;
				}


			}


		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(moveDir, 1);

		}

		public void PlayPaddlesSound()
		{

			// just to stop the annoying wrraning :(
		}
	}
}