/*
 * PL/.NET (pldotnet) - PostgreSQL support for .NET C# and F# as
 *             procedural languages (PL)
 *
 *
 * Copyright 2023 Brick Abode
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * pldotnet_spi.h
 *
 */

#include <postgres.h>
#include <executor/spi.h>

#include "pldotnet_main.h"

#define nullptr ((void *)0)
extern bool is_spi_open;

/**
 * @brief Executes an SQL command using the Server Programming Interface (SPI).
 *
 * This function executes the given SQL command using the SPI interface. The
 * execution mode can be set to read-only or read-write mode using the
 * `read_only` parameter. A limit can also be set using the `limit` parameter to
 * restrict the number of rows returned.
 *
 * @param cmd The SQL command to execute.
 * @param read_only A flag indicating whether the execution mode is read-only
 * (`true`) or read-write (`false`).
 * @param limit The maximum number of rows to return.
 * @param errorData The ErrorData struct with the error that occurred
 *
 * @return the SPITupleTable of the performed query.
 */
extern PGDLLEXPORT SPITupleTable *pldotnet_SPIExecute(char *cmd,
                                   bool read_only,
                                   long limit,
                                   ErrorData **errorData);

/**
 * @brief Executes an SQL command using the Server Programming Interface (SPI).
 *
 * This function executes the given SQL command using the SPI interface after
 * preparing the server. The execution mode can be set to read-only or
 * read-write mode using the `read_only` parameter. A limit can also be set
 * using the `limit` parameter to restrict the number of rows returned.
 *
 * @param plan The SPIPlanPtr pointer returned by SPI_prepare.
 * @param paramValues The array with the datums parameters.
 * @param nullmap The array which maps the null values.
 * @param read_only A flag indicating whether the execution mode is read-only
 * (`true`) or read-write (`false`).
 * @param limit The maximum number of rows to return.
 * @param errorData The ErrorData struct with the error that occurred
 *
 * @return the SPITupleTable of the performed query.
 */
extern PGDLLEXPORT SPITupleTable *pldotnet_SPIExecutePlan(SPIPlanPtr plan,
                                       Datum *paramValues,
                                       const char *nullmap,
                                       bool read_only,
                                       long limit,
                                       ErrorData **errorData);

/**
 * @brief Commits the current transaction using SPI_commit().
 *
 * @param errorData The ErrorData struct with the error that occurred
 *
 */
extern PGDLLEXPORT void pldotnet_SPICommit(ErrorData **errorData);

/**
 * @brief Rolls back the current transaction using SPI_rollback().
 *
 * @param errorData The ErrorData struct with the error that occurred
 *
 */
extern PGDLLEXPORT void pldotnet_SPIRollback(ErrorData **errorData);

/**
 * @brief Gets the number of columns if the provided SPITupleTable
 * stores a returned table. Otherwise, it returns 0.
 *
 * @param tupleTable A pointer to a SPITupleTable struct. It'll be a null
 * pointer if it is not related to a returned table.
 *
 * @return the number of columns in the table stored in the provided
 * SPITupleTable.
 */
extern PGDLLEXPORT int pldotnet_GetTableColumnNumber(SPITupleTable *tupleTable);

/**
 * @brief Gets the tuple description type OID if the provided SPITupleTable
 * stores a returned table. Otherwise, it returns 0.
 *
 * @param tupleTable A pointer to a SPITupleTable struct. It'll be a null
 * pointer if it is not related to a returned table.
 *
 * @return the tuple description type OID of the table stored in the provided
 * SPITupleTable.
 */
extern PGDLLEXPORT int pldotnet_GetTableTypeID(SPITupleTable *tupleTable);

/**
 * @brief Get the properties of the columns in the current query result set.
 *
 * This function retrieves the data types and names of the columns in the
 * current query result set. It stores the data types in `columnTypes`
 * and the names in `columnNames`.
 *
 * @param columnTypes Output parameter for the data types of the columns.
 * @param columnNames Output parameter for the names of the columns.
 * @param columnTypmods Output parameter for the type modifier of the columns.
 * @param columnLens Output parameter for the len/size of the columns.
 * @param tupleTable A pointer to a SPITupleTable struct. It'll be a null
 * pointer if it is not related to a returned table.
 */
extern PGDLLEXPORT void pldotnet_GetColProps(int *columnTypes,
                          char **columnNames,
                          int *columnTypmods,
                          int *columnLens,
                          SPITupleTable *tupleTable);

/**
 * @brief Extracts the data from the SPITupleTable into an array of Datums
 * and one of booleans indicating whether each value is NULL or not.
 *
 * @param row The index of the current row.
 * @param datums Pointer to an array of Datums. On return, this array will
 * contain the extracted data.
 * @param isNull Pointer to an array of booleans indicating whether each
 * value in `datums` is NULL or not. On return, this array will be filled
 * with the corresponding NULL information.
 * @param tupleTable A pointer to a SPITupleTable struct. It'll be a null
 * pointer if it is not related to a returned table.
 */
extern PGDLLEXPORT void pldotnet_GetRow(int row,
                                        Datum *datums,
                                        bool *isNull,
                                        SPITupleTable *tupleTable);

/**
 * @brief Prepares a statement for execution by the SPI manager.
 *
 * @param cmdPointer A pointer to an SPI plan to be filled in by this
 * function.
 * @param command The command string to prepare.
 * @param nargs The number of arguments in the argument type array.
 * @param argTypes An array of argument type OIDs.
 * @param errorData The ErrorData struct with the error that occurred
 */
extern PGDLLEXPORT void pldotnet_SPIPrepare(SPIPlanPtr *cmdPointer,
                                            char *command,
                                            int nargs,
                                            Oid *argTypes,
                                            ErrorData **errorData);

/**
 * @brief Get the string representing elevel
 * @param elevel The error level returned from ErrorData
 * This is the same as psql error_severity, but the function was added to be
 * compatible with pre-15 psql
 * @return the error type as a char pointer.
 */
extern PGDLLEXPORT const char *pldotnet_ErrorSeverity(int elevel);

/**
 * @brief Free the errorData Pointer after using it on C#
 * @param errorData The ErrorData struct with the error to be freed
 */
extern PGDLLEXPORT void pldotnet_FreeErrorData(ErrorData *errorData);

/**
 * @brief Return the value of the global variable SPI_processed.
 * @return the number of processed rows at the last query executed.
 */
extern PGDLLEXPORT uint64 pldotnet_GetProcessedRowsNumber(void);
