using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using SBS.ME;

public class DestroyOnPress : MonoBehaviour
{
    [Header("Assign all breakable objects here")]
    [SerializeField] private GameObject[] vaseObjects;

    [Header("Button Click Sound")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private AudioSource audioSource;

    private void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        if (interactable == null)
        {
            Debug.LogError("XRSimpleInteractable not found on " + gameObject.name);
            return;
        }

        // Add AudioSource automatically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.volume = volume;

        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        // Play click sound
        if (clickSound != null)
        {
            audioSource.Play();
        }

        // Explode vases
        foreach (GameObject vase in vaseObjects)
        {
            if (vase == null) continue;

            MeshExploder exploder = vase.GetComponent<MeshExploder>();
            if (exploder != null)
            {
                exploder.EXPLODE();
            }
        }
    }

    private void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnButtonPressed);
    }
}