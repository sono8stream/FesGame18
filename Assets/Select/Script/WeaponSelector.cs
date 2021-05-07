using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField]
    float selectRadius;
    [SerializeField]
    int playerNo;
    [SerializeField]
    Transform statusWin;
    [SerializeField]
    Sprite[] weaponSprites;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    RectTransform[] gaugeTransforms = new RectTransform[3];
    [SerializeField]
    Image[] selectedImages = new Image[WeaponSet.weaponCount];

    Counter selectCounter;
    int weaponCount;
    float nowAngle, targetAngle;
    float anglePerWeapon;
    bool onRotate;

    float[] targetGaugeX;
    float gaugeWidth;
    int selectNo;//武器何個選択したか
    bool canSelect;

    // Use this for initialization
    void Start()
    {
        weaponCount = transform.childCount;
        selectCounter = new Counter(weaponCount);
        anglePerWeapon = 360f / weaponCount;

        for (int i = 0; i < weaponCount; i++)//再配置
        {
            Transform t = transform.GetChild(i);
            transform.GetChild(i).eulerAngles = Vector3.zero;
            float rad = (anglePerWeapon * i - 90) * Mathf.PI / 180;
            t.localPosition = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * selectRadius;
            t.GetComponent<SpriteRenderer>().sprite = weaponSprites[i];
        }
        nowAngle = 0;
        targetAngle = 0;

        targetGaugeX = new float[gaugeTransforms.Length];
        gaugeWidth = gaugeTransforms[0].sizeDelta.x;
        selectNo = 0;
        canSelect = true;

        SelectWeapon(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (onRotate)
        {
            if (Mathf.Abs(nowAngle - targetAngle) < 0.1f)
            {
                onRotate = false;
            }
            nowAngle = (nowAngle*3 + targetAngle) * 0.25f;
            GetUpTargetStatus();

            RotateList(nowAngle);

            if (Input.GetKeyDown(
                UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Left)))
            {
                SelectWeapon(-1);
            }
            if (Input.GetKeyDown(
                UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Right)))
            {
                SelectWeapon(1);
            }
            return;
        }

        if (Input.GetKeyDown(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Cancel)))
        {
            CancelWeapon();
        }
        if (!canSelect) return;
        if (Input.GetKeyDown(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Enter)))
        {
            ChooseWeapon();
            Debug.Log(canSelect);
        }

        if (Input.GetKey(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Left)))
        {
            SelectWeapon(-1);
        }
        if (Input.GetKey(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Right)))
        {
            SelectWeapon(1);
        }
    }

    void SelectWeapon(int iterator)
    {
        do
        {
            if (selectCounter.Count(iterator))
            {
                selectCounter.Initialize();
                nowAngle = -anglePerWeapon;
            }
            if (selectCounter.Now < 0)
            {
                selectCounter.Now = selectCounter.Limit - 1;
                nowAngle = 360;
            }
        }
        while (transform.GetChild(selectCounter.Now).GetComponent<
            SpriteRenderer>().color == Color.black);

        onRotate = true;
        targetAngle = anglePerWeapon * selectCounter.Now;
        UpdateTargetStatus();
    }

    void RotateList(float angle)
    {
        transform.eulerAngles = Vector3.up * angle;

        foreach (Transform child in transform)
        {
            child.eulerAngles = Vector3.zero;
        }
    }

    void UpdateTargetStatus()
    {
        float max = 30;
        WeaponStatus weaponStatus
            = UserData.instance.playersWeapon[playerNo].GetWeaponByIndex(selectCounter.Now);

        nameText.text = weaponStatus.name;
        descriptionText.text = weaponStatus.description;
        targetGaugeX[0] = weaponStatus.range / max * gaugeWidth;
        targetGaugeX[1] = weaponStatus.power / max * gaugeWidth;
        targetGaugeX[2] = weaponStatus.rapid / max * gaugeWidth;
    }

    void GetUpTargetStatus()
    {
        int length = gaugeTransforms.Length;
        for (int i = 0; i < length; i++)
        {
            float x = gaugeTransforms[i].sizeDelta.x;
            gaugeTransforms[i].sizeDelta
                = new Vector2((x + targetGaugeX[i]) * 0.5f, gaugeTransforms[i].sizeDelta.y);
        }
    }

    void ChooseWeapon()
    {
        int selectWeaponIndex = selectCounter.Now;
        UserData.instance.playersWeapon[playerNo].SetWeaponStatus(
            selectNo, selectWeaponIndex);
        transform.GetChild(selectCounter.Now).GetComponent<SpriteRenderer>().color
            = Color.black;
        selectedImages[selectNo].sprite = weaponSprites[selectWeaponIndex];
        selectedImages[selectNo].enabled = true;

        selectNo++;
        canSelect = selectNo < WeaponSet.weaponCount;
        SelectWeapon(1);
    }

    void CancelWeapon()
    {
        if (selectNo == 0) return;

        selectNo--;
        int weaponIndex
            = UserData.instance.playersWeapon[playerNo].GetWeaponIndexByIndex(selectNo);
        transform.GetChild(weaponIndex).GetComponent<SpriteRenderer>().color = Color.white;
        selectedImages[selectNo].enabled = false;
        canSelect = true;
    }
}