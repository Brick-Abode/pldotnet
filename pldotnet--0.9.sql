\echo Use "CREATE EXTENSION pldotnet" to load this file. \quit

-- .NET C# language
CREATE FUNCTION plcsharp_call_handler()
  RETURNS language_handler AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE FUNCTION plcsharp_inline_handler(internal)
  RETURNS VOID AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE FUNCTION plcsharp_validator(oid)
  RETURNS VOID AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE LANGUAGE plcsharp
  HANDLER plcsharp_call_handler
  INLINE plcsharp_inline_handler
  VALIDATOR plcsharp_validator;

-- .NET F# language
CREATE FUNCTION plfsharp_call_handler()
  RETURNS language_handler AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE FUNCTION plfsharp_inline_handler(internal)
  RETURNS VOID AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE FUNCTION plfsharp_validator(oid)
  RETURNS VOID AS 'MODULE_PATHNAME'
  LANGUAGE C IMMUTABLE STRICT;

CREATE LANGUAGE plfsharp
  HANDLER plfsharp_call_handler
  INLINE plfsharp_inline_handler
  VALIDATOR plfsharp_validator;
