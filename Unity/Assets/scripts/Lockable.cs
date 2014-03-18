using UnityEngine;
using System.Collections;

public interface Lockable {
	void onLock();
	void onFire();
	bool isDead();
}
