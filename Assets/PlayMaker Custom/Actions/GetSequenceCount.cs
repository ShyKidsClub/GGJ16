using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Gets the total Sequence Count for a specified Ballard.")]
	public class GetSequenceCount: FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Ballard Index.")]
		public FsmInt ballardCurrent = -1;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Int variable to be assigned to.")]
		public FsmInt outCount = -1;

		[Tooltip("Is Updated Every Frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			outCount.Value = -1;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			GetCount();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			GetCount();
		}

		private void GetCount()
		{
			BallardData ballard;
			if (BallardDatabase.Instance.GetBallard(ballardCurrent.Value, out ballard) == false)
				return;

			outCount.Value = ballard.sequences.Count;
		}
	}
}