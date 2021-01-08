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
                    ""name"": ""SkullThrow"",
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
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f8c8d89d-c5c6-4e1a-8f9a-211db00633d5"",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""0aa3da6b-c8f6-4f06-a9cc-9533f1e3cc7f"",
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
                    ""id"": ""64813cb4-7231-4592-9051-3726a30638d0"",
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
                    ""id"": ""953b98aa-db21-4719-9645-02e17170a3cb"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2be8d5cd-18d3-4b04-b7e7-f14e5f24793e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bda03e02-9d8b-4045-a9a0-eb25c5bd64c7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""VerticalInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4"",
                    ""id"": ""bada023e-c2ce-4ea4-a3bb-96c40b97f23d"",
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
                    ""id"": ""e0121929-8f77-45de-8e4f-b38fd29b28c3"",
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
                    ""id"": ""3c134836-48d8-4703-ace0-37f9874af2ee"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""EarthShaker"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5c2679fd-e377-412f-aea2-2afeb24ea6f4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""SkullThrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2431c23-5039-4e63-8668-80500091367b"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""StoreOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ed84032-9951-419d-86f4-4d435f95e1e3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
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
        m_Gameplay_SkullThrow = m_Gameplay.FindAction("SkullThrow", throwIfNotFound: true);
        m_Gameplay_QuickItemUse = m_Gameplay.FindAction("QuickItemUse", throwIfNotFound: true);
        m_Gameplay_QuickItemCycle = m_Gameplay.FindAction("QuickItemCycle", throwIfNotFound: true);
        m_Gameplay_StoreOpen = m_Gameplay.FindAction("StoreOpen", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
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
    private readonly InputAction m_Gameplay_SkullThrow;
    private readonly InputAction m_Gameplay_QuickItemUse;
    private readonly InputAction m_Gameplay_QuickItemCycle;
    private readonly InputAction m_Gameplay_StoreOpen;
    private readonly InputAction m_Gameplay_Pause;
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
        public InputAction @SkullThrow => m_Wrapper.m_Gameplay_SkullThrow;
        public InputAction @QuickItemUse => m_Wrapper.m_Gameplay_QuickItemUse;
        public InputAction @QuickItemCycle => m_Wrapper.m_Gameplay_QuickItemCycle;
        public InputAction @StoreOpen => m_Wrapper.m_Gameplay_StoreOpen;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
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
                @SkullThrow.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkullThrow;
                @SkullThrow.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkullThrow;
                @SkullThrow.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSkullThrow;
                @QuickItemUse.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemUse.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemUse.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemUse;
                @QuickItemCycle.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @QuickItemCycle.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @QuickItemCycle.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickItemCycle;
                @StoreOpen.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @StoreOpen.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @StoreOpen.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStoreOpen;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
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
                @SkullThrow.started += instance.OnSkullThrow;
                @SkullThrow.performed += instance.OnSkullThrow;
                @SkullThrow.canceled += instance.OnSkullThrow;
                @QuickItemUse.started += instance.OnQuickItemUse;
                @QuickItemUse.performed += instance.OnQuickItemUse;
                @QuickItemUse.canceled += instance.OnQuickItemUse;
                @QuickItemCycle.started += instance.OnQuickItemCycle;
                @QuickItemCycle.performed += instance.OnQuickItemCycle;
                @QuickItemCycle.canceled += instance.OnQuickItemCycle;
                @StoreOpen.started += instance.OnStoreOpen;
                @StoreOpen.performed += instance.OnStoreOpen;
                @StoreOpen.canceled += instance.OnStoreOpen;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
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
        void OnSkullThrow(InputAction.CallbackContext context);
        void OnQuickItemUse(InputAction.CallbackContext context);
        void OnQuickItemCycle(InputAction.CallbackContext context);
        void OnStoreOpen(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
