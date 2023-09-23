using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class CurrenciesManager : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Text _goldValueText;
    [SerializeField] private Text _dimondsValueText;

    private void Start()
    {
        if (_goldValueText == null)
            _goldValueText = transform.Find("Gold text").GetComponent<Text>();

        if (_dimondsValueText == null)
            _dimondsValueText = transform.Find("Dimonds text").GetComponent<Text>();

        _eventManager.UpdateTextCurrenciesEvent += UpdateTexts;
        UpdateTexts();
    }
    private void UpdateTexts()
    {
        if (_bank != null)
        {
            int goldValue = (int)_bank.Gold;
            _goldValueText.text = goldValue.ToString();
            _dimondsValueText.text = _bank.Dimonds.ToString();
        }
    }
    private void OnDestroy()
    {
        _eventManager.UpdateTextCurrenciesEvent -= UpdateTexts;
    }
}
