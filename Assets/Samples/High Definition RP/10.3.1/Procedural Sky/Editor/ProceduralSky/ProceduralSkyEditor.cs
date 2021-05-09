using UnityEngine.Rendering.HighDefinition;

namespace UnityEditor.Rendering.HighDefinition
{
    [CanEditMultipleObjects]
    [VolumeComponentEditor(typeof(ProceduralSky))]
    internal class ProceduralSkySettingsEditor
        : SkySettingsEditor
    {
        private SerializedDataParameter m_AtmosphericThickness;
        private SerializedDataParameter m_EnableSunDisk;
        private SerializedDataParameter m_GroundColor;
        private SerializedDataParameter m_SkyTint;
        private SerializedDataParameter m_SunSize;
        private SerializedDataParameter m_SunSizeConvergence;

        public override void OnEnable()
        {
            base.OnEnable();

            // Procedural sky orientation depends on the sun direction
            m_CommonUIElementsMask = 0xFFFFFFFF & ~(uint) SkySettingsUIElement.Rotation;

            var o = new PropertyFetcher<ProceduralSky>(serializedObject);

            m_SunSize = Unpack(o.Find(x => x.sunSize));
            m_SunSizeConvergence = Unpack(o.Find(x => x.sunSizeConvergence));
            m_AtmosphericThickness = Unpack(o.Find(x => x.atmosphereThickness));
            m_SkyTint = Unpack(o.Find(x => x.skyTint));
            m_GroundColor = Unpack(o.Find(x => x.groundColor));
            m_EnableSunDisk = Unpack(o.Find(x => x.enableSunDisk));
        }

        public override void OnInspectorGUI()
        {
            PropertyField(m_EnableSunDisk);
            PropertyField(m_SunSize);
            PropertyField(m_SunSizeConvergence);
            PropertyField(m_AtmosphericThickness);
            PropertyField(m_SkyTint);
            PropertyField(m_GroundColor);

            EditorGUILayout.Space();

            CommonSkySettingsGUI();
        }
    }
}