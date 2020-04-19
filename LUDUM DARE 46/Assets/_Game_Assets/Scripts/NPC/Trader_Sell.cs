using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
namespace Huntrox.Games.LD46
{
	public class Trader_Sell : MonoBehaviour
	{
		public static Trader_Sell inctanse;

		private GameObject GM;
		private GameObject Sell_UI;
		[SerializeField] private float kg_price;
		[SerializeField] private float final_price;

	//	[SerializeField] private List<Fish> playerfishes = new List<Fish>();



		void Awake() => inctanse = this;

		// Start is called before the first frame update
		void Start()
		{
			Sell_UI = GameObject.Find("Canvas").transform.Find("Sell_UI").gameObject;
			GM = GameObject.FindGameObjectWithTag("GM");

		}


		public void OpenTrader_Sell(List<Fish> p_playerfishes)
		{


			if (p_playerfishes.Count == 0)
			{
				ScreenToolTip.inctanse.SetTooltip("you don't have any fish to sell");
				return;
			}

			int finalWieght = 0;

			for (int i = 0; i < p_playerfishes.Count; i++)
			{

				finalWieght += p_playerfishes[i].fishWeight;

			}



			final_price = finalWieght * kg_price;

			Sell_UI.transform.Find("WeightText").GetComponent<TextMeshProUGUI>().text = "" + finalWieght+" kg";
			Sell_UI.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = "" + (int)final_price;
			GM.GetComponent<UI_popup>().EnableObject(Sell_UI, .3f,true);



		}




		public void sell()
		{


			FishingSystem.instance.updateCurrentMoney((int)final_price);
			GM.GetComponent<UI_popup>().DisableObject(Sell_UI, .2f);

			final_price = 0;

			if (Tutorial.instance.inTutorial && Tutorial.instance.tutorialState == TutorialState.Sell_to_Trader)
			{

				Tutorial.instance.ChangeState(TutorialState.buy_equipment);
			}
		}
		public void cancel()
		{

			GM.GetComponent<UI_popup>().DisableObject(Sell_UI, .2f);
			
			final_price = 0;

		}

		public int Trader_Sell_ToNPC(List<Fish> p_playerfishes)
		{


			if (p_playerfishes.Count == 0)
				return 0;

			int finalWieght = 0;

			for (int i = 0; i < p_playerfishes.Count; i++)
			{

				finalWieght += p_playerfishes[i].fishWeight;

			}



			 final_price = finalWieght * kg_price;

			return (int)final_price;


		}



		// Update is called once per frame
		void Update()
		{

		}
	}
}
