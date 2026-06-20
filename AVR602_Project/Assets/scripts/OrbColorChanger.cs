using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrbColorChanger : MonoBehaviour
{
    public Color[] colors = { Color.cyan, Color.magenta, Color.yellow };
    public float transitionSpeed = 0.5f;

    private Color originalColor;
    private Material orbMaterial;
    private bool isHeld = false;
    private float lerpT = 0f;
    private int currentIndex = 0;
    private int nextIndex = 1;

    void Start()
    {
        orbMaterial = GetComponent<Renderer>().material;
        orbMaterial.EnableKeyword("_EMISSION");
        originalColor = orbMaterial.color;

        Color[] fullCycle = new Color[colors.Length + 2];
        fullCycle[0] = originalColor;
        for (int i = 0; i < colors.Length; i++)
            fullCycle[i + 1] = colors[i];
        fullCycle[fullCycle.Length - 1] = originalColor;
        colors = fullCycle;

        var grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    void Update()
    {
        if (!isHeld) return;

        lerpT += Time.deltaTime * transitionSpeed;

        if (lerpT >= 1f)
        {
            // Snap to next color and advance indexes
            lerpT = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }

        Color blended = Color.Lerp(colors[currentIndex], colors[nextIndex], lerpT);
        orbMaterial.color = blended;
        orbMaterial.SetColor("_EmissionColor", blended * 3f);
    }
}