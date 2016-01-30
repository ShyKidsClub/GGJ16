using UnityEngine;
using UnityEditor;
using System.Collections;

public class BallardDatabaseEditor : EditorWindow
{
	static private BallardDatabase target = null;
	static private BallardDatabaseEditor window = null;

	// Creation of the Asset
	[MenuItem("Assets/Create/Ballard Database")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<BallardDatabase>();
	}

	// Selecting the Asset
	[MenuItem("Window/Ballard Editor")]
	static void ShowWindow()
	{
		window = (BallardDatabaseEditor)EditorWindow.GetWindow(typeof(BallardDatabaseEditor), true, "Ballard Editor");
		target = (BallardDatabase)AssetDatabase.LoadAssetAtPath("Assets/Project/Resources/BallardDatabase.asset", typeof(ScriptableObject));
	}

	void OnGUI()
	{
		if (target == null)
			ShowWindow();

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Add Ballard"))
			{
				target.AddBallard(new BallardData());
			}
			if (GUILayout.Button("Remove Ballard"))
			{
				int index = target.m_ballards.Count - 1;
				target.RemoveBallard(index);
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();

		// iterate through all the ballards
		for (int i = 0; i < target.m_ballards.Count; ++i)
		{
			OnGUI_Ballard(target.m_ballards[i]);
		}
	}

	bool[] isBallardFoldedOut = new bool[256];
	void OnGUI_Ballard(BallardData ballard)
	{
		int index = target.m_ballards.IndexOf(ballard);

		EditorGUILayout.BeginHorizontal();
		{
			isBallardFoldedOut[index] = EditorGUILayout.Foldout(isBallardFoldedOut[index], ballard.name);
			if (GUILayout.Button(" + "
				, EditorStyles.miniButtonLeft
				, GUILayout.MaxWidth(32.0f)))
			{
				int position = target.MoveBallard(index, -1);
				SwitchValue(ref isBallardFoldedOut[index], ref isBallardFoldedOut[position]);
			}
			if (GUILayout.Button(" - "
				, EditorStyles.miniButtonRight
				, GUILayout.MaxWidth(32.0f)))
			{
				int position = target.MoveBallard(index, 1);
				SwitchValue(ref isBallardFoldedOut[index], ref isBallardFoldedOut[position]);
			}
			if (GUILayout.Button("Remove"
				, EditorStyles.miniButtonRight
				, GUILayout.MaxWidth(128.0f)))
			{
				target.RemoveBallard(index);
			}
		}
		EditorGUILayout.EndHorizontal();

		if (isBallardFoldedOut[index] == false)
			return;

		++EditorGUI.indentLevel;
		// Ballard Options
		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.Space((EditorGUI.indentLevel + 1) * 10.0f);
			GUILayout.Label("Name: ", GUILayout.MaxWidth(64.0f));
			ballard.name = EditorGUILayout.TextField(ballard.name);
		}
		EditorGUILayout.EndHorizontal();

		// Ballard Sequences
		ballard.debug_isFoldedOut = EditorGUILayout.Foldout(ballard.debug_isFoldedOut, "Sequences");
		if (ballard.debug_isFoldedOut == true)
		{
			++EditorGUI.indentLevel;
			for (int i = 0; i < ballard.sequences.Count; ++i)
			{
				OnGUI_Sequence(ballard, ballard.sequences[i]);
			}
			--EditorGUI.indentLevel;
		}

		--EditorGUI.indentLevel;
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
	}

	void OnGUI_Sequence(BallardData parent, SequenceData sequence)
	{
		sequence.debug_isFoldedOut = EditorGUILayout.Foldout(sequence.debug_isFoldedOut, sequence.name);
		if (sequence.debug_isFoldedOut == false)
			return;

		++EditorGUI.indentLevel;

		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.Space((EditorGUI.indentLevel + 2) * 10.0f);
			GUILayout.Label("Name: ", GUILayout.MaxWidth(64.0f));
			sequence.name = EditorGUILayout.TextField(sequence.name);
			if (GUILayout.Button("Add Chord"
				, EditorStyles.miniButtonLeft
				, GUILayout.MaxWidth(128.0f)))
			{
				sequence.AddChord(new ChordData());
			}
			if (GUILayout.Button("Remove Chord"
				, EditorStyles.miniButtonRight
				, GUILayout.MaxWidth(128.0f)))
			{
				int i = sequence.chords.Count - 1;
				sequence.RemoveChord(i);
			}
		}
		EditorGUILayout.EndHorizontal();

		for (int i = 0; i < sequence.chords.Count; ++i)
		{
			OnGUI_Chord(sequence, sequence.chords[i]);
		}

		--EditorGUI.indentLevel;
	}

	void OnGUI_Chord(SequenceData parent, ChordData chord)
	{
		int index = parent.chords.IndexOf(chord);

		EditorGUILayout.BeginHorizontal();
		{
			string clipName = (chord.clip != null ? chord.clip.name : "Chord");
			chord.debug_isFoldedOut = EditorGUILayout.Foldout(chord.debug_isFoldedOut, clipName);

			if (GUILayout.Button(" + "
				, EditorStyles.miniButtonLeft
				, GUILayout.MaxWidth(32.0f)))
			{
				int position = parent.MoveChord(index, -1);
				SwitchValue(ref parent.chords[index].debug_isFoldedOut, ref parent.chords[position].debug_isFoldedOut);
			}
			if (GUILayout.Button(" - "
				, EditorStyles.miniButtonRight
				, GUILayout.MaxWidth(32.0f)))
			{
				int position = parent.MoveChord(index, 1);
				SwitchValue(ref parent.chords[index].debug_isFoldedOut, ref parent.chords[position].debug_isFoldedOut);
			}
			if (GUILayout.Button("Remove"
				, EditorStyles.miniButtonRight
				, GUILayout.MaxWidth(128.0f)))
			{
				parent.RemoveChord(index);
			}
		}
		EditorGUILayout.EndHorizontal();
		
		if (chord.debug_isFoldedOut == false)
			return;

		++EditorGUI.indentLevel;

		chord.type = (ChordData.Type)EditorGUILayout.EnumPopup("Type: ", chord.type);
		switch (chord.type)
		{
			case ChordData.Type.PRESS: 
				chord.timingStart = chord.timingStop = EditorGUILayout.DoubleField("Timing: ", chord.timingStop);
				break;
			case ChordData.Type.HOLD:
				chord.timingStart = EditorGUILayout.DoubleField("Timing Start: ", chord.timingStart);
				chord.timingStop = EditorGUILayout.DoubleField("Timing Stop: ", chord.timingStop);
				break;
		}

		chord.clip = (AudioClip)EditorGUILayout.ObjectField("Clip: ", chord.clip, typeof(AudioClip), false);

		--EditorGUI.indentLevel;
	}

	void SwitchValue(ref bool A, ref bool B)
	{
		bool temp = A;
		A = B;
		B = temp;
	}
}