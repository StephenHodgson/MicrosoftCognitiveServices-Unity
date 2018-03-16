using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public static class AwaitExtensions
{
    public static TaskAwaiter<int> GetAwaiter(this Process process)
    {
        var tcs = new TaskCompletionSource<int>();
        process.EnableRaisingEvents = true;

        process.Exited += (s, e) => tcs.TrySetResult(process.ExitCode);

        if (process.HasExited)
        {
            tcs.TrySetResult(process.ExitCode);
        }

        return tcs.Task.GetAwaiter();
    }

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
