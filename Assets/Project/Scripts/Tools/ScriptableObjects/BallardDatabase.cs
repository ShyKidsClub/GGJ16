using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BallardData
{
	[HideInInspector]
	public bool debug_isFoldedOut = false;

	public string name = "BALLARD OF AWESOME";
	public double duration = 0.0f;
	public List<SequenceData> sequences = new List<SequenceData>();

	public BallardData()
	{
		sequences.Add(new SequenceData());
		sequences.Add(new SequenceData());
		sequences.Add(new SequenceData());
		sequences.Add(new SequenceData());
	}
}

[System.Serializable]
public class SequenceData
{
	[HideInInspector]
	public bool debug_isFoldedOut = false;

	public string name = "SEQUENCE OF COOL";
	public List<ChordData> chords = new List<ChordData>();

	public SequenceData()
	{
		chords.Add(new ChordData());
		chords.Add(new ChordData());
		chords.Add(new ChordData());
		chords.Add(new ChordData());
	}
}

[System.Serializable]
public class ChordData
{
	[HideInInspector]
	public bool debug_isFoldedOut = false;

	public double timing = 0.0f;
	public AudioClip clip = null;
}

public class BallardDatabase : ScriptableObject
{
	private static BallardDatabase m_instance = null;
	public static BallardDatabase Instance
	{
		get
		{
			if (m_instance == null)
				m_instance = (BallardDatabase)Resources.Load("GameParameters", typeof(BallardDatabase));
			if (m_instance == null)
				Debug.LogError("BallardDatabase doesn't exsist yet! Use the Ballard Editor to create one. ->Window/Ballard Editor");
			return m_instance;
		}
	}

	public List<BallardData> m_ballards = new List<BallardData>();

	public void AddBallard(BallardData ballard)
	{
		m_ballards.Add(ballard);
	}

	public int MoveBallard(int index, int amount)
	{
		if (index < 0 || index >= m_ballards.Count)
			return index;

		BallardData ballard = m_ballards[index];
		return MoveBallard(ballard, amount);
	}

	public int MoveBallard(BallardData ballard, int amount)
	{
		int position = m_ballards.IndexOf(ballard) + amount;
		position = Mathf.Min(m_ballards.Count - 1, position); // don't exceed count
		position = Mathf.Max(0, position); // don't exceed 0

		m_ballards.Remove(ballard);
		m_ballards.Insert(position, ballard);
		return position;
	}

	public void RemoveBallard(BallardData ballard)
	{
		m_ballards.Remove(ballard);
	}

	public void RemoveBallard(int index)
	{
		if (index < 0 || index >= m_ballards.Count)
			return;

		BallardData ballard = m_ballards[index];
		RemoveBallard(ballard);
	}
}
