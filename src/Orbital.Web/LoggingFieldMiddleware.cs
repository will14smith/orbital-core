using System;
using GraphQL.Instrumentation;

namespace Orbital.Web
{

    class LoggingFieldMiddleware
    {
        public static FieldMiddlewareDelegate Exec(FieldMiddlewareDelegate next)
        {
            return context =>
            {
                Console.WriteLine("Entered {0}", context.FieldName);
                try
                {
                    var result = next(context);

                    if (result?.Exception != null)
                    {
                        Console.WriteLine("Exception: {0}", result.Exception);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: {0}", ex);
                }
                finally
                {
                    Console.WriteLine("Finished {0}", context.FieldName);
                }

                return null;
            };
        }
    }
}
