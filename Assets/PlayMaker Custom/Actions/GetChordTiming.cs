using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Gets the appropriate Chord index from a Ballard and Sequence using a time.")]
	public class GetChordTiming : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Ballard Index.")]
		public FsmInt ballardCurrent = -1;

		[RequiredField]
		[Tooltip("The Sequence Index.")]
		public FsmInt sequenceCurrent = -1;

		[RequiredField]
		[Tooltip("The Chord Index.")]
		public FsmInt chordCurrent = -1;

		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to be assigned to.")]
		public FsmFloat outTiming = 0.0f;

		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to be assigned to.")]
		public FsmFloat outTimingStart = 0.0f;

		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to be assigned to.")]
		public FsmFloat outTimingStop = 1.0f;

		[Tooltip("Is Updated Every Frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			ballardCurrent		= -1;
			sequenceCurrent		= -1;
			chordCurrent		= -1;
			outTiming			= 0.0f;
			outTimingStart		= 0.0f;
			outTimingStop		= 1.0f;
			isUpdatedEveryFrame = false;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			GetTiming();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			GetTiming();
		}

		private void GetTiming()
		{
			BallardData ballard;
			if (BallardDatabase.Instance.GetBallard(ballardCurrent.Value, out ballard) == false)
			{
				Debug.Log("Ballard '" + ballardCurrent.Value.ToString() 
					+ "' doesn't exist"); 
				return;
			}

			SequenceData sequence;
			if (ballard.GetSequence(sequenceCurrent.Value, out sequence) == false)
			{
				Debug.Log("Ballard '" + ballard.name 
					+ "': Sequence '" + sequenceCurrent.Value.ToString() 
					+ "' doesn't exist!"); 
				return;
			}

			ChordData chord;
			if (sequence.GetChord(chordCurrent.Value, out chord) == false)
			{
				Debug.Log("Ballard '" + ballard.name 
					+ "', Sequence '" + sequenceCurrent.Value.ToString() 
					+ "': Chord '" + chordCurrent.Value 
					+ "' doesn't exist!"); 
				return;
			}

			outTiming.Value			= chord.timingStart;
			outTimingStart.Value	= chord.timingStart;
			outTimingStop.Value		= chord.timingStop;
		}
	}
}
