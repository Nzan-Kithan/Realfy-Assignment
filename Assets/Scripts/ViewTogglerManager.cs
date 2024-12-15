using UnityEngine;
using UnityEngine.UI;

public class ViewTogglerManager : MonoBehaviour
{
    [SerializeField] private Button listViewButton;
    [SerializeField] private Button gridViewButton;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button shutdownButton;

    private Transform listActive;
    private Transform listInactive;
    private Transform gridActive;
    private Transform gridInactive;

    private ScrollViewManager scrollViewManager;
    private bool isGridViewSelected = false;

    void Start()
    {
        scrollViewManager = FindAnyObjectByType<ScrollViewManager>();
        
        listActive = listViewButton.transform.Find("ListActive");
        listInactive = listViewButton.transform.Find("ListInactive");
        gridActive = gridViewButton.transform.Find("GridActive");
        gridInactive = gridViewButton.transform.Find("GridInactive");

        SetInitialState();

        listViewButton.onClick.AddListener(() => OnViewButtonClicked(true));
        gridViewButton.onClick.AddListener(() => OnViewButtonClicked(false));
        reloadButton.onClick.AddListener(() => OnReloadButtonClicked());
        shutdownButton.onClick.AddListener(() => OnShutdownButtonClicked());
    }

    private void OnShutdownButtonClicked()
    {
        Application.Quit();
    }

    private void OnReloadButtonClicked()
    {
        scrollViewManager.FetchItems();
        scrollViewManager.AdjustContentHeight();
    }

    private void OnViewButtonClicked(bool isListView)
    {
        if (isListView)
        {
            SwitchToListView();
        }
        else
        {
            SwitchToGridView();
        }

        scrollViewManager.PopulateScrollView();
        scrollViewManager.AdjustContentHeight();
    }

    private void SwitchToListView()
    {
        listActive.gameObject.SetActive(true);
        listInactive.gameObject.SetActive(false);
        gridActive.gameObject.SetActive(false);
        gridInactive.gameObject.SetActive(true);

        isGridViewSelected = false;
    }

    private void SwitchToGridView()
    {
        listActive.gameObject.SetActive(false);
        listInactive.gameObject.SetActive(true);
        gridActive.gameObject.SetActive(true);
        gridInactive.gameObject.SetActive(false);

        isGridViewSelected = true;
    }

    private void SetInitialState()
    {
        listActive.gameObject.SetActive(true);
        listInactive.gameObject.SetActive(false);
        gridActive.gameObject.SetActive(false);
        gridInactive.gameObject.SetActive(true);

        isGridViewSelected = false;
    }

    public bool IsGridViewSelected()
    {
        return isGridViewSelected;
    }
}
