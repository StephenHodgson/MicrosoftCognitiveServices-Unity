#if !UNITY_WSA || UNITY_EDITOR
using System.Diagnostics;
using System.Runtime.CompilerServices;
#endif

using System.Threading.Tasks;

public static class AwaitExtensions
{
#if !UNITY_WSA || UNITY_EDITOR
    public static TaskAwaiter<int> GetAwaiter(this Process process)
    {
        var tcs = new TaskCompletionSource<int>();
        process.EnableRaisingEvents = true;

        process.Exited += (sender, args) => tcs.TrySetResult(process.ExitCode);

        if (process.HasExited)
        {
            tcs.TrySetResult(process.ExitCode);
        }

        return tcs.Task.GetAwaiter();
    }
#endif

    /// <summary>
    /// Any time you call an async method from sync code, you can either use this wrapper
    /// method or you can define your own `async void` method that performs the await
    /// on the given Task
    /// </summary>
    /// <param name="task"></param>
    public static async void WrapErrors(this Task task)
    {
        await task;
    }
}
