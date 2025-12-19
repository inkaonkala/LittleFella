using UnityEngine;
using System.Collections;
using System.Linq;

public class SpideyBoss : MonoBehaviour
{
    [Header("Refs")]
    public Transform mom;
    public SpiderLeg[] legs;

    [Header("Behavior")]
    public float engageDistance = 7f;
    public float pickInterval = 0.8f;
    public int maxLegsMoving = 2;

    System.Random rng;

    void Awake()
    {
        rng = new System.Random();

        foreach (var le in legs)
            if (le != null) le.controlledByBoss = true;
    }

    void OnEnable()  => StartCoroutine(Brainz());
    void OnDisable() => StopAllCoroutines();

    IEnumerator Brainz()
    {
        while (true)
        {
            if (mom == null)
            {
                yield return null;
                continue;
            }

            float dist = Vector2.Distance(transform.position, mom.position);

            // Only do leg logic when mom is close enough
            if (dist <= engageDistance)
            {
                // how many legs are already in an attack
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

                // wait between “decisions” while engaged
                yield return new WaitForSeconds(pickInterval);
            }
            else
            {
                // mom is too far, just idle-check every so often
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
