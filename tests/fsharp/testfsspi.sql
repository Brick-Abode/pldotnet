CREATE OR REPLACE FUNCTION SPIScalarFSharp() RETURNS integer AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    let command = new NpgsqlCommand("SELECT 2024;", conn)
    let value = command.ExecuteScalar()

    match value with
    | :? int as intValue -> Nullable<int>(intValue)
    | _ -> Nullable<int>()
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-spi', 'SPIScalarFSharp', SPIScalarFSharp() = 2024;

CREATE OR REPLACE FUNCTION SPIMultiQueryFsharp() RETURNS integer AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    let command = new NpgsqlCommand("SELECT 1; SELECT 2; SELECT 3", conn)
    let reader = command.ExecuteReader()

    let mutable sum = 0
    let mutable value = 0

    reader.Read()
    value <- reader.GetInt32(0)
    Elog.Info("[DEBUG - PLFSHARP TEST] Value read: " + value.ToString())
    sum <- sum + value

    reader.NextResult()
    reader.Read()
    value <- reader.GetInt32(0)
    Elog.Info("[DEBUG - PLFSHARP TEST] Value read: " + value.ToString())
    sum <- sum + value

    reader.NextResult()
    reader.Read()
    value <- reader.GetInt32(0)
    Elog.Info("[DEBUG - PLFSHARP TEST] Value read: " + value.ToString())
    sum <- sum + value

    Nullable(sum)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int4-spi', 'SPIMultiQueryFsharp', SPIMultiQueryFsharp() = 6;

CREATE OR REPLACE FUNCTION SPISumIntegersFSharp(a integer, b integer, c integer) RETURNS integer AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    let query1 = "SELECT " + a.Value.ToString() + " as a, " + b.Value.ToString() + " as b, " + c.Value.ToString() + " as c"
    let query2 = "SELECT " + (2*a.Value).ToString() + " as a, " + (2*b.Value).ToString() + " as b, " + (2*c.Value).ToString() + " as c"
    let command = new NpgsqlCommand(query1 + "; " + query2, conn)
    let reader = command.ExecuteReader()

    let mutable sum = 0
    while reader.Read() do
        let _a = reader.GetInt32(0)
        let _b = reader.GetInt32(1)
        let _c = reader.GetInt32(2)
        Elog.Info("Returned values of the FIRST query: " + _a.ToString() + " | " + _b.ToString() + " | " + _c.ToString())
        sum <- sum + _a + _b + _c

    reader.NextResult() |> ignore

    while reader.Read() do
        let _a = reader.GetInt32(0)
        let _b = reader.GetInt32(1)
        let _c = reader.GetInt32(2)
        Elog.Info("Returned values of the SECOND query: " + _a.ToString() + " | " + _b.ToString() + " | " + _c.ToString())
        sum <- sum + _a + _b + _c
    Nullable(sum)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int-spi-multiquery', 'SPISumIntegersFSharp1', SPISumIntegersFSharp(1, 2, 3) = 18;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int-spi-multiquery', 'SPISumIntegersFSharp2', SPISumIntegersFSharp(4, 0, 5) = 27;

CREATE OR REPLACE FUNCTION SPITestTruncateAndAlterFSharp() RETURNS BOOL AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    try
        let createCommand = new NpgsqlCommand("DROP TABLE IF EXISTS test_alter_truncate; CREATE TABLE test_alter_truncate (ID INTEGER); ALTER TABLE test_alter_truncate ADD COLUMN Name TEXT;", conn)
        ignore (createCommand.ExecuteNonQuery())

        let insertCommand = new NpgsqlCommand("INSERT INTO test_alter_truncate (ID, Name) VALUES (1, 'Test');", conn)
        ignore (insertCommand.ExecuteNonQuery())

        let checkCommand = new NpgsqlCommand("SELECT COUNT(*) FROM test_alter_truncate;", conn)
        let mutable countValue = checkCommand.ExecuteScalar()

        Elog.Info("Number of rows before truncation = " + countValue.ToString())

        let truncateCommand = new NpgsqlCommand("TRUNCATE TABLE test_alter_truncate;", conn)
        ignore (truncateCommand.ExecuteNonQuery())

        countValue <- checkCommand.ExecuteScalar()
        Elog.Info("Number of rows after truncation = " + countValue.ToString())

        match countValue with
        | :? int64 as intValue ->
            if intValue = 0 then Nullable(true)
            else Nullable(false)
        | _ -> Nullable(false)
    with
    | _ ->
        Nullable(false)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-bool-spi', 'SPITestTruncateAndAlterFSharp', SPITestTruncateAndAlterFSharp();

CREATE OR REPLACE FUNCTION SPITestCreateFunctionFSharp(inputtext text) RETURNS text AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    try
        // SQL to create a function that duplicates the input text
        let createFunctionSQL = "CREATE OR REPLACE FUNCTION simple_duplicate_function(input text) RETURNS TEXT AS $BODY$ return input + input; $BODY$ LANGUAGE plcsharp;"
        let createFunctionCmd = new NpgsqlCommand(createFunctionSQL, conn)
        ignore (createFunctionCmd.ExecuteNonQuery())

        // Call the newly created function to test it
        let testFunctionCmd = new NpgsqlCommand("SELECT simple_duplicate_function(@input);", conn)
        testFunctionCmd.Parameters.AddWithValue("@input", inputtext)
        let result = testFunctionCmd.ExecuteScalar()
        Elog.Info("Returned value from plcsharp function: " + result.ToString())

        // Return the result as a string
        match result with
        | :? string as stringValue -> stringValue
        | _ -> ""
    with
    | _ ->
        ""
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-spi', 'SPITestCreateFunctionFSharp', SPITestCreateFunctionFSharp('Brazil') = 'BrazilBrazil';

CREATE OR REPLACE FUNCTION SPITestCreateDropIndexFSharp() RETURNS text AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    try
        // Drop the test table if it exists, then create it
        let dropCreateTableCmd = new NpgsqlCommand("DROP TABLE IF EXISTS test_index_table; CREATE TABLE test_index_table (id INTEGER, name TEXT);", conn)
        ignore (dropCreateTableCmd.ExecuteNonQuery())

        // Create an index on the test table
        let createIndexCmd = new NpgsqlCommand("CREATE INDEX idx_test ON test_index_table (id);", conn)
        ignore (createIndexCmd.ExecuteNonQuery())

        // Check if the index was created
        let checkIndexCreatedCmd = new NpgsqlCommand("SELECT COUNT(*) FROM pg_indexes WHERE indexname = 'idx_test';", conn)
        let indexCreatedCount = checkIndexCreatedCmd.ExecuteScalar()

        if (indexCreatedCount :?> int64) = 0L then
            raise (new Exception("Index creation failed"))

        // Drop the index
        let dropIndexCmd = new NpgsqlCommand("DROP INDEX idx_test;", conn)
        ignore (dropIndexCmd.ExecuteNonQuery())

        // Check if the index was removed
        let checkIndexDroppedCmd = new NpgsqlCommand("SELECT COUNT(*) FROM pg_indexes WHERE indexname = 'idx_test';", conn)
        let indexDroppedCount = checkIndexDroppedCmd.ExecuteScalar()

        if (indexDroppedCount :?> int64) > 0L then
            raise (new Exception("Index drop failed"))

        // Return success message
        "Index creation and drop test completed successfully"
    with
    | ex ->
        // Return the error message in case of an exception or error
        ex.Message
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-spi', 'SPITestCreateDropIndexFSharp', SPITestCreateDropIndexFSharp() = 'Index creation and drop test completed successfully';

CREATE OR REPLACE FUNCTION SPITestCreateDropViewFSharp() RETURNS text AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    try
        // Create a test view
        let createViewCmd = new NpgsqlCommand("CREATE OR REPLACE VIEW test_view AS SELECT 1 AS number;", conn)
        ignore (createViewCmd.ExecuteNonQuery())

        // Check if the view was created
        let checkViewCreatedCmd = new NpgsqlCommand("SELECT COUNT(*) FROM pg_views WHERE viewname = 'test_view';", conn)
        let viewCreatedCount = checkViewCreatedCmd.ExecuteScalar()

        if (viewCreatedCount :?> int64) = 0L then
            raise (new Exception("View creation failed"))

        // Drop the view
        let dropViewCmd = new NpgsqlCommand("DROP VIEW test_view;", conn)
        ignore (dropViewCmd.ExecuteNonQuery())

        // Check if the view was removed
        let checkViewDroppedCmd = new NpgsqlCommand("SELECT COUNT(*) FROM pg_views WHERE viewname = 'test_view';", conn)
        let viewDroppedCount = checkViewDroppedCmd.ExecuteScalar()

        if (viewDroppedCount :?> int64) > 0L then
            raise (new Exception("View drop failed"))

        // Return success message
        "View creation and drop test completed successfully"
    with
    | ex ->
        // Return the error message in case of an exception or error
        ex.Message
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-spi', 'SPITestCreateDropViewFSharp', SPITestCreateDropViewFSharp() = 'View creation and drop test completed successfully';

CREATE OR REPLACE FUNCTION SPIUseCSharpSPIFSharp(input TEXT) RETURNS text AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    try
        // Drop/Create a table
        let setupTableCmd = new NpgsqlCommand(
            "DROP TABLE IF EXISTS test_spi_table; " +
            "CREATE TABLE test_spi_table (value TEXT);", conn)
        ignore (setupTableCmd.ExecuteNonQuery())

        // Create a PLCSharp procedure
        let createProcCmdText =
            "CREATE OR REPLACE PROCEDURE insert_into_table(table_name TEXT, value TEXT) " +
            "LANGUAGE plcsharp AS $BODY$ " +
            "NpgsqlCommand command; " +
            "command = new NpgsqlCommand(\"INSERT INTO \" + table_name + " +
            " \" (value) VALUES ('\" + value + \"')\", null); " +
            "command.ExecuteNonQuery(); $BODY$;"
        let createProcCmd = new NpgsqlCommand(createProcCmdText, conn)
        ignore (createProcCmd.ExecuteNonQuery())

        // Call the PLCSharp procedure
        let callProcCmd = new NpgsqlCommand(
            "CALL insert_into_table('test_spi_table', '" + input.ToString() + "');", conn)
        ignore (callProcCmd.ExecuteNonQuery())

        // Check if the value was added
        let checkValueCmd = new NpgsqlCommand(
            "SELECT * FROM test_spi_table;", conn)
        let readValue = checkValueCmd.ExecuteScalar()

        // Drop the procedure
        let dropProcCmd = new NpgsqlCommand(
            "DROP PROCEDURE insert_into_table;", conn)
        ignore (dropProcCmd.ExecuteNonQuery())

        // Check if the procedure was removed
        let checkProcDroppedCmd = new NpgsqlCommand(
            "SELECT COUNT(*) FROM pg_proc WHERE proname = 'insert_into_table';", conn)
        let procDroppedCount = checkProcDroppedCmd.ExecuteScalar()

        if (procDroppedCount :?> int64) > 0L then
            raise (new Exception("Procedure drop failed"))

        match readValue with
        | :? string as stringValue -> stringValue
        | _ -> ""
    with
    | ex ->
        // Return the error message in case of an exception or error
        ex.Message
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-spi', 'SPIUseCSharpSPIFSharp', SPIUseCSharpSPIFSharp('use f# spi to call c# 🤘') = 'use f# spi to call c# 🤘';
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-text-spi', 'SPIUseCSharpSPIFSharp', SPIUseCSharpSPIFSharp('💻 🖥️ 🖨️ 🖱️ 💽 💾') = '💻 🖥️ 🖨️ 🖱️ 💽 💾';

CREATE OR REPLACE PROCEDURE SPITransactionTestRollbackFirstFSharp() AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    let batch = conn.CreateBatch()
    batch.BatchCommands.Add(new NpgsqlBatchCommand("DROP TABLE IF EXISTS TRANSACTION_TEST_FSHARP;"))
    batch.BatchCommands.Add(new NpgsqlBatchCommand("CREATE TABLE IF NOT EXISTS TRANSACTION_TEST_FSHARP (a INTEGER, b text);"))
    ignore (batch.ExecuteReader())
    let transaction = conn.BeginTransaction()

    for i = 0 to 9 do
        let commandText = "INSERT INTO TRANSACTION_TEST_FSHARP (a) VALUES (" + i.ToString() + ")"
        let command = new NpgsqlCommand(commandText, conn)
        use reader = command.ExecuteReader()
        if i % 2 <> 0 then
            transaction.Commit()
        else
            transaction.Rollback()

    let readCommand = new NpgsqlCommand("SELECT * FROM TRANSACTION_TEST_FSHARP", conn)
    use reader = readCommand.ExecuteReader()
    let mutable counter = 0
    while reader.Read() do
        let value = reader.GetInt32(0)
        Elog.Info("Row[" + counter.ToString() + "] = " + value.ToString())
        counter <- counter + 1
$$ LANGUAGE plfsharp;
CALL SPITransactionTestRollbackFirstFSharp();
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-spi-transaction', 'SPITransactionTestRollbackFirstFSharp', CASE WHEN SUM(a) = 25 THEN TRUE ELSE FALSE END AS RESULT FROM TRANSACTION_TEST_FSHARP;

CREATE OR REPLACE FUNCTION SPIBatchCompoundParametersFSharp(init INTEGER, inc INTEGER) RETURNS INTEGER AS $$
    let conn = new NpgsqlConnection()
    conn.Open() |> ignore
    use _ = conn
    let batch = conn.CreateBatch()
    batch.BatchCommands.Add(new NpgsqlBatchCommand("DROP TABLE IF EXISTS SPI_COMPOUND_TESTS;"))
    batch.BatchCommands.Add(new NpgsqlBatchCommand("CREATE TABLE IF NOT EXISTS SPI_COMPOUND_TESTS (ID INTEGER);"))

    let mutable currentInit = init.Value
    for i = 1 to 5 do
        batch.BatchCommands.Add(new NpgsqlBatchCommand("INSERT INTO SPI_COMPOUND_TESTS (ID) VALUES (" + currentInit.ToString() + ");"))
        currentInit <- currentInit + inc.Value

    batch.BatchCommands.Add(new NpgsqlBatchCommand("UPDATE SPI_COMPOUND_TESTS SET ID = 2 * ID;"))
    batch.BatchCommands.Add(new NpgsqlBatchCommand("SELECT * FROM SPI_COMPOUND_TESTS;"))
    use reader = batch.ExecuteReader()

    // Skip to the last result set which contains the SELECT query results
    for i = 1 to 8 do
        reader.NextResult() |> ignore

    let mutable sum = 0
    while reader.Read() do
        let _a = reader.GetInt32(0)
        Elog.Info("Returned value = " + _a.ToString())
        sum <- sum + _a

    Elog.Info("Calling SPIBatchCompoundParametersFSharp with integer arguments.")
    Nullable(sum)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int-spi-batch-query-compoud-parameters', 'SPIBatchCompoundParametersFSharp1', SPIBatchCompoundParametersFSharp(1, 1) = 30;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int-spi-batch-query-compoud-parameters', 'SPIBatchCompoundParametersFSharp2', SPIBatchCompoundParametersFSharp(5, 2) = 90;

CREATE OR REPLACE FUNCTION SPIIntegerArrayWithNullFSharp() RETURNS INTEGER AS $$
    let conn = new NpgsqlConnection() // Ensure proper connection string is provided
    use _ = conn
    conn.Open()
    let cmd = new NpgsqlCommand("SELECT @p1", conn)
    let p1 = new NpgsqlParameter("p1", NpgsqlDbType.Array ||| NpgsqlDbType.Integer)
    cmd.Parameters.Add(p1)

    let arr = Array.CreateInstance(typeof<Nullable<int>>, 5)
    arr.SetValue((int)1, 0)
    arr.SetValue(None, 1)
    arr.SetValue((int)3, 2)
    arr.SetValue(None, 3)
    arr.SetValue((int)5, 4)

    p1.Value <- arr

    use reader = cmd.ExecuteReader()
    reader.Read()

    let array = reader.GetFieldValue<Nullable<int>[]>(0)

    let nullCount = array |> Array.fold (fun acc x -> acc + (if x.HasValue then 0 else 1)) 0
    Nullable(nullCount)
$$ LANGUAGE plfsharp;
INSERT INTO automated_test_results (FEATURE, TEST_NAME, RESULT)
SELECT 'f#-int-spi-array', 'SPIIntegerArrayWithNullFSharp', SPIIntegerArrayWithNullFSharp() = 2;
