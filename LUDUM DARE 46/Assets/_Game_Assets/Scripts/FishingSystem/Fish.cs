using UnityEngine;

namespace Huntrox.Games.LD46
{

	[System.Serializable]
	public class Fish
	{
		public int fishWeight;
		public int fishHealth;
		public float fishTimer;
		public int ReleaseCounter;
		public Vector2 buffchangeRatio;

		public FishSize _fishSize;
		public FishBuff _buff;
		public bool caught = false;



		public Fish createTutorialFish()
		{
			Fish NewFish = new Fish();

			NewFish.fishWeight = 80;
			NewFish.fishHealth = 20;
			NewFish.fishTimer = 25f;
			NewFish.buffchangeRatio = new Vector2(0.45f, 1.2f);
			NewFish._fishSize = FishSize.Medium;
			return NewFish;
		}
		public Fish CreateFish(FishSize fishSize)
		{
			Fish NewFish = new Fish();

			switch (fishSize)
			{
				case FishSize.Small:
					NewFish.fishWeight = Random.Range(40, 80);
					NewFish.fishHealth = 20;
					NewFish.fishTimer = 25f;
					NewFish.buffchangeRatio = new Vector2(0.45f, 1.2f);
					NewFish._fishSize = fishSize;

					break;
				case FishSize.Medium:
					NewFish.fishWeight = Random.Range(80, 120);
					NewFish.fishHealth = 30;
					NewFish.fishTimer = 20f;
					NewFish.buffchangeRatio = new Vector2(0.45f, 1.05f);
					NewFish._fishSize = fishSize;
					break;
				case FishSize.Larg:
					NewFish.fishWeight = Random.Range(120, 160);
					NewFish.fishHealth = 40;
					NewFish.fishTimer = 15f;
					NewFish.buffchangeRatio = new Vector2(0.35f, 1.01f);
					NewFish._fishSize = fishSize;
					break;

			}
			return NewFish;
		}


		public void damage(int dmg)
		{

			fishHealth -= dmg;
			CheckHealth();

		}

		void CheckHealth()
		{
			if (fishHealth <= 0)
			{
				fishHealth = 0;
				caught = true;
				
			}

		}

		public void Release()
		{
			fishWeight = 0;
			fishHealth = 0;
			fishTimer = 0;
			ReleaseCounter = 0;
			caught = false;
		}



		public void SetNextBuff(int index)
		{

			//_buff = fishbuffManag(index);

		}

	

		public FishBuff fishbuffManag(FishBuff index)
		{

			FishBuff nextbuffIndex = (FishBuff)Random.Range((int)FishBuff.Up, 4);
			//Debug.Log("fishbuffManag = :  "+(int)nextbuffIndex+ "    +  "  + nextbuffIndex.ToString());

			if (nextbuffIndex == index)
			{
				return fishbuffManag(index);

			}

			return nextbuffIndex;
		}


	}
	public enum FishSize { Small, Medium, Larg }
	public enum FishBuff { Up, Down, Left, Right }
}