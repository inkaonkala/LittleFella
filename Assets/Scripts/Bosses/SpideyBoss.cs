using UnityEngine;
using System.Collections;
using System.Linq;


public class SpideyBoss : MonoBehaviour
{
//	What do we need?

/*
	The legs
		- calculate the radius for movement leg_nmb (starting point for the round) + movement_radius (pi * ?)
		- how much do we want to move from the joint? 

	void LegMoveLoop()
	{
		Choose random leg
			- Move up, turn(random nmb in radius), move down
		Wait (random time in radius)
	}
	

    // Start is called once before the first execution of Update after the MonoBehaviour is created

	The eyes follow Momma
*/
	[Header("Refs")]
	public Transform mom;
	public SpiderLeg[] legs;

	[Header("Behavior")]
	public float engageDistance = 7f;
	public float pickInterval = 0.8f;
	public int maxLegsMoving = 2;

	System.Random rng;


//	private int leg_num;

    void Awake()
    {
		rng = new System.Random();

		//attach legs to the bos by activating them to loopyloop
		foreach (var le in legs)
			if (le != null) le.controlledByBoss = true;
    }

	void OnEnable() => StartCoroutine(Brainz());
	void OnDisable() => StopAllCoroutines();

/*
	IEnumerator LegMoveLoop() AKA BRAINS!
	{
		Choose random leg
			- Move up, turn(random nmb in radius), move down
		Wait (random time in radius)
	}
	*/
	IEnumerator Brainz()
	{
		while (true)
		{
			while (mom && Vector2.Distance(transform.position, mom.position) <= engageDistance)
			{
				int busy = legs.Count(le => le != null && le.IsBusy);
				int canLaunch = Mathf.Max(0, maxLegsMoving - busy);

				if (canLaunch > 0)
				{
					var free = legs.Where(le => le != null && !le.IsBusy).ToList();
					if (free.Count > 0)
					{
						int toLaunch = Mathf.Clamp(rng.Next(1, canLaunch + 1), 1, free.Count);
						for (int i = 0; i < toLaunch; i++)
						{
							int idx = rng.Next(free.Count);
							var leg = free[idx];
							free.RemoveAt(idx);

							StartCoroutine(leg.AttackOnce(mom.position));
						}
					}
				}

			}
			yield return new WaitForSeconds(pickInterval);
		}
	} 
}
