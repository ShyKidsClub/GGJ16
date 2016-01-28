using UnityEngine;
using System.Collections;

namespace Incredible
{
	public class AimingController : MonoBehaviour
	{
		[SerializeField]
		private int playerId = -1;

		[SerializeField]
		private float aimingSpeed = 0.1f;
		private Vector2 aimingDir = Vector2.zero;

		private LookAtDirection lookAtDir = null;
		private Rewired.Player player = null;

		void Awake()
		{
			lookAtDir = GetComponent<LookAtDirection>();

			player = Rewired.ReInput.players.GetPlayer(playerId);
		}

		void Update()
		{
			Vector2 input = Vector2.zero;
			input.x = player.GetAxis("Aiming X");
			input.y = player.GetAxis("Aiming Y");

			if (input.sqrMagnitude > 0.01f)
			{
				lookAtDir.SetDirection(input);
			}
			SetDirection(input);

			Aiming();
		}

		void Aiming()
		{
			if (aimingDir.sqrMagnitude > 0.01f)
			{
				Quaternion currentRot = Quaternion.LookRotation(Vector3.forward, transform.localPosition);
				Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, aimingDir);

				Quaternion rotation = Quaternion.Slerp(currentRot, targetRot, aimingSpeed);
				rotation = rotation * Quaternion.Inverse(currentRot);
				transform.localPosition = rotation * transform.localPosition;
			}
		}

		public void SetDirection(Vector2 direction)
		{
			aimingDir = direction;
		}
	}
}