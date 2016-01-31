using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Checks if a float value is within the range of another float.")]
	public class IsFloatInRange : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The float to check.")]
		public FsmFloat value = 0.0f;

		[RequiredField]
		[Tooltip("The target value.")]
		public FsmFloat target = 0.0f;

		[RequiredField]
		[Tooltip("The allowed threshold.")]
		public FsmFloat threshold = 0.0f;

		[Tooltip("Event sent if the Value IS between Float 1 and Float 2")]
		public FsmEvent isTrue = null;

		[Tooltip("Event sent if the Value ISN'T between Float 1 and Float 2")]
		public FsmEvent isFalse = null;

		[Tooltip("Is Updated Every Frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			value				= 0.0f;
			target				= 0.0f;
			threshold			= 0.0f;
			isTrue				= null;
			isFalse				= null;
			isUpdatedEveryFrame = false;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			InRange();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			InRange();
		}

		private void InRange()
		{
			if (target.Value - threshold.Value <= value.Value && value.Value <= target.Value + threshold.Value)
			{
				Fsm.Event(isTrue);
			}

			Fsm.Event(isFalse);
		}

		public override string ErrorCheck()
		{
			if (FsmEvent.IsNullOrEmpty(isTrue) &&
				FsmEvent.IsNullOrEmpty(isFalse))
				return "Action sends no events!";
			return "";
		}
	}
}
