// GENERATED AUTOMATICALLY FROM 'Assets/Controls/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""OverworldControls"",
            ""id"": ""1d93d4d0-b93a-409e-a158-0c7d3f549eba"",
            ""actions"": [
                {
                    ""name"": ""MainAction"",
                    ""type"": ""Button"",
                    ""id"": ""1aac8f68-f1b7-4d6d-8447-1a98e76e949f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""780f0006-6bd5-481a-8ff5-f6009ef0103f"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""1fe408c7-5b18-47f0-a3e7-e39b5e39bb2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""17402ffa-fbee-4a84-add8-485dafe5fdb3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleLeft"",
                    ""type"": ""Button"",
                    ""id"": ""ea3e27b0-02b3-4e1a-a542-fbf4f5f6d317"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CycleRight"",
                    ""type"": ""Button"",
                    ""id"": ""90da3192-c3e6-4445-809d-b16c61051bd2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PartnerAction"",
                    ""type"": ""Button"",
                    ""id"": ""37e361b4-6492-43c6-82fb-f71776995af0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4cdcd21e-7c5f-462a-a382-e697e1badacf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cc43a30-8d18-43a4-b88a-0bd9b803b1a1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f808bee5-74f7-466a-82f0-e1e54b58dcad"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07c5d2e7-b8a5-4a2a-b4ea-10a7a3ba4ff0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd9cf773-76e8-4a50-999c-e90014c217e2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""edb4cc91-87d4-48bc-a6c7-c165718dfdf8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3efcb9ee-9562-4902-a289-d9a38b54ad09"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""065a2c92-a782-47e3-94f5-28f607194d58"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""614a66e2-275d-4bed-bf2f-792fb0308eb2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d24f1e89-f7b5-4177-9b2f-6d051b11b987"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""103066b2-8a21-43ee-a0b2-1dc1dfe3a822"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87915e5d-58e6-4708-ab82-24e688f6ab82"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2551574d-1549-4272-9e57-f6df136859b8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22027531-e2c8-49de-9bbc-0657e4987d0d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbab174a-75ee-4184-ac34-dfdfbc3f53a0"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e5bd697-86c5-4cd1-b081-0d8815c0d8ac"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a7fd1f2-b895-4d69-947b-18e9036a6cc4"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c27c0f31-7edf-418d-a893-630cf01ef9cb"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df98b39b-e376-4c3c-883e-64441ee873bb"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PartnerAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""720d8fcc-08b4-499a-a3a0-82259946bd93"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PartnerAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CombatControls"",
            ""id"": ""24293304-07d3-4911-b8c9-53c306000384"",
            ""actions"": [
                {
                    ""name"": ""MainAction"",
                    ""type"": ""Button"",
                    ""id"": ""1c36bb46-7ff7-47ed-bf41-41b7b049d1c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Movement_Clip"",
                    ""type"": ""Value"",
                    ""id"": ""a30dc4b5-cd86-4922-8cef-d9989efa3e88"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""1341d3a2-4b83-4946-867c-3042c2359c45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""da1f000f-b477-4b06-97f6-8c9f9f33e64f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block_Clip"",
                    ""type"": ""Button"",
                    ""id"": ""15724c6e-1770-4c9e-a3a6-e4b113a2823a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block_Partner"",
                    ""type"": ""Button"",
                    ""id"": ""2f796a5f-63ac-42ed-9759-fcdcc9af5dd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement_Partner"",
                    ""type"": ""Value"",
                    ""id"": ""66970c63-9c32-4645-a57a-675b297ebde0"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0e9e8413-a1ad-47c0-b922-f25030cadd8c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72f9b804-db6c-4669-8acd-971647ddf2ed"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bb90523-db5b-4a6d-924b-b51b087484c9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f420ccbd-6e39-47b6-96c2-15e50837a658"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f7a46959-0fa2-445f-b49c-e6c2cb58d928"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9bee1895-9f27-48f7-8284-1dfaa548baf5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""72d3e507-8198-458f-bb11-658841154dbd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""29e0aa56-cf1c-4dc8-9091-f01598c16b25"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""090c14ba-0c6e-4261-b4f2-e05cd4aedede"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4f80748f-a68c-4769-adb4-574d4a0a47cb"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a5d0a2e-021b-4a09-9471-544b5a8dd781"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b0518ac-a0fa-48d1-83ed-ca15cb21ed58"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9979eea0-789f-4f26-8354-42dd3660d6d2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72d55151-90c4-414e-abb7-d52a79a1b802"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7511a1a-32e5-4567-b0ca-672b1b4c27f7"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa03c737-fd0e-45bd-8e98-2ea74d324a64"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block_Clip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82be2859-bc9f-422a-a035-e40e1c10c028"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f08e276-4609-442b-afce-ef93145b2d75"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6d7855f-7534-4f42-a33e-dba556e91fc4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""031ab1b5-d19d-4c56-b3e6-e82bd3fe64c7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""de09fdc3-0409-4781-855d-c22032615e6c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4e697e8f-7e09-4225-979b-35cf4aee4b2b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""20e81ee5-89f2-41ea-aa4f-6d7df2c5c197"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""98287d0c-dbfd-4292-9165-f0b622efb775"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement_Partner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // OverworldControls
        m_OverworldControls = asset.FindActionMap("OverworldControls", throwIfNotFound: true);
        m_OverworldControls_MainAction = m_OverworldControls.FindAction("MainAction", throwIfNotFound: true);
        m_OverworldControls_Movement = m_OverworldControls.FindAction("Movement", throwIfNotFound: true);
        m_OverworldControls_Inventory = m_OverworldControls.FindAction("Inventory", throwIfNotFound: true);
        m_OverworldControls_SecondaryAction = m_OverworldControls.FindAction("SecondaryAction", throwIfNotFound: true);
        m_OverworldControls_CycleLeft = m_OverworldControls.FindAction("CycleLeft", throwIfNotFound: true);
        m_OverworldControls_CycleRight = m_OverworldControls.FindAction("CycleRight", throwIfNotFound: true);
        m_OverworldControls_PartnerAction = m_OverworldControls.FindAction("PartnerAction", throwIfNotFound: true);
        // CombatControls
        m_CombatControls = asset.FindActionMap("CombatControls", throwIfNotFound: true);
        m_CombatControls_MainAction = m_CombatControls.FindAction("MainAction", throwIfNotFound: true);
        m_CombatControls_Movement_Clip = m_CombatControls.FindAction("Movement_Clip", throwIfNotFound: true);
        m_CombatControls_Inventory = m_CombatControls.FindAction("Inventory", throwIfNotFound: true);
        m_CombatControls_SecondaryAction = m_CombatControls.FindAction("SecondaryAction", throwIfNotFound: true);
        m_CombatControls_Block_Clip = m_CombatControls.FindAction("Block_Clip", throwIfNotFound: true);
        m_CombatControls_Block_Partner = m_CombatControls.FindAction("Block_Partner", throwIfNotFound: true);
        m_CombatControls_Movement_Partner = m_CombatControls.FindAction("Movement_Partner", throwIfNotFound: true);
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

    // OverworldControls
    private readonly InputActionMap m_OverworldControls;
    private IOverworldControlsActions m_OverworldControlsActionsCallbackInterface;
    private readonly InputAction m_OverworldControls_MainAction;
    private readonly InputAction m_OverworldControls_Movement;
    private readonly InputAction m_OverworldControls_Inventory;
    private readonly InputAction m_OverworldControls_SecondaryAction;
    private readonly InputAction m_OverworldControls_CycleLeft;
    private readonly InputAction m_OverworldControls_CycleRight;
    private readonly InputAction m_OverworldControls_PartnerAction;
    public struct OverworldControlsActions
    {
        private @GameControls m_Wrapper;
        public OverworldControlsActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MainAction => m_Wrapper.m_OverworldControls_MainAction;
        public InputAction @Movement => m_Wrapper.m_OverworldControls_Movement;
        public InputAction @Inventory => m_Wrapper.m_OverworldControls_Inventory;
        public InputAction @SecondaryAction => m_Wrapper.m_OverworldControls_SecondaryAction;
        public InputAction @CycleLeft => m_Wrapper.m_OverworldControls_CycleLeft;
        public InputAction @CycleRight => m_Wrapper.m_OverworldControls_CycleRight;
        public InputAction @PartnerAction => m_Wrapper.m_OverworldControls_PartnerAction;
        public InputActionMap Get() { return m_Wrapper.m_OverworldControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OverworldControlsActions set) { return set.Get(); }
        public void SetCallbacks(IOverworldControlsActions instance)
        {
            if (m_Wrapper.m_OverworldControlsActionsCallbackInterface != null)
            {
                @MainAction.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMainAction;
                @MainAction.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMainAction;
                @MainAction.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMainAction;
                @Movement.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnMovement;
                @Inventory.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnInventory;
                @SecondaryAction.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnSecondaryAction;
                @CycleLeft.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleLeft;
                @CycleLeft.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleLeft;
                @CycleLeft.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleLeft;
                @CycleRight.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleRight;
                @CycleRight.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleRight;
                @CycleRight.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnCycleRight;
                @PartnerAction.started -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnPartnerAction;
                @PartnerAction.performed -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnPartnerAction;
                @PartnerAction.canceled -= m_Wrapper.m_OverworldControlsActionsCallbackInterface.OnPartnerAction;
            }
            m_Wrapper.m_OverworldControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MainAction.started += instance.OnMainAction;
                @MainAction.performed += instance.OnMainAction;
                @MainAction.canceled += instance.OnMainAction;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @SecondaryAction.started += instance.OnSecondaryAction;
                @SecondaryAction.performed += instance.OnSecondaryAction;
                @SecondaryAction.canceled += instance.OnSecondaryAction;
                @CycleLeft.started += instance.OnCycleLeft;
                @CycleLeft.performed += instance.OnCycleLeft;
                @CycleLeft.canceled += instance.OnCycleLeft;
                @CycleRight.started += instance.OnCycleRight;
                @CycleRight.performed += instance.OnCycleRight;
                @CycleRight.canceled += instance.OnCycleRight;
                @PartnerAction.started += instance.OnPartnerAction;
                @PartnerAction.performed += instance.OnPartnerAction;
                @PartnerAction.canceled += instance.OnPartnerAction;
            }
        }
    }
    public OverworldControlsActions @OverworldControls => new OverworldControlsActions(this);

    // CombatControls
    private readonly InputActionMap m_CombatControls;
    private ICombatControlsActions m_CombatControlsActionsCallbackInterface;
    private readonly InputAction m_CombatControls_MainAction;
    private readonly InputAction m_CombatControls_Movement_Clip;
    private readonly InputAction m_CombatControls_Inventory;
    private readonly InputAction m_CombatControls_SecondaryAction;
    private readonly InputAction m_CombatControls_Block_Clip;
    private readonly InputAction m_CombatControls_Block_Partner;
    private readonly InputAction m_CombatControls_Movement_Partner;
    public struct CombatControlsActions
    {
        private @GameControls m_Wrapper;
        public CombatControlsActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MainAction => m_Wrapper.m_CombatControls_MainAction;
        public InputAction @Movement_Clip => m_Wrapper.m_CombatControls_Movement_Clip;
        public InputAction @Inventory => m_Wrapper.m_CombatControls_Inventory;
        public InputAction @SecondaryAction => m_Wrapper.m_CombatControls_SecondaryAction;
        public InputAction @Block_Clip => m_Wrapper.m_CombatControls_Block_Clip;
        public InputAction @Block_Partner => m_Wrapper.m_CombatControls_Block_Partner;
        public InputAction @Movement_Partner => m_Wrapper.m_CombatControls_Movement_Partner;
        public InputActionMap Get() { return m_Wrapper.m_CombatControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICombatControlsActions instance)
        {
            if (m_Wrapper.m_CombatControlsActionsCallbackInterface != null)
            {
                @MainAction.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMainAction;
                @MainAction.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMainAction;
                @MainAction.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMainAction;
                @Movement_Clip.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Clip;
                @Movement_Clip.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Clip;
                @Movement_Clip.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Clip;
                @Inventory.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnInventory;
                @SecondaryAction.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnSecondaryAction;
                @Block_Clip.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Clip;
                @Block_Clip.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Clip;
                @Block_Clip.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Clip;
                @Block_Partner.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Partner;
                @Block_Partner.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Partner;
                @Block_Partner.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnBlock_Partner;
                @Movement_Partner.started -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Partner;
                @Movement_Partner.performed -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Partner;
                @Movement_Partner.canceled -= m_Wrapper.m_CombatControlsActionsCallbackInterface.OnMovement_Partner;
            }
            m_Wrapper.m_CombatControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MainAction.started += instance.OnMainAction;
                @MainAction.performed += instance.OnMainAction;
                @MainAction.canceled += instance.OnMainAction;
                @Movement_Clip.started += instance.OnMovement_Clip;
                @Movement_Clip.performed += instance.OnMovement_Clip;
                @Movement_Clip.canceled += instance.OnMovement_Clip;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @SecondaryAction.started += instance.OnSecondaryAction;
                @SecondaryAction.performed += instance.OnSecondaryAction;
                @SecondaryAction.canceled += instance.OnSecondaryAction;
                @Block_Clip.started += instance.OnBlock_Clip;
                @Block_Clip.performed += instance.OnBlock_Clip;
                @Block_Clip.canceled += instance.OnBlock_Clip;
                @Block_Partner.started += instance.OnBlock_Partner;
                @Block_Partner.performed += instance.OnBlock_Partner;
                @Block_Partner.canceled += instance.OnBlock_Partner;
                @Movement_Partner.started += instance.OnMovement_Partner;
                @Movement_Partner.performed += instance.OnMovement_Partner;
                @Movement_Partner.canceled += instance.OnMovement_Partner;
            }
        }
    }
    public CombatControlsActions @CombatControls => new CombatControlsActions(this);
    public interface IOverworldControlsActions
    {
        void OnMainAction(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSecondaryAction(InputAction.CallbackContext context);
        void OnCycleLeft(InputAction.CallbackContext context);
        void OnCycleRight(InputAction.CallbackContext context);
        void OnPartnerAction(InputAction.CallbackContext context);
    }
    public interface ICombatControlsActions
    {
        void OnMainAction(InputAction.CallbackContext context);
        void OnMovement_Clip(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSecondaryAction(InputAction.CallbackContext context);
        void OnBlock_Clip(InputAction.CallbackContext context);
        void OnBlock_Partner(InputAction.CallbackContext context);
        void OnMovement_Partner(InputAction.CallbackContext context);
    }
}
