using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class ScrollViewManager : MonoBehaviour
{
    [SerializeField] private GameObject ListItemPrefab;
    [SerializeField] private GameObject GridItemPrefab;
    [SerializeField] private Transform contentContainer;
    [SerializeField] private Transform contentList;
    [SerializeField] private Transform contentGrid;
    [SerializeField] private Sprite misssingImage;
    [SerializeField] private Image loadingSpinner;
    [SerializeField] private Image noData;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float itemSpacing = 30f;
    [SerializeField] private int gridColumns = 2;
    private Boolean isDataFetched;

    private ViewTogglerManager viewTogglerManager;
    private ItemCardPopupWindowManager itemCardPopupWindowManager;

    private GetMethod getMethod;
    private Transform contentPanel;
    private ItemList itemList;

    void Start()
    {
        getMethod = new GetMethod();
        isDataFetched = false;
        viewTogglerManager = FindAnyObjectByType<ViewTogglerManager>();
        itemCardPopupWindowManager = FindAnyObjectByType<ItemCardPopupWindowManager>();
        itemList = null;

        ImageLoader.SetDefaultImage(misssingImage);

        if (loadingSpinner != null)
        {
            loadingSpinner.gameObject.SetActive(false);
        }
        if (noData != null)
        {
            noData.gameObject.SetActive(false);
        }

        FetchItems();
    }

    public void FetchItems()
    {
        StartCoroutine(LoadAndPopulateScrollView());
    }

    IEnumerator LoadAndPopulateScrollView()
    {
        ShowLoadingSpinner(true);

        yield return StartCoroutine(getMethod.FetchItems());

        isDataFetched = true;

        PopulateScrollView();
        AdjustContentHeight();
    }

    void ShowLoadingSpinner(bool show)
    {
        if (loadingSpinner != null)
        {
            loadingSpinner.gameObject.SetActive(show);
            if (show)
            {
                StartCoroutine(RotateSpinner());
            }
        }
    }

    IEnumerator RotateSpinner()
    {
        while (loadingSpinner.gameObject.activeSelf)
        {
            loadingSpinner.transform.Rotate(0, 0, 90 * Time.deltaTime);
            yield return null;
        }
    }

    public void PopulateScrollView()
    {
        if (getMethod.itemList.Count == 0)
        {
            return;
        }

        itemList = getMethod.itemList;
        ClearContent();
        noData.gameObject.SetActive(false);

        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }

        if (viewTogglerManager.IsGridViewSelected())
        {
            contentPanel = contentGrid;
        }
        else
        {
            contentPanel = contentList;
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject newItem = null;

            if (viewTogglerManager.IsGridViewSelected())
            {
                newItem = Instantiate(GridItemPrefab, contentPanel);
            }
            else
            {
                newItem = Instantiate(ListItemPrefab, contentPanel);
            }

            Transform itemButton = newItem.transform.Find("Button");

            Transform itemDescription = itemButton.transform.Find("ItemDescription");
            Transform itemImageContainer = itemButton.transform.Find("ItemImageContainer");

            TextMeshProUGUI itemTitle = itemDescription.GetComponentInChildren<TextMeshProUGUI>();
            if (itemTitle != null)
            {
                itemTitle.text = itemList.data[i].title;
            }

            TextMeshProUGUI[] itemDescriptionDetails = itemDescription.transform.Find("ItemDescriptionDetails").GetComponentsInChildren<TextMeshProUGUI>();
            if (itemDescriptionDetails != null)
            {
                itemDescriptionDetails[1].text = itemList.data[i].description;
            }

            Image itemImage = itemImageContainer.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                StartCoroutine(new ImageLoader().SetImageFromId(itemImage, itemList.data[i].image));
            }

            Button button = itemButton.GetComponent<Button>();
            if (itemButton != null)
            {
                string itemId = itemList.data[i].id;
                button.onClick.AddListener(() => OnItemClicked(itemId));
            }
        }
    }

    // Clears the existing Items that are in the scene.
    void ClearContent()
    {
        foreach (Transform child in contentGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in contentList)
        {
            Destroy(child.gameObject);
        }
    }

    public void AdjustContentHeight()
    {
        int itemCount = contentPanel.childCount;
        if (itemCount == 0) return;

        // Adjust layout based on selected view (Grid or List)
        if (viewTogglerManager.IsGridViewSelected())
        {
            AdjustGridViewHeight(itemCount);
        }
        else
        {
            AdjustListViewHeight(itemCount);
        }
    }

    void AdjustGridViewHeight(int itemCount)
    {
        float itemHeight = GridItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        int rows = Mathf.CeilToInt((float)itemCount / gridColumns);
        float totalHeight = (itemHeight + 75) * rows;

        RectTransform contentRectTransform = contentContainer.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }

    // Adjust the content height for List view
    private void AdjustListViewHeight(int itemCount)
    {
        float itemHeight = ListItemPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float totalHeight = (itemHeight + itemSpacing) * itemCount + 20;

        RectTransform contentRectTransform = contentContainer.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }

    // When an Item is clicke to be viewed.
    private void OnItemClicked(string id)
    {
        // Find the item in the itemList
        Item selectedItem = itemList.GetItemById(id);

        if (selectedItem == null)
        {
            return;
        }
        itemCardPopupWindowManager.OpenPopupWindow(selectedItem);
    }

    private void Update()
    {
        if (isDataFetched == true)
        {
            if (loadingSpinner != null)
                loadingSpinner.gameObject.SetActive(false);

            if (noData != null && getMethod.itemList.Count == 0)
                noData.gameObject.SetActive(true);
        }
    }
}
