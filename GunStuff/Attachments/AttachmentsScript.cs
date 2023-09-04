using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttachmentsScript : MonoBehaviour
{
    [SerializeField] private Gun gunScript;
    [SerializeField] private Transform scopesHolder, muzzlesHolder, gripsHolder;

    [Header("Attachments")]
    public GameObject[] scopes;
    public GameObject[] muzzleDevices;
    public GameObject[] grips;
    public GameObject[] lasers;

    [Header("Exceptions")]
    public int[] unavailableScopes;
    public int[] unavailableMuzzles;
    public int[] unavailableGrips;

    [Header("Iron sights")]
    public GameObject ironSights;
    public GameObject ironSights2;
    public GameObject ironMask;
    public GameObject scopeMount;

    // Testing variables
    private int currentScope = -1;
    private int currentGrip = -1;
    private int currentMuzzle = -1;

    private void OnValidate()
    {
        // if (scopesHolder == null) scopesHolder = GameObject.Find("Scopes").transform;
        // if (muzzlesHolder == null) muzzlesHolder = GameObject.Find("Muzzles").transform;
        // if (gripsHolder == null) gripsHolder = GameObject.Find("Grips").transform;
        if (gunScript == null) gunScript = GetComponent<Gun>();

        if (scopesHolder != null)
        {
            scopes = new GameObject[scopesHolder.childCount];

            for (int i = 0; i < scopes.Length; i++)
            {
                if (unavailableScopes != null && unavailableScopes.Contains(i))
                {
                    scopes[i] = null; // Assign null for unavailable scopes
                }
                else
                {
                    scopes[i] = scopesHolder.transform.GetChild(i).gameObject;
                }
            }
        }

        if (muzzlesHolder != null)
        {
            muzzleDevices = new GameObject[muzzlesHolder.childCount];

            for (int i = 0; i < muzzleDevices.Length; i++)
            {
                if (unavailableMuzzles != null && unavailableMuzzles.Contains(i))
                {
                    muzzleDevices[i] = null; // Assign null for unavailable muzzle devices
                }
                else
                {
                    muzzleDevices[i] = muzzlesHolder.transform.GetChild(i).gameObject;
                }
            }
        }

        if (gripsHolder != null)
        {
            grips = new GameObject[gripsHolder.childCount];
            for (int i = 0; i < grips.Length; i++)
            {
                grips[i] = gripsHolder.transform.GetChild(i).gameObject;
            }
        }
    }

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
                UnequipMuzzle();
            }

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                currentMuzzle += 1;
                if (currentMuzzle >= muzzleDevices.Length)
                {
                    UnequipMuzzle();
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
                    UnequipMuzzle();
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
        if (scopeIndex == -1)
        {
            UnequipScope();
            return;
        }

        foreach (GameObject scope in scopes)
        {
            if (scope != null) scope.SetActive(false);
        }

        if (scopes.Length >= scopeIndex && scopeIndex != -1)
        {
            if (scopes[scopeIndex] != null) scopes[scopeIndex].SetActive(true);
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
        if (scopeMount != null && (scopes[scopeIndex].name == "6 RD Russian" || scopes[scopeIndex].name == "16 Russian Sniper"))
        {
            scopeMount.SetActive(false);
        }
    }

    public void UnequipScope()
    {
        currentScope = -1;
        foreach (GameObject scope in scopes)
        {
            if (scope != null) scope.SetActive(false);
        }

        EquipIrons(true);
    }


    public void EquipGrip(int gripIndex)
    {
        if (gripIndex == -1)
        {
            UnequipGrip();
            return;
        }

        foreach (GameObject grip in grips)
        {
            if (grip != null) grip.SetActive(false);
        }

        if (grips.Length >= gripIndex)
        {
            if (grips[gripIndex] != null) grips[gripIndex].gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Invalid gripIndex: " + gripIndex);
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
        if (muzzleIndex == -1)
        {
            UnequipMuzzle();
            return;
        }

        foreach (GameObject muzzleDevice in muzzleDevices)
        {
            if (muzzleDevice != null) muzzleDevice.SetActive(false);
        }

        if (muzzleDevices.Length >= muzzleIndex)
        {
            if (muzzleDevices[muzzleIndex] != null) muzzleDevices[muzzleIndex].SetActive(true);
        }
    }

    public void UnequipMuzzle()
    {
        currentMuzzle = -1;
        gunScript.ResetGunTip();

        foreach (GameObject muzzle in muzzleDevices)
        {
            if (muzzle != null) muzzle.SetActive(false);
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
