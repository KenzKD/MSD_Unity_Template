/* Copyright (c) 2020 ExT (V.Sigalkin) */

using UnityEngine;
using UnityEditor;

using extOSC.Editor.Windows;

namespace extOSC.Editor.Panels
{
	public class OSCPanelControlCreator : OSCPanel
	{
		#region Static Private Vars

		private static readonly GUIContent _oscAddressContent = new("OSC Address:");

		private static readonly GUIContent _oscTransmitterContent = new("OSC Transmitter:");

		private static readonly GUIContent _informerIntervalContent = new("Informer Interval:");

		private static readonly GUIContent _addInformerContent = new("Add Informer");

		private static readonly GUIContent _controlSettingsContent = new("Create New Control:");

		private static readonly GUIContent _controlColorContent = new("Control Color:");

		private static readonly GUIContent _informOnChangedContent = new("Inform On Changed");

		private static readonly GUIContent _informerSettingsContent = new("Informer Settings:");

		private static readonly GUIContent _createContent = new("Create");

		private static readonly GUIContent _errorCreateContent = new("Create object again.");

		#endregion

		#region Public Vars

		public Color ControlColor;

		public bool AddInformer;

		public string InformerAddress;

		public bool InformOnChanged;

		public float InformerInterval;

		public OSCTransmitter InformerTransmitter;

		#endregion

		#region Private Vars

		private readonly OSCWindowControlCreator _controlCreator;

		#endregion

		#region Public Methods

		public OSCPanelControlCreator(OSCWindow window) : base(window)
		{
			_controlCreator = window as OSCWindowControlCreator;
		}

		#endregion

		#region Protected Methods

		protected override void DrawContent(ref Rect contentRect)
		{
			if (!_controlCreator.IsValid)
			{
				EditorGUILayout.LabelField(_errorCreateContent, OSCEditorStyles.CenterLabel, GUILayout.Height(contentRect.height));

				return;
			}

			contentRect.x += 2;
			contentRect.y += 2;
			contentRect.width -= 4;
			contentRect.height -= 4;

			using (new GUILayout.AreaScope(contentRect))
			{
				OSCEditorInterface.LogoLayout();

				GUILayout.Label(_controlSettingsContent, EditorStyles.boldLabel);
				using (new EditorGUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					ControlColor = EditorGUILayout.ColorField(_controlColorContent, ControlColor);
				}

				GUI.color = AddInformer ? Color.green : Color.red;
				if (GUILayout.Button(_addInformerContent))
				{
					AddInformer = !AddInformer;
				}

				GUI.color = Color.white;

				if (AddInformer)
				{
					GUILayout.Label(_informerSettingsContent, EditorStyles.boldLabel);
					using (new EditorGUILayout.VerticalScope(OSCEditorStyles.Box))
					{
						OSCEditorUtils.FindObjects(TransmitterCallback, true, out var content, out OSCTransmitter[] objects);

						InformerAddress = EditorGUILayout.TextField(_oscAddressContent, InformerAddress);
						InformerTransmitter = OSCEditorInterface.PopupLayout(_oscTransmitterContent,
																			 InformerTransmitter,
																			 content,
																			 objects);

						GUI.color = InformOnChanged ? Color.green : Color.red;
						if (GUILayout.Button(_informOnChangedContent))
						{
							InformOnChanged = !InformOnChanged;
						}

						GUI.color = Color.white;

						if (!InformOnChanged)
						{
							InformerInterval = EditorGUILayout.FloatField(_informerIntervalContent, InformerInterval);

							if (InformerInterval < 0) InformerInterval = 0;

							EditorGUILayout.HelpBox("Set to 0 for send message with each frame.", MessageType.Info);
						}
					}
				}

				GUI.color = Color.green;
				if (GUILayout.Button(_createContent))
				{
                    var data = new OSCWindowControlCreator.ControlData
                    {
                        ControlColor = ControlColor,
                        UseInformer = AddInformer,
                        InformAddress = InformerAddress,
                        InformInterval = InformerInterval,
                        InformOnChanged = InformOnChanged,
                        InformerTransmitter = InformerTransmitter
                    };

                    OSCWindowControlCreator.CreateControl(data);
				}

				GUI.color = Color.white;
			}
		}

		#endregion

		#region Private Methods

		private string TransmitterCallback(OSCTransmitter transmitter) => $"Transmitter: {transmitter.RemoteHost}:{transmitter.RemotePort}";

		#endregion
	}
}