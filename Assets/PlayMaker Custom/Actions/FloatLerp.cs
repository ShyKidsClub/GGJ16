using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Gets the value between float1 and float2 at the delta.")]
	public class FloatLerp : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Float 1.")]
		public FsmFloat float1 = 0.0f;

		[RequiredField]
		[Tooltip("Float 2.")]
		public FsmFloat float2 = 1.0f;

		[RequiredField]
		[Tooltip("Delta.")]
		public FsmFloat delta = 0.0f;

		[RequiredField]
		[Tooltip("Delta.")]
		public FsmFloat outValue = 0.0f;

		[Tooltip("If enabled, it will update every frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			float1				= 0.0f;
			float2				= 1.0f;
			delta				= 0.0f;
			outValue			= 0.0f;
			isUpdatedEveryFrame = false;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			Lerp();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			Lerp();
		}

		private void Lerp()
		{
			outValue.Value = Mathf.Lerp(float1.Value, float2.Value, delta.Value);
		}
	}
}