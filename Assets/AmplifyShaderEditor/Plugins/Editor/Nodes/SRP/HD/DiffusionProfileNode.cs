// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

#if UNITY_2019_1_OR_NEWER
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AmplifyShaderEditor
{
    public static class HDUtilsEx
    {
        private static Type type;
#if UNITY_2019_3_OR_NEWER
        public static Type Type => type == null
            ? type = Type.GetType(
                "UnityEngine.Rendering.HighDefinition.HDUtils, Unity.RenderPipelines.HighDefinition.Runtime")
            : type;
#else
		public static System.Type Type { get { return ( type == null ) ? type =
 System.Type.GetType( "UnityEngine.Experimental.Rendering.HDPipeline.HDUtils, Unity.RenderPipelines.HighDefinition.Runtime" ) : type; } }
#endif

        public static string ConvertVector4ToGUID(Vector4 vector)
        {
            object[] parameters = {vector};
            MethodInfo method;
            method = Type.GetMethod("ConvertVector4ToGUID", BindingFlags.Static | BindingFlags.NonPublic, null,
                new[] {typeof(Vector4)}, null);
            if (method == null)
                method = Type.GetMethod("ConvertVector4ToGUID", new[] {typeof(Vector4)});
            return (string) method.Invoke(null, parameters);
        }

        public static Vector4 ConvertGUIDToVector4(string guid)
        {
            object[] parameters = {guid};
            MethodInfo method;
            method = Type.GetMethod("ConvertGUIDToVector4", BindingFlags.Static | BindingFlags.NonPublic, null,
                new[] {typeof(string)}, null);
            if (method == null)
                method = Type.GetMethod("ConvertGUIDToVector4", new[] {typeof(string)});
            return (Vector4) method.Invoke(null, parameters);
        }
    }

    public static class DiffusionProfileSettingsEx
    {
        private static Type type;
#if UNITY_2019_3_OR_NEWER
        public static Type Type => type == null
            ? type = Type.GetType(
                "UnityEngine.Rendering.HighDefinition.DiffusionProfileSettings, Unity.RenderPipelines.HighDefinition.Runtime")
            : type;
#else
		public static System.Type Type { get { return ( type == null ) ? type =
 System.Type.GetType( "UnityEngine.Experimental.Rendering.HDPipeline.DiffusionProfileSettings, Unity.RenderPipelines.HighDefinition.Runtime" ) : type; } }
#endif

        public static uint Hash(Object m_instance)
        {
            FieldInfo field;
            field = Type.GetField("profile", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                field = Type.GetField("profile");
            var profile = field.GetValue(m_instance);
            var hashField = profile.GetType().GetField("hash");
            return (uint) hashField.GetValue(profile);
        }
    }

    [Serializable]
    [NodeAttributes("Diffusion Profile", "Constants And Properties",
        "Returns Diffusion Profile Hash Id. To be used on Diffusion Profile port on HDRP templates.", KeyCode.None,
        true, 0, int.MaxValue)]
    public sealed class DiffusionProfileNode : PropertyNode
    {
        [SerializeField] private Object m_defaultValue;

        [SerializeField] private Object m_materialValue;

        [SerializeField] private bool m_defaultInspector;

        private bool m_isEditingFields;

        //Vector4 ProfileGUID { get { return ( m_diffusionProfile != null ) ? HDUtils.ConvertGUIDToVector4( AssetDatabase.AssetPathToGUID( AssetDatabase.GetAssetPath( m_diffusionProfile ) ) ) : Vector4.zero; } }
        private uint DefaultHash => m_defaultValue != null ? DiffusionProfileSettingsEx.Hash(m_defaultValue) : 0;

        private uint MaterialHash => m_materialValue != null ? DiffusionProfileSettingsEx.Hash(m_materialValue) : 0;
        //[NonSerialized]
        //private DiffusionProfileSettings m_previousValue;

        protected override void CommonInit(int uniqueId)
        {
            base.CommonInit(uniqueId);
            AddOutputPort(WirePortDataType.FLOAT, Constants.EmptyPortValue);
            m_drawPrecisionUI = false;
            m_currentPrecisionType = PrecisionType.Float;
            m_srpBatcherCompatible = true;
            m_freeType = false;
#if UNITY_2019_3_OR_NEWER
            m_freeType = true;
#endif
        }

        protected override void OnUniqueIDAssigned()
        {
            base.OnUniqueIDAssigned();
            UIUtils.RegisterPropertyNode(this);
        }

        public override void CopyDefaultsToMaterial()
        {
            m_materialValue = m_defaultValue;
        }

        public override void DrawSubProperties()
        {
            m_defaultValue = EditorGUILayoutObjectField(Constants.DefaultValueLabel, m_defaultValue,
                DiffusionProfileSettingsEx.Type, true) /*as UnityEngine.Object*/;
        }

        public override void DrawMaterialProperties()
        {
            if (m_materialMode)
                EditorGUI.BeginChangeCheck();

            m_materialValue = EditorGUILayoutObjectField(Constants.MaterialValueLabel, m_materialValue,
                DiffusionProfileSettingsEx.Type, true) /*as DiffusionProfileSettings*/;

            if (m_materialMode && EditorGUI.EndChangeCheck()) m_requireMaterialUpdate = true;
        }

        public override void DrawMainPropertyBlock()
        {
            EditorGUILayout.BeginVertical();
            {
                if (m_freeType)
                {
                    var parameterType =
                        (PropertyType) EditorGUILayoutEnumPopup(ParameterTypeStr, m_currentParameterType);
                    if (parameterType != m_currentParameterType)
                    {
                        ChangeParameterType(parameterType);
                        BeginPropertyFromInspectorCheck();
                    }
                }

                if (m_freeName)
                    switch (m_currentParameterType)
                    {
                        case PropertyType.Property:
                        case PropertyType.InstancedProperty:
                        {
                            m_defaultInspector = EditorGUILayoutToggle("Default Inspector", m_defaultInspector);
                            if (m_defaultInspector)
                                EditorGUILayout.HelpBox(
                                    "While \"Default Inspector\" is turned ON you can't reorder this property or change it's name, and you can only have one per shader, use it only if you intend to share this shader with non-ASE users",
                                    MessageType.Info);
                            EditorGUI.BeginDisabledGroup(m_defaultInspector);
                            ShowPropertyInspectorNameGUI();
                            ShowPropertyNameGUI(true);
                            EditorGUI.EndDisabledGroup();
                            ShowVariableMode();
                            ShowAutoRegister();
                            ShowPrecision();
                            ShowToolbar();
                        }
                            break;
                        case PropertyType.Global:
                        {
                            ShowPropertyInspectorNameGUI();
                            ShowPropertyNameGUI(false);
                            ShowVariableMode();
                            ShowAutoRegister();
                            ShowPrecision();
                            ShowDefaults();
                        }
                            break;
                        case PropertyType.Constant:
                        {
                            ShowPropertyInspectorNameGUI();
                            ShowPrecision();
                            ShowDefaults();
                        }
                            break;
                    }
            }
            EditorGUILayout.EndVertical();
        }

        //public override void OnNodeLayout( DrawInfo drawInfo )
        //{
        //	base.OnNodeLayout( drawInfo );

        //	m_propertyDrawPos = m_remainingBox;
        //	m_propertyDrawPos.width = drawInfo.InvertedZoom * Constants.FLOAT_DRAW_WIDTH_FIELD_SIZE * 2;
        //	m_propertyDrawPos.height = drawInfo.InvertedZoom * Constants.FLOAT_DRAW_HEIGHT_FIELD_SIZE;
        //}

        //public override void DrawGUIControls( DrawInfo drawInfo )
        //{
        //	base.DrawGUIControls( drawInfo );

        //	if( drawInfo.CurrentEventType != EventType.MouseDown )
        //		return;

        //	Rect hitBox = m_remainingBox;
        //	bool insideBox = hitBox.Contains( drawInfo.MousePosition );

        //	if( insideBox )
        //	{
        //		GUI.FocusControl( null );
        //		m_isEditingFields = true;
        //	}
        //	else if( m_isEditingFields && !insideBox )
        //	{
        //		GUI.FocusControl( null );
        //		m_isEditingFields = false;
        //	}
        //}

        //GUIStyle GetStyle( string styleName )
        //{
        //	GUIStyle s = GUI.skin.FindStyle( styleName ) ?? EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).FindStyle( styleName );
        //	if( s == null )
        //	{
        //		Debug.LogError( "Missing built-in guistyle " + styleName );
        //		s = GUIStyle.none;
        //	}
        //	return s;
        //}

        //public override void Draw( DrawInfo drawInfo )
        //{
        //	base.Draw( drawInfo );

        //	if( !m_isVisible )
        //		return;

        //	var cache = EditorStyles.objectField.fontSize;
        //	EditorStyles.objectField.fontSize = (int)(9 * drawInfo.InvertedZoom);
        //	var style = GetStyle( "ObjectFieldButton" );
        //	var sw = style.stretchWidth;
        //	style.stretchWidth = false;
        //	//style.isHeightDependantOnWidth
        //	style.fixedWidth = (int)( 16 * drawInfo.InvertedZoom );
        //	style.fixedHeight = (int)( 16 * drawInfo.InvertedZoom );
        //	//if( m_isEditingFields && m_currentParameterType != PropertyType.Global )
        //	//{
        //	float labelWidth = EditorGUIUtility.labelWidth;
        //		EditorGUIUtility.labelWidth = 0;

        //		if( m_materialMode && m_currentParameterType != PropertyType.Constant )
        //		{
        //			EditorGUI.BeginChangeCheck();
        //			m_materialValue = EditorGUIObjectField( m_propertyDrawPos, m_materialValue, typeof( DiffusionProfileSettings ), true ) as DiffusionProfileSettings;
        //			if( EditorGUI.EndChangeCheck() )
        //			{
        //				PreviewIsDirty = true;
        //				m_requireMaterialUpdate = true;
        //				if( m_currentParameterType != PropertyType.Constant )
        //					BeginDelayedDirtyProperty();
        //			}
        //		}
        //		else
        //		{
        //			EditorGUI.BeginChangeCheck();
        //			m_defaultValue = EditorGUIObjectField( m_propertyDrawPos, m_defaultValue, typeof( DiffusionProfileSettings ), true ) as DiffusionProfileSettings;
        //			if( EditorGUI.EndChangeCheck() )
        //			{
        //				PreviewIsDirty = true;
        //				BeginDelayedDirtyProperty();
        //			}
        //		}
        //		EditorGUIUtility.labelWidth = labelWidth;

        //	style.fixedWidth = 0;
        //	style.fixedHeight = 0;
        //	style.stretchWidth = sw;
        //	EditorStyles.objectField.fontSize = cache;
        //	//}
        //	//else if( drawInfo.CurrentEventType == EventType.Repaint )
        //	//{
        //	//	bool guiEnabled = GUI.enabled;
        //	//	GUI.enabled = m_currentParameterType != PropertyType.Global;
        //	//	Rect fakeField = m_propertyDrawPos;
        //	//	if( GUI.enabled )
        //	//	{
        //	//		Rect fakeLabel = m_propertyDrawPos;
        //	//		fakeLabel.xMax = fakeField.xMin;
        //	//		EditorGUIUtility.AddCursorRect( fakeLabel, MouseCursor.SlideArrow );
        //	//		EditorGUIUtility.AddCursorRect( fakeField, MouseCursor.Text );
        //	//	}
        //	//	bool currMode = m_materialMode && m_currentParameterType != PropertyType.Constant;
        //	//	var value = currMode ? m_materialValue : m_defaultValue;

        //	//	//if( m_previousValue != value )
        //	//	//{
        //	//	//	m_previousValue = value;
        //	//	// string stuff
        //	//	//}

        //	//	//GUI.Label( fakeField, m_fieldText, UIUtils.MainSkin.textField );
        //	//	GUI.enabled = guiEnabled;
        //	//}
        //}


        public override string GenerateShaderForOutput(int outputId, ref MasterNodeDataCollector dataCollector,
            bool ignoreLocalvar)
        {
            base.GenerateShaderForOutput(outputId, ref dataCollector, ignoreLocalvar);

            if (m_currentParameterType != PropertyType.Constant)
            {
                if (m_defaultInspector)
                    return "_DiffusionProfileHash";
                return PropertyData(dataCollector.PortCategory);
            }

#if UNITY_2019_3_OR_NEWER
            return RoundTrip.ToRoundTrip(HDShadowUtilsEx.Asfloat(DefaultHash));
#else
			return "asfloat(" + DefaultHash.ToString() + ")";
#endif
        }


        public override string GetUniformValue()
        {
            if (m_defaultInspector)
                return "float _DiffusionProfileHash";
            return base.GetUniformValue();
        }

        public override bool GetUniformData(out string dataType, out string dataName, ref bool fullValue)
        {
            if (m_defaultInspector)
            {
                dataType = "float";
                dataName = "_DiffusionProfileHash";
                return true;
            }

            return base.GetUniformData(out dataType, out dataName, ref fullValue);
        }

        public override string GetPropertyValue()
        {
            var asset = Vector4.zero;
            if (m_defaultValue != null)
                asset = HDUtilsEx.ConvertGUIDToVector4(
                    AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_defaultValue)));
            var assetVec = RoundTrip.ToRoundTrip(asset.x) + ", " + RoundTrip.ToRoundTrip(asset.y) + ", " +
                           RoundTrip.ToRoundTrip(asset.z) + ", " + RoundTrip.ToRoundTrip(asset.w);
            var lineOne = string.Empty;
            var lineTwo = string.Empty;
            if (m_defaultInspector)
            {
                lineOne = PropertyAttributes + "[HideInInspector]_DiffusionProfileAsset(\"" + m_propertyInspectorName +
                          "\", Vector) = ( " + assetVec + " )";
                lineTwo = "\n[HideInInspector]_DiffusionProfileHash(\"" + m_propertyInspectorName + "\", Float) = " +
                          RoundTrip.ToRoundTrip(HDShadowUtilsEx.Asfloat(DefaultHash));
            }
            else
            {
                lineOne = PropertyAttributes + "[ASEDiffusionProfile(" + m_propertyName + ")]" + m_propertyName +
                          "_asset(\"" + m_propertyInspectorName + "\", Vector) = ( " + assetVec + " )";
                lineTwo = "\n[HideInInspector]" + m_propertyName + "(\"" + m_propertyInspectorName + "\", Float) = " +
                          RoundTrip.ToRoundTrip(HDShadowUtilsEx.Asfloat(DefaultHash));
            }

            return lineOne + lineTwo;
        }

        public override void UpdateMaterial(Material mat)
        {
            base.UpdateMaterial(mat);

            if (UIUtils.IsProperty(m_currentParameterType) && !InsideShaderFunction)
                if (m_materialValue != null)
                {
                    var asset = HDUtilsEx.ConvertGUIDToVector4(
                        AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_materialValue)));
                    if (m_defaultInspector)
                    {
                        mat.SetVector("_DiffusionProfileAsset", asset);
                        mat.SetFloat("_DiffusionProfileHash", HDShadowUtilsEx.Asfloat(MaterialHash));
                    }
                    else
                    {
                        mat.SetVector(m_propertyName + "_asset", asset);
                        mat.SetFloat(m_propertyName, HDShadowUtilsEx.Asfloat(MaterialHash));
                    }
                }
        }

        public override void ForceUpdateFromMaterial(Material material)
        {
            var propertyAsset = m_propertyName + "_asset";
            if (m_defaultInspector)
                propertyAsset = "_DiffusionProfileAsset";

            if (UIUtils.IsProperty(m_currentParameterType) && material.HasProperty(propertyAsset))
            {
                var guid = HDUtilsEx.ConvertVector4ToGUID(material.GetVector(propertyAsset));
                var profile = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid),
                    DiffusionProfileSettingsEx.Type);
                if (profile != null)
                    m_materialValue = profile;
            }
        }

        public override void WriteToString(ref string nodeInfo, ref string connectionsInfo)
        {
            base.WriteToString(ref nodeInfo, ref connectionsInfo);
            var defaultGuid = m_defaultValue != null
                ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_defaultValue))
                : "0";
            IOUtils.AddFieldValueToString(ref nodeInfo, defaultGuid);
            var materialGuid = m_materialValue != null
                ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m_materialValue))
                : "0";
            IOUtils.AddFieldValueToString(ref nodeInfo, materialGuid);
            IOUtils.AddFieldValueToString(ref nodeInfo, m_defaultInspector);
        }

        public override void ReadFromString(ref string[] nodeParams)
        {
            if (UIUtils.CurrentShaderVersion() > 17004)
                base.ReadFromString(ref nodeParams);
            else
                ParentReadFromString(ref nodeParams);

            var defaultGuid = GetCurrentParam(ref nodeParams);
            if (defaultGuid.Length > 1)
                m_defaultValue = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(defaultGuid),
                    DiffusionProfileSettingsEx.Type);
            if (UIUtils.CurrentShaderVersion() > 17004)
            {
                var materialGuid = GetCurrentParam(ref nodeParams);
                if (materialGuid.Length > 1)
                    m_materialValue = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(materialGuid),
                        DiffusionProfileSettingsEx.Type);
            }

            if (UIUtils.CurrentShaderVersion() > 17900)
                m_defaultInspector = Convert.ToBoolean(GetCurrentParam(ref nodeParams));
        }

        public override void ReadOutputDataFromString(ref string[] nodeParams)
        {
            base.ReadOutputDataFromString(ref nodeParams);
            if (UIUtils.CurrentShaderVersion() < 17005)
                m_outputPorts[0].ChangeProperties(Constants.EmptyPortValue, WirePortDataType.FLOAT, false);
        }

        public override string GetPropertyValStr()
        {
            if (m_defaultInspector)
                return "_DiffusionProfileHash";
            return PropertyName;
        }

        private static class HDShadowUtilsEx
        {
            private static Type type;

            public static Type Type => type == null
                ? type = Type.GetType(
                    "UnityEngine.Rendering.HighDefinition.HDShadowUtils, Unity.RenderPipelines.HighDefinition.Runtime")
                : type;

            public static float Asfloat(uint val)
            {
#if UNITY_2019_3_OR_NEWER
                object[] parameters = {val};
                var method = Type.GetMethod("Asfloat", new[] {typeof(uint)});
                return (float) method.Invoke(null, parameters);
#else
				return HDShadowUtils.Asfloat( val );
#endif
            }

            public static uint Asuint(float val)
            {
#if UNITY_2019_3_OR_NEWER

                object[] parameters = {val};
                var method = Type.GetMethod("Asuint", new[] {typeof(float)});
                return (uint) method.Invoke(null, parameters);
#else
				return HDShadowUtils.Asuint( val );
#endif
            }
        }

        private static class RoundTrip
        {
            private static readonly string[] zeros = new string[1000];

            static RoundTrip()
            {
                for (var i = 0; i < zeros.Length; i++) zeros[i] = new string('0', i);
            }

            public static string ToRoundTrip(double value)
            {
                var str = value.ToString("r");
                var x = str.IndexOf('E');
                if (x < 0) return str;

                var x1 = x + 1;
                var exp = str.Substring(x1, str.Length - x1);
                var e = int.Parse(exp);

                string s = null;
                var numDecimals = 0;
                if (value < 0)
                {
                    var len = x - 3;
                    if (e >= 0)
                    {
                        if (len > 0)
                        {
                            s = str.Substring(0, 2) + str.Substring(3, len);
                            numDecimals = len;
                        }
                        else
                        {
                            s = str.Substring(0, 2);
                        }
                    }
                    else
                    {
                        // remove the leading minus sign
                        if (len > 0)
                        {
                            s = str.Substring(1, 1) + str.Substring(3, len);
                            numDecimals = len;
                        }
                        else
                        {
                            s = str.Substring(1, 1);
                        }
                    }
                }
                else
                {
                    var len = x - 2;
                    if (len > 0)
                    {
                        s = str[0] + str.Substring(2, len);
                        numDecimals = len;
                    }
                    else
                    {
                        s = str[0].ToString();
                    }
                }

                if (e >= 0)
                {
                    e = e - numDecimals;
                    var z = e < zeros.Length ? zeros[e] : new string('0', e);
                    s = s + z;
                }
                else
                {
                    e = -e - 1;
                    var z = e < zeros.Length ? zeros[e] : new string('0', e);
                    if (value < 0)
                        s = "-0." + z + s;
                    else
                        s = "0." + z + s;
                }

                return s;
            }
        }
    }
}
#endif