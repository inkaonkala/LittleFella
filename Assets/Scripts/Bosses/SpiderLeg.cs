using UnityEngine;
using System.Collections;


public class SpiderLeg : MonoBehaviour
{
    [Header("Setup")]
	public Transform tip;
	public Transform target;
	public Transform home;
	public Transform mom;
	public Collider2D killZone;

	[Header("Timer")]
	public float windupHeight = 1f;
	public float windupTime = 0.3f;
	public float slamTime = 0.1f;
	public float recoverTime = 0.4f;
	public float pauseTime = 1f;

	public bool controlledByBoss = true;
	public bool IsBusy => busy;

	bool busy;

	void Start()
    {

		if (home == null)
		{
//			GameObject h = new GameObject(name + "_Home");
			var h = new GameObject(name + "_home");
			h.transform.position = target.position;
			home = h.transform;
		}
		if (killZone) killZone.enabled = false;

	//	killZone.enabled = false;
	if (!controlledByBoss)
			StartCoroutine(AttackLoop());
    }

	IEnumerator AttackLoop()
	{
		while (true)
		{
			if (!busy && mom != null)
				yield return StartCoroutine(AttackMom());

			yield return new WaitForSeconds(pauseTime);
		}
	}

	IEnumerator MoveTarget(Vector3 from, Vector3 to, float duration)
	{
		float t = 0f;
		while (t < duration)
		{
			t += Time.deltaTime;
			float k = Mathf.SmoothStep(0, 1, t / duration);
			target.position = Vector3.Lerp(from, to, k);
			yield return null;
		}
		target.position = to;
	}

    IEnumerator AttackMom()
	{
		busy = true;

		Vector3 startPos = target.position;
		Vector2 momPos = mom.position;

		//lift leg aka windup
		Vector3 liftPos = startPos + Vector3.up * windupHeight;
		yield return MoveTarget(startPos, liftPos, windupTime);

		// slam to mom
		killZone.enabled = true;
		yield return MoveTarget(liftPos, momPos, slamTime);
		killZone.enabled = false; // i might want to keep it active thou

		//recover
		yield return MoveTarget(target.position, home.position, recoverTime);

		busy = false;
		
	}

	public IEnumerator AttackOnce(Vector3 worldPos)
	{
		if (busy)
			yield break;

		busy = true;

		Vector3 startPos = target.position;
		Vector3 liftPos = startPos + Vector3.up * windupHeight;

		yield return MoveTarget(startPos, liftPos, windupTime);

		// slam to mom
		if (killZone) 
			killZone.enabled = true;
		yield return MoveTarget(liftPos, worldPos, slamTime);
		if (killZone)
			killZone.enabled = false; // i might want to keep it active thou

		//recover
		yield return MoveTarget(target.position, home.position, recoverTime);

		busy = false;
	}

}
