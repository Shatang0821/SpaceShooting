using Event;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    Text _waveText;
    Canvas _waveCanvas;
    Animator _animator;
    const string SHOWTRIGGER = "Show";
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        _waveText = GetComponentInChildren<Text>();
        _waveCanvas = GetComponent<Canvas>();
        _animator = GetComponent<Animator>();
        _waveCanvas.enabled = false;
    }

    private void OnEnable()
    {
        EventCenter.Subscribe(UIEventKey.ShowWave,ShowWaveUI);
        EventCenter.Subscribe(UIEventKey.HideWave,HideWaveUI);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(UIEventKey.ShowWave, ShowWaveUI);
        EventCenter.Unsubscribe(UIEventKey.HideWave, HideWaveUI);
    }

    private void ShowWaveUI(object obj)
    {
        if(obj is int waveNumber)
        {
            Debug.Log("ShowUI");
            _waveCanvas.enabled = true;
            _animator.SetTrigger(SHOWTRIGGER);
            _waveText.text = "- WAVE " + (waveNumber + 1) + " -";
        }
        else
        {
            Debug.Log("WaveUI –â‘è‚ ‚è");
        }
    }

    private void HideWaveUI()
    {
        _waveCanvas.enabled = false;
    }
}
