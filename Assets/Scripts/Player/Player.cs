using UnityEngine;
using System.Collections;

namespace Incredible
{
	enum ControlMode
	{
		AIMING,
		FLYING,
	}

	public class Player : MonoBehaviour
	{
		[SerializeField]
		private GameObject playerPink = null;
		[SerializeField]
		private GameObject playerBlue = null;

		private ControlMode controlPink	= ControlMode.FLYING;
		private ControlMode controlBlue	= ControlMode.AIMING;

		void Awake()
		{
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1) == true)
			{
				SetControlMode(controlBlue, controlPink);
			}
		}

		private void SetControlMode(ControlMode pink, ControlMode blue)
		{
			Vector3 positionPink = playerPink.transform.localPosition;
			Vector3 positionBlue = playerBlue.transform.localPosition;

			if (controlPink == ControlMode.AIMING 
			&& pink == ControlMode.FLYING)
			{
				SwitchControlMap(0, "Aiming", "Flying");
				playerPink.transform.localPosition = Vector3.zero;
				playerPink.transform.localScale = Vector3.one;
			}
			else
			{
				SwitchControlMap(0, "Flying", "Aiming");
				playerPink.transform.localPosition = -positionBlue;
				playerPink.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			}

			if (controlBlue == ControlMode.AIMING 
			&& blue == ControlMode.FLYING)
			{
				SwitchControlMap(1, "Aiming", "Flying");
				playerBlue.transform.localPosition = Vector3.zero;
				playerBlue.transform.localScale = Vector3.one;
			}
			else
			{
				SwitchControlMap(1, "Flying", "Aiming");
				playerBlue.transform.localPosition = -positionPink;
				playerBlue.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
			}

			controlPink = pink;
			controlBlue = blue;
		}

		private void SwitchControlMap(int playerId, string disabledMap, string enabledMap)
		{
			Rewired.Player player = Rewired.ReInput.players.GetPlayer(playerId);
			player.controllers.maps.SetMapsEnabled(false, disabledMap);
			player.controllers.maps.SetMapsEnabled(true, enabledMap);
		}
	}
}
