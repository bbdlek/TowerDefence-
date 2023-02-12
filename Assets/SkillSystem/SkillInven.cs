using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillInven : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] public string skillName;
    [SerializeField] public string skillGrade;
    [SerializeField] int tap;
    [SerializeField] float interval = 1f;
    [SerializeField] bool readyForDoubleTap;

    [SerializeField] bool isEquipped;

    [SerializeField] GameObject _EquipGrid;
    [SerializeField] GameObject _SelectGrid;

    [SerializeField] GameObject _Info;

    [SerializeField] GameObject _equipBtn;
    [SerializeField] Text _equipTxt;

    [SerializeField] Text _coolTxt;
    float coolTime;

    public string info = "";


    public int _upgraded = 1;

    public int[] _UpgradeCost = { 300, 400, 500, 600, 800, 1000, 1200, 1400, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 8000, 0 };
    public void OnPointerClick(PointerEventData eventData)
    {
        tap++;
        if(tap == 1)
        {
            _equipBtn.SetActive(true);
            readyForDoubleTap = true;
            StartCoroutine(DoubleTapInterval());
        }
        else if(tap > 1 && readyForDoubleTap)
        {
            EquipChange();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(Input.GetMouseButtonDown(0))
            _equipBtn.SetActive(false);
    }

    IEnumerator DoubleTapInterval()
    {
        yield return new WaitForSeconds(interval);
        tap = 0;
        readyForDoubleTap = false;
    }

    public bool CheckEquip()
    {
        return _EquipGrid.transform.childCount < 4;
    }

    public void Upgrade()
    {
        _upgraded += 1;
        Debug.Log(_upgraded);
    }

    public void UpdateEquip()
    {
        for(int i = 0; i < _EquipGrid.transform.childCount; i++)
        {
            PlayerPrefs.SetString("selectedSkill" + (i + 1).ToString(), _EquipGrid.transform.GetChild(i).GetComponent<SkillInven>().skillName);
        }
        for(int j = _EquipGrid.transform.childCount; j < 4; j++)
        {
            PlayerPrefs.SetString("selectedSkill" + (j + 1).ToString(), "");
        }
    }

    public void UnEquip()
    {
        isEquipped = false;
        gameObject.transform.SetParent(_SelectGrid.transform);
        UpdateEquip();
        tap = 0;
        readyForDoubleTap = false;
    }

    public void EquipChange()
    {
        if(!isEquipped && CheckEquip())
        {
            isEquipped = true;
            gameObject.transform.SetParent(_EquipGrid.transform);
            UpdateEquip();
            tap = 0;
            readyForDoubleTap = false;
            _equipTxt.text = "�� ��";
            _equipBtn.SetActive(false);

        }
        else if (isEquipped)
        {
            isEquipped = false;
            gameObject.transform.SetParent(_SelectGrid.transform);
            UpdateEquip();
            tap = 0;
            readyForDoubleTap = false;
            _equipTxt.text = "�� ��";
            _equipBtn.SetActive(false);

        }
    }

    public void Setup()
    {
        isEquipped = true;
        gameObject.transform.SetParent(_EquipGrid.transform);
        tap = 0;
        readyForDoubleTap = false;
    }

    private void Start()
    {
        CalCool();
        _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
    }

    public void CalCool()
    {
        if (skillName == "Meteor")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Meteor");
            coolTime = 20f - (_upgraded - 1) * 5f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "��ų ������ �ϴÿ��� �ҵ��̵��� ��ȯ�Ͽ�\n3�ʰ� �ʴ� " + string.Format("{0:0.#}", (2f + (_upgraded - 1) * 3f / 19f)) + "%�� �������� ������.";
        }
        else if (skillName == "Sacrifice")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Sacrifice");
            coolTime = 2f;
            int _hpAmount = 0 ;
            if (_upgraded >= 1 && _upgraded < 6)
                _hpAmount = 5;
            else if (_upgraded < 13)
                _hpAmount = 6;
            else if (_upgraded <= 20)
                _hpAmount = 7;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "ü���� " + _hpAmount.ToString() + "��ŭ �Ҹ��Ͽ� " + string.Format("{0:0.#}", (10 + (_upgraded - 1) * 5)) + "��ŭ�� ���������� ȹ���Ѵ�.";
        }
        else if (skillName == "Heal")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Heal");
            coolTime = 2f;
            int _moneyAmount = 0;
            if (_upgraded >= 1 && _upgraded < 6)
                _moneyAmount = 150;
            else if (_upgraded < 11)
                _moneyAmount = 130;
            else if (_upgraded < 16)
                _moneyAmount = 120;
            else if (_upgraded < 20)
                _moneyAmount = 110;
            else if (_upgraded == 20)
                _moneyAmount = 100;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "���������� " + _moneyAmount + "��ŭ �Ҹ��Ͽ� ü���� ������ ȸ���Ѵ�. \n��, �� ��ų�� �������� �� �ѹ��� ����� �����ϴ�.";
        }
        else if (skillName == "PowerUp")
        {
            _upgraded = PlayerPrefs.GetInt("grade_PowerUp");
            coolTime = 15f - (+_upgraded - 1) * 2f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "��ų ���� ���� Ÿ���� 4�� ���� ���ݷ��� " + string.Format("{0:0.#}", (1f + (_upgraded - 1) * 3f / 19f)) + "%��ŭ ������Ų��.";
        }
        else if (skillName == "Earthquake")
        {
            _upgraded = PlayerPrefs.GetInt("grade_Earthquake");
            coolTime = 35f - (_upgraded - 1) * 10f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "��ų�� �� ������ ���� �ִ� " + string.Format("{0:0.#}", (1f + (_upgraded - 1) * 2f / 19f)) + "�ʸ�ŭ ������ ����Ų��. \n���� �������� ��Ÿ���� �������.";
        }
        else if (skillName == "LandSmash")
        {
            _upgraded = PlayerPrefs.GetInt("grade_LandSmash");
            coolTime = 30f - (_upgraded - 1) * 5f / 19f;
            _coolTxt.text = string.Format("{0:0.#}", coolTime) + "��";
            info = "������ ���ϰ� ������� ������ Ÿ���� " + string.Format("{0:0.#}", (2f + (_upgraded - 1) * 2f / 19f)) +"�� ���� �ھ� ������.";
        }

    }

    private void Update()
    {
        if (this.GetComponent<Toggle>())
            if(this.GetComponent<Toggle>().isOn)
                _Info.GetComponent<SkillManager>().InfoSetting();

        if (this.GetComponent<Toggle>())
            if (!this.GetComponent<Toggle>().isOn)
                _equipBtn.SetActive(false);
    }

}
