using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IngredientsManager : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Text _cutletsValueText;
    [SerializeField] private Text _sugarValueText;

    private void Awake()
    {
         _eventManager.UpdateTextIngredientsEvent += UpdateTexts;
        
        if (_cutletsValueText == null)
            _cutletsValueText = transform.Find("Cutlets text").GetComponent<Text>();

        if (_sugarValueText == null)
            _sugarValueText = transform.Find("Sugar text").GetComponent<Text>();
    }
    private void Start()
    {
        UpdateTexts();
    }
    private void UpdateTexts()
    {
        if (_bank != null)
        {
            _cutletsValueText.text = _bank.Cutlets.ToString();
            _sugarValueText.text = _bank.Sugar.ToString();
        }
    }
    private void OnDestroy()
    {
        _eventManager.UpdateTextCurrenciesEvent -= UpdateTexts;
    }

}
