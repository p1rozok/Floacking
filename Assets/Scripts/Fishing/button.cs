using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Button ThrowButton;
    [SerializeField] private ThrowHook _throwHook;
    [SerializeField] private LogicMiniGame _logic;

    private bool checkThrow = false;

    private void Start()
    {
        if (ThrowButton == null || _throwHook == null || _logic == null)
        {
            return;
        }

        SetButtonToThrow();
    }

    private void Update()
    {
        BlockButton();
    }

    private void SetButtonToThrow()
    {
        ThrowButton.onClick.RemoveAllListeners();
        ThrowButton.onClick.AddListener(Throw);
    }

    private void SetButtonToRetrieve()
    {
        ThrowButton.onClick.RemoveAllListeners();
        ThrowButton.onClick.AddListener(Retrieve);
    }

    public void Throw()
    {
        if (!checkThrow)
        {
            checkThrow = true;
            _throwHook.Throw();
            SetButtonToRetrieve();
        }
    }

    public void Retrieve()
    {
        if (checkThrow)
        {
            checkThrow = false;
            _throwHook.Retrieve();
            SetButtonToThrow();
        }
    }

    private void BlockButton()
    {
        ThrowButton.interactable = !_logic.IsActive;
    }
}
