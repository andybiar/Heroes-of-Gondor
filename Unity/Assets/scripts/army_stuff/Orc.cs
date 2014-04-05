using UnityEngine;
using System.Collections;
using BoothGame;

public class Orc : Infantry, Enemy, Lockable {
	private GameObject selectionAura;
	private float highlightStrength = .35f;
	private float separationL = 999;
	private float separationR = 999;
	private string lName, rName;

	public Stats stats;

	void Awake() {
		selectionAura = transform.GetChild(0).gameObject;
	}

	public void onLock() {
		selectionAura.SetActive(true);
		Color c = renderer.material.color;
		float h = highlightStrength;
		renderer.material.color = new Color(c.r + h, c.g + h, c.b + h);
	}

	// When Gandalf hit an orc with a spell, it dies
	public void onFire() {
		die();
	}

	public void onStab() {
		die();
	}

	public void onArrow() {
		die();
	}

	public void onRelease() {
		selectionAura.SetActive(false);
		Color c = renderer.material.color;
		float h = highlightStrength;
		renderer.material.color = new Color(c.r - h, c.g - h, c.b - h);
	}

	public bool isItAlive() {
		return getIsAlive();
	}

	private bool isAlly(Transform t) {
		Component[] allies = t.GetComponents(typeof(Ally));
		
		if (allies.Length > 0 && ((Ally)allies[0]).getIsAlive()) return true;
		return false;
	}
		
	protected override Transform aggroCast() {
		Debug.DrawRay(transform.position, Quaternion.Euler(0, -6, 0) * transform.forward * aggroRange);
		Debug.DrawRay(transform.position, Quaternion.Euler(0, 6, 0) * transform.forward * aggroRange);

		// Double Raycast
		RaycastHit l = new RaycastHit();
		RaycastHit r = new RaycastHit();

		bool isL = Physics.Raycast(
			transform.position, Quaternion.Euler(0,-6,0)*transform.forward, out l, aggroRange);
		bool isR = Physics.Raycast(
			transform.position, Quaternion.Euler(0,6,0)*transform.forward, out r, aggroRange);

		float allyDist = 999;
		Transform ally = null;

		// Figure out what to return
		if (isL) {
			if (isAlly(l.transform)) {
				ally = l.transform;
				allyDist = l.distance;
			}
			else {
				separationL = l.distance;
				lName = l.transform.name;
			}
		}
		if (isR) {
			if (isAlly(r.transform) && r.distance < allyDist) {
				allyDist = r.distance;
				ally = r.transform;
			}
			else {
				separationR = r.distance;
				rName = r.transform.name;
			}
		}
		return ally;
	}

	public void setAttackRun() {
		currentStance = Stance.ATTACK;
		currentTask = Task.RUNNING;
	}

	public void die() {
		setIsAlive(false);
		renderer.material = deathColor;
		stats.orcDied();
	}

	protected override void charge() {
		move();
		
		Transform t = aggroCast();
		
		if (t) {
			setTarget(t);
			currentTask = Task.ENGAGING;
		}
	}

	private void move() {
		if (separationL < .5 && separationR < .5) {
			Debug.DrawRay(transform.position, transform.forward, Color.magenta);
			Debug.Log(lName + ", " + rName);
			return;
		}
		else {
			transform.position += transform.forward * speed * Time.deltaTime;
		}
	}
}