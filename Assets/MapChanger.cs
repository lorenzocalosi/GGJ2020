﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : Singleton<MapChanger>
{
	public int currentMap = 1;

	public MapHotSpot map1Hotspot;
	public Animator map1Plane;
	public MapHotSpot map2Hotspot;
	public Animator map2Plane;

	public Animator animator;

	public void ChangeMap() {
		if (currentMap == 1) {
			currentMap = 2;
			map1Hotspot.ChangeMap();
			animator.enabled = true;
			map2Plane.SetTrigger("PlaneFromHawaii");
		}
		else {
			currentMap = 1;
			map2Hotspot.ChangeMap();
			animator.enabled = true;
			map1Plane.SetTrigger("PlaneFromHawaii");
		}
	}

	public void PlacePlayer() {
		animator.enabled = false;
		PlayerMovement.Instance.GetComponentInChildren<SpriteRenderer>().enabled = true;
		PlayerMovement.Instance.transform.position = currentMap == 1 ? map2Hotspot.teleportPosition : map1Hotspot.teleportPosition;
		PlayerMovement.Instance.canMove = true;
	}
}
