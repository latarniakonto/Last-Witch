 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FootStepsMaterials
{
    Default = 0,
    Grass = 0,
    Dirt = 1,
    Wood = 2
}
public enum MovementTypes
{
    Default = 0,
    Walk = 0,
    Sneak = 1
}

public class SoundController : MonoBehaviour
{    
    [SerializeField] [FMODUnity.EventRef] private string footStepsEventPath;
    [SerializeField] [FMODUnity.EventRef] private string enemyScreamEventPath;
    [SerializeField] [FMODUnity.EventRef] private string neckSnappingEventPath;
    [SerializeField] [FMODUnity.EventRef] private string doorOpeningEventPath;
    [SerializeField] [FMODUnity.EventRef] private string doorClosingEventPath;
    [SerializeField] [FMODUnity.EventRef] private string doorBlockingEventPath;
    [SerializeField] [FMODUnity.EventRef] private string waterPickingEventPath;
    [SerializeField] [FMODUnity.EventRef] private string letterPickingEventPath;
        
    public void PlayFootStepsSound(int material, int type)
    {
        if (footStepsEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(footStepsEventPath));
        FMOD.Studio.EventInstance footStep = FMODUnity.RuntimeManager.CreateInstance(footStepsEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footStep, transform, empty);
        footStep.setParameterByName("powierzchnia", material);
        footStep.setParameterByName("bieg lub marsz", type);
        footStep.start();
        footStep.release();
    }
    public void PlayDoorOpeningSound()
    {        
        if (doorOpeningEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(doorOpeningEventPath));
        FMOD.Studio.EventInstance doorOpen = FMODUnity.RuntimeManager.CreateInstance(doorOpeningEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(doorOpen, transform, empty);
        doorOpen.setVolume(0.2f);
        doorOpen.start();
        doorOpen.release();
    }
    public void PlayDoorClosingSound()
    {        
        if (doorClosingEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(doorClosingEventPath));
        FMOD.Studio.EventInstance doorClose = FMODUnity.RuntimeManager.CreateInstance(doorClosingEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(doorClose, transform, empty);
        doorClose.setVolume(0.2f);
        doorClose.start();
        doorClose.release();
    }
    public void PlayDoorBlockingSound()
    {
        if (doorBlockingEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(doorBlockingEventPath));
        FMOD.Studio.EventInstance doorBlock = FMODUnity.RuntimeManager.CreateInstance(doorBlockingEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(doorBlock, transform, empty);
        doorBlock.setVolume(0.2f);
        doorBlock.start();
        doorBlock.release();
    }
    public void PlayPickupSound(string objectName)
    {
        if(objectName == "Water")
        {
            if (waterPickingEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(waterPickingEventPath));
            FMOD.Studio.EventInstance waterPickup = FMODUnity.RuntimeManager.CreateInstance(waterPickingEventPath);
            Rigidbody empty = null;
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(waterPickup, transform, empty);
            waterPickup.setVolume(0.5f);
            waterPickup.start();
            waterPickup.release();
        }else
        {
            if (letterPickingEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(waterPickingEventPath));
            FMOD.Studio.EventInstance letterPickup = FMODUnity.RuntimeManager.CreateInstance(letterPickingEventPath);
            Rigidbody empty = null;
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(letterPickup, transform, empty);
            letterPickup.setVolume(0.5f);
            letterPickup.start();
            letterPickup.release();
        }
    }
    public void PlayScreamingSound()
    {
        Debug.Log("SCARY");
        if (enemyScreamEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(enemyScreamEventPath));
        FMOD.Studio.EventInstance enemyScream = FMODUnity.RuntimeManager.CreateInstance(enemyScreamEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(enemyScream, transform, empty);        
        enemyScream.start();
        enemyScream.release();
    }
    public void PlayNeckSnappingSound()
    {
        if (neckSnappingEventPath is null) throw new UnassignedReferenceException("Parameter cannot be null" + nameof(neckSnappingEventPath));
        FMOD.Studio.EventInstance neckSnap = FMODUnity.RuntimeManager.CreateInstance(neckSnappingEventPath);
        Rigidbody empty = null;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(neckSnap, transform, empty);        
        neckSnap.start();
        neckSnap.release();        
    }
}
