using UnityEngine;
using System.Collections.Generic;

namespace Rewired.Integration.PlayMaker {

    using HutongGames.PlayMaker;
    using HutongGames.PlayMaker.Actions;
    using HutongGames.Extensions;
    using HutongGames.Utility;

    #region Base Classes

    public abstract class RewiredPlayerFsmStateAction : FsmStateAction {
        
        [RequiredField]
        [Tooltip("The Rewired Player Id. To use the System Player, enter any value < 0 or 9999999.")]
        public FsmInt playerId;

        public bool everyFrame = true;

        protected Player Player {
            get {
                if(playerId.Value < 0 || playerId.Value == Rewired.Consts.systemPlayerId) return ReInput.players.GetSystemPlayer();
                return ReInput.players.GetPlayer(playerId.Value);
            }
        }

        public override void Reset() {
            base.Reset();
            playerId = 0;
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

    public abstract class RewiredPlayerNoEFFsmStateAction : FsmStateAction {

        [RequiredField]
        [Tooltip("The Rewired Player Id. To use the System Player, enter any value < 0 or 9999999.")]
        public FsmInt playerId;

        public bool everyFrame = false;

        protected Player Player {
            get {
                if(playerId.Value < 0 || playerId.Value == Rewired.Consts.systemPlayerId) return ReInput.players.GetSystemPlayer();
                return ReInput.players.GetPlayer(playerId.Value);
            }
        }

        public override void Reset() {
            base.Reset();
            playerId = 0;
        }

        // Code that runs on entering the state.
        public override void OnEnter() {
            DoUpdate();

            if(!everyFrame)
                Finish();
        }

        public override void OnUpdate() {
            DoUpdate();
        }

        public abstract void DoUpdate();

    }

    public abstract class RewiredPlayerActionFsmStateAction : RewiredPlayerFsmStateAction {

        [RequiredField]
        [Tooltip("The Action name string. Must match Action name exactly in the Rewired Input Manager.")]
        public FsmString actionName;

        public override void Reset() {
            base.Reset();
            actionName = string.Empty;
        }
    }

    public abstract class RewiredPlayerActionGetFloatFsmStateAction : RewiredPlayerActionFsmStateAction {

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        [Tooltip("The comparison operation to perform.")]
        public CompareOperation compareOperation;

        [Tooltip("The value to which to compare the returned value.")]
        public FsmFloat compareToValue;

        [Tooltip("Compare using the absolute values of the two operands.")]
        public FsmBool compareAbsValues;

        [Tooltip("Event to send when the result of comparison returns true.")]
        public FsmEvent isTrueEvent;

        [Tooltip("Event to send when the result of comparison returns false.")]
        public FsmEvent isFalseEvent;

        [Tooltip("Event to send when the value changes.")]
        public FsmEvent valueChangedEvent;

        public override void Reset() {
            base.Reset();
            storeValue = 0f;
            compareOperation = CompareOperation.None;
            compareToValue = 0f;
            compareAbsValues = false;
            isTrueEvent = null;
            isFalseEvent = null;
            valueChangedEvent = null;
        }

        protected void UpdateStoreValue(float newValue) {
            if(newValue != storeValue.Value) { // value changed
                // Store new value
                storeValue.Value = newValue;
                if(valueChangedEvent != null) Fsm.Event(valueChangedEvent); // send value changed event
            }

            // Compare values
            if(compareOperation != CompareOperation.None) {
                bool result = Compare(newValue);
                if(result) {
                    // send true event
                    if(isTrueEvent != null) Fsm.Event(isTrueEvent);
                } else {
                    // send false event
                    if(isFalseEvent != null) Fsm.Event(isFalseEvent);
                }
            }
        }

        private bool Compare(float value) {
            if(compareOperation == CompareOperation.None) return true;

            float val1, val2;
            if(compareAbsValues.Value) {
                val1 = Mathf.Abs(value);
                val2 = Mathf.Abs(compareToValue.Value);
            } else {
                val1 = value;
                val2 = compareToValue.Value;
            }

            switch(compareOperation) {
                case CompareOperation.LessThan:
                    return val1 < val2;
                case CompareOperation.LessThanOrEqualTo:
                    return val1 <= val2;
                case CompareOperation.EqualTo:
                    return val1 == val2;
                case CompareOperation.NotEqualTo:
                    return val1 != val2;
                case CompareOperation.GreaterThanOrEqualTo:
                    return val1 >= val2;
                case CompareOperation.GreaterThan:
                    return val1 > val2;
            }

            return false;
        }
    }

    public abstract class RewiredPlayerActionGetBoolFsmStateAction : RewiredPlayerActionFsmStateAction {

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        [Tooltip("Event to send when bool value is true.")]
        public FsmEvent isTrueEvent;

        [Tooltip("Event to send when bool value is false.")]
        public FsmEvent isFalseEvent;

        [Tooltip("Event to send when the value changes.")]
        public FsmEvent valueChangedEvent;

        public override void Reset() {
            base.Reset();
            storeValue = false;
            isTrueEvent = null;
            isFalseEvent = null;
            valueChangedEvent = null;
        }

        protected void UpdateStoreValue(bool newValue) {
            if(newValue != storeValue.Value) { // value changed
                // Store new value
                storeValue.Value = newValue;
                if(valueChangedEvent != null) Fsm.Event(valueChangedEvent); // send value changed event
            }

            // Send true event
            if(newValue) {
                if(isTrueEvent != null) Fsm.Event(isTrueEvent);
            } else {
                if(isFalseEvent != null) Fsm.Event(isFalseEvent);
            }
        }
    }

    public abstract class RewiredPlayerGetBoolFsmStateAction : RewiredPlayerFsmStateAction {

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        [Tooltip("Event to send when bool value is true.")]
        public FsmEvent isTrueEvent;

        [Tooltip("Event to send when bool value is false.")]
        public FsmEvent isFalseEvent;

        [Tooltip("Event to send when the value changes.")]
        public FsmEvent valueChangedEvent;

        public override void Reset() {
            base.Reset();
            storeValue = false;
            isTrueEvent = null;
            isFalseEvent = null;
            valueChangedEvent = null;
        }

        protected void UpdateStoreValue(bool newValue) {
            if(newValue != storeValue.Value) { // value changed
                // Store new value
                storeValue.Value = newValue;
                if(valueChangedEvent != null) Fsm.Event(valueChangedEvent); // send value changed event
            }

            // Send true event
            if(newValue) {
                if(isTrueEvent != null) Fsm.Event(isTrueEvent);
            } else {
                if(isFalseEvent != null) Fsm.Event(isFalseEvent);
            }
        }
    }

    public abstract class RewiredPlayerActionGetAxis2DFsmStateAction : RewiredPlayerFsmStateAction {

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a Vector2 variable.")]
        public FsmVector2 storeValue;

        [RequiredField]
        [Tooltip("The Action name string for the X axis value. Must match Action name exactly in the Rewired Input Manager.")]
        public FsmString actionNameX;

        [RequiredField]
        [Tooltip("The Action name string for the Y axis value. Must match Action name exactly in the Rewired Input Manager.")]
        public FsmString actionNameY;

        [Tooltip("Event to send when the value changes.")]
        public FsmEvent valueChangedEvent;

        public override void Reset() {
            base.Reset();
            storeValue = Vector2.zero;
            actionNameX = string.Empty;
            actionNameY = string.Empty;
        }

        protected void UpdateStoreValue(Vector2 newValue) {
            if(newValue != storeValue.Value) { // value changed
                // Store new value
                storeValue.Value = newValue;
                if(valueChangedEvent != null) Fsm.Event(valueChangedEvent); // send value changed event
            }
        }
    }

    public abstract class RewiredPlayerInputBehaviorFsmStateAction : RewiredPlayerNoEFFsmStateAction {

        [RequiredField]
        [Tooltip("Input Behavior name string.")]
        public FsmString behaviorName;

        public InputBehavior Behavior {
            get {
                InputBehavior behavior = Player.controllers.maps.GetInputBehavior(behaviorName.Value);
                if(behavior == null) {
                    Debug.LogError("Input Behavior \"" + behaviorName.Value + "\" does not exist!");
                }
                return behavior;
            }
        }

        public override void Reset() {
            base.Reset();
            behaviorName = string.Empty;
        }

    }

    #endregion

    #region Get Button

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button held state of an Action. This will return TRUE as long as the button is held. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetButton : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButton(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button just pressed state of an Action. This will only return TRUE only on the first frame the button is pressed or for the duration of the Button Down Buffer time limit if set in the Input Behavior assigned to this Action. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetButtonDown : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonDown(actionName.Value));
        }

    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the button just released state for an Action. This will only return TRUE for the first frame the button is released. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetButtonUp : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonUp(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button held state of an Action during the previous frame.")]
    public class RewiredPlayerGetButtonPrev : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonPrev(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button double pressed and held state of an Action. This will return TRUE after a double press and the button is then held. The double press speed is set in the Input Behavior assigned to the Action.")]
    public class RewiredPlayerGetButtonDoublePressHold : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonDoublePressHold(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button double pressed state of an Action. This will return TRUE only on the first frame of a double press. The double press speed is set in the Input Behavior assigned to the Action.")]
    public class RewiredPlayerGetButtonDoublePressDown : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonDoublePressDown(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that a button has been continuously held down. Returns 0 if the button is not currently pressed.")]
    public class RewiredPlayerGetButtonTimePressed : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonTimePressed(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that a button has not been pressed. Returns 0 if the button is currently pressed.")]
    public class RewiredPlayerGetButtonTimeUnpressed : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetButtonTimeUnpressed(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button held state of all Actions. This will return TRUE as long as any button is held. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyButton : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyButton());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button just pressed state of all Actions. This will only return TRUE only on the first frame any button is pressed or for the duration of the Button Down Buffer time limit if set in the Input Behavior assigned to the Action. This will return TRUE each time any button is pressed even if others are being held down. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyButtonDown : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyButtonDown());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the button just released state for all Actions. This will only return TRUE for the first frame the button is released. This will return TRUE each time any button is released even if others are being held down. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyButtonUp : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyButtonUp());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the button held state of an any Action during the previous frame. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyButtonPrev : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyButtonPrev());
        }
    }

    #endregion

    #region Get Negative Button

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button held state of an Action. This will return TRUE as long as the negative button is held. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetNegativeButton : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButton(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button just pressed state of an Action. This will only return TRUE only on the first frame the negative button is pressed or for the duration of the Button Down Buffer time limit if set in the Input Behavior assigned to this Action. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetNegativeButtonDown : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonDown(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the negative button just released state for an Action. This will only return TRUE for the first frame the negative button is released. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetNegativeButtonUp : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonUp(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button held state of an Action during the previous frame.")]
    public class RewiredPlayerGetNegativeButtonPrev : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonPrev(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button double pressed state of an Action. This will return TRUE only on the first frame of a double press. The double press speed is set in the Input Behavior assigned to the Action.")]
    public class RewiredPlayerGetNegativeButtonDoublePressDown : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonDoublePressDown(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button double pressed and held state of an Action. This will return TRUE after a double press and the negative button is then held. The double press speed is set in the Input Behavior assigned to the Action.")]
    public class RewiredPlayerGetNegativeButtonDoublePressHold : RewiredPlayerActionGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonDoublePressHold(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that a negative button has been continuously held down. Returns 0 if the negative button is not currently pressed.")]
    public class RewiredPlayerGetNegativeButtonTimePressed : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonTimePressed(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that a negative button has not been pressed. Returns 0 if the negative button is currently pressed.")]
    public class RewiredPlayerGetNegativeButtonTimeUnpressed : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetNegativeButtonTimeUnpressed(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button held state of all Actions. This will return TRUE as long as any negative button is held. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyNegativeButton : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyNegativeButton());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button just pressed state of all Actions. This will only return TRUE only on the first frame any negative button is pressed or for the duration of the Button Down Buffer time limit if set in the Input Behavior assigned to the Action. This will return TRUE each time any negative button is pressed even if others are being held down. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyNegativeButtonDown : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyNegativeButtonDown());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the negative button just released state for all Actions. This will only return TRUE for the first frame the negative button is released. This will return TRUE each time any negative button is released even if others are being held down. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyNegativeButtonUp : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyNegativeButtonUp());
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the negative button held state of an any Action during the previous frame. This also applies to axes being used as buttons.")]
    public class RewiredPlayerGetAnyNegativeButtonPrev : RewiredPlayerGetBoolFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAnyNegativeButtonPrev());
        }
    }

    #endregion

    #region Get Axis

    [ActionCategory("Rewired")]
    [Tooltip("Gets the axis value of an Action.")]
    public class RewiredPlayerGetAxis : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxis(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the axis value of an Action during the previous frame.")]
    public class RewiredPlayerGetAxisPrev : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisPrev(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the raw axis value of an Action. The raw value excludes any digital axis simulation modification by the Input Behavior assigned to this Action. This raw value is modified by deadzone and axis calibration settings in the controller. To get truly raw values, you must get the raw value directly from the Controller element.")]
    public class RewiredPlayerGetAxisRaw : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisRaw(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the raw axis value of an Action during the previous frame. The raw value excludes any digital axis simulation modification by the Input Behavior assigned to this Action. This raw value is modified by deadzone and axis calibration settings in the controller. To get truly raw values, you must get the raw value directly from the Controller element.")]
    public class RewiredPlayerGetAxisRawPrev : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisRawPrev(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that an axis has been continuously active as calculated from the raw value. Returns 0 if the axis is not currently active.")]
    public class RewiredPlayerGetAxisTimeActive : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisTimeActive(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that an axis has been inactive as calculated from the raw value. Returns 0 if the axis is currently active.")]
    public class RewiredPlayerGetAxisTimeInactive : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisTimeInactive(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that an axis has been continuously active as calculated from the raw value. Returns 0 if the axis is not currently active.")]
    public class RewiredPlayerGetAxisRawTimeActive : RewiredPlayerActionGetFloatFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisRawTimeActive(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the length of time in seconds that an axis has been inactive as calculated from the raw value. Returns 0 if the axis is currently active.")]
    public class RewiredPlayerGetAxisRawTimeInactive : RewiredPlayerActionGetFloatFsmStateAction {
        
        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxisRawTimeInactive(actionName.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the axis value of two Actions.")]
    public class RewiredPlayerGetAxis2d : RewiredPlayerActionGetAxis2DFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxis2D(actionNameX.Value, actionNameY.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the axis value of two Actions during the previous frame. ")]
    public class RewiredPlayerGetAxis2dPrev : RewiredPlayerActionGetAxis2DFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxis2DPrev(actionNameX.Value, actionNameY.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the raw axis value of two Actions. The raw value excludes any digital axis simulation modification by the Input Behavior assigned to this Action. This raw value is modified by deadzone and axis calibration settings in the controller. To get truly raw values, you must get the raw value directly from the Controller element.")]
    public class RewiredPlayerGetAxis2dRaw : RewiredPlayerActionGetAxis2DFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxis2DRaw(actionNameX.Value, actionNameY.Value));
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Gets the raw axis value of two Actions during the previous frame. The raw value excludes any digital axis simulation modification by the Input Behavior assigned to this Action. This raw value is modified by deadzone and axis calibration settings in the controller. To get truly raw values, you must get the raw value directly from the Controller element.")]
    public class RewiredPlayerGetAxis2dRawPrev : RewiredPlayerActionGetAxis2DFsmStateAction {

        public override void DoUpdate() {
            UpdateStoreValue(Player.GetAxis2DRawPrev(actionNameX.Value, actionNameY.Value));
        }
    }

    #endregion

    #region Vibration

    [ActionCategory("Rewired")]
    [Tooltip("Sets vibration level for a motor at a specified index on controllers assigned to this Player.")]
    public class RewiredPlayerSetAllControllerVibration : RewiredPlayerNoEFFsmStateAction {

        [Tooltip("Sets the vibration motor level. [0 - 1]")]
        public FsmFloat motorLevel;

        [Tooltip("The index of the motor to vibrate.")]
        public FsmInt motorIndex;

        [Tooltip("Stop all other motors except this one.")]
        public FsmBool stopOtherMotors;

        public override void Reset() {
            base.Reset();
            motorLevel = 0.0f;
            motorIndex = 0;
            stopOtherMotors = false;
        }

        public override void DoUpdate() {
            if(motorIndex.Value < 0) return;
            motorLevel.Value = Mathf.Clamp01(motorLevel.Value);

            int joystickCount = Player.controllers.joystickCount;
            IList<Joystick> joysticks = Player.controllers.Joysticks;
            for(int i = 0; i < joystickCount; i++) {
                Joystick joystick = joysticks[i];
                if(!joystick.supportsVibration) continue;
                if(motorIndex.Value >= joystick.vibrationMotorCount) continue;
                joystick.SetVibration(motorIndex.Value, motorLevel.Value, stopOtherMotors.Value);
            }
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Stops vibration on all controllers assigned to this Player.")]
    public class RewiredPlayerStopAllControllerVibration : RewiredPlayerNoEFFsmStateAction {

        public override void DoUpdate() {
            int joystickCount = Player.controllers.joystickCount;
            IList<Joystick> joysticks = Player.controllers.Joysticks;
            for(int i = 0; i < joystickCount; i++) {
                Joystick joystick = joysticks[i];
                if(!joystick.supportsVibration) continue;
                joystick.StopVibration();
            }
        }
    }

    #endregion

    #region Player Properties

    [ActionCategory("Rewired")]
    [Tooltip("The descriptive name of the Player.")]
    public class RewiredPlayerGetName : RewiredPlayerNoEFFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a string variable.")]
        public FsmString storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.name;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("The scripting name of the Player.")]
    public class RewiredPlayerGetDescriptiveName : RewiredPlayerNoEFFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a string variable.")]
        public FsmString storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.descriptiveName;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Is this Player currently playing?")]
    public class RewiredPlayerGetIsPlaying : RewiredPlayerNoEFFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.isPlaying;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Sets whether this Player currently playing.")]
    public class RewiredPlayerSetIsPlaying : RewiredPlayerNoEFFsmStateAction {

        [RequiredField]
        [Tooltip("Sets the boolean value.")]
        public FsmBool value;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            Player.isPlaying = value.Value;
        }
    }

    #endregion

    #region ControllerHelper

    [ActionCategory("Rewired")]
    [Tooltip("Is the mouse assigned to this Player?")]
    public class RewiredPlayerGetHasMouse : RewiredPlayerFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.controllers.hasMouse;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Sets whether the mouse is assigned to this Player.")]
    public class RewiredPlayerSetHasMouse : RewiredPlayerNoEFFsmStateAction {

        [RequiredField]
        [Tooltip("Sets the boolean value.")]
        public FsmBool value;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            Player.controllers.hasMouse = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("The number of joysticks assigned to this Player.")]
    public class RewiredPlayerGetJoystickCount : RewiredPlayerFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.controllers.joystickCount;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("The number of Custom Controllers assigned to this Player.")]
    public class RewiredPlayerGetCustomControllerCount : RewiredPlayerFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
        }

        public override void DoUpdate() {
            storeValue.Value = Player.controllers.customControllerCount;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Unassign controllers from this Player.")]
    public class RewiredPlayerRemoveControllers : RewiredPlayerNoEFFsmStateAction {

        [Tooltip("Remove only controllers of a certain type. If false, all assignable controllers will be removed.")]
        public FsmBool byControllerType;

        [Tooltip("Controller type to remove from Player. Not used if Clear By Controller Type is false.")]
        public ControllerType controllerType = ControllerType.Joystick;

        public override void Reset() {
            base.Reset();
            byControllerType = false;
            controllerType = ControllerType.Joystick;
        }

        public override void DoUpdate() {
            if(byControllerType.Value) {
                Player.controllers.ClearControllersOfType(controllerType);
            } else {
                Player.controllers.ClearAllControllers();
            }
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get the last controller type that contributed input through the Player.")]
    public class RewiredPlayerGetLastActiveControllerType : RewiredPlayerFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
        }

        public override void DoUpdate() {
            Controller controller = Player.controllers.GetLastActiveController();
            if(controller == null) {
                storeValue.Value = 0;
                return;
            }
            storeValue.Value = (int)controller.type;
        }
    }

    #endregion

    #region MapHelper

    [ActionCategory("Rewired")]
    [Tooltip("Set the enabled state in all maps in a particular category and/or layout.")]
    public class RewiredPlayerSetControllerMapsEnabled : RewiredPlayerNoEFFsmStateAction {

        [RequiredField]
        [Tooltip("Set the enabled state.")]
        public FsmBool enabledState;

        [RequiredField]
        [Tooltip("The Controller Map category name.")]
        public FsmString categoryName;

        [Tooltip("The Controller Map layout name. [Optional]")]
        public FsmString layoutName;

        [Tooltip("Set the enabled state of maps for a particular controller type.")]
        public FsmBool byControllerType;

        [Tooltip("Set the enabled state of maps for a particular controller type. Not used if Set By Controller Type is false.")]
        public ControllerType controllerType = ControllerType.Joystick;

        public override void Reset() {
            base.Reset();
            categoryName = string.Empty;
            layoutName = string.Empty;
            byControllerType = false;
            controllerType = ControllerType.Joystick;
        }

        public override void DoUpdate() {
            SetMapsEnabled();
        }

        private void SetMapsEnabled() {
            if(byControllerType.Value) {
                SetMapsEnabled(enabledState.Value, controllerType, categoryName.Value, layoutName.Value);
            } else {
                SetMapsEnabled(enabledState.Value, categoryName.Value, layoutName.Value);
            }
        }

        private void SetMapsEnabled(bool state, ControllerType controllerType, string categoryName, string layoutName) {
            if(string.IsNullOrEmpty(layoutName)) Player.controllers.maps.SetMapsEnabled(state, controllerType, categoryName);
            else Player.controllers.maps.SetMapsEnabled(state, controllerType, categoryName, layoutName);
        }

        private void SetMapsEnabled(bool state, string categoryName, string layoutName) {
            if(string.IsNullOrEmpty(layoutName)) Player.controllers.maps.SetMapsEnabled(state, categoryName);
            else Player.controllers.maps.SetMapsEnabled(state, categoryName, layoutName);
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the enabled state in all controller maps.")]
    public class RewiredPlayerSetAllControllerMapsEnabled : RewiredPlayerNoEFFsmStateAction {

        [RequiredField]
        [Tooltip("Set the enabled state.")]
        public FsmBool enabledState;

        [Tooltip("Set the enabled state of maps for a particular controller type.")]
        public FsmBool byControllerType;

        [Tooltip("Set the enabled state of maps for a particular controller type. Not used if Set By Controller Type is false.")]
        public ControllerType controllerType = ControllerType.Joystick;

        public override void Reset() {
            base.Reset();
            byControllerType = false;
            controllerType = ControllerType.Joystick;
        }

        public override void DoUpdate() {
            SetMapsEnabled();
        }

        private void SetMapsEnabled() {
            if(byControllerType.Value) {
                Player.controllers.maps.SetAllMapsEnabled(enabledState.Value, controllerType);
            } else {
                Player.controllers.maps.SetAllMapsEnabled(enabledState.Value);
            }
        }
    }

    #endregion

    #region Input Behaviors

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.digitalAxisGravity for a Player.")]
    public class RewiredPlayerInputBehaviorGetDigitalAxisGravity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.digitalAxisGravity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.digitalAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorGetDigitalAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.digitalAxisSensitivity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.digitalAxisSnap for a Player.")]
    public class RewiredPlayerInputBehaviorGetDigitalAxisSnap : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = false;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.digitalAxisSnap;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.digitalAxisInstantReverse for a Player.")]
    public class RewiredPlayerInputBehaviorGetDigitalAxisInstantReverse : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a boolean variable.")]
        public FsmBool storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = false;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.digitalAxisInstantReverse;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.joystickAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorGetJoystickAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.joystickAxisSensitivity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.customControllerAxisGravity for a Player.")]
    public class RewiredPlayerInputBehaviorGetCustomControllerAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.customControllerAxisSensitivity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.mouseXYAxisMode for a Player.")]
    public class RewiredPlayerInputBehaviorGetMouseXYAxisMode : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = (int)Behavior.mouseXYAxisMode;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.mouseXYAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorGetMouseXYAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.mouseXYAxisSensitivity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.mouseXYAxisDeltaCalc for a Player.")]
    public class RewiredPlayerInputBehaviorGetMouseXYAxisDeltaCalc : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = (int)Behavior.mouseXYAxisDeltaCalc;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.mouseOtherAxisMode for a Player.")]
    public class RewiredPlayerInputBehaviorGetMouseOtherAxisMode : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = (int)Behavior.mouseOtherAxisMode;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.mouseOtherAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorGetMouseOtherAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.mouseOtherAxisSensitivity;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.buttonDeadZone for a Player.")]
    public class RewiredPlayerInputBehaviorGetButtonDeadZone : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.buttonDeadZone;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.buttonDoublePressSpeed for a Player.")]
    public class RewiredPlayerInputBehaviorGetButtonDoublePressSpeed : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.buttonDoublePressSpeed;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Get and store the value of InputBehavior.buttonDownBuffer for a Player.")]
    public class RewiredPlayerInputBehaviorGetButtonDownBuffer : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField, UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a float variable.")]
        public FsmFloat storeValue;

        public override void Reset() {
            base.Reset();
            storeValue = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            storeValue.Value = Behavior.buttonDownBuffer;
        }
    }

    // Set

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.digitalAxisGravity for a Player.")]
    public class RewiredPlayerInputBehaviorSetDigitalAxisGravity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.digitalAxisGravity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.digitalAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorSetDigitalAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.digitalAxisSensitivity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.digitalAxisSnap for a Player.")]
    public class RewiredPlayerInputBehaviorSetDigitalAxisSnap : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmBool value;

        public override void Reset() {
            base.Reset();
            value = false;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.digitalAxisSnap = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.digitalAxisInstantReverse for a Player.")]
    public class RewiredPlayerInputBehaviorSetDigitalAxisInstantReverse : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmBool value;

        public override void Reset() {
            base.Reset();
            value = false;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.digitalAxisInstantReverse = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.joystickAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorSetJoystickAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.joystickAxisSensitivity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.customControllerAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorSetCustomControllerAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.customControllerAxisSensitivity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.mouseXYAxisMode for a Player.")]
    public class RewiredPlayerInputBehaviorSetMouseXYAxisMode : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt value;

        public override void Reset() {
            base.Reset();
            value = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.mouseXYAxisMode = (MouseXYAxisMode)value.Value;
        }
    }


    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.mouseXYAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorSetMouseXYAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.mouseXYAxisSensitivity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.mouseXYAxisDeltaCalc for a Player.")]
    public class RewiredPlayerInputBehaviorSetMouseXYAxisDeltaCalc : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt value;

        public override void Reset() {
            base.Reset();
            value = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.mouseXYAxisDeltaCalc = (MouseXYAxisDeltaCalc)value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.mouseOtherAxisMode for a Player.")]
    public class RewiredPlayerInputBehaviorSetMouseOtherAxisMode : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Store the result in an int variable.")]
        public FsmInt value;

        public override void Reset() {
            base.Reset();
            value = 0;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.mouseOtherAxisMode = (MouseOtherAxisMode)value.Value;
        }
    }


    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.mouseOtherAxisSensitivity for a Player.")]
    public class RewiredPlayerInputBehaviorSetMouseOtherAxisSensitivity : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.mouseOtherAxisSensitivity = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.buttonDeadZone for a Player.")]
    public class RewiredPlayerInputBehaviorSetButtonDeadZone : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.buttonDeadZone = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.buttonDoublePressSpeed for a Player.")]
    public class RewiredPlayerInputBehaviorSetButtonDoublePressSpeed : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.buttonDoublePressSpeed = value.Value;
        }
    }

    [ActionCategory("Rewired")]
    [Tooltip("Set the value of InputBehavior.buttonDownBuffer for a Player.")]
    public class RewiredPlayerInputBehaviorSetButtonDownBuffer : RewiredPlayerInputBehaviorFsmStateAction {

        [RequiredField]
        [Tooltip("Set the value.")]
        public FsmFloat value;

        public override void Reset() {
            base.Reset();
            value = 0.0f;
        }

        public override void DoUpdate() {
            if(Behavior == null) return;
            Behavior.buttonDownBuffer = value.Value;
        }
    }

    #endregion
}