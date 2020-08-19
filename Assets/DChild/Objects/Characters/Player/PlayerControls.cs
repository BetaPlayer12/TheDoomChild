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
    }
}
