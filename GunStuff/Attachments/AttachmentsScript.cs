using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentsScript : MonoBehaviour
{
    [SerializeField] private Gun gunScript;

    [Header("Attachments")]
    public GameObject[] scopes;
    public GameObject[] muzzleDevices;
    public GameObject[] grips;
    public GameObject[] lasers;

    [Header("Iron sights")]
    public GameObject ironSights;
    public GameObject ironSights2;
    public GameObject ironMask;
    public GameObject scopeMount;

    // Testing variables
    private int currentScope = -1;
    private int currentGrip = -1;
    private int currentMuzzle = -1;

    private void Awake()
    {
        if (gunScript == null) gunScript = GetComponent<Gun>();
    }

    private void Update()
    {
        // Scroll attachments with number keys
        #region Scopes Cycle

        if (scopes.Length > 0)
        {

            // SCOPE CYCLING AND UNEQUIPPING
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                UnequipScope();
            }

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                currentScope += 1;
                if (currentScope >= scopes.Length)
                {
                    UnequipScope();
                }
                else
                {
                    EquipIrons(false);
                    EquipScope(currentScope);
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                currentScope -= 1;
                if (currentScope == -1)
                {
                    UnequipScope();
                }
                else if (currentScope < -1)
                {
                    currentScope = scopes.Length - 1;
                    EquipScope(currentScope);
                }
                else
                {
                    EquipIrons(false);
                    EquipScope(currentScope);
                }
            }
        }

        #endregion 

        #region Grips Cycle

        if (grips.Length > 0)
        {

            // GRIPS CYCLING AND UNEQUIPPING
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                UnequipGrip();
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                currentGrip += 1;
                if (currentGrip >= grips.Length)
                {
                    UnequipGrip();
                }
                else
                {
                    EquipGrip(currentGrip);
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                currentGrip -= 1;
                if (currentGrip == -1)
                {
                    UnequipGrip();
                }
                else if (currentGrip < -1)
                {
                    currentGrip = grips.Length - 1;
                    EquipGrip(currentGrip);
                }
                else
                {
                    EquipGrip(currentGrip);
                }
            }
        }

        #endregion

        #region Silencers Cycle

        if (muzzleDevices.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                UnequipSilencer();
            }

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                currentMuzzle += 1;
                if (currentMuzzle >= muzzleDevices.Length)
                {
                    UnequipSilencer();
                }
                else
                {
                    EquipMuzzle(currentMuzzle);
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                currentMuzzle -= 1;
                if (currentMuzzle == -1)
                {
                    UnequipSilencer();
                }
                else if (currentMuzzle < -1)
                {
                    currentMuzzle = muzzleDevices.Length - 1;
                    EquipMuzzle(currentMuzzle);
                }
                else
                {
                    EquipMuzzle(currentMuzzle);
                }
            }
        }

        #endregion

    }

    public void EquipScope(int scopeIndex)
    {

        foreach (GameObject scope in scopes)
        {
            scope.SetActive(false);
        }

        if (scopes.Length >= scopeIndex && scopeIndex != -1)
        {
            if (scopeIndex < scopes.Length) scopes[scopeIndex].SetActive(true);
        }

        if (scopeIndex != -1)
        {
            EquipIrons(false);
        }
        else if (scopeIndex == -1)
        {
            EquipIrons(true);
        }

        // Disable scope mount for certain scopes. Most weapons never use scope mounts
        if (scopeMount != null && scopes[scopeIndex].name == "4 Kobra" || scopeMount != null && scopes[scopeIndex].name == "14 PSO-1")
        {
            scopeMount.SetActive(false);
        }
    }

    public void UnequipScope()
    {
        currentScope = -1;
        foreach (GameObject scope in scopes)
        {
            scope.SetActive(false);
        }

        EquipIrons(true);
    }


    public void EquipGrip(int gripIndex)
    {
        foreach (GameObject grip in grips)
        {
            grip.SetActive(false);
        }

        if (grips.Length >= gripIndex)
        {
            grips[gripIndex].gameObject.SetActive(true);
        }

    }

    public void UnequipGrip()
    {
        currentGrip = -1;
        foreach (GameObject grip in grips)
        {
            grip.SetActive(false);
        }
    }


    public void EquipMuzzle(int muzzleIndex)
    {
        foreach (GameObject muzzleDevice in muzzleDevices)
        {
            muzzleDevice.SetActive(false);
        }

        if (muzzleIndex < 0) return; // Unequipping
        if (muzzleDevices.Length >= muzzleIndex)
        {
            muzzleDevices[muzzleIndex].gameObject.SetActive(true);
        }
    }

    public void UnequipSilencer()
    {
        currentMuzzle = -1;
        gunScript.ResetGunTip();

        foreach (GameObject silencer in muzzleDevices)
        {
            silencer.SetActive(false);
        }

    }

    public void EquipIrons(bool boolean)
    {
        if (boolean)
        {
            if (ironSights != null) ironSights.SetActive(true);
            if (ironSights2 != null) ironSights2.SetActive(true);
            if (ironMask != null) ironMask.SetActive(false);
            if (scopeMount != null) scopeMount.SetActive(false);
        }
        else
        {
            if (ironSights != null) ironSights.SetActive(false);
            if (ironSights2 != null) ironSights2.SetActive(false);
            if (ironMask != null) ironMask.SetActive(true);
            if (scopeMount != null) scopeMount.SetActive(true);
        }
    }

}