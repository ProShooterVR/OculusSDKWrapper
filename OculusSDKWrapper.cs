using System;
using Oculus.Platform;
using Oculus.Platform.Models;
using PlayFab;
using UnityEngine;

public class OculusSDKWrapper : MonoBehaviour
{
    public static OculusSDKWrapper instance { get; private set; }
    public static Action OculusSDKManagerInitialized;
    [field: Header("Oculus Player Info")]
    [field: SerializeField] public oculusUserData OculusUserData { get; private set; }
    [Serializable]
    public class oculusUserData
    {
        [field: SerializeField] public string OculusUserID;
        [field: SerializeField] public string OculusUsername;
        [field: SerializeField] public string OculusDisplayName;
    }
    [field: Header("Oculus IAPs")]
    [field: SerializeField] public bool UseOculusIAPs { get; private set; }
    [field: SerializeField] public string[] ItemSkus { get; private set; }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Core.AsyncInitialize().OnComplete(r =>
        {
            if (r.IsError)
            {
                Debug.Log("Error core initializing: " + r.GetError().Message);
            }
            else
            {
                Entitlements.IsUserEntitledToApplication().OnComplete(r =>
                {
                    if (r.IsError)
                    {
                        Debug.Log("Error entitling user to the application: " + r.GetError().Message);
                    }
                    else
                    {
                        if (UseOculusIAPs)
                        {
                            PrepareIAPs();
                        }
                        else
                        {
                            Debug.LogWarning("Oculus SDK Manager (Mini): use IAPs are disabled");
                            DontDestroyOnLoad(gameObject);
                            OculusSDKManagerInitialized?.Invoke();
                        }
                    }
                });
            }
        });
    }

    #region Methods

    /// <summary>
    /// Gets Oculus Users Profile Data: User ID, Username, Display Name
    /// WARNING: You must have permissions to get User data in Meta Quest Developer Dashboard
    /// (Requirements -> Data Use Checkup -> User ID & User Profile)
    /// </summary>
    /// <param name="success">On Get User Data Success</param>
    /// <param name="error">On Error</param>
    public static void GetOculusUserData(Action<oculusUserData> success, Action error)
    {
        Users.GetLoggedInUser().OnComplete(r =>
        {
            if (r.IsError)
            {
                Debug.Log("Error getting Oculus User Data: " + r.GetError().Message);
            }
            else
            {
                instance.OculusUserData.OculusUserID = r.Data.ID.ToString();
                instance.OculusUserData.OculusUsername = r.Data.OculusID;
                instance.OculusUserData.OculusDisplayName = r.Data.DisplayName;
                success?.Invoke(instance.OculusUserData);
            }
        });
    }

    /// <summary>
    /// Purchases a DURABLE item
    /// 
    /// WARNING: You must have permissions to use IAPs in Meta Quest Developer Dashboard
    /// (Requirements -> Data Use Checkup -> In-App Purchases)
    /// </summary>
    /// <param name="sku">Your DURABLE item SKU</param>
    /// <param name="purchaseSuccess">Event that contains a bool. Purchase successful - bool is true, Purchase is not successful - bool is false</param>
    public static void PurchaseOculusDurable(string sku, Action<bool> purchaseSuccess)
    {
        if (instance.UseOculusIAPs)
        {
            IAP.LaunchCheckoutFlow(sku).OnComplete(r =>
            {
                if (r.IsError)
                {
                    Debug.Log("Error during Oculus Purchase: " + r.GetError().Message);
                    purchaseSuccess?.Invoke(false);
                }
                else
                {
                    Debug.Log("Purchase Successful");
                    purchaseSuccess?.Invoke(true);
                }
            });
        }
    }

    /// <summary>
    /// Purchases a CONSUMABLE item
    /// WARNING: You MUST be logged in via PlayFab
    /// WARNING: You must have permissions to use IAPs in Meta Quest Developer Dashboard
    /// (Requirements -> Data Use Checkup -> In-App Purchases)
    /// </summary>
    /// <param name="sku">Your CONSUMABLE item SKU</param>
    /// <param name="purchaseSuccess">Event that contains a bool. Purchase successful - bool is true, Purchase is not successful - bool is false</param>
    public static void PurchaseOculusConsumable(string sku, Action<bool> purchaseSuccess)
    {
        if (instance.UseOculusIAPs && PlayFabClientAPI.IsClientLoggedIn())
        {
            IAP.LaunchCheckoutFlow(sku).OnComplete(r =>
            {
                if (r.IsError)
                {
                    Debug.Log("Error during Oculus Purchase: " + r.GetError().Message);
                    purchaseSuccess?.Invoke(false);
                }
                else
                {
                    Debug.Log("Purchase Successful, consuming");
                    PlayFabWrapper.ConsumeOculusConsumablePurchase(sku, purchaseSuccess);
                }
            });
        }
    }

    #endregion

    #region Private

    void PrepareIAPs()
    {
        IAP.GetProductsBySKU(ItemSkus).OnComplete(r =>
        {
            if (r.IsError)
            {
                Debug.Log("Error getting products by SKU: " + r.GetError().Message);
            }
            else
            {
                IAP.GetViewerPurchases().OnComplete(r =>
                {
                    if (r.IsError)
                    {
                        Debug.Log("Error getting users purchases list. Getting cached durable purchases list");
                        IAP.GetViewerPurchasesDurableCache().OnComplete(r =>
                        {
                            if (r.IsError)
                            {
                                Debug.Log("Error getting users cahced durable purchases. Oculus SDK Manager (Mini) cant be initialized");
                            }
                            else
                            {
                                DontDestroyOnLoad(gameObject);
                                OculusSDKManagerInitialized?.Invoke();
                            }
                        });
                    }
                    else
                    {
                        DontDestroyOnLoad(gameObject);
                        OculusSDKManagerInitialized?.Invoke();
                    }
                });
            }
        });
    }

    #endregion

}
