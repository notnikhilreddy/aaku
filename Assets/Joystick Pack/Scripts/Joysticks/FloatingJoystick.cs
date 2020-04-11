using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;


namespace JoystickPack.JoystickInput {
	public class FloatingJoystick : Joystick
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}
		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical";
		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void OnEnable()
		{
			CreateVirtualAxes();
		}

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		protected override void Start()
		{
			base.Start();
			background.gameObject.SetActive(false);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);
			Vector3 newPos = new Vector3(m_StartPos.x + base.handle.anchoredPosition.x, m_StartPos.y + base.handle.anchoredPosition.y, m_StartPos.z);
			UpdateVirtualAxes(newPos);
			// Debug.Log(base.input);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
			background.gameObject.SetActive(true);
			base.OnPointerDown(eventData);
			m_StartPos = background.anchoredPosition;
			UpdateVirtualAxes(m_StartPos);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			background.gameObject.SetActive(false);
			base.OnPointerUp(eventData);
			UpdateVirtualAxes(m_StartPos);
		}

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}