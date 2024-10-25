// GENERATED AUTOMATICALLY FROM 'Assets/DChild/Objects/Characters/Player/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""8ebe72dc-eba9-4499-8e97-dcb3ed219753"",
            ""actions"": [
                {
                    ""name"": ""HorizontalInput"",
                    ""type"": ""Button"",
                    ""id"": ""daeebb6b-2ea7-4b19-862c-2a28d9eb156d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalInput"",
                    ""type"": ""Button"",
                    ""id"": ""559b0e2f-47dc-49fa-ba03-b4918ab4149f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""2a23e5e6-034f-4eb7-9047-a8052e9a00fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""c45691ef-fb79-4a6a-a15e-dc3729121b18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""95527310-afc7-43e1-911b-2332c9c4751a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slash"",
                    ""type"": ""Button"",
                    ""id"": ""9d4ab075-e02d-4431-b650-e1d29cbed247"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EarthShaker"",
                    ""type"": ""Button"",
                    ""id"": ""f157fa1c-dc90-4a05-ae1a-65432abdf41c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SlashHeld"",
                    ""type"": ""Button"",
                    ""id"": ""b7a8ddbf-5a58-4bdb-adef-92a19b4f16d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SlashReleased"",
                    ""type"": ""Button"",
                    ""id"": ""d630c732-3d64-4496-b72c-87f2e8972780"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""c981b6a5-3e67-4bf4-9916-d9599928b8c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Levitate"",
                    ""type"": ""Button"",
                    ""id"": ""36e5d574-ceaa-48a8-8a99-bead8721c918"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Whip"",
                    ""type"": ""Button"",
                    ""id"": ""cd3e6366-7546-4939-bd0b-8a0c5b36b152"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""56d2adb8-e9d0-450f-8df9-020441bace75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ProjectileThrow"",
                    ""type"": ""Button"",
                    ""id"": ""45bdfdf7-5e75-4839-be13-6dc77f575e8d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickItemUse"",
                    ""type"": ""Button"",
                    ""id"": ""677553d6-20ed-4e6d-82bb-7a2a5f0fb77c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""QuickItemCycle"",
                    ""type"": ""Button"",
                    ""id"": ""0fa95fc1-904f-4304-99ae-1bbdea430345"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StoreOpen"",
                    ""type"": ""Button"",
                    ""id"": ""620e2f13-ed4d-4faa-9f19-83fd271c6bbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShadowMorph"",
                    ""type"": ""Button"",
                    ""id"": ""407d129f-98d0-432f-90b4-022e5f7eb20c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ControllerCursorHorizontalInput"",
                    ""type"": ""Button"",
                    ""id"": ""129a9721-9dfe-42a8-927a-109744f805be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ControllerCursorVerticalInput"",
                    ""type"": ""Button"",
                    ""id"": ""e079bcc3-05ab-4b2d-8377-9f923a8c44d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""93e311c7-a8f4-4ee7-85c5-180b21f3845f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f8c8d89d-c5c6-4e1a-8f9a-211db00633d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Store"",
                    ""type"": ""Button"",
                    ""id"": ""e058a4d8-59f4-4a82-b0a2-85e8900bf141"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""PC"",
                    ""id"": ""9ac86c67-2d0f-44e6-ad87-a0ab77906285"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""374a898c-a2d9-465d-a55f-980bc3088231"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a0534955-4ea2-46f8-b1d4-a1f357c9b16f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""64b5d659-dfd5-4b4f-ba3c-56f967629ec8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f8e37b1b-e7cc-425f-88aa-5e23028dc12c"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""41e6b199-5817-45d1-bb6f-244e3b194a91"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""66b409bf-a1c8-4350-828d-eb3c14a52bf1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0a090c09-c32e-459b-af6a-4b1cec927f38"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4cf705cc-9d57-47a7-9cdd-39d9e813eba5"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bda03e02-9d8b-4045-a9a0-eb25c5bd64c7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""296d31e7-ea05-481c-bbe0-521f329b4b44"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8078715-d9bf-475e-acf7-5753eebf0ca0"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3934d723-e12c-4b59-92df-79a35981d2f2"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7f7f0b1-6865-4048-8c2d-8666031d1582"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4af4b426-cf54-468c-b877-2fe1c6095669"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6917b8b5-7fa8-4c54-9f5c-3648ee158061"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cb6d3f5-2d14-4a21-9ca2-0d85ee9abbae"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""738a5561-b0f5-4ac6-af76-12e3d711dd80"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""8d09bcc6-2cf1-452c-be25-5a7f42b99acc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""57c23099-4fd3-4221-b7b4-21e376156482"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""076582a9-a73a-4dd2-8d45-26a0aabd402f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""1b894a11-b54d-4c50-8f22-2101a43525c2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""695397a4-e1d6-4bc8-ab37-405ecf710f41"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""de318550-488a-400e-98b4-d6eafb6fa70a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""3f57e50f-7777-4fbd-b60a-6b73f5b9374d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d363459f-d397-4aa4-aaa7-833948da1b8f"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8974fc8d-ed07-4ed9-bc74-5cdfa76880fc"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""49c3bf7e-acb0-4aa6-9a8f-08f473576dee"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Slash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""246dcd27-4502-419d-b360-4dc3704fcbdf"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Slash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79a180ea-b10a-4aec-ac5e-a8cf718ec065"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Slash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""396b4b4c-a7bf-4985-8ae3-032b019ef2e6"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""591c33ae-3fdd-4aa2-b9f3-12f41c67cf00"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""5ce2cd19-6377-4021-82d6-9b464fde0915"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""1d9b22b4-9dc4-4198-97ac-943272bdb736"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""3a66f45d-df0b-4f38-b718-e1a1fceb8c33"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""adff64df-9187-45f6-b843-10c7f3888e6f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""d8f64fd7-108f-40eb-88f9-7afbdab5e310"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""d4009af0-9692-444d-b73d-35b3ff125a44"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""aa8fb548-6a88-4c84-8f7a-9568534deea8"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5abad6c5-be96-4713-bd49-cb97c329fb8e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SlashHeld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""420d2ee9-4a31-4da5-a181-23867d6be014"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SlashHeld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62e62b4c-773a-4b02-9295-d3c7170db832"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""SlashHeld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29924936-5015-43b4-8bea-2362b7ff0ee4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb610d18-5df2-4d26-956a-44c1f302e1ac"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18904431-ca17-4d7d-bd19-e25a4f08deda"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SlashReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdecf389-5d29-4dea-972b-422cebe26f5b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SlashReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f6308bc-4a3d-48f3-873a-2493813e904e"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""SlashReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d38a314-5fc3-4084-835b-30be5ba72acb"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cb2564e-a4b9-4c32-b2b1-c453b0029cdb"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""574dd7ca-70d4-4cd7-b2f9-daf41db80542"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Whip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69b58a3a-3617-47fb-a7fa-ec069e74159c"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Whip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a60632ac-7dd2-4b4b-a2ea-59c0186dcb3f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e08702bd-1a38-4775-ae18-a97315bb0870"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e430af7-8e71-47b8-a47f-6efe9cb0376a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ProjectileThrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af6e8ded-2fbd-4ee4-82c8-8ff0a6d94c93"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ProjectileThrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2431c23-5039-4e63-8668-80500091367b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""QuickItemUse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94699ef3-a33b-4643-a020-3f80d3bad930"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""QuickItemUse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""de7b048c-fce5-47b2-b8dd-45ac5bfd5934"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e5c2e4f9-1b70-4a9f-b004-842431038c84"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8e1e4bf0-4c3b-40a0-8087-d5143ba0208b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""1ca05952-8c4a-4a22-ad6d-4b62ac2003b9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""fd42632c-7b16-4dca-a88b-9d20e0cc4706"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6daf965c-27cc-4880-98a3-2c579fc47184"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""QuickItemCycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9d9129ef-4910-4d66-8c95-b800d75991eb"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""StoreOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""872205e8-6626-47b0-b0d5-620faba0d0dd"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""StoreOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf276c84-55d4-4d9f-8dcd-d16bbb948c4f"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ShadowMorph"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92be224b-717d-4bac-8bef-4c9ef3f2a305"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShadowMorph"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""300dbc7d-42b0-41ec-b786-c583799f9d5b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""314b8c88-2b17-4c71-8b71-48aba52c8187"",
                    ""path"": ""<XInputController>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6b615219-3221-4967-80d2-0c72269146f0"",
                    ""path"": ""<XInputController>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""2c06d2fb-2c86-478b-8574-56f9d0a51b7f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""efd328c5-a647-4c16-814d-7a28a70d2c62"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1050b717-26a6-453b-a6b9-16d1a0e1e000"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ControllerCursorHorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""b55b8592-51b2-45c8-9a27-fe001da75ef8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""460a7f61-a45f-407a-b8d2-cca3070e1f95"",
                    ""path"": ""<XInputController>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""cb504dee-99ef-4fc7-913a-b2ddf1021c1b"",
                    ""path"": ""<XInputController>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""e01227aa-16ad-4000-a36c-f199d490542c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""aaac14ec-fde4-4fff-afa6-90d53571d00e"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a36d7e99-d589-4f4a-bc8d-6d271fe963da"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ControllerCursorVerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""08afc6a3-5662-46d5-8599-b1357a76a4a3"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0f438c2-13af-4478-82fa-26fde2e2aaf0"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ed84032-9951-419d-86f4-4d435f95e1e3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00c637d9-b565-4a15-b73c-9001545b2b82"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6def886f-81a0-490b-9b83-a87dd998285d"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Store"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""386a20eb-5e4e-4293-8f45-32199e24ac57"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Store"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""b4e8da2f-0e27-4bec-9225-b621a24afdef"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""def7b3aa-40ac-431c-b933-f335a39ff958"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""f7be1133-b71e-4406-a76b-2fa76934225a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""7977d6a9-c4db-4ebe-bc72-113a95812fc0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4697399e-60dc-460e-88c7-26499c8357d2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""06bf917c-5da3-41e6-9638-a1655a4b46a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5ca4f6c9-f76c-4aa7-ad2f-570515ac9a9f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0e9d60da-acf5-48d5-a9ba-15aab590df12"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7ecb4010-142b-4425-8f7f-807147a61950"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""18e687e8-5e00-491a-bd3f-da83ff75cdda"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""09b1fcff-d788-449c-a06c-62bb03f042e6"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""bd7fefae-8dcd-4ac8-a6b1-5d5897ac1953"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4a9e2a9e-1a37-4456-8fba-c31752feeb04"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""909cf157-025c-4276-8a83-735e095350d9"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f252291d-61a6-4ba4-9c98-8db985210e25"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7fdfde1a-d182-4d1f-88a6-9b7564889463"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5ceda6e4-632e-40d4-a07d-37b5e836140b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""abc2e206-9eb7-42df-8422-b29081337c81"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""238571b5-a9c4-4a97-a73e-92459d3b5efc"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""75207e53-d1a6-4686-bd98-68f10274b7f7"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""43ebbc16-0a89-4d52-93c7-f3d860c9a1b7"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""ee9ab429-3ebe-4827-981b-2ae49df7e9f7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9be3b8d7-0033-416a-adee-67389bc4e246"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ea9f2baa-4a87-4d2e-9dd1-de66dac1c3df"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7da7a12d-5ddf-40d7-a047-3006181865f5"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e1ee4508-19fb-4fe2-bc4d-2e66ee0dd156"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""d5d8f302-fa83-431e-a466-dcb799878e00"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""49d228a9-943b-4118-b8ea-229e4baba947"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""269ddb46-ff00-4976-a78d-20d6af184860"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b2a187c7-b64e-4c7c-8f35-e151616b06da"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eff69ce1-be68-44d4-955a-1a49a75d721f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b405685a-c00b-46c4-a3f2-69cf3d9618b3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f173eab6-f9a7-47b9-8f72-5345d6ebca61"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4be61a57-c9d3-4cbf-a233-fbd31f26e0f5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1af00239-aabc-4c91-8490-650d2caff1ae"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0ef384a4-e4c8-4c2e-96c1-0b3b6898ccfa"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49d1a86a-f260-4bd6-8dc5-8238d726bdf3"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""630ce0fc-55d9-49ad-b6a9-d9f690d4a939"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ed3a347-5f57-4b9b-a413-aee6fe89d0d8"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e8e80ff-d13a-4fd3-af62-35ca462f55da"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66271bcf-d52b-4bf2-89c1-832ae4c6c6df"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edd8910e-79d3-44f7-bed6-cc8320903c57"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfd9ab57-e9d3-496b-a326-a85b950bd028"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3e0a7b8-0c63-4452-96ac-f6cfa2282d9d"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19abb8b3-025e-44bc-b816-2a14c9a0f810"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bd14393-914f-4dbc-8461-b61d5ccf7003"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13d7cebc-7428-4763-86f7-dd755ed89124"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""530e5daf-33fa-41fc-bf18-a20396f87c9b"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e062567-901f-4239-9432-620e7428cf98"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""506672bd-767d-4a75-89f1-07019d20aa3a"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5596c6a-2f88-454d-b434-9005029374e0"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Minimap"",
            ""id"": ""d608406e-b19f-4076-8bb4-d1c347f848f5"",
            ""actions"": [
                {
                    ""name"": ""HorizontalInput"",
                    ""type"": ""Button"",
                    ""id"": ""14ca7357-0bb4-466b-92a4-6b0f18fb6755"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalInput"",
                    ""type"": ""Button"",
                    ""id"": ""9036a914-78ce-4922-8fe5-4a134abc4da8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""fc6ba452-5eec-43b4-975c-57ec38b51caa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""PC"",
                    ""id"": ""741ac221-d7db-411b-964c-3ed76a18352e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""457a1524-1736-44a7-9c39-ccc837863e39"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""246f9b07-adbc-430d-881c-7f6c99f49b58"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""47b809f8-ac62-4906-baf1-2f3381efdecf"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""88df554d-9d4f-42b7-b6d6-b86f5bb56713"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3c0dc175-5a53-433e-9c80-9f92a23c0822"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""XBOX"",
                    ""id"": ""e86267e0-0808-4b1e-9086-f8e53401723e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""df8c2e49-bdb6-4c03-afb2-e7674a5b66e4"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9934196b-99a5-41ae-abfe-346c73a0e310"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PC"",
                    ""id"": ""f9b94557-2573-472c-920e-1944ca987c68"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d9c67345-6a35-40c7-826f-21fad58ccd76"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0cfd0507-84be-42e5-b470-1b982a97953a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""616ce9f3-3dab-4276-9f9c-3a5aff3da72f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3f81c7df-8d6b-44fa-96f4-77d358003651"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ebec30c8-a18a-4085-a079-f13868453eec"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""XBOX"",
                    ""id"": ""7ecff44b-f3a1-435a-afff-3c1f6f5ed6dd"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ef59dde7-4ced-41c7-8f20-5d8998d6b9e1"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""91a196b7-3602-4d82-99a7-869fd67ddf60"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d3d68158-bae8-4e62-a65d-27cdbfe86864"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PS4"",
            ""bindingGroup"": ""PS4"",
            ""devices"": []
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_HorizontalInput = m_Gameplay.FindAction("HorizontalInput", throwIfNotFound: true);
        m_Gameplay_VerticalInput = m_Gameplay.FindAction("VerticalInput", throwIfNotFound: true);
        m_Gameplay_Crouch = m_Gameplay.FindAction("Crouch", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Slash = m_Gameplay.FindAction("Slash", throwIfNotFound: true);
        m_Gameplay_EarthShaker = m_Gameplay.FindAction("EarthShaker", throwIfNotFound: true);
        m_Gameplay_SlashHeld = m_Gameplay.FindAction("SlashHeld", throwIfNotFound: true);
        m_Gameplay_SlashReleased = m_Gameplay.FindAction("SlashReleased", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_Levitate = m_Gameplay.FindAction("Levitate", throwIfNotFound: true);
        m_Gameplay_Whip = m_Gameplay.FindAction("Whip", throwIfNotFound: true);
        m_Gameplay_Grab = m_Gameplay.FindAction("Grab", throwIfNotFound: true);
        m_Gameplay_ProjectileThrow = m_Gameplay.FindAction("ProjectileThrow", throwIfNotFound: true);
        m_Gameplay_QuickItemUse = m_Gameplay.FindAction("QuickItemUse", throwIfNotFound: true);
        m_Gameplay_QuickItemCycle = m_Gameplay.FindAction("QuickItemCycle", throwIfNotFound: true);
        m_Gameplay_StoreOpen = m_Gameplay.FindAction("StoreOpen", throwIfNotFound: true);
        m_Gameplay_ShadowMorph = m_Gameplay.FindAction("ShadowMorph", throwIfNotFound: true);
        m_Gameplay_ControllerCursorHorizontalInput = m_Gameplay.FindAction("ControllerCursorHorizontalInput", throwIfNotFound: true);
        m_Gameplay_ControllerCursorVerticalInput = m_Gameplay.FindAction("ControllerCursorVerticalInput", throwIfNotFound: true);
        m_Gameplay_Block = m_Gameplay.FindAction("Block", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_Store = m_Gameplay.FindAction("Store", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        // Minimap
        m_Minimap = asset.FindActionMap("Minimap", throwIfNotFound: true);
        m_Minimap_HorizontalInput = m_Minimap.FindAction("HorizontalInput", throwIfNotFound: true);
        m_Minimap_VerticalInput = m_Minimap.FindAction("VerticalInput", throwIfNotFound: true);
        m_Minimap_Interact = m_Minimap.FindAction("Interact", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_HorizontalInput;
    private readonly InputAction m_Gameplay_VerticalInput;
    private readonly InputAction m_Gameplay_Crouch;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Slash;
    private readonly InputAction m_Gameplay_EarthShaker;
    private readonly InputAction m_Gameplay_SlashHeld;
    private readonly InputAction m_Gameplay_SlashReleased;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_Levitate;
    private readonly InputAction m_Gameplay_Whip;
    private readonly InputAction m_Gameplay_Grab;
    private readonly InputAction m_Gameplay_ProjectileThrow;
    private readonly InputAction m_Gameplay_QuickItemUse;
    private readonly InputAction m_Gameplay_QuickItemCycle;
    private readonly InputAction m_Gameplay_StoreOpen;
    private readonly InputAction m_Gameplay_ShadowMorph;
    private readonly InputAction m_Gameplay_ControllerCursorHorizontalInput;
    private readonly InputAction m_Gameplay_ControllerCursorVerticalInput;
    private readonly InputAction m_Gameplay_Block;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_Store;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalInput => m_Wrapper.m_Gameplay_HorizontalInput;
        public InputAction @VerticalInput => m_Wrapper.m_Gameplay_VerticalInput;
        public InputAction @Crouch => m_Wrapper.m_Gameplay_Crouch;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Slash => m_Wrapper.m_Gameplay_Slash;
        public InputAction @EarthShaker => m_Wrapper.m_Gameplay_EarthShaker;
        public InputAction @SlashHeld => m_Wrapper.m_Gameplay_SlashHeld;
        public InputAction @SlashReleased => m_Wrapper.m_Gameplay_SlashReleased;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @Levitate => m_Wrapper.m_Gameplay_Levitate;
        public InputAction @Whip => m_Wrapper.m_Gameplay_Whip;
        public InputAction @Grab => m_Wrapper.m_Gameplay_Grab;
        public InputAction @ProjectileThrow => m_Wrapper.m_Gameplay_ProjectileThrow;
        public InputAction @QuickItemUse => m_Wrapper.m_Gameplay_QuickItemUse;
        public InputAction @QuickItemCycle => m_Wrapper.m_Gameplay_QuickItemCycle;
        public InputAction @StoreOpen => m_Wrapper.m_Gameplay_StoreOpen;
        public InputAction @ShadowMorph => m_Wrapper.m_Gameplay_ShadowMorph;
        public InputAction @ControllerCursorHorizontalInput => m_Wrapper.m_Gameplay_ControllerCursorHorizontalInput;
        public InputAction @ControllerCursorVerticalInput => m_Wrapper.m_Gameplay_ControllerCursorVerticalInput;
        public InputAction @Block => m_Wrapper.m_Gameplay_Block;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @Store => m_Wrapper.m_Gameplay_Store;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @HorizontalInput.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontalInput;
                @HorizontalInput.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontalInput;
                @HorizontalInput.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontalInput;
                @VerticalInput.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVerticalInput;
                @VerticalInput.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVerticalInput;
                @VerticalInput.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVerticalInput;
                @Crouch.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCrouch;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Slash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlash;
                @Slash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlash;
                @Slash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlash;
                @EarthShaker.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEarthShaker;
                @EarthShaker.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEarthShaker;
                @EarthShaker.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEarthShaker;
                @SlashHeld.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashHeld;
                @SlashHeld.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashHeld;
                @SlashHeld.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashHeld;
                @SlashReleased.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashReleased;
                @SlashReleased.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashReleased;
                @SlashReleased.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSlashReleased;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Levitate.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLevitate;
                @Levitate.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLevitate;
                @Levitate.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLevitate;
                @Whip.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWhip;
                @Whip.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWhip;
                @Whip.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWhip;
                @Grab.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGrab;
                @Grab.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGrab;
                @Grab.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGrab;
                @ProjectileThrow.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnProjectileThrow;
                @ProjectileThrow.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnProjectileThrow;
                @ProjectileThrow.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnProjectileThrow;
                @QuickItemUse.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemUse.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemUse.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemCycle.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @QuickItemCycle.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @QuickItemCycle.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @StoreOpen.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @StoreOpen.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @StoreOpen.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @ShadowMorph.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShadowMorph;
                @ShadowMorph.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShadowMorph;
                @ShadowMorph.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShadowMorph;
                @ControllerCursorHorizontalInput.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorHorizontalInput;
                @ControllerCursorHorizontalInput.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorHorizontalInput;
                @ControllerCursorHorizontalInput.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorHorizontalInput;
                @ControllerCursorVerticalInput.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorVerticalInput;
                @ControllerCursorVerticalInput.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorVerticalInput;
                @ControllerCursorVerticalInput.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnControllerCursorVerticalInput;
                @Block.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBlock;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Store.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStore;
                @Store.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStore;
                @Store.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStore;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalInput.started += instance.OnHorizontalInput;
                @HorizontalInput.performed += instance.OnHorizontalInput;
                @HorizontalInput.canceled += instance.OnHorizontalInput;
                @VerticalInput.started += instance.OnVerticalInput;
                @VerticalInput.performed += instance.OnVerticalInput;
                @VerticalInput.canceled += instance.OnVerticalInput;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Slash.started += instance.OnSlash;
                @Slash.performed += instance.OnSlash;
                @Slash.canceled += instance.OnSlash;
                @EarthShaker.started += instance.OnEarthShaker;
                @EarthShaker.performed += instance.OnEarthShaker;
                @EarthShaker.canceled += instance.OnEarthShaker;
                @SlashHeld.started += instance.OnSlashHeld;
                @SlashHeld.performed += instance.OnSlashHeld;
                @SlashHeld.canceled += instance.OnSlashHeld;
                @SlashReleased.started += instance.OnSlashReleased;
                @SlashReleased.performed += instance.OnSlashReleased;
                @SlashReleased.canceled += instance.OnSlashReleased;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Levitate.started += instance.OnLevitate;
                @Levitate.performed += instance.OnLevitate;
                @Levitate.canceled += instance.OnLevitate;
                @Whip.started += instance.OnWhip;
                @Whip.performed += instance.OnWhip;
                @Whip.canceled += instance.OnWhip;
                @Grab.started += instance.OnGrab;
                @Grab.performed += instance.OnGrab;
                @Grab.canceled += instance.OnGrab;
                @ProjectileThrow.started += instance.OnProjectileThrow;
                @ProjectileThrow.performed += instance.OnProjectileThrow;
                @ProjectileThrow.canceled += instance.OnProjectileThrow;
                @QuickItemUse.started += instance.OnQuickItemUse;
                @QuickItemUse.performed += instance.OnQuickItemUse;
                @QuickItemUse.canceled += instance.OnQuickItemUse;
                @QuickItemCycle.started += instance.OnQuickItemCycle;
                @QuickItemCycle.performed += instance.OnQuickItemCycle;
                @QuickItemCycle.canceled += instance.OnQuickItemCycle;
                @StoreOpen.started += instance.OnStoreOpen;
                @StoreOpen.performed += instance.OnStoreOpen;
                @StoreOpen.canceled += instance.OnStoreOpen;
                @ShadowMorph.started += instance.OnShadowMorph;
                @ShadowMorph.performed += instance.OnShadowMorph;
                @ShadowMorph.canceled += instance.OnShadowMorph;
                @ControllerCursorHorizontalInput.started += instance.OnControllerCursorHorizontalInput;
                @ControllerCursorHorizontalInput.performed += instance.OnControllerCursorHorizontalInput;
                @ControllerCursorHorizontalInput.canceled += instance.OnControllerCursorHorizontalInput;
                @ControllerCursorVerticalInput.started += instance.OnControllerCursorVerticalInput;
                @ControllerCursorVerticalInput.performed += instance.OnControllerCursorVerticalInput;
                @ControllerCursorVerticalInput.canceled += instance.OnControllerCursorVerticalInput;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Store.started += instance.OnStore;
                @Store.performed += instance.OnStore;
                @Store.canceled += instance.OnStore;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Minimap
    private readonly InputActionMap m_Minimap;
    private IMinimapActions m_MinimapActionsCallbackInterface;
    private readonly InputAction m_Minimap_HorizontalInput;
    private readonly InputAction m_Minimap_VerticalInput;
    private readonly InputAction m_Minimap_Interact;
    public struct MinimapActions
    {
        private @PlayerControls m_Wrapper;
        public MinimapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalInput => m_Wrapper.m_Minimap_HorizontalInput;
        public InputAction @VerticalInput => m_Wrapper.m_Minimap_VerticalInput;
        public InputAction @Interact => m_Wrapper.m_Minimap_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Minimap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MinimapActions set) { return set.Get(); }
        public void SetCallbacks(IMinimapActions instance)
        {
            if (m_Wrapper.m_MinimapActionsCallbackInterface != null)
            {
                @HorizontalInput.started -= m_Wrapper.m_MinimapActionsCallbackInterface.OnHorizontalInput;
                @HorizontalInput.performed -= m_Wrapper.m_MinimapActionsCallbackInterface.OnHorizontalInput;
                @HorizontalInput.canceled -= m_Wrapper.m_MinimapActionsCallbackInterface.OnHorizontalInput;
                @VerticalInput.started -= m_Wrapper.m_MinimapActionsCallbackInterface.OnVerticalInput;
                @VerticalInput.performed -= m_Wrapper.m_MinimapActionsCallbackInterface.OnVerticalInput;
                @VerticalInput.canceled -= m_Wrapper.m_MinimapActionsCallbackInterface.OnVerticalInput;
                @Interact.started -= m_Wrapper.m_MinimapActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_MinimapActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_MinimapActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_MinimapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalInput.started += instance.OnHorizontalInput;
                @HorizontalInput.performed += instance.OnHorizontalInput;
                @HorizontalInput.canceled += instance.OnHorizontalInput;
                @VerticalInput.started += instance.OnVerticalInput;
                @VerticalInput.performed += instance.OnVerticalInput;
                @VerticalInput.canceled += instance.OnVerticalInput;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public MinimapActions @Minimap => new MinimapActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_PS4SchemeIndex = -1;
    public InputControlScheme PS4Scheme
    {
        get
        {
            if (m_PS4SchemeIndex == -1) m_PS4SchemeIndex = asset.FindControlSchemeIndex("PS4");
            return asset.controlSchemes[m_PS4SchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnHorizontalInput(InputAction.CallbackContext context);
        void OnVerticalInput(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSlash(InputAction.CallbackContext context);
        void OnEarthShaker(InputAction.CallbackContext context);
        void OnSlashHeld(InputAction.CallbackContext context);
        void OnSlashReleased(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnLevitate(InputAction.CallbackContext context);
        void OnWhip(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
        void OnProjectileThrow(InputAction.CallbackContext context);
        void OnQuickItemUse(InputAction.CallbackContext context);
        void OnQuickItemCycle(InputAction.CallbackContext context);
        void OnStoreOpen(InputAction.CallbackContext context);
        void OnShadowMorph(InputAction.CallbackContext context);
        void OnControllerCursorHorizontalInput(InputAction.CallbackContext context);
        void OnControllerCursorVerticalInput(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnStore(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
    }
    public interface IMinimapActions
    {
        void OnHorizontalInput(InputAction.CallbackContext context);
        void OnVerticalInput(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
