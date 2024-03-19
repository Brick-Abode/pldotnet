// <copyright file="TriggerData.cs" company="Brick Abode">
//
// PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
//                      procedural languages (PL)
//
//
// Copyright (c) 2023 Brick Abode
//
// This code is subject to the terms of the PostgreSQL License.
// The full text of the license can be found in the LICENSE file
// at the top level of the pldotnet repository.
//
// </copyright>

using System;
using System.Linq;

namespace PlDotNET.Common
{
#nullable enable
    public class TriggerData
    {
        public TriggerData(
            object?[] oldRow,
            object?[] newRow,
            string triggerName,
            string triggerWhen,
            string triggerLevel,
            string triggerEvent,
            int relationId,
            string tableName,
            string tableSchema,
            string[] arguments)
        {
            this.OldRow = oldRow;
            this.NewRow = newRow;
            this.TriggerName = triggerName;
            this.TriggerWhen = triggerWhen;
            this.TriggerLevel = triggerLevel;
            this.TriggerEvent = triggerEvent;
            this.RelationId = relationId;
            this.TableName = tableName;
            this.TableSchema = tableSchema;
            this.Arguments = arguments;
        }

        // Row-level information for operations
        public object?[] OldRow { get; set; }

        public object?[] NewRow { get; set; }

        // Trigger metadata
        public string TriggerName { get; set; }

        public string TriggerWhen { get; set; }

        public string TriggerLevel { get; set; } // TODO: make this an enum

        public string TriggerEvent { get; set; } // TODO: make this an enum

        // Table-related details
        public int RelationId { get; set; }

        // [Obsolete("RelationName is deprecated and may be removed in future releases. Use TableName instead.")]
        // public string RelationName { get; set; }
        public string TableName { get; set; }

        public string TableSchema { get; set; }

        // Trigger arguments
        public string[] Arguments { get; set; }

        public override string ToString()
        {
            string newline = Environment.NewLine;

            static string FormatItem(object? item) => $"- ({item?.GetType().Name ?? "null"}){item?.ToString() ?? "null"}";

            return $"TriggerName: (string){this.TriggerName}\n" +
                   $"TriggerWhen: (string){this.TriggerWhen}\n" +
                   $"TriggerLevel: (string){this.TriggerLevel}\n" +
                   $"TriggerEvent : (string){this.TriggerEvent}\n" +
                   $"RelationId: (int){this.RelationId}\n" +
                   $"TableName: (string){this.TableName}\n" +
                   $"TableSchema: (string){this.TableSchema}\n" +
                   "OldRow:\n" +
                   string.Join(newline, this.OldRow.Select(FormatItem)) +
                   "\nNewRow:\n" +
                   string.Join(newline, this.NewRow.Select(FormatItem)) +
                   "\nArguments:\n" +
                   string.Join(newline, this.Arguments.Select(arg => $"- (string){arg}")) +
                   "\n";
        }
    }
#nullable disable
}