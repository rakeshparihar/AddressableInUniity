using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private AssetReference playerAssetReference;
    [SerializeField] private List<AssetReference> playerMaterialsReference;

    [SerializeField]
    private GameObject player;
    private bool clearPreviousScene = false;
    private SceneInstance previousLodedScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Debug.Log("Initialize--- ");
        Addressables.InitializeAsync().Completed += AddressableManagere_Completed;
    }

    private void AddressableManagere_Completed(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("Initializingg--- ");
        playerAssetReference.InstantiateAsync().Completed += (go) =>
        {
            Debug.Log("Initialized--- ");
            player = go.Result;
            player.SetActive(true);
        };

    }

    public void LoadAddressableLevel(string addressableKey)
    {
        if(clearPreviousScene)
        {
            Addressables.UnloadSceneAsync(previousLodedScene).Completed += (asyncHandled) =>
            {
                clearPreviousScene = false;
                previousLodedScene = new SceneInstance();
                Debug.Log("Scene unload sucessfully--- ");
            };
            
        }

        Addressables.LoadSceneAsync(addressableKey, LoadSceneMode.Additive).Completed += (asyncHandled) =>
        {
            clearPreviousScene = true;
            previousLodedScene = asyncHandled.Result;
            Debug.Log("Scene load sucessfully--- ");
        };
    }
}
