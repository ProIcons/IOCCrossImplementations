using System.Diagnostics;
using Castle.DynamicProxy;

public class CallLogger : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var stopWatch = new Stopwatch();
        Console.WriteLine("---> Calling method {0}#{1}({2}) ",
            invocation.MethodInvocationTarget.DeclaringType,
            invocation.Method.Name,
            string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

        stopWatch.Start();
        try
        {
            invocation.Proceed();
        }
        catch (Exception exc)
        {
            stopWatch.Stop();
            Console.WriteLine("<--- Exiting Method [Fail] {0} ({1}ms): exception: {2}.",
                invocation.Method.Name,
                stopWatch.ElapsedMilliseconds,
                exc.Message
            );

            throw;
        }

        if (invocation.ReturnValue is not Task task)
        {
            stopWatch.Stop();
            Console.WriteLine("<--- Exiting Method [Succ] {0} ({1}ms): result: {2}.",
                invocation.Method.Name,
                stopWatch.ElapsedMilliseconds,
                invocation.ReturnValue
            );
        }
        else if (invocation.ReturnValue.GetType().IsGenericType)
        {
            task.ContinueWith((t) =>
            {
                var result = t.GetType().GetProperty(nameof(Task<object>.Result)).GetValue(task);
                stopWatch.Stop();
                Console.WriteLine("<--- Exiting Method [Succ] {0} ({1}ms): result: {2}.",
                    invocation.Method.Name,
                    stopWatch.ElapsedMilliseconds,
                    result
                );
            }, TaskContinuationOptions.None);
        }
        else
        {
            task.ContinueWith((t) =>
            {
                stopWatch.Stop();
                Console.WriteLine("<--- Exiting Method [Succ] {0} ({1}ms).",
                    invocation.Method.Name,
                    stopWatch.ElapsedMilliseconds
                );
            }, TaskContinuationOptions.None);
        }
    }
}