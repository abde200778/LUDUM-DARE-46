using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Huntrox.Games.LD46
{
	public class Tutorial : MonoBehaviour
	{



		public static Tutorial instance;
		public bool inTutorial = true;
		public TutorialState tutorialState;
		[SerializeField] private GameObject Dirarrow;
		[SerializeField] private GameObject FishingTutorialUI;
		[SerializeField] private GameObject Target_fish_spot;
		[SerializeField] private GameObject Target_Trader_Sell;
		[SerializeField] private GameObject Target_Trader_Equipment;
		[SerializeField] private GameObject MoveAndRotateTutorial;
		[SerializeField] private GameObject Pause;



		bool VerticalPress;
		bool HorizontalPress;

		private void Awake()
		{
			instance = this;
			inTutorial = true;
			Dirarrow = GameObject.Find("Dir_arrow");
			Target_fish_spot = GameObject.Find("TutorialFishSpot");
			Target_Trader_Sell = GameObject.Find("TraderTrigger");
			Target_Trader_Equipment = GameObject.Find("TraderEquipment");
			FishingTutorialUI = GameObject.Find("Canvas").transform.Find("FishingTutorialUI").gameObject;
			MoveAndRotateTutorial = GameObject.Find("Canvas").transform.Find("MoveAndRotateTutorial").gameObject;
			Pause = GameObject.Find("Canvas").transform.Find("Pause").gameObject;
		}
		// Start is called before the first frame update
		void Start()
		{
			ChangeState(TutorialState.Start);

		}

		// Update is called once per frame
		void Update()
		{
			if (tutorialState == TutorialState.Start) {
				//ScreenToolTip.inctanse.SetTooltip("press AWSD to move and rotate");

				if (Input.GetButtonDown("Vertical")) VerticalPress = true;
				if (Input.GetButtonDown("Horizontal")) HorizontalPress = true;

				if (VerticalPress && HorizontalPress)
				{
					ScreenToolTip.inctanse.SetTooltip("");
					ChangeState(TutorialState.FindFishSpot);
				}

			}


			if (Input.GetKeyDown(KeyCode.Escape)){

				PuaseTheGame();
			}

		}
		public void pause()
		{

			GetComponent<UI_popup>().EnableObject(FishingTutorialUI, 0.29f, true);
			Invoke("PauseGame", 0.31f);
		}
		void PauseGame()
		{
			Time.timeScale = 0;
		}


		public void ResumeGame()
		{
			Time.timeScale = 1;
			GetComponent<UI_popup>().DisableObject(FishingTutorialUI, 0.3f);
		}

		public void ChangeState(TutorialState p_tutorialState)
		{

			tutorialState = p_tutorialState;
			switch (p_tutorialState)
			{
				case TutorialState.Start:
					Dirarrow.SetActive(false);
					MoveAndRotateTutorial.SetActive(true);
					break;

				case TutorialState.FindFishSpot:
					Dirarrow.GetComponent<direction_arrow>().SetTarget(Target_fish_spot.transform);
					MoveAndRotateTutorial.SetActive(false);
					Dirarrow.SetActive(true);

					break;
				case TutorialState.Fishing:
					Dirarrow.SetActive(false);


					break;
				case TutorialState.Sell_to_Trader:

					Dirarrow.GetComponent<direction_arrow>().SetTarget(Target_Trader_Sell.transform);
					Dirarrow.SetActive(true);
					ScreenToolTip.inctanse.SetTooltip("follow the red arrow to find fish merchant");

					break;
				case TutorialState.buy_equipment:

					Dirarrow.GetComponent<direction_arrow>().SetTarget(Target_Trader_Equipment.transform);
					Dirarrow.SetActive(true);
					ScreenToolTip.inctanse.SetTooltip("follow the red arrow to buy upgrade  your fishing equipments");
					break;
				case TutorialState.finish:
					ScreenToolTip.inctanse.SetTooltip("Now you are ready ");

					inTutorial = false;
					Dirarrow.SetActive(false);
					SpawnHandler.inctanse.SpawnFishspots();
					break;
				default:
					break;
			}






		}



		public void Resume()
		{

			//Time.timeScale = 1;
			GetComponent<UI_popup>().DisableObject(Pause, 0.3f);
		}
		public void PuaseTheGame()
		{

			GetComponent<UI_popup>().EnableObject(Pause, 0.2f, true);
		//	Invoke("PauseGame", 0.22f);

		}

		public void OpenSettings()
		{
			Settings.instance.OpenSetting();

		}
		public void Quiting()
		{
			Settings.instance.QuitGame();

		}

		

	}

	public enum TutorialState {Start,FindFishSpot,Fishing,Sell_to_Trader,buy_equipment,finish }
}
