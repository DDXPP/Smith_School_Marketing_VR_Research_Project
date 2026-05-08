using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectorMenuBuilder : MonoBehaviour
{
    public ObjectSpawner spawner;
    public GameObject buttonPrefab;
    public Transform container;
    private GameObject[] loadedPrefabs;

    void Start()
    {
        // Load all prefabs under Resources/SpawnedObjects/
        loadedPrefabs = Resources.LoadAll<GameObject>("SpawnableObjects");

        System.Array.Sort(loadedPrefabs, (a, b) => a.name.CompareTo(b.name));
        BuildMenu();
    }

    void BuildMenu()
    {
        GameObject btnDestroyAll = Instantiate(buttonPrefab, container);
        PrettifyButton(btnDestroyAll, "DESTROY ALL");

        btnDestroyAll.GetComponent<Button>().onClick.AddListener(() =>
        {
             for (int i = 0; i < loadedPrefabs.Length; i++)
            {
                spawner.DestroyObject(loadedPrefabs[i]);
            }
        });

        
        for (int i = 0; i < loadedPrefabs.Length; i++)
        {
            int index = i;

            GameObject btn = Instantiate(buttonPrefab, container);
            PrettifyButton(btn, loadedPrefabs[i].name);
            
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                spawner.SpawnObject(loadedPrefabs[index]);
            });
        }
    }

    void PrettifyButton(GameObject btn, string btnText)
    {
        TextMeshProUGUI tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = btnText;
        tmp.fontSize = 24;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 14;
        tmp.fontSizeMax = 28;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.margin = new Vector4(5, 5, 5, 5);
    }
}