using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("")]
	public class PlayMechanimTrigger : FsmStateAction
	{
		[RequiredField]
		[Tooltip("")]
		public FsmString trigger = "";

		public override void OnEnter()
		{
			Animator controller = Owner.GetComponent<Animator>();
			if (controller != null)
			{
				controller.SetTrigger(trigger.Value);
			}
		}
	}
}
