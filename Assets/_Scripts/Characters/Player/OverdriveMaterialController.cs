using Event;
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
        EventCenter.Subscribe(EventKeyManager.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Subscribe(EventKeyManager.OverDriveOff, PlayerOverdriveOff);
    }

    void OnDisable()
    {
        EventCenter.Unsubscribe(EventKeyManager.PlayerOverDriveOn, PlayerOverdriveOn);
        EventCenter.Unsubscribe(EventKeyManager.OverDriveOff, PlayerOverdriveOff);     
    }

    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}