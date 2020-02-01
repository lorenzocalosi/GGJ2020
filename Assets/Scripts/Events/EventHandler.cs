﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Event {
	public class EventHandler : Singleton<EventHandler> {
		public Transform display;
		public EventUI eventUI;
		public Image background;
		
		public Event testEvent;

		[ReadOnly]
		public Actor selectedActor;
		[ReadOnly]
		public Solution selectedItem;
		[ReadOnly]
		public Actor currentActor;
		[ShowInInspector]
		private int questCompleted;
		private const int MaxQuest = 3;
		[ShowInInspector] private List<Actor> goodEndingActors = new List<Actor>();
		
		private void Start() {
			//Debug
			//ShowEvent(testEvent);
		}

		private void Update()
		{
			if (questCompleted > MaxQuest)
			{
				//end game state
			}
		}

		public void ShowEvent(Event newEvent) {
			currentActor = newEvent.actor;
			background.sprite = newEvent.background;
			eventUI.gameObject.SetActive(true);
			eventUI.ShowEventUI(newEvent);
			selectedActor = null;
			selectedItem = null;
		}

		public void ExitEvent() {
			CameraHandler.Instance.FadeOut(() =>
			{
				eventUI.gameObject.SetActive(false);
				eventUI.characterAnimator.SetTrigger("Reset");
				PlayerMovement.Instance.canMove = true;
			});
		}

		public void Evaluate() {
			if (currentActor.relatedActor == selectedActor && currentActor.relatedSolution == selectedItem) {
				EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Success, EventManager.Instance.progress[currentActor].tries);
				EventManager.Instance.progress[currentActor.relatedActor] = new EventManager.ActorStateTries(EventManager.ActorState.Success, EventManager.Instance.progress[currentActor.relatedActor].tries);
				eventUI.ShowNonePage();
				eventUI.characterAnimator.SetBool("Happy", true);
				//
				goodEndingActors.Add(currentActor);
				goodEndingActors.Add(currentActor.relatedActor);
				//
				eventUI.inventory.actors.Remove(currentActor);
				eventUI.inventory.actors.Remove(currentActor.relatedActor);
				eventUI.inventory.items.Remove(currentActor.relatedSolution);
				//
				questCompleted++;
			}
			else {
				if (EventManager.Instance.progress[currentActor].tries + 1 >= EventManager.Instance.triesLimit) {
					eventUI.ShowNonePage();
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Failure, EventManager.Instance.progress[currentActor].tries + 1);
					eventUI.characterAnimator.SetTrigger("Angry");
					eventUI.inventory.actors.Remove(currentActor);
					//
					questCompleted++;
				}
				else {
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Pending, EventManager.Instance.progress[currentActor].tries + 1);
					eventUI.characterAnimator.SetTrigger("Confused");
					eventUI.ShowMainPage();
				}
				selectedActor = null;
				selectedItem = null;
			}
		}
	}
}