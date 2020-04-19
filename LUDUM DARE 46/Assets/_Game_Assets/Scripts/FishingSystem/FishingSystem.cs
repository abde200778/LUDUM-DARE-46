using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Huntrox.Games.LD46
{
	public class FishingSystem : MonoBehaviour
	{


		public static FishingSystem instance;
		public delegate void OnBuffChange(FishBuff fishBuff);
		public OnBuffChange onBuffChanged;
		public GameObject CastBar;
		public int feshesCapacity = 3; 
		public int LargFishChance = 15; 

		public List<Fish> Inventory = new List<Fish>();


		[SerializeField] private Fish currFish;
		[SerializeField] private GameObject currentFishObject;
		[SerializeField] private bool m_isFishing;
		public bool isFishing { get { return m_isFishing; } }
		public bool isCasting;
		[SerializeField] private bool m_Holding;
		public bool holding { get { return m_Holding; } }

		public GameObject fishingSpotObject;
		public GameObject[] fishesPrefabs;


		[SerializeField] private int fishMaxHealth;
		[SerializeField] private Image test;
		[SerializeField] private float CastinggSpeed;
		[SerializeField] private float CastingProgres;
		[SerializeField] private float CastingTimer;
		[SerializeField] private float fishingTimer = 1f;
		[SerializeField] private int dmg = 3;
		[SerializeField] private int currentMoney = 0;
		[SerializeField] private FishBuff Buff;
		[SerializeField] private Transform OrigPosition;
		[SerializeField] private GameObject PlayerMesh;
		[SerializeField] private GameObject cath_UI;
		[SerializeField] private GameObject money_ui;
		[SerializeField] private GameObject target_Fish_UI;
		private GameObject fishesCountText;
		private GameObject GM;


		private void Awake() => instance = this;



		// Start is called before the first frame update
		void Start()
		{
			GM = GameObject.FindGameObjectWithTag("GM");
			cath_UI = GameObject.Find("Canvas").transform.Find("cath_UI").gameObject;
			fishesCountText = GameObject.Find("Canvas").transform.Find("inv_fishes").transform.Find("fishes_count").gameObject;
			money_ui = GameObject.Find("Canvas").transform.Find("money_ui").transform.Find("moneyText").gameObject;
			target_Fish_UI = GameObject.Find("Canvas").transform.Find("target_Fish_UI").gameObject;
			OrigPosition = transform.Find("OrigPosition").transform;
			PlayerMesh = transform.Find("Player").gameObject;
			RefreshUI();

		}



		void Update()
		{
			fishing();

			if (isCasting)
			{
				GM.GetComponent<UI_popup>().EnableObject(CastBar, .2f,false);
				Casting();
			}
			else
			{

				GM.GetComponent<UI_popup>().DisableObject(CastBar, .2f);
				CastingProgres = 0;
			}



			if (m_isFishing && currentFishObject != null)
			{
				if (Input.GetKeyDown(KeyCode.UpArrow)) setBuff(0);
				if (Input.GetKeyDown(KeyCode.DownArrow)) setBuff(1);
				if (Input.GetKeyDown(KeyCode.LeftArrow)) setBuff(2);
				if (Input.GetKeyDown(KeyCode.RightArrow)) setBuff(3);

				Vector3 direction = (currentFishObject.transform.position - transform.position).normalized;

				Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

				//	lookRotation = lookRotation * Quaternion.Inverse(PlayerMesh.transform.parent.rotation);

				//lookRotation.x = -90k;
				//	Debug.DrawRay(transform.position, direction, Color.blue);

				//PlayerMesh.transform.localRotation 
				PlayerMesh.transform.rotation = Quaternion.Slerp(PlayerMesh.transform.rotation, lookRotation, Time.deltaTime * 5f);
				//PlayerMesh.transform.localRotation = new Quaternion(-0.7071068f, PlayerMesh.transform.localRotation.y, PlayerMesh.transform.localRotation.z, PlayerMesh.transform.localRotation.w);
				//	PlayerMesh.transform.LookAt(direction, Vector3.forward);
			}
			else
			{
				PlayerMesh.transform.localRotation = OrigPosition.localRotation;
			}


#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.L))
			{
				Fish t_fish = new Fish().CreateFish((FishSize)Random.Range(0, 3));
				AddFish(t_fish);
			}


			if (Input.GetKeyDown(KeyCode.U))
			{
				updateCurrentMoney(Random.Range(1000,2001));
			}

#endif

		}

		public void StopFishing()
		{
			CastingProgres = 0;
			m_isFishing = false;
			isCasting = false;
			currFish.Release();
			StopAllCoroutines();
			GM.GetComponent<UI_popup>().DisableObject(target_Fish_UI, .3f);
			if (currentFishObject != null)
			{
				//	currentFishObject.GetComponent<TargetFish>().unsubscribe();
				GM.GetComponent<UI_popup>().DisableObject(target_Fish_UI, .3f);
				if (fishingSpotObject == null)
					GetComponent<PlayerController>().pLayerState = PlayerState.idle;

				Destroy(currentFishObject);
			}
		}



		public bool AddFish(Fish p_fish)
		{



			if (Inventory.Count >= feshesCapacity)
			{
				RefreshUI();
				return false;
			}


			Inventory.Add(p_fish);
			RefreshUI();

			return true;
		}


		void HoldingFish()
		{

			m_Holding = true;
			StopAllCoroutines();
			GM.GetComponent<UI_popup>().EnableObject(cath_UI, .3f,true);
			GetComponent<PlayerController>().pLayerState = PlayerState.hold;
			cath_UI.transform.Find("WeightText").GetComponent<TextMeshProUGUI>().text = currFish.fishWeight + " KG";
			cath_UI.transform.Find("SizeText").GetComponent<TextMeshProUGUI>().text = currFish._fishSize.ToString();
			fishingSpotObject.GetComponent<FishingSpot>().removeFish(2);
			//cath_UI
			//StopCoroutine(RandomMovement());
			GM.GetComponent<UI_popup>().DisableObject(target_Fish_UI, .3f);

			//bool addfiish = AddFish(currFish);
			//if (addfiish)
			//{
			//	//currFish = null;

			//}
			//currFish.Release();



		}
		public void Add()
		{

			bool addfiish = AddFish(currFish);
			if (addfiish)
			{
				//currFish = null;
				m_Holding = false;
				currFish = new Fish();
				GM.GetComponent<UI_popup>().DisableObject(cath_UI, .3f);
				if (Tutorial.instance.inTutorial && Tutorial.instance.tutorialState == TutorialState.Fishing)
				{

					Tutorial.instance.ChangeState(TutorialState.Sell_to_Trader);
				}



			}
			else
			{
				ScreenToolTip.inctanse.SetTooltip("you fish storage capacity is full");
			}

		}
		public void Release()
		{
			m_Holding = false;
			currFish.Release();
			GM.GetComponent<UI_popup>().DisableObject(cath_UI, 0.3f);

		}


		public bool CheckMoney(int price)
		{
			if (price > currentMoney)
				return false;
			else
				return true;
		}


		void fishing()
		{
			if (m_isFishing && !m_Holding)
			{

				if (currFish.fishWeight != 0 && currentFishObject != null && currFish.fishTimer <= 10)
					GM.GetComponent<UI_popup>().PunchUi(target_Fish_UI.transform.Find("Timer").gameObject, 1f, new Vector3(0.2f, .2f, .2f));

				if (currFish.fishWeight != 0 && currentFishObject != null && currFish.fishTimer > 0)
				{
					currFish.fishTimer -= Time.deltaTime;
				}
				else
				{
					StopAllCoroutines();
					m_isFishing = false;
					isCasting = false;
					CastingProgres = 0;
					//currFish = null;
					//			currentFishObject.GetComponent<TargetFish>().unsubscribe();
					currFish.Release();
					Destroy(currentFishObject);
					GM.GetComponent<UI_popup>().DisableObject(target_Fish_UI, .3f);
					//StopCoroutine(RandomBuff());
				}


				if (currFish.fishWeight != 0 && currentFishObject != null && currFish.ReleaseCounter >= 5)
				{
					StopAllCoroutines();
					m_isFishing = false;
					isCasting = false;
					CastingProgres = 0;
					//currFish = null;
					GM.GetComponent<UI_popup>().DisableObject(target_Fish_UI, .3f);
					//	currentFishObject.GetComponent<TargetFish>().unsubscribe();
					currFish.Release();
					Destroy(currentFishObject);
					fishingSpotObject.GetComponent<FishingSpot>().removeFish(1);
					//StopCoroutine(RandomBuff());

				}

				if (currFish.fishWeight != 0 && currentFishObject != null && currFish.caught)
				{
					HoldingFish();
					StopAllCoroutines();
					m_isFishing = false;
					isCasting = false;
					CastingProgres = 0;
					//StopCoroutine(RandomMovement());
					//			currentFishObject.GetComponent<TargetFish>().unsubscribe();

					Destroy(currentFishObject);
				}


				float healthratio = (float)currFish.fishHealth / (float)fishMaxHealth;
				target_Fish_UI.transform.Find("FishHealthBar").GetChild(0).GetComponent<Image>().fillAmount =
					Mathf.Lerp(target_Fish_UI.transform.Find("FishHealthBar").GetChild(0).GetComponent<Image>().fillAmount, healthratio, Time.deltaTime * 8f);
				float resistRatio = (float)currFish.ReleaseCounter / (float)5;
				target_Fish_UI.transform.Find("FishResist").GetChild(0).GetComponent<Image>().fillAmount =
					Mathf.Lerp(target_Fish_UI.transform.Find("FishResist").GetChild(0).GetComponent<Image>().fillAmount, resistRatio, Time.deltaTime * 8f);

				target_Fish_UI.transform.Find("Timer").GetComponent<TextMeshProUGUI>().text = "" + (int)currFish.fishTimer;


			}


		}

		void RefreshUI()
		{
			fishesCountText.GetComponent<TextMeshProUGUI>().text = Inventory.Count + " / " + feshesCapacity;
			money_ui.GetComponent<TextMeshProUGUI>().text = currentMoney.ToString();


		}
		public void setBuff(int p_buffINDEX)
		{
			bool hit_or_miss;
			Buff = (FishBuff)p_buffINDEX;

			if (currFish._buff == Buff)
			{
				currFish.damage(dmg);
				hit_or_miss = true;
				Debug.Log("Hit");
			}
			else
			{
				currFish.ReleaseCounter++;
				hit_or_miss = false;
				Debug.Log("Miss");
			}

			currentFishObject.GetComponent<TargetFish>().shake(hit_or_miss);

			currFish._buff = currFish.fishbuffManag(currFish._buff);
			//	currentFishObject.GetComponent<TargetFish>().SetBuff(currFish._buff);

			//	Debug.Log(Buff.ToString());
		}

		public void Casting()
		{
			if (!m_isFishing)
			{








				CastingProgres += CastinggSpeed * Time.deltaTime;
				float curProgres = CastingProgres / 100;
				CastBar.transform.GetChild(0).localScale = Vector3.Lerp(CastBar.transform.GetChild(0).localScale, new Vector3(curProgres, 1, 1), Time.deltaTime * 8f);


				//CastBar.fillAmount = curProgres;
				if (CastingProgres >= 100)
				{
					StartFishing();

					//StartFishing();

				}
			}
		}

		public void updateCurrentMoney(int price)
		{
			if (price > 0)
			{
				Inventory.Clear();
				GM.GetComponent<UI_popup>().AddCashText(money_ui, .5f, new Vector3(0.2f, 0.2f, 0.2f), Color.green,"  "+ price);
			}
			if (price < 0)
			{
				GM.GetComponent<UI_popup>().AddCashText(money_ui, .5f, new Vector3(0.2f, 0.2f, 0.2f), Color.red, " " + price);
			}
			currentMoney += price;
			AudioManager.inctanse.PlayOneshot(SFX_TYPE.Coins_SFX, 0.12f);
			RefreshUI();
		}
		public void updateNpcFee(int price)
		{
			GM.GetComponent<UI_popup>().AddCashText(money_ui, .5f, new Vector3(0.2f, 0.2f, 0.2f), Color.green, " " + price);
			currentMoney += price;
			AudioManager.inctanse.PlayOneshot(SFX_TYPE.Coins_SFX, 0.12f);
			RefreshUI();

		}
		public void openTrader()
		{
			GM.GetComponent<Trader_Sell>().OpenTrader_Sell(Inventory);

		}
		
		public void upgradeBucket(int value)
		{

			feshesCapacity = value;
			RefreshUI();
		}
		public void upgradeFishing(int value)
		{

			dmg = value;

		}

		public void upgrade_boat(int id)
		{

			GetComponent<PlayerController>().SetBoat(id);
		}

	
		public void upgrade_Lure(int value)
		{

			LargFishChance = value;
		}



		private void StartFishing()
		{


			if (Tutorial.instance.inTutorial && Tutorial.instance.tutorialState == TutorialState.Fishing)
			{

				Tutorial.instance.pause();












			}



				//	GetComponent<PlayerController>()._animator.SetInteger("State", GetComponent<PlayerController>().SetPlayerState(PlayerState.fishing));
				GetComponent<PlayerController>().pLayerState = PlayerState.fishing;
			CastingProgres = 0;
			//Debug.Log("Called");
			m_isFishing = true;
			m_Holding = false;
			isCasting = false;
			if (Tutorial.instance.inTutorial && Tutorial.instance.tutorialState == TutorialState.Fishing)
			{
				currFish = currFish.createTutorialFish();

				fishMaxHealth = currFish.fishHealth;
			}
			else
			{

				int Catch_RNG = Random.Range(0, 100);
				Debug.Log(Catch_RNG);
				FishSize RandomfishSize;
				if (Catch_RNG <= LargFishChance)
					RandomfishSize = FishSize.Larg;
				else
					RandomfishSize = (FishSize)Random.Range(0, 2);

				currFish = currFish.CreateFish(RandomfishSize);
				fishMaxHealth = currFish.fishHealth;
				fishingSpotObject.GetComponent<FishingSpot>().removeFish(0);

			}

			GameObject t_fish = Instantiate(fishesPrefabs[Random.Range(0, fishesPrefabs.Length)], fishingSpotObject.transform.position, Quaternion.identity, fishingSpotObject.transform);
			float newScale;

			switch (currFish._fishSize)
			{
				case FishSize.Small:
					newScale = Random.Range(0.2f, .28f);
					break;
				case FishSize.Medium:
					newScale = Random.Range(0.28f, .38f);
					break;
				case FishSize.Larg:
					newScale = Random.Range(0.38f, .5f);
					break;
				default:
					newScale = Random.Range(0.2f, .28f);
					break;
			}
			GM.GetComponent<UI_popup>().EnableObject(target_Fish_UI, .3f,false);
			target_Fish_UI.transform.Find("Timer").GetComponent<TextMeshProUGUI>().color = Color.white;


			//	transform.Find("FishSkin").
			t_fish.transform.Find("FishSkin").localScale = t_fish.transform.Find("FishSkin").localScale * newScale;
			currentFishObject = t_fish;

			StartCoroutine(RandomMovement());
			StartCoroutine(RandomBuff());

		}

		IEnumerator RandomMovement()
		{
			while (true)
			{
				if (currFish == null)
					yield break;


				if (currentFishObject != null)
				{


					Vector3 randomposition = Random.insideUnitSphere * 10;


					currentFishObject.GetComponent<TargetFish>().SetDirection(randomposition);

				}



				


				yield return new WaitForSeconds(Random.Range(0.5f, 3.5f));

			}

		}
		IEnumerator RandomBuff()
		{
			while (true)
			{
				if (currFish == null)
					yield break;


				    currFish._buff = currFish.fishbuffManag(currFish._buff);
		


					onBuffChanged.Invoke(currFish._buff);



				Debug.Log(currFish._buff.ToString());


				yield return new WaitForSeconds(Random.Range(currFish.buffchangeRatio.x, currFish.buffchangeRatio.y));

			}

		}
	}
}