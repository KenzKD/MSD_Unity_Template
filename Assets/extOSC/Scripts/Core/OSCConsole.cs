﻿/* Copyright (c) 2020 ExT (V.Sigalkin) */

using System;
using System.Collections.Generic;

namespace extOSC.Core
{
    public static class OSCConsole
    {
        #region Public Vars

        public static List<OSCConsolePacket> ConsoleBuffer { get; set; } = new List<OSCConsolePacket>();

		public static bool LogConsole { get; set; } = false;

		#endregion

        #region Public Methods

        public static void Received(OSCReceiver receiver, IOSCPacket packet)
        {
			var ip = packet.Ip != null ? $"{packet.Ip}:{packet.Port}" : "Debug";

            var consolePacket = new OSCConsolePacket
            {
                Info = $"Receiver: {receiver.LocalPort}. From: {ip}",
                TimeStamp = DateTime.Now.ToString("[HH:mm:ss]"),
                PacketType = OSCConsolePacketType.Received,
                Packet = packet
            };

            Log(consolePacket);
        }

        public static void Transmitted(OSCTransmitter transmitter, IOSCPacket packet)
        {
            var consolePacket = new OSCConsolePacket
            {
                Info = $"Transmitter: {transmitter.RemoteHost}:{transmitter.RemotePort}",
                TimeStamp = DateTime.Now.ToString("[HH:mm:ss]"),
                PacketType = OSCConsolePacketType.Transmitted,
                Packet = packet
            };

            Log(consolePacket);
        }

        #endregion

        #region Private Methods

        private static void Log(OSCConsolePacket consolePacket)
        {
#if UNITY_EDITOR
            // COPY PACKET
	        consolePacket.Packet = consolePacket.Packet.Copy();
            
            ConsoleBuffer.Add(consolePacket);
#else
            if (LogConsole)
            {
                UnityEngine.Debug.Log($"[OSCConsole] Packed {consolePacket.PacketType}: {consolePacket.Packet}");
            }
#endif
        }

        #endregion
    }
}