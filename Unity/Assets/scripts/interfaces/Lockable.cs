using UnityEngine;
using System.Collections;

public interface Lockable {
	void onLock();
	void onRelease();
	void onFire();
	void onStab();
	void onArrow();
	bool isItAlive();
}
