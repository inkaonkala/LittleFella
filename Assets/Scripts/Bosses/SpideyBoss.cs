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
		{
			if (le != null) 
				le.controlledByBoss = true;
		}
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
			bool engaged = mom && Vector2.Distance(transform.position, mom.position) <= engageDistance;
            if (engaged)
            {
                // Count how many are truly in motion right now
                int moving = legs.Count(le => le && le.IsBusy);
                int slots  = Mathf.Max(0, maxLegsMoving - moving);

                if (slots > 0)
                {
                    // Only consider legs that are free AND off cooldown
                    var free = legs.Where(le => le && !le.IsBusy && le.IsReady).ToList();

                    if (free.Count > 0)
                    {
                        // Launch 1..slots, but never more than what's free
                        int toLaunch = Mathf.Clamp(rng.Next(1, slots + 1), 1, free.Count);
                        // Snapshot mom’s position ONCE per pick
                        Vector3 targetPos = mom.position;

                        for (int i = 0; i < toLaunch; i++)
                        {
                            int idx = rng.Next(free.Count);
                            var leg = free[idx];
                            free.RemoveAt(idx);

                            // fire-and-forget; the leg will mark itself busy & cooldown
                            StartCoroutine(leg.AttackOnce(targetPos));
                        }
                    }
                }

			}
			yield return new WaitForSeconds(pickInterval);
		}
	} 
}
