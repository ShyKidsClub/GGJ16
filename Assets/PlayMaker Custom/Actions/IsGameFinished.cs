using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameLogic)]
	[Tooltip("")]
	public class IsGameFinished : FsmStateAction
	{
		[RequiredField]
		[Tooltip("")]
		public FsmFloat scoreTarget = 0.0f;

		[RequiredField]
		public FsmFloat scoreA = 0.0f;
		[RequiredField]
		public FsmFloat scoreB = 0.0f;
		[RequiredField]
		public FsmFloat scoreC = 0.0f;
		[RequiredField]
		public FsmFloat scoreD = 0.0f;

		[Tooltip("")]
		public FsmEvent isTrue = null;

		[Tooltip("")]
		public FsmEvent isFalse = null;

		public override void OnUpdate()
		{
			if (scoreA.Value < scoreTarget.Value
			|| scoreB.Value < scoreTarget.Value
			|| scoreC.Value < scoreTarget.Value
			|| scoreD.Value < scoreTarget.Value)
			{
				Fsm.Event(isFalse);
				return;
			}

			Fsm.Event(isTrue);
		}
	}
}
