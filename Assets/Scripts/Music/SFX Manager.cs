using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
  public static SFXManager instance;

  private void Awake()
  {
    instance = this;
  }

  public AudioSource[] soundsEffects;
  // Start is called before the first frame update
  public void PlaySFX(int sfxToPlay)
  {
    soundsEffects[sfxToPlay].Stop();
    soundsEffects[sfxToPlay].Play();
  }

  public void PlayeSFXPitched(int sfxToPlay)
  {
    soundsEffects[sfxToPlay].pitch = Random.Range(.9f, 1.2f);

    PlaySFX(sfxToPlay);
  }
}
