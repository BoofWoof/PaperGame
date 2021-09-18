using UnityEngine;
using UnityEngine.InputSystem;

public class MousePainter : MonoBehaviour
{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;

    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    private GameControls control;

    private void Awake()
    {
        control = new GameControls();
    }

    private void OnEnable()
    {
        control.MapCraftControls.Enable();
    }

    private void OnDisable()
    {
        control.MapCraftControls.Disable();
    }

    void Update()
    {
        bool click;
        click = Mouse.current.leftButton.isPressed;

        if (click)
        {
            Vector3 position = Mouse.current.position.ReadValue();
            if (cam == null) cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                }
            }
        }
    }
}