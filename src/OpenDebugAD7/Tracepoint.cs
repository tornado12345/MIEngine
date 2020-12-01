﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Microsoft.VisualStudio.Debugger.Interop;

namespace OpenDebugAD7
{
    internal class Tracepoint
    {
        // LogMessage after it has been Parsed()
        public string LogMessage { get; set; }

        // Map of the index in LogMessage to insert the evaluated expression ($TOKEN or { expression })
        private readonly IDictionary<int, string> m_indexToExpressions;

        private Tracepoint(string logMessage)
        {
            m_indexToExpressions = new Dictionary<int, string>();
            LogMessage = Parse(logMessage);
        }

        internal static Tracepoint CreateTracepoint(string logMessage)
        {
            return new Tracepoint(logMessage);
        }

        internal int GetLogMessage(IDebugThread2 pThread, uint radix, string processName, int processId, out string logMessage)
        {
            int hr = HRConstants.S_OK;
            string message = LogMessage;

            // There is strings to interpolate in the log message.
            if (m_indexToExpressions.Count != 0)
            {
                hr = GetInterpolatedLogMessage(message, pThread, radix, processName, processId, out message);
            }

            logMessage = message;
            return hr;
        }

        private int GetInterpolatedLogMessage(string logMessage, IDebugThread2 pThread, uint radix, string processName, int processId, out string message)
        {
            int hr = HRConstants.S_OK;

            if (pThread == null)
            {
                message = AD7Resources.Error_InterpolateMissingThread;
                return HRConstants.E_FAIL;
            }

            // Get topFrame
            IEnumDebugFrameInfo2 frameInfoEnum;
            hr = pThread.EnumFrameInfo(enum_FRAMEINFO_FLAGS.FIF_FRAME | enum_FRAMEINFO_FLAGS.FIF_FLAGS, Constants.EvaluationRadix, out frameInfoEnum);
            if (hr < 0)
            {
                message = AD7Resources.Error_InterpolateMissingFrames;
                return hr;
            }

            FRAMEINFO[] topFrame = new FRAMEINFO[1];
            uint fetched = 0;
            hr = frameInfoEnum.Next(1, topFrame, ref fetched);
            if (hr < 0 || fetched != 1 || topFrame[0].m_pFrame == null)
            {
                message = AD7Resources.Error_InterpolateMissingTopFrame;
                return hr;
            }

            Dictionary<string, string> seenExpressions = new Dictionary<string, string>();
            StringBuilder sb = new StringBuilder();
            int currIndex = 0;

            foreach (KeyValuePair<int, string> keyValuePair in m_indexToExpressions)
            {
                // Add all characters between current index to the insertion index.
                sb.Append(LogMessage.Substring(currIndex, keyValuePair.Key - currIndex));

                // Move current index to end of replaced string.
                currIndex = keyValuePair.Key + keyValuePair.Value.Length;

                if (!seenExpressions.TryGetValue(keyValuePair.Value, out string value))
                {
                    if (keyValuePair.Value[0] == '$')
                    {
                        value = InterpolateToken(keyValuePair.Value, pThread, topFrame[0].m_pFrame, radix, processName, processId);
                    }
                    else
                    {
                        string toInterpolate = keyValuePair.Value;
                        if (InterpolateVariable(toInterpolate.Substring(1, toInterpolate.Length - 2), topFrame[0].m_pFrame, radix, out value) < 0)
                        {
                            hr = HRConstants.E_FAIL;
                            DebuggerTelemetry.ReportError(DebuggerTelemetry.TelemetryTracepointEventName, value);

                            // Re-write error message
                            value = string.Format(CultureInfo.CurrentCulture, "<Failed to interpolate {0}: \"{1}\">", toInterpolate, value);
                        }
                    }

                    // Cache expression
                    seenExpressions[keyValuePair.Value] = value;
                }
                sb.Append(value);
            }

            // Append the rest of LogMessage
            sb.Append(LogMessage.Substring(currIndex, LogMessage.Length - currIndex));

            message = sb.ToString();

            return hr;
        }

        private string InterpolateToken(string token, IDebugThread2 pThread, IDebugStackFrame2 topFrame, uint radix, string processName, int processId)
        {
            switch (token)
            {
                case "$FUNCTION":
                    {
                        int hr = topFrame.GetCodeContext(out IDebugCodeContext2 pCodeContext);
                        if (hr >= 0)
                        {
                            CONTEXT_INFO[] pInfo = new CONTEXT_INFO[1];
                            hr = pCodeContext.GetInfo(enum_CONTEXT_INFO_FIELDS.CIF_FUNCTION, pInfo);
                            if (hr >= 0)
                            {
                                return pInfo[0].bstrFunction;
                            }
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No Function Found>");
                    }
                case "$ADDRESS":
                    {
                        int hr = topFrame.GetCodeContext(out IDebugCodeContext2 pCodeContext);
                        if (hr >= 0)
                        {
                            CONTEXT_INFO[] pInfo = new CONTEXT_INFO[1];
                            hr = pCodeContext.GetInfo(enum_CONTEXT_INFO_FIELDS.CIF_ADDRESS, pInfo);
                            if (hr >= 0)
                            {
                                return pInfo[0].bstrAddress;
                            }
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No Address Found>");
                    }
                case "$FILEPOS":
                    {
                        int hr = topFrame.GetDocumentContext(out IDebugDocumentContext2 pDocumentContext);
                        if (hr >= 0)
                        {
                            hr = pDocumentContext.GetName(enum_GETNAME_TYPE.GN_FILENAME, out string fileName);
                            if (hr >= 0)
                            {
                                TEXT_POSITION[] textPosBeg = new TEXT_POSITION[1];
                                TEXT_POSITION[] textPosEnd = new TEXT_POSITION[1];
                                hr = pDocumentContext.GetStatementRange(textPosBeg, textPosEnd);
                                if (hr >= 0)
                                {
                                    return string.Format(CultureInfo.InvariantCulture, "{0}({1})", fileName, textPosBeg[0].dwLine + 1);
                                }
                            }
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No File Position>");
                    }
                case "$CALLER":
                    {
                        IEnumDebugFrameInfo2 frameInfoEnum;
                        int hr = pThread.EnumFrameInfo(enum_FRAMEINFO_FLAGS.FIF_FRAME | enum_FRAMEINFO_FLAGS.FIF_FLAGS, Constants.EvaluationRadix, out frameInfoEnum);
                        if (hr >= 0)
                        {
                            FRAMEINFO[] frames = new FRAMEINFO[2];
                            uint fetched = 0;
                            hr = frameInfoEnum.Next(2, frames, ref fetched);
                            if (hr < 0 && fetched == 2)
                            {
                                return frames[1].m_bstrFuncName;
                            }
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No Caller Avaliable>");
                    }
                case "$TID":
                    {
                        int hr = pThread.GetThreadId(out uint threadId);
                        if (hr == 0)
                        {
                            if (radix == 16)
                            {
                                return string.Format(CultureInfo.InvariantCulture, "{0:X}", threadId);
                            }
                            else
                            {
                                return string.Format(CultureInfo.InvariantCulture, "{0}", threadId);
                            }
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No Thread Id>");
                    }
                case "$TNAME":
                    {
                        int hr = pThread.GetName(out string name);
                        if (hr == 0)
                        {
                            return name;
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<No Thread Name>");
                    }
                case "$PID":
                    {
                        if (processId != Constants.InvalidProcessId)
                        {
                            return processId.ToString(CultureInfo.InvariantCulture);
                        }
                        return string.Format(CultureInfo.InvariantCulture, "<Unknown PID>", token);
                    }
                case "$PNAME":
                    {
                        return processName;
                    }
                case "$CALLSTACK":
                    {
                        IEnumDebugFrameInfo2 frameInfoEnum;
                        int hr = pThread.EnumFrameInfo(enum_FRAMEINFO_FLAGS.FIF_FRAME | enum_FRAMEINFO_FLAGS.FIF_FLAGS, Constants.EvaluationRadix, out frameInfoEnum);
                        int count = 0;
                        StringBuilder sb = new StringBuilder();
                        while (count < Constants.DefaultTracepointCallstackDepth)
                        {
                            FRAMEINFO[] frames = new FRAMEINFO[1];
                            uint fetched = 0;
                            hr = frameInfoEnum.Next(1, frames, ref fetched);
                            if (fetched == 1)
                            {
                                // TODO: Do we want function arguments?
                                frames[0].m_pFrame.GetName(out string name);
                                sb.AppendLine(name);
                            }
                            count++;
                        }
                        return sb.ToString();
                    }
                case "$TICK":
                    return string.Format(CultureInfo.InvariantCulture, "{0}", Environment.TickCount);
                default:
                    return string.Format(CultureInfo.InvariantCulture, "<Unknown Token: ${0}>", token);
            }
        }

        private int InterpolateVariable(string variable, IDebugStackFrame2 topFrame, uint radix, out string interpolatedVariableStr)
        {
            int hr = HRConstants.S_OK;

            IDebugExpressionContext2 expressionContext;
            hr = topFrame.GetExpressionContext(out expressionContext);
            if (hr < 0)
            {
                interpolatedVariableStr = AD7Resources.Error_InterpolateVariableMissingContext;
                return hr;
            }

            IDebugExpression2 expressionObject;
            hr = expressionContext.ParseText(variable, enum_PARSEFLAGS.PARSE_EXPRESSION, radix, out expressionObject, out string errStr, out uint errIdx);
            if (hr < 0)
            {
                interpolatedVariableStr = string.Format(CultureInfo.InvariantCulture, "{0} at index {1}", errStr, errIdx);
                return hr;
            }
            if (expressionObject == null)
            {
                interpolatedVariableStr = AD7Resources.Error_InterpolateVariableMissingExpressionObject;
                return HRConstants.E_FAIL;
            }

            IDebugProperty2 property;
            enum_EVALFLAGS flags = enum_EVALFLAGS.EVAL_RETURNVALUE |
                enum_EVALFLAGS.EVAL_NOEVENTS |
                (enum_EVALFLAGS)enum_EVALFLAGS110.EVAL110_FORCE_REAL_FUNCEVAL |
                enum_EVALFLAGS.EVAL_NOSIDEEFFECTS;
            hr = expressionObject.EvaluateSync(flags, Constants.EvaluationTimeout, null, out property);
            if (hr < 0 || property == null)
            {
                interpolatedVariableStr = AD7Resources.Error_InterpolateVariableEvaluateFailed;
                return hr;
            }

            DEBUG_PROPERTY_INFO[] propertyInfo = new DEBUG_PROPERTY_INFO[1];
            enum_DEBUGPROP_INFO_FLAGS propertyInfoFlags = enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_NAME |
                enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_VALUE |
                enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_TYPE |
                enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_ATTRIB |
                enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_PROP |
                enum_DEBUGPROP_INFO_FLAGS.DEBUGPROP_INFO_FULLNAME |
                (enum_DEBUGPROP_INFO_FLAGS)enum_DEBUGPROP_INFO_FLAGS110.DEBUGPROP110_INFO_FORCE_REAL_FUNCEVAL |
                (enum_DEBUGPROP_INFO_FLAGS)enum_DEBUGPROP_INFO_FLAGS110.DEBUGPROP110_INFO_NOSIDEEFFECTS;
            hr = property.GetPropertyInfo(propertyInfoFlags, Constants.EvaluationRadix, Constants.EvaluationTimeout, null, 0, propertyInfo);
            if (hr < 0)
            {
                interpolatedVariableStr = AD7Resources.Error_InterpolateVariableMissingProperties;
                return hr;
            }

            if ((propertyInfo[0].dwAttrib & enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_ERROR) == enum_DBG_ATTRIB_FLAGS.DBG_ATTRIB_VALUE_ERROR)
            {
                // bstrValue has useful information.
                interpolatedVariableStr = propertyInfo[0].bstrValue;
                return HRConstants.E_FAIL;
            }

            interpolatedVariableStr = propertyInfo[0].bstrValue;

            return hr;
        }

        #region Tracepoint Parsing

        /// <summary>
        /// The parse method for Tracepoints.
        /// 
        /// Algorithm:
        ///     Go though each character in the string.
        ///         1. If previous character was an escape, check to see if current is curl brace. If so, just output curl brace, else output escape and character.
        ///         2. If it is an escape character, toggle isEscape to be true. Goto 1.
        ///         3. If it is a '$', find the end of the token. If it is a known token, it will be added to m_indexToExpressions and skip over all the characters
        ///             in the token. If not, it will just add $ and continue.
        ///         4. If it is a open curl brace, try to find end match curl brace for the interpolated expression. Add to m_indexToExpressions.
        ///             If there is no matching end brace, an exception will be thrown.
        ///         5. Other character, just add to message to output.
        /// </summary>
        /// <param name="input">The logMessage to parse</param>
        /// <returns>The new string after it has been parsed, it will check for excaped curl braces.</returns>
        private string Parse(string input)
        {
            StringBuilder replace = new StringBuilder();
            bool isEscaped = false;
            for (int index = 0; index < input.Length; index++)
            {
                char c = input[index];

                if (isEscaped)
                {
                    if (c != '{' && c != '}')
                    {
                        replace.Append('\\');
                    }

                    // Not interpolating, output character for user's string.
                    replace.Append(c);
                    isEscaped = false;
                }
                else
                {
                    if (c == '\\')
                    {
                        isEscaped = true;
                    }
                    else if (c == '$')
                    {
                        if (FindToken(input.Substring(index), out int endIndex))
                        {
                            string buffer = input.Substring(index, endIndex + 1);

                            m_indexToExpressions[replace.Length] = buffer;

                            replace.Append(input.Substring(index, endIndex + 1));

                            index += endIndex;
                        }
                        else
                        {
                            replace.Append(c);
                        }
                    }
                    else if (c == '{')
                    {
                        if (FindInterpolatedExpression(input.Substring(index), out int endIndex))
                        {
                            string buffer = input.Substring(index, endIndex + 1);

                            if (!string.IsNullOrWhiteSpace(buffer))
                            {
                                m_indexToExpressions[replace.Length] = buffer;
                            }

                            replace.Append(input.Substring(index, endIndex + 1));

                            index += endIndex;
                        }
                        else
                        {
                            throw new InvalidTracepointException("Can not find matching brace '}' for interpolated expression.");
                        }
                    }
                    else
                    {
                        replace.Append(c);
                    }
                }
            }

            return replace.ToString();
        }

        private bool FindToken(string input, out int endIndex)
        {
            StringBuilder buffer = new StringBuilder("$");
            int index;
            for (index = 1; index < input.Length; index++)
            {
                if (!char.IsUpper(input[index]))
                {
                    break;
                }
                buffer.Append(input[index]);
            }
            endIndex = index - 1;

            string token = buffer.ToString();
            switch (token)
            {
                case "$FILEPOS":
                case "$FUNCTION":
                case "$ADDRESS":
                case "$TID":
                case "$TNAME":
                case "$PID":
                case "$PNAME":
                case "$CALLER":
                case "$CALLSTACK":
                case "$TICK":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Method to find the end matching quote.
        /// </summary>
        /// <param name="input">string to find quote. Index 0 is the open quote.</param>
        /// <param name="endIndex">output of where the index of the end quote relative to 'input'</param>
        /// <returns>true if a end quote was found</returns>
        private bool FindEndQuote(string input, out int endIndex)
        {
            endIndex = -1;

            int index;
            bool isEscaped = false;
            for (index = 1; index < input.Length; index++)
            {
                char c = input[index];

                if (c == '"' && !isEscaped)
                {
                    endIndex = index;
                    return true;
                }

                if (isEscaped)
                {
                    isEscaped = false;
                }

                if (c == '\\')
                {
                    isEscaped = true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method to find the interpolated expression.
        /// E.g. Find the end curl brace. 
        /// </summary>
        /// <param name="input">String to find the end brace. Index 0 is '{'</param>
        /// <param name="endIndex">Index of '}' relative to input.</param>
        /// <returns>true if an end brace was found.</returns>
        private bool FindInterpolatedExpression(string input, out int endIndex)
        {
            endIndex = -1;
            int index = 0;
            StringBuilder buffer = new StringBuilder();
            int nested = 0;
            bool isEscaped = false;
            while (++index < input.Length)
            {
                char c = input[index];
                if (isEscaped)
                {
                    buffer.Append(c);
                    isEscaped = false;
                }
                else
                {
                    if (c == '\\')
                    {
                        isEscaped = true;
                    }
                    else if (c == '{')
                    {
                        nested++;
                    }
                    else if (c == '}')
                    {
                        if (nested > 0)
                        {
                            nested--;
                        }
                        else
                        {
                            endIndex = index;
                            return true;
                        }
                    }
                    else if (c == '"')
                    {
                        if (FindEndQuote(input.Substring(index), out int length))
                        {
                            length = length + 1;
                        }
                        else
                        {
                            length = input.Length - index;
                        }
                        buffer.Append(input.Substring(index, length));
                        index += length;
                    }
                }
            }

            return false;
        }

        #endregion
    }

    public class InvalidTracepointException : Exception
    {
        public InvalidTracepointException(string message) : base(message)
        {
        }
    }

}
