using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Huntrox.Games.LD46
{
 [System.Serializable]
	public class Boats
	{

		public string name;
		public float BoatMaxSpeed;
		public float SpeedMulti;
		public float boatRotateSpeed;
		public bool Paddles;

		


	}
	public class PlayerController : MonoBehaviour
	{


		private Rigidbody _rigidbody;
		private AudioSource m_audioSource;
		private FishingSystem _fishingSystem;
        private Animator _animator;

		private Vector3 MovePos;
		Vector3 m_EulerAngleVelocity;
		Quaternion deltaRotation;

		public PlayerState pLayerState;
	


		[SerializeField] private float currentBoatSpeed;
		[SerializeField] private float BoatMaxSpeed;
		[SerializeField] private float SpeedMulti;
		[SerializeField] private float boatRotateSpeed;


		[SerializeField] private bool _moving;// holding  key  = true
		[SerializeField] private bool inMovement;// is the  boat still moving -->  currentBoatSpeed != 0 
		[SerializeField] private bool fishingSpotDetected;
		[SerializeField] private bool _AllowFishing;
		[SerializeField] private bool _AllowCasting;
		[SerializeField] private bool _onTrader;
		[SerializeField] private bool _onTraderequipment;

		[SerializeField] Boats[] _boats;
	
		[SerializeField] private GameObject[] BoatsObjects;
		[SerializeField] private GameObject fishingRod;

		[SerializeField] private GameObject fishTriggerTooltip;
		[SerializeField] private GameObject tradertriggerTooltip;

		private GameObject GM;
		[SerializeField] private GameObject[] Paddles_objects;
		private int CurrentBoatindex;
		private float RotateSpeed;


		void Start()
		{
			GM = GameObject.FindGameObjectWithTag("GM");

			_rigidbody = GetComponent<Rigidbody>();
			_animator =transform.Find("Player").GetComponent<Animator>();
			m_audioSource = GetComponent<AudioSource>();
			_fishingSystem = GetComponent<FishingSystem>();
			fishTriggerTooltip = GameObject.Find("Canvas").transform.Find("FishingTriggerTooltip").gameObject;

			tradertriggerTooltip = GameObject.Find("Canvas").transform.Find("TraderTriggerTooltip").gameObject;


			Init();

		}

		private void Init()
		{

			BoatsObjects = new GameObject[]
			{
			   transform.Find("RowBoat").gameObject,
			   transform.Find("Boat2").gameObject
			};

			Paddles_objects = new GameObject[]
			{
				transform.Find("LeftPaddle").gameObject,
				transform.Find("RightPaddle").gameObject
			};
			fishingRod = transform.Find("FishingRod").gameObject;

			SetBoat(0);
		}

		void Update()
		{

			_animator.SetInteger("State", SetPlayerState(pLayerState));
			if (!_fishingSystem.holding)
			{
				if (!_moving)
				{
					if (currentBoatSpeed >= 0)
					{
						currentBoatSpeed -= Time.deltaTime;

					}
					if (currentBoatSpeed <= 0)
					{
						currentBoatSpeed += Time.deltaTime;

					}
					if (!_AllowFishing)
					{
						//	_animator.SetInteger("State", SetPlayerState(PlayerState.idle));
						pLayerState = PlayerState.idle;

					}
				}

				if (_AllowFishing &&!_fishingSystem.isFishing && _fishingSystem.fishingSpotObject.GetComponent<FishingSpot>().isfishingable && Input.GetKeyDown(KeyCode.F)) StartCasting();



				if (Mathf.Abs(currentBoatSpeed) > 0.2f || Mathf.Abs(RotateSpeed) > 0.2f)
				{
					inMovement = true;
				}
				else
				{
					inMovement = false;
				}

				if (Input.GetButton("Vertical"))
				{

					//	_animator.SetInteger("State", SetPlayerState(PlayerState.moving));
					pLayerState = PlayerState.moving;
					if (!_moving)
						_moving = true;

					inMovement = true;

					currentBoatSpeed = currentBoatSpeed += Time.deltaTime * Input.GetAxisRaw("Vertical") * SpeedMulti;

					if (Mathf.Abs(currentBoatSpeed) > BoatMaxSpeed)
						currentBoatSpeed = BoatMaxSpeed * Input.GetAxis("Vertical");
				}

				if (Input.GetButtonUp("Vertical"))
				{
					_moving = false;

				}
				RotateSpeed = Input.GetAxisRaw("Horizontal") * boatRotateSpeed ;
			
			m_EulerAngleVelocity = new Vector3(0, RotateSpeed, 0);



			}

			checkoutConditions();



		}


		void FixedUpdate()
		{
			deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
			


			MovePos = transform.position + transform.forward * currentBoatSpeed * Time.deltaTime;
			_rigidbody.MovePosition(MovePos);
			_rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
			//_rigidbody.rOT


			

		}



		public void PlayAudioOneShot(AudioClip p_audioClip)
		{
			if (p_audioClip != null)
				m_audioSource.PlayOneShot(p_audioClip);


		}
		public void SetBoat(int boatIndex)
		{

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

			if (_boats[boatIndex].Paddles)
			{

				Paddles_objects[0].SetActive(true);
				Paddles_objects[1].SetActive(true);
			}
			else
			{
				Paddles_objects[0].SetActive(false);
				Paddles_objects[1].SetActive(false);

			}



			BoatMaxSpeed = _boats[boatIndex].BoatMaxSpeed;
			boatRotateSpeed = _boats[boatIndex].boatRotateSpeed;
			SpeedMulti = _boats[boatIndex].SpeedMulti;

			CurrentBoatindex = boatIndex;
		}


		public int SetPlayerState(PlayerState p_pLayerState)
		{

			int state = 0;
			if (p_pLayerState == PlayerState.moving)
				if (_boats[CurrentBoatindex].Paddles)
					state = 1;
			    else
					state = 6;
			if (p_pLayerState == PlayerState.idle)
			{
				if (_boats[CurrentBoatindex].Paddles)
					state = 0;
				else 
	                state = 6;
			}
			if (p_pLayerState == PlayerState.casting)
				state = 2;
			if (p_pLayerState == PlayerState.fishing_idle)
				state = 3;
			if (p_pLayerState == PlayerState.fishing)
				state = 4;		
            if (p_pLayerState == PlayerState.hold)
				state = 5;

			return state;

		}

		void checkoutConditions()
		{

			if (inMovement)
			{
				if (_onTrader)
					Trader_Sell.inctanse.cancel();
					if (_onTraderequipment)
					Trader_Equipment.inctanse.CloseTrader();




				if (_boats[CurrentBoatindex].Paddles)
				{
					Paddles_objects[0].SetActive(true);
					Paddles_objects[1].SetActive(true);
				}

				fishingRod.SetActive(false);
				if (fishingSpotDetected)
				{
					
					_AllowCasting = false;
					_fishingSystem.StopFishing();

				}

			}
			if (fishingSpotDetected && !inMovement)
			{

				if (_boats[CurrentBoatindex].Paddles)
				{
					Paddles_objects[0].SetActive(false);
					Paddles_objects[1].SetActive(false);
				}

				fishingRod.SetActive(true);

				_AllowFishing = true;
				_AllowCasting = true;

			if(!_fishingSystem.isCasting&&!_fishingSystem.isFishing && !_fishingSystem.holding)
			//_animator.SetInteger("State", SetPlayerState(PlayerState.fishing_idle));
				pLayerState = PlayerState.fishing_idle;

			}
			else
			{
				_AllowFishing = false;

			}

			if (_AllowFishing && !_fishingSystem.isCasting &&! _fishingSystem.isFishing &&!_fishingSystem.holding&&_fishingSystem.fishingSpotObject.GetComponent<FishingSpot>().isfishingable)
			{
				GM.GetComponent<UI_popup>().EnableObject(fishTriggerTooltip, 0.15f,false);
				if (Tutorial.instance.inTutorial&& Tutorial.instance.tutorialState == TutorialState.FindFishSpot)
				{
					Tutorial.instance.ChangeState(TutorialState.Fishing);

				}

			}
			else
			{
				GM.GetComponent<UI_popup>().DisableObject(fishTriggerTooltip, 0.15f);
			}
			if (!inMovement)
			{

				if (_onTrader)
				{


				if(Input.GetKeyDown(KeyCode.F)) _fishingSystem.openTrader();
					if (!tradertriggerTooltip.activeInHierarchy)
					GM.GetComponent<UI_popup>().EnableObject(tradertriggerTooltip, 0.15f,false);


				}
				if (_onTraderequipment)
				{
					if (Input.GetKeyDown(KeyCode.F)) Trader_Equipment.inctanse.OpenTrader();
					if (!tradertriggerTooltip.activeInHierarchy)
						GM.GetComponent<UI_popup>().EnableObject(tradertriggerTooltip, 0.15f,false);
				}
		
			}
			else
			{
				if (tradertriggerTooltip.activeInHierarchy)
				GM.GetComponent<UI_popup>().DisableObject(tradertriggerTooltip, 0.15f);
			}


			//if (_onTraderequipment && !inMovement)
			//{
			//	if (Input.GetKeyDown(KeyCode.F)) Trader_Equipment.inctanse.OpenTrader();
			//	if (!tradertriggerTooltip.activeInHierarchy)
			//		GM.GetComponent<UI_popup>().EnableObject(tradertriggerTooltip, 0.15f);
			//}
			//else
			//{
			//	if (tradertriggerTooltip.activeInHierarchy)
			//		GM.GetComponent<UI_popup>().DisableObject(tradertriggerTooltip, 0.15f);
			//}


			if (_AllowFishing)
			{
				if (_fishingSystem.fishingSpotObject == null)
				{
					fishingSpotDetected = false;
					_AllowFishing = false;
					_AllowCasting = false;
				}
			}


		}


		public void StartCasting()
		{
			if (_AllowCasting)
			{
				_fishingSystem.isCasting = true;
			//	_animator.SetInteger("State", SetPlayerState(PlayerState.casting));
				pLayerState = PlayerState.casting;
			}


		}



		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("FishingSpot"))
			{

				fishingSpotDetected = true;
				_fishingSystem.fishingSpotObject = other.gameObject;


			}
			if (other.gameObject.CompareTag("TraderTrigger"))
			{

				_onTrader = true;
			}if (other.gameObject.CompareTag("TraderEquipment"))
			{

				_onTraderequipment = true;
			}



		}
		private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.CompareTag("FishingSpot"))
			{
				_fishingSystem.fishingSpotObject = null;
				fishingSpotDetected = false;
			//	if(_fishingSystem.isFishing)
				_fishingSystem.StopFishing();
				_AllowFishing = false;

			}
			if (other.gameObject.CompareTag("TraderTrigger"))
			{

				_onTrader = false; 

			}

			if (other.gameObject.CompareTag("TraderEquipment"))
			{

				_onTraderequipment = false;
			}

		}





	}
	
	public enum PlayerState { idle, moving, casting, fishing_idle, fishing,hold}
}