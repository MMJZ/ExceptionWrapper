# ExceptionWrapper

This project uses a custom async/awaitable type to handle exceptions. The type can be awaited in an async block such that any instances in an error state or thrown exceptions are automatically caught and saved for the caller of the async function to handle.

This type replaces try/catch/finally blocks, because all exceptions are automatically caught and saved into the BaseError type.

This type replaces if statements that check success conditions (that is to say, almost all if statements) using the 'await' keyword.
