using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_Joystick : UI_Base
{
	enum GameObjects
	{
		JoystickBG,
		JoystickCursor,
	}

	private GameObject _background;
	private GameObject _cursor;
	private float _radius;

    private RectTransform _bgRect;
    private RectTransform _cursorRect;
    private Vector2 _initAnchoredPos;
    private Vector2 _pointerDownLocalPos;

    public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindObjects(typeof(GameObjects));

		_background = GetObject((int)GameObjects.JoystickBG);
		_cursor = GetObject((int)GameObjects.JoystickCursor);
		_radius = _background.GetComponent<RectTransform>().sizeDelta.y / 5;

        _bgRect = _background.GetComponent<RectTransform>();
        _cursorRect = _cursor.GetComponent<RectTransform>();
        _initAnchoredPos = _cursorRect.anchoredPosition;

        gameObject.BindEvent(OnPointerDown, type: EUIEvent.PointerDown);
		gameObject.BindEvent(OnPointerUp, type: EUIEvent.PointerUp);
		gameObject.BindEvent(OnDrag, type: EUIEvent.Drag);

		GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
		GetComponent<Canvas>().worldCamera = Camera.main;

		return true;
	}

    #region Event
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _bgRect, eventData.position, eventData.pressEventCamera, out _pointerDownLocalPos);

        Managers.Game.JoystickState = EJoystickState.PointerDown;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _cursorRect.anchoredPosition = _initAnchoredPos;

        Managers.Game.MoveDir = Vector2.zero;
        Managers.Game.JoystickState = EJoystickState.PointerUp;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _bgRect, eventData.position, eventData.pressEventCamera, out currentLocalPos);

        Vector2 delta = currentLocalPos - _pointerDownLocalPos;

        float moveDist = Mathf.Min(delta.magnitude, _radius);
        Vector2 moveDir = delta.sqrMagnitude > 0f ? delta.normalized : Vector2.zero;

        _cursorRect.anchoredPosition = _initAnchoredPos + moveDir * moveDist;

        Managers.Game.MoveDir = moveDir;
        Managers.Game.JoystickState = EJoystickState.Drag;
    }
    #endregion
}
