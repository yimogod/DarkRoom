using System;
using UnityEngine;

namespace DarkRoom.Utility {
	#region GameCursor
	public class CGameCursor : MonoBehaviour{
		private static CGameCursor m_instance;

		//拖进来并赋值
		public CursorSet CursorSet;

		//当前的光标
		private CursorType m_activeCursor = CursorType.None;
		//当前期望的光标, 目前不知道干啥用?
		private CursorType m_desiredCursor = CursorType.None;

		//cast光标是否可用, 只有在施法阶段才可用
		private bool m_castCursorValid = false;
		private Texture2D m_castCursor = null;
		private Texture2D m_castCursorInvalid = null;
		private Texture2D m_castCursorLoS = null;
		private Texture2D m_castCursorMove = null;
		//当前施法图标的texture的像素数据
		private Color[] m_castCursorBuffer;

		private Vector2 m_centerHotSpot = new Vector2(32f, 32f);
		private bool m_ShowCursor = true;

		public static CGameCursor Instance {
			get{ return m_instance; }
		}

		private void Awake(){
			if (m_instance == null){
				m_instance = this;
			}else{
				Debug.LogError("Singleton component 'GameCursor' awoke multiple times! Remove this component from all objects except global prefabs. (Second object: '" + base.name + "')");
			}

			Cursor.lockState = CursorLockMode.None;
			//Cursor.visible = false;

			m_activeCursor = CursorType.None;
			m_desiredCursor = CursorType.Normal;
		}

		public bool DisableCursor { get; private set; }

		/// <summary>
		/// 当前的光标类型
		/// </summary>
		public CursorType ActiveCursor{
			get{ return m_activeCursor; }
		}

		/// <summary>
		/// 当前的光标是否是指向性光标--攻击和施法
		/// </summary>
		public bool ActiveCursorIsTargeting{
			get{ return IsTargetCursor(ActiveCursor); }
		}

		/// <summary>
		/// 根据规则获取真正期望的光标
		/// </summary>
		public CursorType DesiredCursor{
			get{
				if (IsCastCursor(m_desiredCursor))return CursorType.CastAbilityInvalid;
				return m_desiredCursor;
			}

			set{ m_desiredCursor = value; }
		}

		public bool LockCursor{
			get{ return Cursor.visible; }
			set{ Cursor.visible = value; }
		}

		//是否显示cursor
		//如果设隐藏, 那么我们会给系统一个不可见的cursor
		public bool ShowCursor{
			get{ return m_ShowCursor; }
			private set{
				m_ShowCursor = value;
				DisableCursor = !value;
				m_activeCursor = CursorType.None;
				if (!m_ShowCursor)
					Cursor.SetCursor(CursorSet.InvisibleCursor, Vector2.zero, CursorMode.Auto);
			}
		}

		public bool IsAttackCursor(CursorType type){
			return type == CursorType.Attack || type == CursorType.SpecialAttack || type == CursorType.AttackAdvantage;
		}

		public bool IsCastCursor(CursorType type){
			return type == CursorType.CastAbility || type == CursorType.CastAbilityInvalid ||
				type == CursorType.CastAbilityNoLOS || type == CursorType.CastAbilityFar || type == CursorType.SpecialAttack;
		}

		public bool IsInteractCursor(CursorType type){
			return type == CursorType.Examine || type == CursorType.DuplicateItem || type == CursorType.Interact ||
				type == CursorType.Talk || type == CursorType.Stealing || type == CursorType.StealingLocked ||
				type == CursorType.CloseDoor || type == CursorType.OpenDoor || type == CursorType.LockedDoor ||
				type == CursorType.Loot || type == CursorType.Disarm;
		}

		public bool IsTargetCursor(CursorType type){
			return IsAttackCursor(type) || IsCastCursor(type);
		}



		public void ResetShowCursor(){
			ShowCursor = true;
		}

		public void BeginCasting(System.Object abil){
			NewCastingCursor(ref m_castCursor, CursorSet.CastAbilityFrame);
			NewCastingCursor(ref m_castCursorInvalid, CursorSet.CastAbilityInvalidFrame);
			NewCastingCursor(ref m_castCursorMove, CursorSet.CastAbilityMoveFrame);
			NewCastingCursor(ref m_castCursorLoS, CursorSet.CastAbilityNoLosFrame);

			if ((CursorSet.CastAbilityFrameData == null) && (CursorSet.CastAbilityFrame != null)){
				CursorSet.CastAbilityFrameData = CursorSet.CastAbilityFrame.GetPixels();
			}

			if ((CursorSet.CastAbilityInvalidFrameData == null) && (CursorSet.CastAbilityInvalidFrame != null)){
				CursorSet.CastAbilityInvalidFrameData = CursorSet.CastAbilityInvalidFrame.GetPixels();
			}

			if ((CursorSet.CastAbilityMoveFrameData == null) && (CursorSet.CastAbilityMoveFrame != null)){
				CursorSet.CastAbilityMoveFrameData = CursorSet.CastAbilityMoveFrame.GetPixels();
			}

			if ((CursorSet.CastAbilityNoLosFrameData == null) && (CursorSet.CastAbilityNoLosFrame != null)){
				CursorSet.CastAbilityNoLosFrameData = CursorSet.CastAbilityNoLosFrame.GetPixels();
			}

			if ((CursorSet.CastAbilityIconMaskData == null) && (CursorSet.CastAbilityIconMask != null)){
				CursorSet.CastAbilityIconMaskData = CursorSet.CastAbilityIconMask.GetPixels();
			}

			if ((m_castCursorBuffer == null) || (m_castCursorBuffer.Length != CursorSet.CastAbilityFrameData.Length)){
				m_castCursorBuffer = new Color[Instance.CursorSet.CastAbilityFrameData.Length];
			}

			CursorSet.CastAbilityFrameData.CopyTo(m_castCursorBuffer, 0);
			try{
				//CreateCastCursorForIcon(CursorSet.CastAbilityFrameData, abil.icon, _castCursor, 1f);
				//CreateCastCursorForIcon(CursorSet.CastAbilityInvalidFrameData, abil.icon, _castCursorInvalid, 0.4f);
				//CreateCastCursorForIcon(CursorSet.CastAbilityNoLosFrameData, abil.icon, _castCursorLoS, 0.4f);
				//CreateCastCursorForIcon(CursorSet.CastAbilityMoveFrameData, abil.icon, _castCursorMove, 1f);
				//_activeCursor = CursorType.Normal;
				//_castCursorValid = true;
			}catch (Exception exception){
				Debug.LogException(exception);
				m_castCursorValid = false;
			}
		}

		//结束施法
		public void EndCasting(){
			m_castCursorValid = false;
		}

		private void CreateCastCursorForIcon(Color[] frameColorBuffer, Texture2D abilityIcon, Texture2D outputTexture, float iconSaturation){
			if(outputTexture == null)return;
			if(frameColorBuffer == null)return;
			if(abilityIcon == null)return;

			if ((m_castCursorBuffer == null) || (m_castCursorBuffer.Length != frameColorBuffer.Length)){
				m_castCursorBuffer = new Color[frameColorBuffer.Length];
			}

			frameColorBuffer.CopyTo(m_castCursorBuffer, 0);
			int width = Instance.CursorSet.CastAbilityIconMask.width;
			int height = Instance.CursorSet.CastAbilityIconMask.height;
			for (int i = 0; i < width; i++){
				for (int j = 0; j < height; j++){
					int index = i + (j * width);
					if (Instance.CursorSet.CastAbilityIconMaskData[index].r > 0f){
						Color pixelBilinear = abilityIcon.GetPixelBilinear(((float) i) / ((float) width), ((float) j) / ((float) height));
						if (iconSaturation != 1f)
						{

						//pixelBilinear = new HSBColor(pixelBilinear) { s = color2.s * iconSaturation, b = color2.b * iconSaturation }.ToColor();
						}
						Color color3 = frameColorBuffer[index];
						pixelBilinear.a = Instance.CursorSet.CastAbilityIconMaskData[index].r;
						pixelBilinear.r = (color3.r * color3.a) + (pixelBilinear.r * (1f - color3.a));
						pixelBilinear.g = (color3.g * color3.a) + (pixelBilinear.g * (1f - color3.a));
						pixelBilinear.b = (color3.b * color3.a) + (pixelBilinear.b * (1f - color3.a));
						pixelBilinear.a = 1f - ((1f - color3.a) * (1f - pixelBilinear.a));
						m_castCursorBuffer[index] = pixelBilinear;
					}
				}
			}
			outputTexture.SetPixels(m_castCursorBuffer);
			outputTexture.Apply();
		}

		private CursorType HandleDownState(CursorType current, CursorType up, CursorType down){
			if (current != up && current != down)return current;
			if (CMouseInput.Instance.HasDown)return down;
			return up;
		}

		private void NewCastingCursor(ref Texture2D texture, Texture2D frame){
			if (texture != null)	GameObject.Destroy(texture);
			if (frame != null)
				texture = new Texture2D(frame.width, frame.height, TextureFormat.RGBA32, false);
		}

		private void LateUpdate(){
			if (CursorSet == null)return;
			if (!ShowCursor)return;

			CursorType desiredCursor = DesiredCursor;
			desiredCursor = HandleDownState(desiredCursor, CursorType.Normal, CursorType.NormalHeld);
			desiredCursor = HandleDownState(desiredCursor, CursorType.Walk, CursorType.WalkHeld);
			desiredCursor = HandleDownState(desiredCursor, CursorType.Disengage, CursorType.DisengageHeld);
			desiredCursor = HandleDownState(desiredCursor, CursorType.SelectionSubtract, CursorType.SelectionSubtractHeld);
			desiredCursor = HandleDownState(desiredCursor, CursorType.SelectionAdd, CursorType.SelectionAddHeld);

			//如果cursor一样就不更换了
			if (m_activeCursor == desiredCursor)return;

			m_activeCursor = desiredCursor;

			switch (desiredCursor){
				case CursorType.Normal:
				Cursor.SetCursor(CursorSet.NormalCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Walk:
				Cursor.SetCursor(CursorSet.WalkCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.NoWalk:
				Cursor.SetCursor(CursorSet.NoWalkCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.RotateFormation:
				Cursor.SetCursor(CursorSet.RotateFormationCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.Attack:
				Cursor.SetCursor(CursorSet.AttackCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.OpenDoor:
				Cursor.SetCursor(CursorSet.OpenDoorCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.CloseDoor:
				Cursor.SetCursor(CursorSet.CloseDoorCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.LockedDoor:
				Cursor.SetCursor(CursorSet.LockedDoorCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.AreaTransition:
				Cursor.SetCursor(CursorSet.AreaTransition, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Examine:
				Cursor.SetCursor(CursorSet.ExamineCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Talk:
				Cursor.SetCursor(CursorSet.TalkCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Interact:
				Cursor.SetCursor(CursorSet.InteractCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.DoubleArrow_L_R:
				Cursor.SetCursor(CursorSet.DoubleArrow_L_R, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.DoubleArrow_U_D:
				Cursor.SetCursor(CursorSet.DoubleArrow_U_D, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.DoubleArrow_DL_UR:
				Cursor.SetCursor(CursorSet.DoubleArrow_DL_UR, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.DoubleArrow_UL_DR:
				Cursor.SetCursor(CursorSet.DoubleArrow_UL_DR, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.Disarm:
				Cursor.SetCursor(CursorSet.DisarmCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Stealing:
				Cursor.SetCursor(CursorSet.Stealing, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Loot:
				Cursor.SetCursor(CursorSet.LootCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.NormalHeld:
				Cursor.SetCursor(CursorSet.NormalHeldCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.WalkHeld:
				Cursor.SetCursor(CursorSet.WalkHeldCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.StealingLocked:
				Cursor.SetCursor(CursorSet.StealingLocked, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.Disengage:
				Cursor.SetCursor(CursorSet.DisengageCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.DisengageHeld:
				Cursor.SetCursor(CursorSet.DisengageHeldCursor, m_centerHotSpot, CursorMode.Auto);
				break;
				case CursorType.AttackAdvantage:
				Cursor.SetCursor(CursorSet.AttackAdvantageCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.SelectionAdd:
				Cursor.SetCursor(CursorSet.SelectionAddCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.SelectionSubtract:
				Cursor.SetCursor(CursorSet.SelectionSubtractCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.SelectionAddHeld:
				Cursor.SetCursor(CursorSet.SelectionAddHeldCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.SelectionSubtractHeld:
				Cursor.SetCursor(CursorSet.SelectionSubtractHeldCursor, Vector2.zero, CursorMode.Auto);
				break;
				case CursorType.DuplicateItem:
				Cursor.SetCursor(CursorSet.DuplicateItemCursor, Vector2.zero, CursorMode.Auto);
				break;

				case CursorType.CastAbility:
					if (!m_castCursorValid || m_castCursor == null)
						Cursor.SetCursor(CursorSet.CastAbility, Vector2.zero, CursorMode.Auto);
					else
						Cursor.SetCursor(m_castCursor, Vector2.zero, CursorMode.Auto);
					break;
				case CursorType.CastAbilityInvalid:
					if (!m_castCursorValid || (m_castCursorInvalid == null))
						Cursor.SetCursor(CursorSet.CastAbilityInvalid, Vector2.zero, CursorMode.Auto);
					else
						Cursor.SetCursor(m_castCursorInvalid, Vector2.zero, CursorMode.Auto);
					break;
				case CursorType.CastAbilityNoLOS:
					if (!m_castCursorValid || (m_castCursorLoS == null))
						Cursor.SetCursor(CursorSet.CastAbilityNoLOS, Vector2.zero, CursorMode.Auto);
					else
						Cursor.SetCursor(m_castCursorLoS, Vector2.zero, CursorMode.Auto);
					break;

				case CursorType.CastAbilityFar:
					if (!m_castCursorValid || (m_castCursorMove == null))
						Cursor.SetCursor(CursorSet.CastAbilityFar, Vector2.zero, CursorMode.Auto);
					else
						Cursor.SetCursor(m_castCursorMove, Vector2.zero, CursorMode.Auto);
					break;
				case CursorType.SpecialAttack:
					if (!m_castCursorValid || (m_castCursor == null))
						Cursor.SetCursor(CursorSet.SpecialAttackCursor, Vector2.zero, CursorMode.Auto);
					else
						Cursor.SetCursor(m_castCursor, Vector2.zero, CursorMode.Auto);
					break;

				default:
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
				break;
			}
		}

		private void OnDestroy(){
			m_instance = null;
			ResetShowCursor();
		}
	}
	#endregion

	#region CursorSet
	[Serializable]
	public class CursorSet{
		public Texture2D AttackCursor;
		public Texture2D NoWalkCursor;

		public Texture2D AreaTransition;
		public Texture2D ArrowDown;
		public Texture2D ArrowDownLeft;
		public Texture2D ArrowDownRight;
		public Texture2D ArrowLeft;
		public Texture2D ArrowRight;
		public Texture2D ArrowUp;
		public Texture2D ArrowUpLeft;
		public Texture2D ArrowUpRight;
		public Texture2D AttackAdvantageCursor;
		public Texture2D CastAbility;
		public Texture2D CastAbilityFar;
		public Texture2D CastAbilityFrame;
		public Texture2D CastAbilityIconMask;
		public Texture2D CastAbilityInvalid;
		public Texture2D CastAbilityInvalidFrame;
		public Texture2D CastAbilityMoveFrame;
		public Texture2D CastAbilityNoLOS;
		public Texture2D CastAbilityNoLosFrame;
		public Texture2D CloseDoorCursor;
		public Texture2D DisarmCursor;
		public Texture2D DisengageCursor;
		public Texture2D DisengageHeldCursor;
		public Texture2D DoubleArrow_DL_UR;
		public Texture2D DoubleArrow_L_R;
		public Texture2D DoubleArrow_U_D;
		public Texture2D DoubleArrow_UL_DR;
		public Texture2D DuplicateItemCursor;
		public Texture2D ExamineCursor;
		public Texture2D InteractCursor;
		public Texture2D InvisibleCursor;
		public Texture2D LockedDoorCursor;
		public Texture2D LootCursor;
		public Texture2D NormalCursor;
		public Texture2D NormalHeldCursor;
		public Texture2D OpenDoorCursor;
		public Texture2D RotateFormationCursor;
		public Texture2D SelectionAddCursor;
		public Texture2D SelectionAddHeldCursor;
		public Texture2D SelectionSubtractCursor;
		public Texture2D SelectionSubtractHeldCursor;
		public Texture2D SpecialAttackCursor;
		public Texture2D Stealing;
		public Texture2D StealingLocked;
		public Texture2D TalkCursor;
		public Texture2D WalkCursor;
		public Texture2D WalkHeldCursor;

		public Color[] CastAbilityFrameData { get; set; }

		public Color[] CastAbilityIconMaskData { get; set; }

		public Color[] CastAbilityInvalidFrameData { get; set; }

		public Color[] CastAbilityMoveFrameData { get; set; }

		public Color[] CastAbilityNoLosFrameData { get; set; }
	}
	#endregion

	#region CursorType
	//光标类型
	public enum CursorType{
		None,
		Normal,
		Walk,
		NoWalk,
		RotateFormation,
		Attack,
		OpenDoor,
		CloseDoor,
		LockedDoor,
		AreaTransition,
		Examine,
		Talk,
		Interact,
		CastAbility,
		CastAbilityInvalid,
		ArrowUp,
		ArrowRight,
		ArrowDown,
		ArrowLeft,
		ArrowUpRight,
		ArrowDownRight,
		ArrowUpLeft,
		ArrowDownLeft,
		DoubleArrow_L_R,
		DoubleArrow_U_D,
		DoubleArrow_DL_UR,
		DoubleArrow_UL_DR,
		Disarm,
		CastAbilityNoLOS,
		CastAbilityFar,
		Stealing,
		Loot,
		NormalHeld,
		WalkHeld,
		SpecialAttack,
		StealingLocked,
		Disengage,
		DisengageHeld,
		AttackAdvantage,
		SelectionAdd,
		SelectionSubtract,
		SelectionAddHeld,
		SelectionSubtractHeld,
		DuplicateItem
	}
	#endregion

}