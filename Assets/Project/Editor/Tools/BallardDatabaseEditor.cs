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
			OnGUI_Ballard(i, target.m_ballards[i]);
		}
	}

	bool[] isBallardFoldedOut = new bool[256];
	void OnGUI_Ballard(int index, BallardData ballard)
	{
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
				OnGUI_Sequence(ballard, ballard.sequences[i], i);
			}
			--EditorGUI.indentLevel;
		}

		--EditorGUI.indentLevel;
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
	}

	void OnGUI_Sequence(BallardData parent, SequenceData sequence, int index)
	{
		sequence.debug_isFoldedOut = EditorGUILayout.Foldout(sequence.debug_isFoldedOut, sequence.name);
		if (sequence.debug_isFoldedOut == false)
			return;

		++EditorGUI.indentLevel;

		EditorGUILayout.BeginHorizontal();

			GUILayout.Space((EditorGUI.indentLevel+2) * 10.0f);
			GUILayout.Label("Name: ", GUILayout.MaxWidth(64.0f));
			sequence.name = EditorGUILayout.TextField(sequence.name);

		EditorGUILayout.EndHorizontal();

		for (int i = 0; i < sequence.chords.Count; ++i)
		{
			OnGUI_Chord(sequence, sequence.chords[i], i);
		}

		--EditorGUI.indentLevel;
	}

	void OnGUI_Chord(SequenceData parent, ChordData chord, int index)
	{
		string clipName = (chord.clip != null ? chord.clip.name : "Clip");
		chord.debug_isFoldedOut = EditorGUILayout.Foldout(chord.debug_isFoldedOut, clipName);
		if (chord.debug_isFoldedOut == false)
			return;

		++EditorGUI.indentLevel;

		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.Space((EditorGUI.indentLevel) * 10.0f);
			chord.timing = EditorGUILayout.DoubleField(chord.timing);
			chord.clip = (AudioClip)EditorGUILayout.ObjectField(chord.clip, typeof(AudioClip), false);
		}
		EditorGUILayout.EndHorizontal();

		--EditorGUI.indentLevel;
	}

	void SwitchValue(ref bool A, ref bool B)
	{
		bool temp = A;
		A = B;
		B = temp;
	}
}