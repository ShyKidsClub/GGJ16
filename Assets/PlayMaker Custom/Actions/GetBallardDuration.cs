using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Gets the duartion of a Ballard from the game.")]
	public class GetBallardDuration : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Ballard Index.")]
		public FsmInt ballardCurrent = -1;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to be assigned to.")]
		public FsmFloat outDuration = -1;

		[Tooltip("Is Updated Every Frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			outDuration.Value = -1.0f;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			GetDuration();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			GetDuration();
		}

		private void GetDuration()
		{
			BallardData ballard;
			if (BallardDatabase.Instance.GetBallard(ballardCurrent.Value, out ballard) == false)
				return;

			outDuration.Value = (float)ballard.duration;
		}
	}
}