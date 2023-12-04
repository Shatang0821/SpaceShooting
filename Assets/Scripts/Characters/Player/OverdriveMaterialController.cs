using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
    [SerializeField] Material overdriveMaterial;
    
    Material defaultMaterial;

    new Renderer renderer;
    
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
    }
    
    void OnEnable()
    {
        EventCenter.Subscribe(EventNames.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Subscribe(EventNames.OverDriveOff, PlayerOverdriveOff);
    }

    void OnDisable()
    {
        EventCenter.Unsubscribe(EventNames.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Unsubscribe(EventNames.OverDriveOff, PlayerOverdriveOff);     
    }

    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}