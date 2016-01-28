using UnityEngine;
using System.Collections;

namespace Incredible
{
	public class LookAtDirection : MonoBehaviour
	{
		[SerializeField]
		private GameObject playerHead = null;
		[SerializeField]
		private GameObject playerFace = null;

		[SerializeField]
		private float headOffset = 1.0f;
		[SerializeField]
		private float faceOffset = 1.0f;

		[SerializeField]
		private float lookingSpeed = 0.1f;
		private Vector2 lookingDir = Vector2.zero;

		void Awake()
		{
			if (playerHead == null) playerHead = transform.Find("Head").gameObject;
			if (playerFace == null) playerFace = transform.Find("Face").gameObject;
		}

		void Update()
		{
			Looking();
		}

		void Looking()
		{
			if (playerHead == null) { Debug.LogError("Missing reference to body party", playerHead); return; }
			if (playerFace == null) { Debug.LogError("Missing reference to body party", playerFace); return; }

			if (lookingDir.sqrMagnitude > 0.01f)
			{
				playerHead.transform.localPosition = Vector2.Lerp(playerHead.transform.localPosition, lookingDir * headOffset, lookingSpeed);
				playerFace.transform.localPosition = Vector2.Lerp(playerFace.transform.localPosition, lookingDir * faceOffset, lookingSpeed);
			}
		}

		public void SetDirection(Vector2 direction)
		{
			Debug.Log(direction);
			lookingDir = direction;
		}
	}
}
