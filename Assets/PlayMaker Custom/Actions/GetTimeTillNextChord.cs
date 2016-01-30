using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Gets the time remaining until the next Chord.")]
	public class GetTimeTillNextChord : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The Ballard Index.")]
		public FsmInt ballardCurrent = -1;

		[RequiredField]
		[Tooltip("The Sequence Index.")]
		public FsmInt sequenceCurrent = -1;

		[RequiredField]
		[Tooltip("The current Ballard time.")]
		public FsmFloat timeCurrent = 0.0f;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Int variable to be assigned to.")]
		public FsmFloat outTimeRemaining = -1;

		[Tooltip("If enabled, it will update every frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			ballardCurrent = -1;
			sequenceCurrent = -1;
			timeCurrent = 0.0f;
			outTimeRemaining = -1;
			isUpdatedEveryFrame = false;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			GetTimeRemaining();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			GetTimeRemaining();
		}

		private void GetTimeRemaining()
		{
			BallardData ballard;
			if (BallardDatabase.Instance.GetBallard(ballardCurrent.Value, out ballard) == false)
				return;

			SequenceData sequence;
			if (ballard.GetSequence(sequenceCurrent.Value, out sequence) == false)
				return;

			for (int i = 0; i < sequence.chords.Count; ++i)
			{
				ChordData chord = sequence.chords[i];

				float timeRemaining = chord.timingStart - timeCurrent.Value;
				if (timeRemaining >= 0.0f)
				{
					outTimeRemaining.Value = timeRemaining;
					return;
				}
			}

			outTimeRemaining.Value = 0.0f;
		}
	}
}
