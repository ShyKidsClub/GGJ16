using UnityEngine;
using System.Collections;

namespace Incredible
{
	public class FlyingController : MonoBehaviour
	{
		[SerializeField]
		private int playerId = -1;

		[SerializeField]
		private ForceMode2D flyingForceMode = ForceMode2D.Impulse;
		[SerializeField]
		private float flyingAcceleration = 1.0f;
		private Vector2 flyingDir = Vector2.zero;

		private Rigidbody2D playerRigidBody = null;
		private LookAtDirection lookAtDir = null;
		private Rewired.Player player = null;

		void Awake()
		{
			playerRigidBody = transform.parent.GetComponent<Rigidbody2D>();
			lookAtDir = GetComponent<LookAtDirection>();

			player = Rewired.ReInput.players.GetPlayer(playerId);
		}

		void Update()
		{
			Vector3 input = Vector2.zero;
			input.x = player.GetAxis("Flying X");
			input.y = player.GetAxis("Flying Y");

			if (input.sqrMagnitude > 0.01f)
			{
				lookAtDir.SetDirection(input);
			}
			SetDirection(input);

			Flying();
		}

		void Flying()
		{
			playerRigidBody.AddForce(flyingDir * flyingAcceleration, flyingForceMode);
		}

		public void SetDirection(Vector2 direction)
		{
			flyingDir = direction;
		}
	}
}
