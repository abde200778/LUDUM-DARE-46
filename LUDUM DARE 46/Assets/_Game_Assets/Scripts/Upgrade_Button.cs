using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Huntrox.Games.LD46;
using TMPro;



public class Upgrade_Button : MonoBehaviour
{
	private TextMeshProUGUI upgradename;
	private TextMeshProUGUI upgradeprice;
	private TextMeshProUGUI upgradedescription;
	private TextMeshProUGUI UpgradeQuantity;
	private Button Buybutton;
	private GameObject GM;
	[SerializeField] private GameObject NextUpgrade;
	[SerializeField] private Upgrade m_upgrade;
	[SerializeField] private int quantity =1;



	void Start()
    {
		GM = GameObject.FindGameObjectWithTag("GM");
		upgradename = transform.Find("Upgrade_name_text").GetComponent<TextMeshProUGUI>();
		upgradeprice = transform.Find("Upgrade_Price_text").GetComponent<TextMeshProUGUI>();
		upgradedescription = transform.Find("Upgrade_Description_text").GetComponent<TextMeshProUGUI>();
		UpgradeQuantity = transform.Find("Upgrade_Quantity_text").GetComponent<TextMeshProUGUI>();
		Buybutton = transform.Find("Button").GetComponent<Button>();
		Init();


	}


	public void Init(/*Upgrade upgrade*/)
	{
		//m_upgrade = upgrade;
		upgradename.text = m_upgrade.name;
		upgradeprice.text = "" + m_upgrade.price;
		upgradedescription.text =m_upgrade.Description;
		UpgradeQuantity.text = "" + quantity;

	}


	public void buyButton()
	{
		bool t_Enoughmoney = false;
		t_Enoughmoney = FishingSystem.instance.CheckMoney(m_upgrade.price);

		if (t_Enoughmoney)
			FishingSystem.instance.updateCurrentMoney(-m_upgrade.price);
		else
		{
			ScreenToolTip.inctanse.SetTooltip("you don't have enough money to buy this");
			return;
		}


		switch (m_upgrade._upgrade)
		{
			case upgrades.Boat_T2:
				FishingSystem.instance.upgrade_boat(1);
				break;
			case upgrades.Fishing_Rod_T2:
				FishingSystem.instance.upgradeFishing(5);
				break;
			case upgrades.Fishing_Rod_T3:
				FishingSystem.instance.upgradeFishing(7);
				break;
			case upgrades.Bucket_T_1:
				FishingSystem.instance.upgradeBucket(3);
				break;
			case upgrades.Bucket_T_2:
				FishingSystem.instance.upgradeBucket(6);
				break;
			case upgrades.Bucket_T_3:
				FishingSystem.instance.upgradeBucket(9);
				break;
			case upgrades.Fishing_Lure_T1:
				FishingSystem.instance.upgrade_Lure(15);
				break;
			case upgrades.Fishing_Lure_T2:
				FishingSystem.instance.upgrade_Lure(25);
				break;
			case upgrades.Fishing_Lure_T3:
				FishingSystem.instance.upgrade_Lure(35);
				break;
			case upgrades.Fishing_Gear_1:
				SpawnHandler.inctanse.SpawnFisherman(0);
				break;
			case upgrades.Fishing_Gear_2:
				SpawnHandler.inctanse.SpawnFisherman(1);
				break;
			default:
				t_Enoughmoney = false;
				break;
		}


		quantity--;
		UpgradeQuantity.text = "" + quantity;
		if (quantity <= 0)
		{

			Buybutton.interactable = false;
			GM.GetComponent<UI_popup>().DisableObject(this.gameObject, .3f);

			if (NextUpgrade != null)
				GM.GetComponent<UI_popup>().EnableObject(NextUpgrade, .4f, false);
		}

		if (Tutorial.instance.inTutorial && Tutorial.instance.tutorialState == TutorialState.buy_equipment)
		{

			Tutorial.instance.ChangeState(TutorialState.finish);

		}
	}

}
