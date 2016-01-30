using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayChord : MonoBehaviour
{
	[SerializeField]
	private string playerId = "Player1";

	private BallardData ballardCurrent = null;
	private SequenceData sequenceCurrent = null;
	private ChordData chordCurrent = null;

	private Rewired.Player player = null;
	private AudioSource audioSource = null;

	void Awake ()
	{
		audioSource = GetComponent<AudioSource>();
		player = Rewired.ReInput.players.GetPlayer(playerId);

		ballardCurrent = BallardDatabase.Instance.m_ballards[0];
		sequenceCurrent = ballardCurrent.sequences[0];
		chordCurrent = sequenceCurrent.chords[0];
	}
	
	void Update ()
	{
		if (player.GetButtonDown("Play"))
		{
			Debug.Log(playerId);
			audioSource.PlayOneShot(chordCurrent.clip);
		}
	}
}
