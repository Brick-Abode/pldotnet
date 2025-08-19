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
    /// <summary>
    /// Represents metadata and row-level data passed to a PostgreSQL trigger function,
    /// including old and new row values, trigger context, and table identifiers.
    /// </summary>
    /// <param name="oldRow">The contents of the row before the triggering operation (for UPDATE or DELETE).</param>
    /// <param name="newRow">The contents of the row after the triggering operation (for INSERT or UPDATE).</param>
    /// <param name="triggerName">The name of the trigger as defined in PostgreSQL.</param>
    /// <param name="triggerWhen">Specifies whether the trigger fires BEFORE or AFTER the operation.</param>
    /// <param name="triggerLevel">The level of the trigger: ROW or STATEMENT.</param>
    /// <param name="triggerEvent">The type of DML operation that triggered this call: INSERT, UPDATE, or DELETE.</param>
    /// <param name="relationId">The OID (object identifier) of the table on which the trigger fired.</param>
    /// <param name="tableName">The name of the table that the trigger is defined on.</param>
    /// <param name="tableSchema">The schema that contains the table.</param>
    /// <param name="arguments">Arguments passed to the trigger from the PostgreSQL CREATE TRIGGER statement.</param>
    public class TriggerData(
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
        /// <summary>
        /// Gets or sets the values of the row before the triggering operation.
        /// Only populated for UPDATE and DELETE triggers.
        /// </summary>
        public object?[] OldRow { get; set; } = oldRow;

        /// <summary>
        /// Gets or sets the values of the row after the triggering operation.
        /// Only populated for INSERT and UPDATE triggers.
        /// </summary>
        public object?[] NewRow { get; set; } = newRow;

        /// <summary>
        /// Gets or sets the name of the trigger that was fired.
        /// </summary>
        public string TriggerName { get; set; } = triggerName;

        /// <summary>
        /// Gets or sets the timing of the trigger execution (e.g., BEFORE or AFTER).
        /// </summary>
        public string TriggerWhen { get; set; } = triggerWhen;

        /// <summary>
        /// Gets or sets the level at which the trigger was fired (e.g., ROW or STATEMENT).
        /// </summary>
        public string TriggerLevel { get; set; } = triggerLevel;

        /// <summary>
        /// Gets or sets the DML event that caused the trigger (INSERT, UPDATE, or DELETE).
        /// </summary>
        public string TriggerEvent { get; set; } = triggerEvent;

        /// <summary>
        /// Gets or sets the PostgreSQL internal relation OID (object identifier) of the table.
        /// </summary>
        public int RelationId { get; set; } = relationId;

        /// <summary>
        /// Gets or sets the name of the table associated with the trigger.
        /// </summary>
        public string TableName { get; set; } = tableName;

        /// <summary>
        /// Gets or sets the schema name of the table associated with the trigger.
        /// </summary>
        public string TableSchema { get; set; } = tableSchema;

        /// <summary>
        /// Gets or sets the array of arguments passed to the trigger.
        /// </summary>
        public string[] Arguments { get; set; } = arguments;

        /// <summary>
        /// Returns a string representation of the trigger data for debugging purposes.
        /// Includes trigger metadata, old and new rows, and arguments.
        /// </summary>
        /// <returns>A multi-line string summarizing the contents of this instance.</returns>
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
