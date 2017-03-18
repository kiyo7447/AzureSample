

・NoSQL チュートリアル: DocumentDB C# コンソール アプリケーションの作成
https://docs.microsoft.com/ja-jp/azure/documentdb/documentdb-get-started



・エラー１

DocDBTrace Error: 0 : DocumentClientException with status code: NotFound, message: Message: {"Errors":["Resource Not Found"]}
ActivityId: 31c8b82e-f360-4f60-92ad-32c4419525aa, Request URI: /apps/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/services/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/partitions/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/replicas/999999999999999999x, and response headers: {
"Transfer-Encoding": "chunked",
"x-ms-last-state-change-utc": "Wed, 08 Mar 2017 23:25:31.541 GMT",
"x-ms-schemaversion": "1.3",
"x-ms-xp-role": "1",
"x-ms-request-charge": "1",
"x-ms-serviceversion": "version=1.11.165.1",
"x-ms-activity-id": "31c8b82e-f360-4f60-92ad-32c4419525aa",
"x-ms-session-token": "0:6554",
"Strict-Transport-Security": "max-age=31536000",
"x-ms-gatewayversion": "version=1.11.165.1",
"Date": "Thu, 09 Mar 2017 01:48:18 GMT",
"Server": "Microsoft-HTTPAPI/2.0",
}
DocDBTrace Error: 0 : Operation will NOT be retried. Current attempt 0, Exception: Microsoft.Azure.Documents.DocumentClientException: Message: {"Errors":["Resource Not Found"]}
ActivityId: 31c8b82e-f360-4f60-92ad-32c4419525aa, Request URI: /apps/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/services/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/partitions/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/replicas/999999999999999999x
   場所 Microsoft.Azure.Documents.Client.ClientExtensions.<ParseResponseAsync>d__4.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.ValidateEnd(Task task)
   場所 Microsoft.Azure.Documents.GatewayStoreModel.<>c__DisplayClass10.<<InvokeAsync>b__f>d__12.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<>c__DisplayClass2.<<ExecuteAsync>b__0>d__4.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<ExecuteRetry>d__1b.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<ExecuteRetry>d__1b.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<ExecuteAsync>d__a.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.GatewayStoreModel.<InvokeAsync>d__1f.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.GatewayStoreModel.<ProcessMessageAsync>d__2.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.Client.DocumentClient.<ReadAsync>d__2f6.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.Client.DocumentClient.<ReadDocumentPrivateAsync>d__179.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<>c__DisplayClass2.<<ExecuteAsync>b__0>d__4.MoveNext()
--- 直前に例外がスローされた場所からのスタック トレースの終わり ---
   場所 System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   場所 System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   場所 Microsoft.Azure.Documents.BackoffRetryUtility`1.<ExecuteRetry>d__1b.MoveNext() 
Family.ID Wakefield.6018 を作成しました。

Consistency Levelは、Sessoinにしています。

https://github.com/Azure/azure-documentdb-dotnet/issues/80



・エラー２
