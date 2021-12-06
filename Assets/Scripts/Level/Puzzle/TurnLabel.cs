using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnLabel : MonoBehaviour
{

	[SerializeField] private Text _turnText = null;
	[SerializeField] private Sprite _playerIco = null;
	[SerializeField] private Sprite _bossIco = null;
	[SerializeField] private Image _holderImage = null;

	public void setBossIcon(Sprite ico)
	{
		_bossIco = ico;
	}

	public void setTurn(bool isPlayerTurn){
		if (isPlayerTurn){
			_holderImage.sprite = _playerIco;
			_turnText.text = "Player Turn";
		}
		else {
			_holderImage.sprite = _bossIco;
			_turnText.text = "Real player enemy Turn";
		}
	}
}
