﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.Shared;

namespace Microsoft.Build.Utilities
{
    /// <summary>
    /// Processor architecture utilities
    /// </summary>
    static public class ProcessorArchitecture
    {
        // Known processor architectures
        public const string X86 = "x86";
        public const string IA64 = "IA64";

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "AMD", Justification = "This is the correct casing for ProcessorArchitecture")]
        public const string AMD64 = "AMD64";

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MSIL", Justification = "This is the correct casing for ProcessorArchitecture")]
        public const string MSIL = "MSIL";

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ARM", Justification = "This is the correct casing for ProcessorArchitecture")]
        public const string ARM = "ARM";

        static private string s_currentProcessArchitecture = null;
        static private bool s_currentProcessArchitectureInitialized = false;

        /// <summary>
        /// Lazy-initted property for getting the architecture of the currently running process
        /// </summary>
        static public string CurrentProcessArchitecture
        {
            get
            {
                if (!NativeMethodsShared.IsWindows)
                {
                    return String.Empty;
                }

                if (s_currentProcessArchitectureInitialized)
                {
                    return s_currentProcessArchitecture;
                }

                s_currentProcessArchitectureInitialized = true;
                s_currentProcessArchitecture = ProcessorArchitecture.GetCurrentProcessArchitecture();

                return s_currentProcessArchitecture;
            }
        }

        // PInvoke delegate for IsWow64Process
        private delegate bool IsWow64ProcessDelegate([In] IntPtr hProcess, [Out] out bool Wow64Process);

        /// <summary>
        /// Gets the processor architecture of the currently running process
        /// </summary>
        /// <returns>null if unknown architecture or error, one of the known architectures otherwise</returns>
        static private string GetCurrentProcessArchitecture()
        {
            string architecture = null;

            NativeMethodsShared.SYSTEM_INFO systemInfo = new NativeMethodsShared.SYSTEM_INFO();

            NativeMethodsShared.GetSystemInfo(ref systemInfo);

            switch (systemInfo.wProcessorArchitecture)
            {
                case NativeMethodsShared.PROCESSOR_ARCHITECTURE_INTEL:
                    architecture = ProcessorArchitecture.X86;
                    break;

                case NativeMethodsShared.PROCESSOR_ARCHITECTURE_AMD64:
                    architecture = ProcessorArchitecture.AMD64;
                    break;

                case NativeMethodsShared.PROCESSOR_ARCHITECTURE_IA64:
                    architecture = ProcessorArchitecture.IA64;
                    break;

                case NativeMethodsShared.PROCESSOR_ARCHITECTURE_ARM:
                    architecture = ProcessorArchitecture.ARM;
                    break;

                // unknown architecture? return null
                default:
                    architecture = null;
                    break;
            }
            
            return architecture;
        }
    }
}
