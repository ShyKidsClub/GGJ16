using UnityEngine;
using System.Collections.Generic;

namespace Rewired.Integration.PlayMaker {

    using HutongGames.PlayMaker;
    using HutongGames.PlayMaker.Actions;
    using HutongGames.Extensions;
    using HutongGames.Utility;

    public abstract class GetIntFsmStateAction : FsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeValue;

        public bool everyFrame = true;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
            everyFrame = true;
        }

        public override void OnEnter() {
            DoUpdate();
            
            if(!everyFrame) Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public abstract void DoUpdate();
    }

    public abstract class GetFloatFsmStateAction : FsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public bool everyFrame = true;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
            everyFrame = true;
        }

        public override void OnEnter() {
            DoUpdate();

            if(!everyFrame) Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public abstract void DoUpdate();
    }

    public abstract class GetBoolFsmStateAction : FsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        public bool everyFrame = true;

        public override void Reset() {
            base.Reset();
            storeValue = false;
            everyFrame = true;
        }

        public override void OnEnter() {
            DoUpdate();

            if(!everyFrame) Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public abstract void DoUpdate();
    }

    #region Players

    [ActionCategory("Rewired")]
    [Tooltip("Count of Players excluding system player.")]
    public class RewiredGetPlayerCount : GetIntFsmStateAction {

        public override void DoUpdate() {
            storeValue.Value = ReInput.players.playerCount;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Count of all players including system player.")]
    public class RewiredGetAllPlayersCount : GetIntFsmStateAction {

        public override void DoUpdate() {
            storeValue.Value = ReInput.players.allPlayerCount;
        }
    }

    #endregion

    #region Controllers

    [ActionCategory("Rewired")]
    [Tooltip("The number of joysticks currently connected.")]
    public class RewiredGetJoystickCount : GetIntFsmStateAction {

        public override void DoUpdate() {
            storeValue.Value = ReInput.controllers.joystickCount;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("The number of custom controllers.")]
    public class RewiredGetCustomControllerCount : GetIntFsmStateAction {
        
        public override void DoUpdate() {
            storeValue.Value = ReInput.controllers.customControllerCount;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the last controller type that produced input.")]
    public class RewiredGetLastActiveControllerType : GetIntFsmStateAction {

        public override void DoUpdate() {
            Controller controller = ReInput.controllers.GetLastActiveController();
            if(controller == null) {
                storeValue.Value = 0;
                return;
            }
            storeValue.Value = (int)controller.type;
        }
    }

    #endregion

    #region Time

    [ActionCategory("Rewired")]
    [Tooltip("Current unscaled time since start of the game. Always use this when doing current time comparisons for button and axis active/inactive times instead of Time.time or Time.unscaledTime.")]
    public class RewiredGetUnscaledTime : GetFloatFsmStateAction {

        public override void DoUpdate() {
            storeValue.Value = ReInput.time.unscaledTime;
        }
    }

    #endregion

    #region Events

    [ActionCategory("Rewired")]
    [Tooltip("Event triggered when a controller is conected.")]
    public class RewiredControllerConnectedEvent : FsmStateAction {
        
        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a string variable.")]
        public FsmString storeControllerName;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeControllerId = -1;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeControllerType = 0;

        [Tooltip("Send event when a controller is connected.")]
        public FsmEvent sendEvent;

        public bool everyFrame = true;
        private bool hasEvent = false;

        public override void Awake() {
            base.Awake();
            ReInput.ControllerConnectedEvent += OnControllerConnected;
        }

        public override void Reset() {
            base.Reset();
            storeControllerName = string.Empty;
            storeControllerId = -1;
            storeControllerType = 0;
            everyFrame = true;
            hasEvent = false;
        }

        public override void OnEnter() {
            DoUpdate();

            if(!everyFrame) Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public void DoUpdate() {
            if(hasEvent) {
                if(sendEvent != null) Fsm.Event(sendEvent);
                hasEvent = false;
            }
        }

        private void OnControllerConnected(ControllerStatusChangedEventArgs args) {
            hasEvent = true;
            storeControllerName.Value = args.name;
            storeControllerId.Value = args.controllerId;
            storeControllerType.Value = (int)args.controllerType;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Event triggered after a controller is disconnected.")]
    public class RewiredControllerDisconnectedEvent : FsmStateAction {

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a string variable.")]
        public FsmString storeControllerName;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeControllerId = -1;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeControllerType = 0;

        [Tooltip("Send event when a controller is disconnected.")]
        public FsmEvent sendEvent;

        public bool everyFrame = true;
        private bool hasEvent = false;

        public override void Awake() {
            base.Awake();
            ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        }

        public override void Reset() {
            base.Reset();
            storeControllerName = string.Empty;
            storeControllerId = -1;
            storeControllerType = 0;
            everyFrame = true;
            hasEvent = false;
        }

        public override void OnEnter() {
            DoUpdate();

            if(!everyFrame) Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public void DoUpdate() {
            if(hasEvent) {
                if(sendEvent != null) Fsm.Event(sendEvent);
                hasEvent = false;
            }
        }

        private void OnControllerDisconnected(ControllerStatusChangedEventArgs args) {
            hasEvent = true;
            storeControllerName.Value = args.name;
            storeControllerId.Value = args.controllerId;
            storeControllerType.Value = (int)args.controllerType;
        }
    }

    #endregion
}
