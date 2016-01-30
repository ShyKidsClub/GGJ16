using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Gets the appropriate Chord index from a Ballard and Sequence using a time.")]
	public class GetChordAtTime : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Ballard Index.")]
		public FsmInt ballardCurrent = -1;

		[RequiredField]
		[Tooltip("The Sequence Index.")]
		public FsmInt sequenceCurrent = -1;

		[RequiredField]
		[Tooltip("The Time.")]
		public FsmFloat timeCurrent = 0.0f;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Int variable to be assigned to.")]
		public FsmInt outChordIndex = -1;

		[RequiredField]
		[Tooltip("A tolerance value for when getting a chord (becareful about making this too big).")]
		public FsmFloat tolerance = 0;

		[Tooltip("If enabled, it will get the closest chord.")]
		public bool isClosestEnabled;

		[Tooltip("If enabled, it will update every frame.")]
		public bool isUpdatedEveryFrame;

		public override void Reset()
		{
			ballardCurrent		= -1;
			sequenceCurrent		= -1;
			timeCurrent			= 0.0f;
			outChordIndex		= -1;
			tolerance			= 0.0f;
			isUpdatedEveryFrame = false;
		}

		public override void OnEnter()
		{
			if (isUpdatedEveryFrame == true)
				return;

			if (isClosestEnabled == false)
				GetChordExact();
			else
				GetChordClosest();
		}

		public override void OnUpdate()
		{
			if (isUpdatedEveryFrame == false)
				return;

			if (isClosestEnabled == false)
				GetChordExact();
			else
				GetChordClosest();
		}

		private void GetChordExact()
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

				// is it too early?
				if (timeCurrent.Value < chord.timingStart - tolerance.Value)
					continue;
				// is it too late?
				if (timeCurrent.Value > chord.timingStop + tolerance.Value)
					continue;

				// juuuuuust right
				outChordIndex = i;
				return;
			}
		}

		private void GetChordClosest()
		{
			BallardData ballard;
			if (BallardDatabase.Instance.GetBallard(ballardCurrent.Value, out ballard) == false)
				return;

			SequenceData sequence;
			if (ballard.GetSequence(sequenceCurrent.Value, out sequence) == false)
				return;

			int closestIndex = -1;
			float closestTime = float.PositiveInfinity;
			for (int i = 0; i < sequence.chords.Count; ++i)
			{
				ChordData chord = sequence.chords[i];

				// we want to the value that is closest to the current time
				// for a PRESS chord, timingStart and timingStop are identical
				float lower = chord.timingStart - (tolerance.Value/2);
				lower = Mathf.Abs(timeCurrent.Value - lower);
				float upper = chord.timingStop + (tolerance.Value/2);
				upper = Mathf.Abs(timeCurrent.Value - upper);
				float smallest = Mathf.Min(lower, upper);

				// if we're smaller than what we've already found
				// we become the new smallest
				if (smallest < closestTime)
				{
					closestIndex = i;
					closestTime = smallest;
				}
			}

			outChordIndex.Value = closestIndex;
		}
	}
}
